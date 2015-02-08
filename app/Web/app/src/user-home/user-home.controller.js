(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('UserHomeController', userHomeController);

    userHomeController.$inject = ['States', 'User', 'Profile', 'profileData'];

    function userHomeController(States, User, Profile, profileData) {
        var ctrl = this;
        ctrl.user = {};
        ctrl.tabs = [];

        activate();

        function activate() {
            ctrl.user.userName = profileData.data.publicInfo.user.userName;
            ctrl.user.displayName = profileData.data.publicInfo.user.fullName || ctrl.user.userName;

            ctrl.tabs = createTabs();
        }

        function createTabs() {
            ctrl.tabs.push({
                parentState: States.UserHome.Darkroom.Name,
                state: States.UserHome.Darkroom.Name,
                text: 'UserHome_Tab_Darkroom',
                image: 'tab-darkroom'
            });

            return ctrl.tabs;
        }
    }
})();