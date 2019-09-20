var ServerObject = require('./ServerObject.js');
var Vector2 = require('./Vector2.js');

module.exports = class Ship extends ServerObject {
    constructor() {
        this.velocity = new Vector2();
    }
}