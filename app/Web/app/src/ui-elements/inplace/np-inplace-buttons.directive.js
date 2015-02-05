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
                inplaceButtonYes: '&'
            }
        };

        return dir;

        function link(scope, element) {
            var dirScope = scope;
            element.on('$destroy', cleanup);
            var transcluded = angular.element(element[0].querySelector('[ng-transclude]'));
            var inlineButtonsContainer = angular.element(element[0].getElementsByClassName('inline-buttons'));
            var yesButton = angular.element(element[0].querySelector('input.yes'));
            var noButton = angular.element(element[0].querySelector('input.no'));
            yesButton.on('click', yesClick);
            noButton.on('click', noClick);
            transcluded.on('click', onClick);

            function yesClick() {
                if (dirScope.inplaceButtonYes && typeof (dirScope.inplaceButtonYes) == 'function') {
                    dirScope.inplaceButtonYes();
                }
                hideInlineButtons();
            }
            function noClick() {
                hideInlineButtons();
            }

            function hideInlineButtons() {
                transcluded.removeClass('hidden');
                inlineButtonsContainer.addClass('hidden');
            }

            function showInlineButtons() {
                transcluded.addClass('hidden');
                inlineButtonsContainer.removeClass('hidden');
            }

            function onClick() {
                showInlineButtons();
            }

            function cleanup() {
                transcluded.off('click', onClick);
            }
        }
    }
})();