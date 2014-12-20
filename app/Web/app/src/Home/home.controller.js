(function() {
    'use strict';

    angular.module('np.home')
        .controller('HomeController', homeController);

    homeController.$inject = ['$http', 'Messages', 'Progress', 'Url', 'Config'];

    function homeController($http, Messages, Progress, Url, Config) {
        var ctrl = this;
        ctrl.error = error;
        ctrl.message = message;
        ctrl.userInfo = userInfo;

        function userInfo() {
            $http.get(Url.build(Config.ApiUris.Base, 'account/user-info'))
            .then(userInfoSuccess, userInfoError);
        }

        function userInfoSuccess(response) {
            debugger;
            alert(response.data);
        }

        function userInfoError(error) {
            debugger;
            alert(error);
        }

        function error() {
            Progress.start();
        }

        function message() {
            Messages.message({
                main: {
                    text: 'Simple text message'
                }
            });
        }
    }
})();