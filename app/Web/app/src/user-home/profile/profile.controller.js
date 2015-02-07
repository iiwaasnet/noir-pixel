(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('ProfileController', profileController);

    profileController.$inject = ['States', 'profileData'];

    function profileController(States, profileData) {
        var ctrl = this;
        ctrl.tabs = [];

        activate();

        function activate() {
            createTabs();            
        }

        function createTabs() {
            var params = { userName: profileData.data.publicInfo.user.userName };

            ctrl.tabs.push({
                state: States.UserHome.Profile.Public.Name,
                params: params,
                text: 'UserHome_Profile_Tab_Public',
                image: 'tab-public'
            });
            ctrl.tabs.push({
                state: States.UserHome.Profile.Wall.Name,
                params: params,
                text: 'UserHome_Profile_Tab_Wall',
                image: 'tab-wall'
            });
            ctrl.tabs.push({
                state: States.UserHome.Profile.Private.Name,
                params: params,
                text: 'UserHome_Profile_Tab_Private',
                image: 'tab-private'
            });
        }
    }
})();
