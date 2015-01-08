(function() {
    'use strict';

    angular.module('np.ui-elements')
        .directive('npThisUser', thisUserDirective);

    thisUserDirective.$inject = ['$templateFactory', '$compile', '$rootScope', 'User'];

    function thisUserDirective($templateFactory, $compile, $rootScope, User) {
        var dir = {
            restrict: 'EA',
            scope: {
                displayName: '@',
                image: '@'
            },
            transclude: true,
            templateUrl: '/app/src/ui-elements/this-user/this-user.html',
            link: link
        };

        return dir;

        function link(scope, element, attrs, ctrl, transcludeFn) {
            var userData = User.getUserData();
            scope.image = scope.image || userData.thumbnail;
            scope.displayName = scope.displayName || userData.fullName || userData.userName;
            scope.image = '';
            transcludeFn(scope, cloneFunc);

            function cloneFunc(clone, _) {
                $templateFactory.fromUrl(dir.templateUrl)
                    .then(function(template) {
                        template = angular.element(template);
                        var transcluded = angular.element(template[0].querySelector('[transclude]'));
                        var parent = transcluded.parent();
                        parent.append(clone.append(transcluded));
                        $compile(template)(scope);
                        element.replaceWith(template);
                    });
            }
        }
    }
})();