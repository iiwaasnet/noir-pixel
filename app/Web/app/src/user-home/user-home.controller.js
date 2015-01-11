(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('UserHomeController', userHomeController);

    userHomeController.$inject = ['User', 'Profile', 'profileData'];

    function userHomeController(User, Profile, profileData) {
        var ctrl = this;
        ctrl.user = {};

        activate();

        function activate() {
            ctrl.user.userName = profileData.data.user.userName;
            ctrl.user.displayName = profileData.data.user.fullName || ctrl.user.userName;
        }
    }
})();