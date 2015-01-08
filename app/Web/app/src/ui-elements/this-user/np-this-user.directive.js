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
            //scope.image = '';
            var transclusionScope,
                transcludedContent = [];
            transcludeFn(scope, cloneFunc);

            function cloneFunc(clone, transScope) {
                $templateFactory.fromUrl(dir.templateUrl)
                    .then(function(template) {
                        template = angular.element(template);
                        var transcluded = template[0].querySelectorAll('[transclude]');

                        transclusionScope = transScope;
                        transcludedContent = cloneElement(clone, transcluded.length);

                        angular.forEach(transcluded, function(el, i) {
                            debugger;
                            var content = transcludedContent[i];
                            el = angular.element(el);
                            var parent = el.parent();
                            parent.append(content.append(el));
                        });

                        $compile(template)(scope);
                        element.replaceWith(template);

                        element.on('$destroy', cleanup);
                    });
            }

            function cloneElement(clone, times) {
                var clones = [clone];
                for (var i = 0; i < times - 1; i++) {
                    clones.push(clone.clone(false));
                }

                return clones;
            }

            function cleanup() {
                debugger;
                if (transclusionScope) {
                    transclusionScope.$destroy();
                }
                angular.forEach(transcludedContent, function(el) {
                    el.remove();
                });
            }
        }
    }
})();