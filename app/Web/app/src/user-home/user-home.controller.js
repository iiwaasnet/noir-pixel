(function () {
    'use strict';

    angular.module('np.user-home')
        .controller('UserHomeController', userHomeController);

    userHomeController.$inject = ['User'];

    function userHomeController(User) {
        var ctrl = this;
        ctrl.user = {};
        ctrl.time = new Date().getTime();
        activate();

        function activate() {
            var userData = User.getUserData();

            ctrl.user.userName = userData.userName;
        }
    }
})();