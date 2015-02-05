(function() {
    'use strict';

    angular.module('np.ui-elements')
        .directive('npInplaceButtons', inplaceButtons);

    function inplaceButtons() {
        var dir = {
            restrict: 'AE',
            templateUrl: '/app/src/ui-elements/inplace/inplace-buttons.html',
            transclude: true,
            link: link,
            scope: {
                inplaceButtonsYes: '@'
            }
        };

        return dir;

        function link(scope, element) {
            var transcluded = angular.element(element[0].querySelector('[ng-transclude]'));
            var inlineButtons = angular.element(element[0].getElementsByClassName('inline-buttons'));
            transcluded.on('click', onClick);

            function onClick() {
                transcluded.addClass('hidden');
                inlineButtons.removeClass('hidden');
            }
        }
    }
})();