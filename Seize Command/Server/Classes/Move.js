var Vector2 = require('./Vector2.js');

module.exports = class Move {
    constructor(X = 0, Y = 0, Z = new Vector2()) {
        this.id = X;
        this.timeSent = Y;
        this.position = Z;
    }
}