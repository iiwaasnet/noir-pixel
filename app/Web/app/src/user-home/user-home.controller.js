(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('UserHomeController', userHomeController);

    userHomeController.$inject = ['User'];

    function userHomeController(User) {
        var ctrl = this;
        ctrl.user = {};
        ctrl.tabs = [
            {
                text: 'UserHome_Tab_Gallery',
                image: 'tab-public',
                state: 'userHome.profile',
                params: '',
                beforeActivate: function() { return true; }
            },
            {
                text: 'UserHome_Tab_Favorites',
                image: 'tab-gallery',
                state: 'userHome.profile',
                params: '',
                beforeActivate: function () { return true; }
            }
        ];
        activate();

        function activate() {
            var userData = User.getUserData();

            ctrl.user.userName = userData.userName;
        }
    }
})();