var Vector2 = require('./Vector2');

module.exports = class ForceMove {
    constructor(X = 0) {
        this.id = X;
        this.velocity = new Vector2();
    }
}