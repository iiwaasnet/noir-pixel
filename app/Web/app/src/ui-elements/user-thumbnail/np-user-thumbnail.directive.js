(function() {
    'use strict';

    angular.module('np.ui-elements')
        .directive('npUserThumbnail', userThumbnail);

    userThumbnail.$inject = ['$templateFactory', '$compile', '$rootScope', 'User'];

    function userThumbnail($templateFactory, $compile, $rootScope, User) {
        var dir = {
            restrict: 'EA',
            scope: {
                displayName: '@',
                image: '@'
            },
            templateUrl: '/app/src/ui-elements/user-thumbnail/user-thumbnail.html',
            link: link
        };

        return dir;

        function link(scope, element, attrs) {
            var userData = User.getUserData() || {},
                thisUser = attrs.thisUser !== undefined;

            if (thisUser) {
                scope.image = userData.thumbnail;
                scope.displayName = userData.fullName || userData.userName;
            } else {
                scope.image = scope.image;
                scope.displayName = scope.displayName;
            }
        }
    }
})();