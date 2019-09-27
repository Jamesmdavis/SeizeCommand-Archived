var shortID = require('shortid');
var ServerObject = require('./ServerObject.js');

module.exports = class Player extends ServerObject {
    constructor(MirrorName = '') {
        this.mirrorName = MirrorName;
        this.mirrorID = shortID.generate();
    }
}