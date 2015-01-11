(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('ProfilePublicController', profilePublicController);

    profilePublicController.$inject = ['Profile', 'profileData'];

    function profilePublicController(Profile, profileData) {
        
    }
})();