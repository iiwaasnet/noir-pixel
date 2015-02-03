module.exports.id = "1-indexes";

module.exports.up = function (done) {
    var profiles = this.db.collection('users');
    profiles.ensureIndex({ 'UserName': 1 }, { unique: true, name: 'udx_user_name' }, done);
};

module.exports.down = function (done) {
    var profiles = this.db.collection('users');
    profiles.dropIndex('udx_user_name', done);
};