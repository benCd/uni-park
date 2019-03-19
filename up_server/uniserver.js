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
const port = process.env.PORT || 8080;

//configuring mysql...
//mysql database connection info
var mysql = require('mysql');
var dbconnection = mysql.createConnection({
  host     : config.database.host,
  user     : config.database.user,
  password : config.database.password,
  database : config.database.db,
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
  cookie : {maxAge: 365 * 24 * 60 * 60 * 1000, httpOnly: false}, //set cookie expiration to 1 year, 'maxAge' used instead of 'expires' at express-session's reccomendation
}));
app.use(passport.initialize());
app.use(passport.session());


//routing for web services starts here!
//register
app.post('/register', function (req, res, next) { //need to dynamically set uni!!
  var newuser = req.body;
  var unimail = newuser.email.substring(newuser.email.indexOf('@')+1, newuser.email.length)
  console.log(unimail);

  dbconnection.query('SELECT `id` FROM `universities` WHERE `email` = ?', unimail, function (error, results, fields) {
    if (error) return next(error);
    if (results.length ==0 )
      res.status(400).send('Not a valid university email');
    else {
      var uid = results[0].id;


      dbconnection.query('SELECT * FROM `users` WHERE `name` = ?', newuser.email, function (error, results, fields) { //checks if this email already exists in db
        if (error) return next(error);
        if (results.length !=0 ) { //if we get a result, it does
          res.status(409).send('Email already registered'); //don't proceed to creating new row, stop here
        } else {
          dbconnection.query({sql: 'INSERT INTO `users`(name,pass,university_id) VALUES(?,?,?)',
          values: [newuser.email, newuser.password, uid]}, function (error, results, fields) {
            if (error) return next(error);
            res.status(201).send('User created');
          });
        }
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
      console.log(user.name+" logged in");
      return res.send('Logged in as ' + user.name);
    });
  })(req, res, next);
});

//logout
app.get('/logout', requireAuth, function (req, res) {
  req.session.destroy();
  res.send('Logged out as ' + req.user.name);
});

//new and untested
//get gps pins for given lot
//updated
app.post('/lotpins', function (req, res, next) { //lot_id has not been added to db yet...
  dbconnection.query('SELECT * FROM `gpsdata` WHERE `lot_id` = ?', req.body.lot_id, function (error, results, fields) {
    if (error) return next(error);

    var jsonobj = {
      pins: []
    };

    if (results.length == 0) return res.json(jsonobj);

    for (var i = 0; i < results.length; i++) {
      var pin = rdpToObj(results[i]);
      jsonobj.pins.push(pin);
    }

    res.json(jsonobj);
  });
});

//working
//send a new gps pin to db
app.post('/newpin', requireAuth, function (req, res, next) {
   var pin = req.body;
   var volume = truncateDecimal(pin.volume, 2);
   dbconnection.query({sql: 'INSERT INTO `gpsdata`(user_id,timestamp,longitude,latitude,lot_id,volume) VALUES(?,?,?,?,?,?)',
   values: [req.user.id, new Date(), pin.longitude, pin.latitude, pin.lot_id, volume]}, function (error, results, fields) {
     if (error) return next(error);
   });
   res.send('Pin added');
});

//determine current lot from gps data
app.post('/findlot', function (req, res, next) {
  var pos = req.body;
  var acc = req.body.accuracy;
  dbconnection.query('SELECT * FROM `lots` WHERE `min_lat` <= ? AND `max_lat` >= ? AND `min_long` <= ? AND `max_long` >= ?',
  [pos. latitude, pos.latitude, pos.longitude, pos.longitude], function (error, results, fields) {
    if (error) return next(error);

    if (results.length == 0) return res.status(404).send('No lots');

    var lot = results[0];
    return res.json(rdpToObj(lot));

    res.status(404).send('Lot was not found');
  });
});

//get lot reports from lot id
//may be removed later
app.post('/getlotinfo', function (req, res, next) {
  dbconnection.query('SELECT * FROM `lotdata` WHERE `id` = ?', req.body.lot_id, function (error, results, fields) {
    if (error) return next(error);

    if (results.length == 0) return res.status(404).send('Lot contains no reports');

    var jsonobj = {
      reports: []
    };

    for (var i = 0; i < results.length; i++) {
      var report = rdpToObj(results[i]);
      jsonobj.reports.push(report);
    }

    res.json(jsonobj);
  });
});

//get lots by university id
app.get('/getmyunilots', requireAuth, function (req, res, next) {
  dbconnection.query('SELECT * FROM `lots` WHERE `university_id` = ?', req.user.university_id, function (error, results, fields) {
    if (error) return next(error);

    var jsonobj = {
      lots: []
    };

    if (results.length == 0) return res.json(jsonobj);

    for (var i = 0; i < results.length; i++) {
      var report = rdpToObj(results[i]);
      jsonobj.lots.push(report);
    }

    res.json(jsonobj);
  });
});

//new
//get university info for a specific user given their id
app.post('/getuni', function (req, res, next) {
  dbconnection.query('SELECT `university_id` FROM `users` WHERE `id` = ?', req.body.user_id, function (error, results, fields) {
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

//see if survey taken
app.get('/surveystatus', requireAuth, function (req, res, next) {
  var status = req.user.surveytaken;
  res.send(status.toString());
});

//update survey status for current user
app.get('/surveyset', requireAuth, function (req, res, next) {
  dbconnection.query('UPDATE `users` SET `surveytaken` = ? WHERE `id` = ?', [1, req.user.id], function (error, results, fields) {
    if (error) return next(error);
  });
  res.send('Survey status updated for user');
});

//voting on pins
app.post('/vote', requireAuth, function (req, res, next) {
  var vote = req.body.vote;
  if (typeof vote != "number") return next(error);
  if (vote > 0){
    dbconnection.query('UPDATE `gpsdata` SET `upvotes` = `upvotes` + ? WHERE `id` = ?',
    [vote, req.body.pin_id], function (error, results, fields) {
      if (error) return next(error);
      res.send("upvote added");
    });
  }else{
    dbconnection.query('UPDATE `gpsdata` SET `downvotes` = `downvotes` + ? WHERE `id` = ?',
    [Math.abs(vote), req.body.pin_id], function (error, results, fields) {
      if (error) return next(error);
      res.send("downvote added");
    });
  }
});

//3/17
app.post('/getpinbyid', requireAuth, function (req, res, next) {
  var id = req.body.id;
  dbconnection.query('SELECT * FROM `gpsdata` WHERE `id` = ?', id, function (error, results, fields) {
    if (error) return next(error);

    if (results.length == 0) return res.status(404).send('No pin');

    res.json(rdpToObj(results[0]));
  });
});

app.post('/getuserbyid', requireAuth, function (req, res, next) {
  var id = req.body.id;
  dbconnection.query('SELECT `name` FROM `users` WHERE `id` = ?', id, function (error, results, fields) {
    if (error) return next(error);

    if (results.length == 0) return res.status(404).send('No user');

    res.json(results[0].name);
  });
});

app.post('/surveyresults', requireAuth, function (req, res, next) {
  var myArr = req.body;
  console.log(myArr);
  var dates = previousCalendarWeek();

  for (var i = 0; i < myArr.length; i++) {
    var dayName = myArr[i].Day;
    var lid = myArr[i].Lot_id;
    var sVolume = truncateDecimal(myArr[i].StartVolume, 2);
    var eVolume = truncateDecimal(myArr[i].EndVolume, 2);

    var newDate1 = dates[dayName];
    var hmsStart = myArr[i].StartTime;
    var a1 = hmsStart.split(':');
    newDate1.setHours(a1[0],a1[1],a1[2],0);

    var newDate2 = dates[dayName];
    var hmsEnd = myArr[i].EndTime;
    var a2 = hmsEnd.split(':');
    newDate2.setHours(a2[0],a2[1],a2[2],0);

    var values = [
      [lid, req.user.id, newDate1, sVolume],
      [lid, req.user.id, newDate2, eVolume]
    ];

    dbconnection.query('INSERT INTO `surveydata` (lot_id, user_id, timestamp, volume) VALUES ?',
    [values], function (error, results, fields) {
      if (error) return next(error);
    });
  }
  console.log("Survey recieved by "+req.user.name);
  res.send("Survey recieved");
});

function truncateDecimal(num, fixed) {
    var re = new RegExp('^-?\\d+(?:\.\\d{0,' + (fixed || -1) + '})?');
    var str = num.toString().match(re)[0];
    return parseFloat(str);
}

function previousCalendarWeek(){
  var d0 = new Date();
  d0.setDate(d0.getDate() - d0.getDay() - 7);
  var d1 = new Date();
  d1.setDate(d0.getDate() + 1);
  var d2 = new Date();
  d2.setDate(d0.getDate() + 2);
  var d3 = new Date();
  d3.setDate(d0.getDate() + 3);
  var d4 = new Date();
  d4.setDate(d0.getDate() + 4);
  var d5 = new Date();
  d5.setDate(d0.getDate() + 5);
  var d6 = new Date();
  d6.setDate(d0.getDate() + 6);

  var dates = {"Sunday": d0, "Monday": d1, "Tuesday": d2, "Wednesday": d3, "Thursday": d4, "Friday": d5, "Saturday": d6};
  return dates;
}

//finally, make our https server and listen for requests
http.createServer(app).listen(port,() => {
  console.log(`Server running at http://${hostname}:${port}/`);
});
