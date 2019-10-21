var io = require('socket.io')(process.env.PORT || 52300);

//Custom Classes
var ServerObject =          require('./Classes/ServerObject.js');
var Ship =                  require('./Classes/Ship.js');
var MirroredPairPackage =   require('./Classes/MirroredPairPackage.js');
var TakeDamage =            require('./Classes/TakeDamage.js');
var RotationPackage =       require('./Classes/RotationPackage.js');
var Vector2Package =        require('./Classes/Vector2Package.js');
var Vector2 =               require('./Classes/Vector2.js');
var SendReceivePackage =    require('./Classes/SendReceivePackage.js');

var serverObjects = [];
var ships = [];
var sockets = [];

var mainshipId = 0;

var maxPlayersPerShip = 3;

console.log('Server has started');

io.on('connection', function(socket) {
    console.log('Connection Made!');

    var player = new ServerObject('Player');
    var playerMirror = new ServerObject('Player Mirror');
    var thisPlayerID = player.id;
    var thisPlayerMirrorID = playerMirror.id;

    serverObjects[thisPlayerID] = player;
    serverObjects[thisPlayerMirrorID] = playerMirror;
    sockets[thisPlayerID] = socket;

    //Tell the client that this is our id for the server
    socket.emit('register', {   id1: thisPlayerID,
                                id2: thisPlayerMirrorID });

    if(ships.length == 0) {
        console.log('Spawn Ship');
        var ship = new Ship('Space Ship');
        var thisShipID = ship.id;

        var pilotSeat = ship.pilotSeat;
        var gunSeat = ship.gunSeat;

        var pilotSeatID = pilotSeat.id;
        var gunSeatID = gunSeat.id;

        serverObjects[pilotSeatID] = pilotSeat;
        serverObjects[gunSeatID] = gunSeat;

        serverObjects[thisShipID] = ship;
        ships[thisShipID] = ship;

        socket.emit('serverSpawn', ship);
        socket.broadcast.emit('serverSpawn', ship);

        mainshipId = thisShipID;
    }

    var playerPackage = new MirroredPairPackage('Player');
    playerPackage.spawn1ID = thisPlayerID;
    playerPackage.spawn2ID = thisPlayerMirrorID;
    playerPackage.parentID = mainshipId;

    //Tell myself that I have spawned
    socket.emit('serverSpawnMirroredPair', playerPackage);
    //Tell others that I have spawned
    socket.broadcast.emit('serverSpawnMirroredPair', playerPackage);

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

    socket.on('attack', function() {
        socket.broadcast.emit('attack', {id: thisPlayerID});
    });

    socket.on('interact', function(data) {
        var senderID = data.senderID;
        var receiverID = data.receiverID;

        var package = new SendReceivePackage(senderID, receiverID);

        socket.broadcast.emit('interact', package);
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
        var name = data.name;
        
        var spawn = new ServerObject(name);
        spawn.position = new Vector2(data.position.x, data.position.y);
        spawn.rotation = data.rotation;

        var thisSpawnID = spawn.id;
        serverObjects[thisSpawnID] = spawn;

        socket.emit('serverSpawn', spawn);
        socket.broadcast.emit('serverSpawn', spawn);
    });

    socket.on('serverSpawnMirroredPair', function(data) {
        var name = data.name;
        var position = new Vector2(data.position.x, data.position.y);
        var rotation = data.rotation;

        var spawn1 = new ServerObject(name);
        var spawn2 = new ServerObject(name + " Mirror");
        spawn1.position = position;
        spawn2.position = position;
        spawn1.rotation = rotation;
        spawn2.rotation = rotation;

        serverObjects[spawn1.id] = spawn1;
        serverObjects[spawn2.id] = spawn2;

        var package = new MirroredPairPackage(name);
        package.spawn1ID = spawn1.id;
        package.spawn2ID = spawn2.id;
        package.position = position;
        package.rotation = rotation;

        socket.emit('serverSpawnMirroredPair', package);
        socket.broadcast.emit('serverSpawnMirroredPair', package);
    })


    socket.on('disconnect', function() {
        console.log('A player has disconnected');
        delete sockets[thisPlayerID];
        socket.broadcast.emit('disconnected', player);
    });
});