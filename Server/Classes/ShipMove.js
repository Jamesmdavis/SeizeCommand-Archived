var Vector2 = require('./Vector2.js');

module.exports = class ShipMove {
    constructor(X = 0) {
        this.id = X;
        this.position = new Vector2();
    }
}