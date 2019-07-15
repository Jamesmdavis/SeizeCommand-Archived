var shortID = require('shortid');
var Vector2 = require('./Vector2.js');

module.exports = class Player {
    constructor(rot = 0) {
        this.username = '';
        this.id = shortID.generate();
        this.position = new Vector2();
        this.rotation = rot;
    }
}