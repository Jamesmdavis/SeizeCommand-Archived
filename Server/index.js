var io = require('socket.io')(process.env.PORT || 52300);

//Custom Classes
var ServerObject = require('./Classes/ServerObject.js');
var Ship = require('./Classes/Ship.js');
var TakeDamage = require('./Classes/TakeDamage.js');
var Move = require('./Classes/Move.js');
var CollisionMove = require('./Classes/CollisionMove.js');
var forceMove = require('./Classes/ForceMove.js');
var Aim = require('./Classes/Aim.js');
var SeatMove = require('./Classes/SeatMove.js');
var Vector2 = require('./Classes/Vector2.js');
var Velocity2D = require('./Classes/Velocity2D.js');

var serverObjects = [];
var ships = [];
var sockets = [];
//var spawnPoints = [[0, 0], [-15, 5], [8, 11], [12, -7], [-16, -6], [0, -12]];
var spawnPoints = [[0, 0]];

var masterSocket = 0;

console.log('Server has started');

io.on('connection', function(socket) {
    console.log('Connection Made!');

    var player = new ServerObject('Player');

    var thisPlayerID = player.id;

    serverObjects[thisPlayerID] = player;
    sockets[thisPlayerID] = socket;

    //If This is connection is the first player, assign them as the master User
    //This allows us to designate this player as the one who's messages we trust
    if(sockets.length == 1) {
        masterSocket = socket;
    }

    //Random Spawn Point for Player
    //var num = Math.floor((Math.random() * 6));
    var num = 0;
    player.position.x = spawnPoints[num][0];
    player.position.y = spawnPoints[num][1];

    //Tell the client that this is our id for the server
    socket.emit('register', {id: thisPlayerID});

    if(ships.length == 0) {
        var ship = new Ship('SpaceShip');
        var thisShipID = ship.id;
        serverObjects[thisShipID] = ship;

        socket.emit('serverSpawn', ship);
        socket.broadcast.emit('serverSpawn', ship);
    }

    //Tell myself that I have spawned
    socket.emit('serverSpawn', player);
    //Tell others that I have spawned
    socket.broadcast.emit('serverSpawn', player);

    //Tell myself about everyone else in the game
    for(var key in serverObjects) {
        if(serverObjects[key].name == 'Player') {
            if(key != thisPlayerID) {
                socket.emit('serverSpawn', serverObjects[key]);
            }
        }
    }

    //Update the Player's Position based on the Inputs received from the client
    socket.on('move', function(data) {
        var clientInputs = new Vector2(data.clientInputs.x, data.clientInputs.y);
        var speed = data.speed;
        var deltaTime = data.deltaTime;
        var timeSent = data.timeSent;

        //The amount of change the position will undergo
        var newDeltaPositions = new Vector2(clientInputs.x * speed * deltaTime, 
            clientInputs.y * speed * deltaTime);

        player.position.x += newDeltaPositions.x;
        player.position.y += newDeltaPositions.y;

        //Round to two decimal places
        player.position.x = Math.round(player.position.x * 100) / 100;
        player.position.y = Math.round(player.position.y * 100) / 100;

        //Creates a Move Message to be sent to the clients
        var move = new Move(thisPlayerID, timeSent, player.position);

        socket.broadcast.emit('move', move);
        socket.emit('move', move);
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

        var collisionMove = new CollisionMove(thisPlayerID);
        collisionMove.position = player.position;

        socket.broadcast.emit('collisionMove', collisionMove);
    });

    socket.on('forceMove', function(data) {
        var ship = serverObjects[data.id];

        var velocity = new Vector2();
        velocity.x = data.velocity.x;
        velocity.y = data.velocity.y;

        var thrust = data.thrust;
        var deltaTime = data.deltaTime;

        var dV = velocity * deltaTime * thrust;
        velocity += dV;

        var package = new forceMove(id);
        package.velocity.x = velocity.x;
        package.velocity.y = velocity.y;

        ship.velocity.x = velocity.x;
        ship.velocity.y = velocity.y;

        socket.emit('forceMove', package);
        socket.broadcast.emit('forceMove', package);
        
        /*
        var clientPosition = new Vector2(data.x, data.y);
        ship.position = clientPosition;

        var shipMove = new ShipMove(ship.id);
        shipMove.position = ship.position;

        socket.broadcast.emit('shipMove', shipMove);
        */
    });

    socket.on('updateVelocity', function(data) {
        if(sockets[thisPlayerID] == socket) {
            var thisShipID = data.id;
            var velocity = new Vector2();
            velocity.x = data.velocity.x;
            velocity.y = data.velocity.y;
    
            var ship = serverObjects[thisShipID];
            ship.velocity.x = velocity.x;
            ship.velocity.y = velocity.y;
    
            var package = new Velocity2D(thisShipID);
            package.velocity.x = velocity.x;
            package.velocity.y = velocity.y;
    
            socket.broadcast.emit('updateVelocity', package);
        }
    });

    socket.on('aim', function(data) {
        player.rotation = data.rotation;
        var aim = new Aim(thisPlayerID, data.rotation);
        socket.broadcast.emit('aim', aim);
    });

    socket.on('shipAim', function(data) {
        ship.rotation = data.rotation;
        var aim = new Aim(ship.id, data.rotation);
        socket.broadcast.emit('shipAim', aim);
    });

    socket.on('seatMove', function(data) {
        var x = data.position.x;
        var y = data.position.y;
        var rotation = data.rotation;

        player.position.x = x;
        player.position.y = y;
        player.rotation = rotation;

        var seatMove = new SeatMove();
        seatMove.id = thisPlayerID;
        seatMove.position.x = x;
        seatMove.position.y = y;
        seatMove.rotation = rotation;

        socket.broadcast.emit('seatMove', seatMove);
        socket.emit('seatMove', seatMove);
    });

    socket.on('attack', function() {
        socket.broadcast.emit('attack', {id: thisPlayerID});
    });

    socket.on('interact', function() {
        socket.broadcast.emit('interact', {id: thisPlayerID});
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

    socket.on('respawn', function() {
        var num = Math.floor((Math.random() * 6));
        player.position.x = spawnPoints[num][0];
        player.position.y = spawnPoints[num][1];
        socket.emit('respawn', player);
        socket.broadcast.emit('respawn', player);
    });

    socket.on('serverSpawn', function(data) {
        var position = new Vector2();
        position.x = data.position.x;
        position.y = data.position.y;

        var parent = new Vector2();
        parent.x = data.parent.x;
        parent.y = data.parent.y;

        var spawn = new Spawn();
        spawn.name = data.name;
        spawn.position = position;
        spawn.rotation = data.rotation;
        spawn.parent = data.parent;

        socket.emit('serverSpawn', spawn);
        socket.broadcast.emit('serverSpawn', spawn);
    });


    socket.on('disconnect', function() {
        console.log('A player has disconnected');
        delete sockets[thisPlayerID];
        socket.broadcast.emit('disconnected', player);
    });
});