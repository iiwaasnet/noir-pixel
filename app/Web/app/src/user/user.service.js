(function() {
    'use strict';

    angular.module('np.user')
        .service('User', userService);

    userService.$inject = ['Storage', 'EventsHub'];

    function userService(Storage, EventsHub) {
        var srv = this;
        srv.saveUserData = saveUserData;
        srv.getUserData = getUserData;
        srv.updateUserData = updateUserData;
        var userDataStorageKey = 'userData';

        activate();

        function saveUserData(userData) {
            Storage.set(userDataStorageKey, createUserDataObject(userData));
        }

        function getUserData() {
            return Storage.get(userDataStorageKey);
        }

        function updateUserData(userData) {
            var saved = Storage.get(userDataStorageKey);
            saved = angular.extend(saved, createUserDataObject(userData));
            Storage.set(userDataStorageKey, saved);
        }

        function activate() {
            EventsHub.addListener(EventsHub.events.Auth.SignedOut, signedOut);
        }

        function signedOut() {
            Storage.remove(userDataStorageKey);
        }

        function createUserDataObject(userData) {
            var obj = {}
            userData = userData || {};
            if (userData.userName) {
                obj.userName = userData.userName;
            }
            if (userData.thumbnail) {
                obj.thumbnail = userData.thumbnail;
            }
            if (userData.roles && userData.roles instanceof Array) {
                obj.roles = userData.roles;
            }

            return obj;
        }
    }
})();