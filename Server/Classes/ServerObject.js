var shortID = require('shortid');
var Vector2 = require('./Vector2.js');

module.exports = class ServerObject {
    constructor(Name = '') {
        this.name = Name;
        this.id = shortID.generate();
        this.position = new Vector2();
        this.rotation = 0;
    }
}