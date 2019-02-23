//config file for storing private connection info
var config = require('./config');
//node js housekeeping, getting packages and making an express instance
const http = require('http');
var express = require('express');
var app = express();

//nodejs server connection info
const hostname = config.server.host;
const port = config.server.port;

//mysql database connection info
var mysql      = require('mysql');
var dbconnection = mysql.createConnection({
  host     : config.database.host,
  user     : config.database.user,
  password : config.database.password,
  database : config.database.db
});

app.use(express.json());
dbconnection.connect();

//routing for web services starts here!
app.get('/pins', function (req, res) {
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

//no more routing so here is a last resort route for sending 404's
app.get('*', function (req, res) {
  res.status(404).send('No implementation for this request!');
});

//finally, make our https server and listen for requests
http.createServer(app).listen(port, hostname, () => {
  console.log(`Server running at http://${hostname}:${port}/`);
});
