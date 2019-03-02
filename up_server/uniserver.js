//config file for storing private connection info
const config = require('./config');
//node js housekeeping, getting modules and making an express instance
const http = require('http');
const express = require('express');
const uuid = require('uuid/v4');
const session = require('express-session');
const passport = require('passport');
const FileStore = require('session-file-store')(session);
const LocalStrategy = require('passport-local').Strategy;
//make main express instance
const app = express();

//nodejs server connection info
const hostname = config.server.host;
const port = config.server.port;

//configuring mysql...
//mysql database connection info
var mysql = require('mysql');
var dbconnection = mysql.createConnection({
  host     : config.database.host,
  user     : config.database.user,
  password : config.database.password,
  database : config.database.db
});
dbconnection.connect();

//custom function for translating mysql's rowdatapacket results into standard js objects
function rdpToObj(rowObject) {
  var result = {};
  Object.keys(rowObject).forEach(function(key, index) {
    result[key] = rowObject[key];
  });
  return result;
}

//configuring passport...
//configure passport.js to use the local strategy
passport.use(new LocalStrategy({ usernameField: 'email' }, function (email, password, done) {
  dbconnection.query({sql: 'SELECT * FROM `users` WHERE `name` = ? AND `pass` = ?',
  values: [email, password]}, function (error, results, fields) {
    if (error) return done(error); //return error object if mysql error
    if (results.length == 0) return done(null, false); //return nothing if no results
    //else we have a result with matching credentials, let's return it
    const user = rdpToObj(results[0]); //convert result rowdatapacket to js object
    return done(null, user); //return said js object
  });
}));

//serializing and deserializing user from cookie
//serialize
passport.serializeUser((user, done) => {
  done(null, user.id);
});

//deserialize
passport.deserializeUser((id, done) => {
  dbconnection.query('SELECT * FROM `users` WHERE `id` = ?', id, function (error, results, fields) {
    if (error) return done(error);
    if (results.length == 0) return done(null, false);

    const user = rdpToObj(results[0]);
    return done(null, user);
  });
});

//custom function for requirng authentication in a http request
function requireAuth (req, res, next) {
  if(req.isAuthenticated()) {
    return next();
  } else {
    res.status(401).send('Authentication failure');
  }
};

//app.use for additional functionality
app.use(express.json());
app.use(session({
  genid: (req) => {
    return uuid(); //name session with new uuid
  },
  store : new FileStore({path : './sessions/'}), //store our sessions here
  secret: config.secret,
  resave: false,
  saveUninitialized: false,
  cookie : {maxAge: 365 * 24 * 60 * 60 * 1000}, //set cookie expiration to 1 year, 'maxAge' used instead of 'expires' at express-session's reccomendation
}));
app.use(passport.initialize());
app.use(passport.session());


//routing for web services starts here!
//register
app.post('/register', function (req, res, next) {
  var newuser = req.body;

  dbconnection.query('SELECT * FROM `users` WHERE `name` = ?', newuser.email, function (error, results, fields) { //checks if this email already exists in db
    if (error) return next(error);
    if (results.length !=0 ) { //if we get a result, it does
      res.status(409).send('Email already registered'); //don't proceed to creating new row, stop here
    } else {
      dbconnection.query({sql: 'INSERT INTO `users`(name,pass,university_id,credibility) VALUES(?,?,?,?)', //create new rot for new user
      values: [newuser.email, newuser.password, 1, 10]}, function (error, results, fields) {
        if (error) return next(error);
        res.send('User created');
      });
    }
  });
});

//login
app.post('/login', function (req, res, next) {
  passport.authenticate('local', function (error, user, info) {
    if (info) return res.send(info.message);
    if (error) return next(error);
    if (!user) return res.status(401).send('Invalid credentials');
    req.login(user, (error) => {
      if (error) return next(error);
      return res.send('Logged in as ' + user.name);
    });
  })(req, res, next);
});

//logout
app.get('/logout', function (req, res) {
  if (req.user) {
    req.session.destroy();
    res.send('Logged out as ' + req.user.name);
  } else res.send('Not logged in!');
});

//testing authentication
/*
app.get('/auth', function (req, res) {
  if(req.isAuthenticated()) {
    console.log("omg yes");
    res.send('success');
  } else {
    res.send('failure');
  }
});
*/

//new and untested
//get gps pins for given lot
app.get('/lotpins', requireAuth, function (req, res, next) { //lot_id has not been added to db yet...
  dbconnection.query('SELECT * FROM `gpsdata` WHERE `lot_id` = ?', req.body.lot_id, function (error, results, fields) {
    if (error) return next(error);

    if (results.length == 0) return res.status(404).send('Lot contains no pins');

    var jsonobj = {
      pins: []
    };

    for (var i = 0; i < results.length; i++) {
      var pin = rdpToObj(results[i]);
      jsonobj.pins.push(pin);
    }

    res.json(jsonobj);
  });
});

//new, make sure values match up with db and app model
//send a new gps pin to db
app.post('/newpin', function (req, res, next) {
   var pin = req.body;
   dbconnection.query({sql: 'INSERT INTO `gpsdata`(userid,timestamp,longtitude,latitude,lotid) VALUES(?,?,?,?)', //create new rot for new user
   values: [req.user.id, pin.date, pin.longtitude, pin.latitude, pin.lotid]}, function (error, results, fields) {
     if (error) return next(error);
   });
   res.send('Pin added');
});

//new
//determine current lot from gps data
app.post('/findlot', function (req, res, next) {
  var pos = req.body;
  var acc = req.body.accuracy;
  dbconnection.query('SELECT * FROM `lots`', function (error, results, fields) {
    if (error) return next(error);

    if (results.length == 0) return res.status(404).send('No lots');

    for (var i = 0; i < results.length; i++) {
      var lot = results[i];
      if (pos.latitude >= (lot.min_lat-acc) && pos.latitude <= (lot.max_lat+acc)
      && pos.longtitude >= (lot.min_long-acc) && pos.longtitude <= (lot.max_long+acc)){
        return res.json(rdpToObj(lot));
      }
    }

    res.status(404).send('Lot was not found');
  });
});

//new
//get university info for a specific user given their id
app.post('/getuni', function (req, res, next) {
  dbconnection.query('SELECT `university_id` FROM `users` WHERE `id` = ?', req.body.userid, function (error, results, fields) {
    if (error) return next(error);

    if (results.length == 0) return res.status(404).send('No user with given id');

    dbconnection.query('SELECT * FROM `universities` WHERE `id` = ?', results[0].university_id, function (error, results, fields) {
      if (error) return next(error);

      if (results.length == 0) return res.status(404).send('No uni associated with uni id in user');

      res.json(rdpToObj(results[0]));
    });
  });
});

//new
//get university info for current user
app.get('/myuni', requireAuth, function (req, res, next) {
  dbconnection.query('SELECT * FROM `universities` WHERE `id` = ?', req.user.university_id, function (error, results, fields) {
    if (error) return next(error);

    if (results.length == 0) return res.status(404).send('No uni associated with uni id in user');

    res.json(rdpToObj(results[0]));
  });
});

//finally, make our https server and listen for requests
http.createServer(app).listen(port, hostname, () => {
  console.log(`Server running at http://${hostname}:${port}/`);
});
