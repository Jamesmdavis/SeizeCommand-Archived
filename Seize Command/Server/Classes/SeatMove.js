var Vector2 = require('./Vector2.js');

module.exports = class SeatUpdatePositionRotation {
    constructor(X = '', rot = 0) {
        this.id = X;
        this.position = new Vector2();
        this.rotation = rot;
    }
}