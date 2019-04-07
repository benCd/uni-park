const config = require('./config');
var mysql = require('mysql');
var dbconnection = mysql.createConnection({
  host     : config.database.host,
  user     : config.database.user,
  password : config.database.password,
  database : config.database.db,
});
dbconnection.connect();

var uid = 2;
var interval = 5;
var coeff = 1000 * 60 * interval;

//insertEmptyRows();
updateDailyAvgs();

function updateDailyAvgs(){
  dbconnection.query('SELECT * FROM `lotdata` WHERE `lot_id` IN (SELECT `id` FROM `lots` WHERE `university_id` = ?) ORDER BY `timestamp` DESC'
  , uid, function (error, results, fields) {
    if (error) throw(error);

    for (var i = 0; i < results.length; i++){
      var date = results[i].timestamp;

      console.log(date);
      var testdate = new Date(results[i].timestamp.getTime() + 5*60000);

      var rounded = new Date(Math.round(date.getTime() / coeff) * coeff);
      console.log("ROUNDED: ");
      console.log(rounded);
      console.log("--");
    }
  });
}

function insertEmptyRows() {
  dbconnection.query('SELECT `id` FROM `lots` WHERE `university_id` = ?', uid, function (error, results, fields) {
    if (error) throw(error);

    var values = [];

    var numberOfLots = results.length;
    var numberOfRows = 1440/interval;

    var datetime = new Date('1970-01-01T00:00:00Z');

    for (var i = 0; i < numberOfLots; i++) {
      for (var k = 0; k < numberOfRows; k++) {
        var rowDate = new Date(datetime.getTime() + k*5*60000);
        var rowTime = dateToHMSString(rowDate);
        values.push([results[i].id, rowTime, 0]);
      }
    }

    dbconnection.query('INSERT INTO `dailyavg` (lot_id, time, occupancy) VALUES ?',
    [values], function (error, results, fields) {
      if (error) throw(error);
    });
  });
}

function dateToHMSString(dateObj){
  var hours = ("0"+dateObj.getUTCHours()).slice(-2);
  var minutes = ("0"+dateObj.getUTCMinutes()).slice(-2);
  var seconds = ("0"+dateObj.getUTCSeconds()).slice(-2);
  var string = ""+hours+":"+minutes+":"+seconds;
  return string;
}
