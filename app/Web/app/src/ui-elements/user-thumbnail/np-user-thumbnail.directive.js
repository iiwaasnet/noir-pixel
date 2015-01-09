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
            transclude: true,
            templateUrl: '/app/src/ui-elements/user-thumbnail/user-thumbnail.html',
            link: link
        };

        return dir;

        function link(scope, element, attrs, ctrl, transcludeFn) {
            var transclusionScope,
                transcludedContent = [],
                userData = User.getUserData() || {};
            debugger;
            var thisUser = attrs.thisUser !== undefined;
            if (thisUser) {
                scope.image = userData.thumbnail;
                scope.displayName = userData.fullName || userData.userName;
            } else {
                scope.image = scope.image;
                scope.displayName = scope.displayName;
            }
            

            if (transcludeFn) {
                transcludeFn(scope, cloneFunc);
            }

            function cloneFunc(clone, transScope) {
                $templateFactory.fromUrl(dir.templateUrl)
                    .then(function(template) { transcludeDirective(template, clone, transScope); });
            }

            function transcludeDirective(template, clone, transScope) {
                template = angular.element(template);
                var transcluded = template[0].querySelectorAll('[transclude]');

                transclusionScope = transScope;
                transcludedContent = cloneElement(clone, transcluded.length);

                angular.forEach(transcluded, transcludeElement);

                $compile(template)(scope);
                element.replaceWith(template);

                element.on('$destroy', cleanup);
            }

            function transcludeElement(el, i) {
                var content = transcludedContent[i];
                el = angular.element(el);
                var parent = el.parent();
                parent.append(content.append(el));
            }

            function cloneElement(clone, times) {
                var clones = [clone];
                for (var i = 0; i < times - 1; i++) {
                    clones.push(clone.clone(false));
                }

                return clones;
            }

            function cleanup() {
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