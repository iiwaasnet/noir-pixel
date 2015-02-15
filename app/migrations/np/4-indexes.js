module.exports.id = "4-indexes";

module.exports.up = function (done) {
  var profiles = this.db.collection('photos');
    profiles.ensureIndex({ 'OwnerId': 1, 'Status': 1 }, {name: 'idx_ownerId_status'}, done);
};

module.exports.down = function (done) {
  var profiles = this.db.collection('photos');
    profiles.dropIndex('idx_ownerId_status', done);
};