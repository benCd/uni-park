const http = require('http');

const hostname = '127.0.0.1';
const port = 3000;

var mysql      = require('mysql');
var connection = mysql.createConnection({
  host     : 'db4free.net',
  user     : 'testerman',
  password : 'bigtests',
  database : 'agreattest'
});

connection.connect();

http.createServer((request, response) => {
  if (request.method === 'GET' && request.url === '/pins') {
    connection.query('SELECT * FROM pins', function (error, results, fields) {
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

      var jsonresult = JSON.stringify(jsonobj);

      response.writeHead(200, {'Content-Type': 'application/json'})
      response.write(jsonresult);
      response.end();
      console.log(jsonresult);
    });
  } else if (request.method === 'POST' && request.url === '/pins') {

    //request.pipe(response);
    let body = [];
    request.on('data', (chunk) => {
      body.push(chunk);
    }).on('end', () => {
      body = Buffer.concat(body).toString();
      var pin = JSON.parse(body);

      connection.query('INSERT INTO pins SET ?', pin, function (error, results, fields) {
        if (error) throw error;
      });

      console.log(`inserted ${pin.Longtitude}, ${pin.Latitude}, ${pin.Accuracy}, ${pin.Date}`);
    });

    response.statusCode = 200;
    response.end();

  } else {
    response.statusCode = 404;
    response.end();
  }
}).listen(port, hostname, () => {
  console.log(`Server running at http://${hostname}:${port}/`);
});
