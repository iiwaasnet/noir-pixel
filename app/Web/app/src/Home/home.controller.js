(function() {
    'use strict';

    angular.module('np.home')
        .controller('HomeController', homeController);

    homeController.$injector = ['Auth'];

    function homeController(Auth) {
        var ctrl = this;
        ctrl.getUserInfo = getUserInfo;

        function getUserInfo() {
            Auth.getUserInfo()
                .then(userInfoReceived, userInfoError);
        }

        function userInfoReceived(data) {
            alert(data);
        }

        function userInfoError(err) {
            alert(err);
        }
    }
})();