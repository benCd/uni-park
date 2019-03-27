//config file for storing private connection info
const config = require('./config');
const async = require('async');
//google maps distance matrix api
const googleMapsClient = require('@google/maps').createClient({
  key: config.gkay
});

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

var destinations = [];
var origins = [];
var originIds = [];
var resultsArr = [];
var uid = 2;

async.series([
  function(callback) {
      dbconnection.query('SELECT * FROM `destinations` WHERE `university_id` = ?', uid, function (error, results, fields) {
        if (error) throw error;

        for (var i = 0; i < results.length; i++) {
          var destinationObj = {};
          destinationObj.id = results[i].building_id;
          destinationObj.address = results[i].address;

          destinations.push(destinationObj);
        }

        callback();
      });
    },
    function(callback) {
      dbconnection.query('SELECT * FROM `lots` WHERE `university_id` = ?', uid, function (error, results, fields) {
        if (error) throw error;

        for (var i = 0; i < results.length; i++) {
          var originCoords = results[i].center_lat;
          originCoords += ',';
          originCoords += results[i].center_long;

          originIds.push(results[i].id);
          origins.push(originCoords);
        }

        callback();
      });
    },
],
function() {
  console.log(origins);
  console.log(destinations);

  async.each(destinations, function (building, callback) {
    googleMapsClient.distanceMatrix({
      origins: origins,
      destinations: building.address,
      units: 'imperial',
      mode: 'walking'
    }, function(error, response) {
      if (!error) {
        var resultObj = {};
        resultObj.building_id = building.id;
        resultObj.rows = response.json.rows;

        resultsArr.push(resultObj);
        callback();
      }
    });

  }, function () {
    console.log("inserting...");
    for (var i = 0; i < resultsArr.length; i++) {
      var buildingRows = resultsArr[i].rows;
      for (var k = 0; k < buildingRows.length; k++) {
        var distance = ((buildingRows[k].elements[0].distance.value)*0.000621371192).toFixed(2);
        var duration = Math.round((buildingRows[k].elements[0].duration.value)/60);
        console.log(">building id: "+resultsArr[i].building_id+" // lot id: "+originIds[k]+" // distance: "+distance+" // duration: "+duration);

        dbconnection.query({sql: 'INSERT INTO `distancematrix`(lot_id, building_id, time, distance) VALUES(?,?,?,?)',
        values: [originIds[k], resultsArr[i].building_id, duration, distance]}, function (error, results, fields) {
          if (error) throw(error);
        });
      }
    }

  });
});
