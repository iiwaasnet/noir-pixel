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
                text: 'UserHome_Tab_Favorites',
                handler: function(text) { alert(text); },
                image: 'tab-gallery-img',
                state: 'userHome.profile'
            },
            {
                text: 'UserHome_Tab_Gallery',
                handler: function(text) { alert(text); },
                image: 'tab-gallery-img'
            }
        ];
        activate();

        function activate() {
            var userData = User.getUserData();

            ctrl.user.userName = userData.userName;
        }
    }
})();