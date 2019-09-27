var Vector2 = require('./Vector2.js');

module.exports = class MirroredPairPackage {
    constructor(NAME = '') {
        this.spawn1Name = NAME;
        this.spawn2Name = NAME + " Mirror";
        this.spawn1ID = 0;
        this.spawn2ID = 0;
        this.position = new Vector2();
        this.rotation = 0;
    }
}