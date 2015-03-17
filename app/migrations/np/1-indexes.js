module.exports.id = "1-indexes";

module.exports.up = function(done) {
    var profiles = this.db.collection('profiles');
    profiles.ensureIndex({ 'UserId': 1 }, {unique: true, name: 'udx_userId'}, done);
};

module.exports.down = function(done) {
    var profiles = this.db.collection('profiles');
    profiles.dropIndex('udx_userId', done);
};