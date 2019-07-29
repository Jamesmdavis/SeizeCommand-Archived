var io = require('socket.io')(process.env.PORT || 52300);

//Custom Classes
var Player = require('./Classes/Player.js');
var TakeDamage = require('./Classes/TakeDamage.js');
var UpdatePosition = require('./Classes/UpdatePosition.js');
var Vector3 = require('./Classes/Vector3.js');
var Vector2 = require('./Classes/Vector2.js');

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

    //Positional Data from Client
    socket.on('updatePosition', function(data) {
        var horizontal = data.horizontal;
        var vertical = data.vertical;
        var speed = data.speed;
        var deltaTime = data.deltaTime;
        var timeSent = data.timeSent;

        var newHorizontalPosition = horizontal * speed * deltaTime;
        var newVerticalPosition = vertical * speed * deltaTime;

        player.position.x += newHorizontalPosition;
        player.position.y += newVerticalPosition;

        player.position.x = Math.round(player.position.x * 100) / 100;
        player.position.y = Math.round(player.position.y * 100) / 100;

        var updatePosition = new UpdatePosition(thisPlayerID, player.position, timeSent);

        socket.broadcast.emit('updatePosition', updatePosition);
        socket.emit('updatePosition', updatePosition);
    });

    socket.on('updateRotation', function(data) {
        player.rotation = data.rotation;

        socket.broadcast.emit('updateRotation', player);
    });

    socket.on('attack', function() {
        socket.broadcast.emit('attack', player);
    });

    socket.on('takeDamage', function(data) {
        var senderID = data.senderID;
        var receiverID = data.receiverID;
        var damage = data.damage;

        console.log('myID: ' + thisPlayerID);
        console.log('senderID: ' + senderID);

        if(thisPlayerID === senderID)
        {
            console.log('Take Damage');

            var takeDamage = new TakeDamage(receiverID, damage);

            socket.emit('takeDamage', takeDamage);
            socket.broadcast.emit('takeDamage', takeDamage);
        }
    });

    socket.on('disconnect', function() {
        console.log('A player has disconnected');
        delete players[thisPlayerID];
        delete sockets[thisPlayerID];
        socket.broadcast.emit('disconnected', player);
    });
});