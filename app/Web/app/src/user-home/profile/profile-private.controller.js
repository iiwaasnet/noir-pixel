(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('ProfilePrivateController', profilePrivateController);

    profilePrivateController.$inject = ['Profile', 'profileData'];

    function profilePrivateController(Profile, profileData) {
        var ctrl = this;
        ctrl.profileData = profileData.data.privateInfo;
        ctrl.save = save;

        function save() {
            
        }
    }
})();