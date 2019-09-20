var Vector2 = require('./Vector2.js');

module.exports = class Velocity2D {
    constructor(ID = 0) {
        this.id = ID;
        this.velocity = new Vector2();
    }
}