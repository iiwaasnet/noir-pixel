(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('ProfileWallController', profileWallController);

    profileWallController.$inject = ['$rootScope', '$scope', '$filter', 'States', 'Url', 'Config', 'Strings', 'Profile', 'Progress', 'profileData'];

    function profileWallController($rootScope, $scope, $filter, States, Url, Config, Strings, Profile, Progress, profileData) {
        var ctrl = this;
        ctrl.profileData = profileData.data.publicInfo;
        ctrl.startEditingPost = startEditingPost;
        ctrl.cancelEditingPost = cancelEditingPost;
        ctrl.addPostToWall = addPostToWall;
        ctrl.editingWallPost = undefined;
        var unsubscribe;

        activate();

        function addPostToWall() {

        }

        function cancelEditingPost() {
            ctrl.editingWallPost = false;
        }

        function startEditingPost() {
            ctrl.editingWallPost = true;
        }


        function stateChangeStart(event, toState, toParams, fromState, fromParams) {
            if (fromState.name === States.UserHome.Profile.Public.Name) {
            }
        }

        function cleanup() {
            unsubscribe && unsubscribe();
        }

        function activate() {
            $scope.$on('$destroy', cleanup);
            unsubscribe = $rootScope.$on('$stateChangeStart', stateChangeStart);
        }

    }
})();