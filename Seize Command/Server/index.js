var io = require('socket.io')(process.env.PORT || 52300);

//Custom Classes
var Player = require('./Classes/Player.js');
var TakeDamage = require('./Classes/TakeDamage.js');
var UpdatePosition = require('./Classes/UpdatePosition.js');
var SeatUpdatePositionRotation = require('./Classes/SeatUpdatePositionRotation.js');
var CollisionMove = require('./Classes/CollisionMove.js');
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

    //Update the Player's Position based on the Inputs received from the client
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

        //Round to two decimal places
        player.position.x = Math.round(player.position.x * 100) / 100;
        player.position.y = Math.round(player.position.y * 100) / 100;

        var updatePosition = new UpdatePosition(thisPlayerID, timeSent);
        updatePosition.position = player.position;

        socket.broadcast.emit('updatePosition', updatePosition);
        socket.emit('updatePosition', updatePosition);
    });

    socket.on('collisionMove', function(data) {
        var clientInputs = new Vector2(data.clientInputs.x, data.clientInputs.y);
        var clientPosition = new Vector2(data.clientPosition.x, data.clientPosition.y);
        var speed = data.speed;
        var deltaTime = data.deltaTime;

        var deltaXPosition = clientInputs.x * speed * deltaTime;
        var deltaYPosition = clientInputs.y * speed * deltaTime;

        var xRange = new Vector2(player.position.x - deltaXPosition, player.position.x + deltaXPosition);
        var yRange = new Vector2(player.position.y - deltaYPosition, player.position.y + deltaYPosition);

        player.position.x = clientPosition.x;
        player.position.y = clientPosition.y;

        var collisionMove = new CollisionMove(thisPlayerID)
        collisionMove.position = player.position;

        socket.broadcast.emit('collisionMove', collisionMove);

        /*
        if((clientPosition.x >= xRange.x && clientPosition.x <= xRange.y) && 
            (clientPosition.y >= yRange.x && clientPosition.y <= yRange.y))
        {
            player.position.x = clientPosition.x;
            player.position.y = clientPosition.y;
        }
        else
        {
            var collisonMove = new CollisionMove(thisPlayerID);
            collisonMove.position = player.position;
            
            socket.broadcast.emit('collisionMove', collisonMove);
            socket.emit('collisionMove', collisonMove);
        }
        */
    });

    socket.on('seatUpdatePositionRotation', function(data) {
        console.log('Interact');
        var x = data.position.x;
        var y = data.position.y;
        var rotation = data.rotation;

        player.position.x = x;
        player.position.y = y;
        player.rotation = rotation;

        var seatUpdatePositionRotation = new SeatUpdatePositionRotation();
        seatUpdatePositionRotation.id = thisPlayerID;
        seatUpdatePositionRotation.position.x = x;
        seatUpdatePositionRotation.position.y = y;
        seatUpdatePositionRotation.rotation = rotation;

        socket.emit('seatUpdatePositionRotation', seatUpdatePositionRotation);
    });

    socket.on('updateRotation', function(data) {
        player.rotation = data.rotation;

        socket.broadcast.emit('updateRotation', player);
    });

    socket.on('attack', function() {
        socket.broadcast.emit('attack', player);
    });

    socket.on('interact', function() {
        socket.broadcast.emit('interact', player);
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