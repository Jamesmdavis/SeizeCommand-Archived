var shortID = require('shortid');
var Vector2 = require('./Vector2');

module.exports = class Ship {
    constructor(rot = 0) {
        this.username = '';
        this.id = shortID.generate();
        this.position = new Vector2();
        this.rotation = rot;
    }
}