(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('ProfilePrivateController', profilePrivateController);

    profilePrivateController.$inject = ['$scope', 'Profile', 'profileData'];

    function profilePrivateController($scope, Profile, profileData) {
        var ctrl = this;
        ctrl.profileData = profileData.data.privateInfo;
        ctrl.save = save;

        function save() {
            Profile.updatePrivateInfo({
                    email: ctrl.profileData.email
                })
                .then(updatePrivateInfoSuccess);
        }

        function updatePrivateInfoSuccess() {
            $scope.profilePrivate.$setPristine();
        }
    }
})();