module.exports.id = "3-indexes";

module.exports.up = function (done) {
	var profiles = this.db.collection('photos');
    profiles.ensureIndex({ 'OwnerId': 1, 'ShortId': 1 }, {unique: true, name: 'udx_ownerId_shortId'}, done);
};

module.exports.down = function (done) {
	var profiles = this.db.collection('photos');
    profiles.dropIndex('udx_ownerId_shortId', done);
};