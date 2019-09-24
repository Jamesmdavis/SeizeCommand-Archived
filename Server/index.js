var io = require('socket.io')(process.env.PORT || 52300);

//Custom Classes
var ServerObject =      require('./Classes/ServerObject.js');
var Player =            require('./Classes/Player.js');
var Ship =              require('./Classes/Ship.js');
var TakeDamage =        require('./Classes/TakeDamage.js');
var RotationPackage =   require('./Classes/RotationPackage.js');
var SeatMove =          require('./Classes/SeatMove.js');
var Vector2Package =    require('./Classes/Vector2Package.js');
var Vector2 =           require('./Classes/Vector2.js');

var serverObjects = [];
var ships = [];
var sockets = [];

console.log('Server has started');

io.on('connection', function(socket) {
    console.log('Connection Made!');

    var player = new Player('Player', 'Player Mirror');
    var thisPlayerID = player.id;

    serverObjects[thisPlayerID] = player;
    sockets[thisPlayerID] = socket;

    //Tell the client that this is our id for the server
    socket.emit('register', {id: thisPlayerID});

    if(ships.length == 0) {
        var ship = new Ship('Space Ship');
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
        if(key != thisPlayerID) {
            if(serverObjects[key].name == 'Player') {
                socket.emit('serverSpawn', serverObjects[key]);
            }
        }
    }

    //Update the Transforms Position based on the Inputs received from the client
    socket.on('changePosition', function(data) {
        var objectID = data.id;
        var object = serverObjects[objectID];

        var position = new Vector2(data.vector2.x, data.vector2.y);
        object.position.x = position.x;
        object.position.y = position.y;

        var package = new Vector2Package(objectID);
        package.vector2.x = position.x;
        package.vector2.y = position.y;

        socket.broadcast.emit('changePosition', package);
    });

    socket.on('changeVelocity', function(data) {
        var objectID = data.id;
        var object = serverObjects[objectID];

        var velocity = new Vector2(data.vector2.x, data.vector2.y);
        object.velocity.x = velocity.x;
        object.velocity.y = velocity.y;

        var package = new Vector2Package(objectID);
        package.vector2.x = velocity.x;
        package.vector2.y = velocity.y;

        socket.broadcast.emit('changeVelocity');
    });

    socket.on('changeRotation', function(data) {
        var objectID = data.id;
        var object = serverObjects[objectID];

        var rotation = data.rotation;
        object.rotation = rotation

        var package = new RotationPackage(objectID);
        package.rotation = rotation;

        socket.broadcast.emit('changeRotation', package);
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

function isInBounds(position) {
    if((position.x >= playerXBoundaries[0] || position.x <= playerXBoundaries[1])
        || (position.y >= playerYBoundaries[0] || position.y <= playerYBoundaries[1])) {
        return true;
    }

    return false;
}