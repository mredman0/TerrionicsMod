var express = require('express');
var app = express();
var server = require('http').Server(app);
var io = require('socket.io')(server);
var port = process.env.PORT || 5000;

server.listen(port);

app.use(express.static('public'));

io.on('connection', function(socket) {
    console.log("connected");
    socket.on('SocketTest', function(data) {
        console.log(data);
    });
    socket.on('disconnect', function() {
        console.log('disconnected');
    });
	socket.on('player-position', function (data) {
		io.sockets.emit('display-player-position', data);
		
	});
    socket.on('socket-connected', function(data) {
        io.sockets.emit('news', data);
    });

    socket.on('chat message', function(data) {
        if (data.substring(0, "Spawn ".length) == "Spawn ")
            io.sockets.emit('spawn item', data.substring("Spawn ".length, data.length));
        else
            io.sockets.emit('chat message', data);
    });
});

console.log(`\nServer is listening on port ${port}.\nGoto http://localhost:${port} to open the interface.`);
