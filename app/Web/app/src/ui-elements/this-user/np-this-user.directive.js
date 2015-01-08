(function() {
    'use strict';

    angular.module('np.ui-elements')
        .directive('npThisUser', thisUserDirective);

    thisUserDirective.$inject = ['User'];

    function thisUserDirective(User) {
        var dir = {
            restrict: 'EA',
            scope: {
                displayName: '@',
                img: '@'
            },
            templateUrl: '/app/src/ui-elements/this-user/this-user.html',
            link: link
        };

        return dir;

        function link(scope, element, attrs) {
            debugger;
            var userData = User.getUserData();
            scope.img = scope.img || userData.thumbnail;
            scope.displayName = scope.displayName || userData.fullName || userData.userName;
        }
    }
})();