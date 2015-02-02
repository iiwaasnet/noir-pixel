module.exports.id = "indexes";

module.exports.up = function (done) {
 var profiles = this.db.collection('countries');
    profiles.ensureIndex({ 'Code': 1 }, {unique: true, name: 'udx_code'}, done);
};

module.exports.down = function (done) {
    var profiles = this.db.collection('countries');
    profiles.dropIndex('udx_code', done);
};