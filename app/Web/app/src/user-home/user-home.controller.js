(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('UserHomeController', userHomeController);

    userHomeController.$inject = ['States', 'User', 'Profile', 'profileData', 'Strings'];

    function userHomeController(States, User, Profile, profileData, Strings) {
        var ctrl = this;
        ctrl.user = {};
        ctrl.tabs = [];

        activate();

        function activate() {
            ctrl.user.userName = profileData.data.publicInfo.user.userName;
            ctrl.user.displayName = Strings.getLocalizedString('UserHome_Tab_You');

            ctrl.tabs = createTabs();
        }

        function createTabs() {
            ctrl.tabs.push({
                parentState: States.UserHome.Darkroom.Name,
                state: States.UserHome.Darkroom.Name,
                text: 'UserHome_Tab_Darkroom',
                image: 'tab-darkroom'
            });
            ctrl.tabs.push({
                parentState: States.UserHome.Photos.Name,
                state: States.UserHome.Photos.Name,
                text: 'UserHome_Tab_Photos',
                image: 'tab-photos'
            });

            return ctrl.tabs;
        }
    }
})();