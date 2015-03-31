(function() {
    'use strict';

    angular.module('np.user-home')
        .directive('npTagAdd', tagAdd);

    function tagAdd() {
        var dir = {
            restrict: 'A',
            link: link,
            scope: {
                tags: '=tags'
            }
        };

        return dir;

        function link(scope, element, attrs) {
            element.on('$destroy', cleanup);
            element.on('keydown', keydown);

            function keydown(event) {
                event.stopPropagation();

                if (event.which == KeyEvent.DOM_VK_RETURN) {
                    addNewTag();
                }
            }

            function addNewTag() {
                var newTag = element[0].value && element[0].value.trim();

                if (newTag) {
                    scope.$evalAsync(function() {
                        scope.tags.push({
                            tag: newTag,
                            selected: true
                        });
                    });
                }
                element[0].value = '';
            }

            function cleanup() {
                element.off('keydown', keydown);
            }
        }
    }
})();