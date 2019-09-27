var ServerObject = require('./ServerObject.js');
var Vector2 = require('./Vector2.js');

module.exports = class Ship extends ServerObject {
    constructor(NAME = '') {
        super(NAME);
        this.velocity = new Vector2();
    }
}