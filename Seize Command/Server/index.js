var io = require('socket.io')(process.env.PORT || 52300);

//Custom Classes
var Player = require('./Classes/Player.js');

var players = [];
var sockets = [];

console.log('Server has started');

io.on('connection', function(socket) {
    console.log('Connection Made!');

    var player = new Player();
    var thisPlayerID = player.id;

    players[thisPlayerID] = player;
    sockets[thisPlayerID] = socket;

    //Tell the client that this is our id for the server
    socket.emit('register', {id: thisPlayerID});
    //Tell myself that I have spawned
    socket.emit('spawn', player);
    //Tell others that I have spawned
    socket.broadcast.emit('spawn', player);

    //Tell myself about everyone else in the game
    for(var playerID in players) {
        if(playerID != thisPlayerID)
        {
            socket.emit('spawn', players[playerID]);
        }
    }

    socket.on('disconnect', function() {
        console.log('A player has disconnected');
        delete players[thisPlayerID];
        delete sockets[thisPlayerID];
        socket.broadcast.emit('disconnected', player);
    });
});