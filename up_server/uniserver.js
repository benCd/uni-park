//config file for storing private connection info
const config = require('./config');

//node js housekeeping, getting modules and making an express instance
const http = require('http');
const express = require('express');

//modules for sessions and authentication
const uuid = require('uuid/v4');
const session = require('express-session');
const passport = require('passport');
const FileStore = require('session-file-store')(session);
const LocalStrategy = require('passport-local').Strategy;

const app = express();

//nodejs server connection info
const hostname = config.server.host;
const port = config.server.port;

//mysql database connection info
var mysql = require('mysql');
var dbconnection = mysql.createConnection({
  host     : config.database.host,
  user     : config.database.user,
  password : config.database.password,
  database : config.database.db
});
dbconnection.connect();

//START OF AUTH STUFF
function rdpToJson(rowObject) {
  var result = {};
  Object.keys(rowObject).forEach(function(key, index) {
    result[key] = rowObject[key];
  });
  return result;
}

// configure passport.js to use the local strategy
passport.use(new LocalStrategy({ usernameField: 'email' }, function (email, password, done) {
  dbconnection.query({sql: 'SELECT * FROM `users` WHERE `name` = ? AND `pass` = ?',
  values: [email, password]}, function (error, results, fields) {
    if (error) return done(error);
    if (results.length == 0) return done(null, false);

    const user = rdpToJson(results[0]);
    return done(null, user);
  });
}));

//serializing and deserializing from cookie
passport.serializeUser((user, done) => {
  done(null, user.id);
});


passport.deserializeUser((id, done) => {
  dbconnection.query('SELECT * FROM users WHERE id = ?', id, function (error, results, fields) {
    if (error) return done(error);
    if (results.length == 0) return done(null, false);

    const user = rdpToJson(results[0]);
    return done(null, user);
  });
});

//app.use for additional functionality
app.use(express.json());
app.use(session({
  genid: (req) => {
    return uuid();
  },
  store: new FileStore(),
  secret: config.secret,
  resave: false,
  saveUninitialized: true,
  cookie : {maxAge: 365 * 24 * 60 * 60 * 1000},
}));
app.use(passport.initialize());
app.use(passport.session());

//routing for web services starts here!
//login
app.post('/login', function (req, res, next) {
  passport.authenticate('local', function (error, user, info) {
    if (info) return res.send(info.message);
    if (error) return next(err);
    if (!user) return res.status(401).send('Invalid Credentials');
    req.login(user, (error) => {
      if (error) return next (error);
      return res.send('You were authenticated & logged in!');
    });
  })(req, res, next);
});

app.get('/logout', function (req, res){
  req.session.destroy(function() {
    res.send('logged out');
  });
});

app.get('/auth', function (req, res) {
  if(req.isAuthenticated()) {
    res.send('success');
  } else {
    res.send('failure');
  }
});

function requireAuth (req, res, next) {
  if(req.isAuthenticated()) {
    return next();
  } else {
    res.status(401).send('authentication failure');
  }
};
//END OF AUTH STUFF

app.get('/pins', requireAuth, function (req, res) {
  dbconnection.query('SELECT * FROM pins', function (error, results, fields) {
    if (error) throw error;

    var jsonobj = {
      pins: []
    };

    for (var i = 0; i < results.length; i++) {
      var pin = results[i];
      jsonobj.pins.push({
        "longtitude" : pin.Longtitude,
        "latitude"   : pin.Latitude,
        "accuracy"   : pin.Accuracy,
        "date"       : pin.Date
      });
    }

    res.json(jsonobj);
    console.log(jsonobj);
  });
});

app.post('/pins', (req, res) => {
   var pin = req.body;

   dbconnection.query('INSERT INTO pins SET ?', pin, function (error, results, fields) {
     if (error) throw error;
   });

   console.log(`inserted ${pin.Longtitude}, ${pin.Latitude}, ${pin.Accuracy}, ${pin.Date}`);
   res.json({requestBody: req.body});
});

//finally, make our https server and listen for requests
http.createServer(app).listen(port, hostname, () => {
  console.log(`Server running at http://${hostname}:${port}/`);
});
