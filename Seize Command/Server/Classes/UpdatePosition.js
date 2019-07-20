var Vector2 = require('./Vector2.js');

module.exports = class UpdatePosition {
    constructor(X = 0, Y = 0) {
        this.id = X;
        this.position = new Vector2();
        this.timeSent = Y;
    }
}