var shortID = require('shortid');
var ServerObject = require('./ServerObject.js');

module.exports = class MirrorObject extends ServerObject {
    constructor(MirrorName = '') {
        this.mirrorName = MirrorName;
        this.mirrorID = shortID.generate();
    }
}