(function() {
    'use strict';

    angular.module('np.user-home')
        .directive('npExifEdit', exifEdit);

    exifEdit.$inject = ['$document', '$rootScope'];

    function exifEdit($document, $rootScope) {
        var dir = {
            restrict: 'E',
            link: link,
            templateUrl: '/app/src/user-home/darkroom/np-exif-edit.html'
        };
        var CANCEL_EDIT = 'np.exif-edit.cancel';

        return dir;

        function link(scope, element, attrs) {
            scope.keydown = keydown;
            element.on('$destroy', cleanup);
            element.on('click', click);
            $document.on('click', cancelEdit);
            var unsubscribe = $rootScope.$on(CANCEL_EDIT, cancelEdit);

            function click(e) {
                e.stopPropagation();

                $rootScope.$emit(CANCEL_EDIT);
                editable(true);
            }

            function keydown(event) {
                event.stopPropagation();

                if (event.which == 13 || event.which == 27 || event.which == 9) {
                    cancelEdit();
                }
            }

            function editable(yes) {
                scope.$evalAsync(function () {
                    scope.edit = yes;
                });
            }

            function cancelEdit() {
                if (scope.edit) {
                    editable(false);
                }
            }

            function cleanup() {
                element.off('click', click);
                $document.off('click', cancelEdit);
                unsubscribe && unsubscribe();
            }
        }
    }
})();