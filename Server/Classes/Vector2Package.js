var Vector2 = require('./Vector2.js');

module.exports = class Vector2Package {
    constructor(X = 0) {
        this.id = X;
        this.vector2 = new Vector2();
    }
}