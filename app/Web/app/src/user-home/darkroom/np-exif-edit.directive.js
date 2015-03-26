(function() {
    'use strict';

    angular.module('np.user-home')
        .directive('npExifEdit', exifEdit);

    exifEdit.$inject = ['$document'];

    function exifEdit($document) {
        var dir = {
            restrict: 'E',
            link: link,
            templateUrl: '/app/src/user-home/darkroom/np-exif-edit.html'
        };

        return dir;

        function link(scope, element, attrs) {
            scope.keydown = keydown;
            element.on('$destroy', cleanup);
            element.on('click', click);
            $document.on('click', cancelEdit);

            function click(e) {
                e.stopPropagation();
                editable(true);
            }

            function keydown(event) {
                if (event.which == 13 || event.which == 27 || event.which == 9) {
                    cancelEdit();
                }
            }

            function editable(yes) {
                scope.$evalAsync(function () { scope.edit = yes; });
            }

            function cancelEdit() {
                editable(false);
            }

            function cleanup() {
                element.off('click', click);
                $document.off('click', cancelEdit);
            }
        }
    }
})();