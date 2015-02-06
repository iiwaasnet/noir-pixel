(function() {
    'use strict';

    angular.module('np.ui-elements')
        .directive('npInplaceButtons', inplaceButtons);

    inplaceButtons.$inject = ['$document'];

    function inplaceButtons($document) {
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
            transcluded.on('click', actionClick);

            function yesClick(e) {
                e.stopPropagation();

                if (dirScope.inplaceButtonYes && typeof (dirScope.inplaceButtonYes) == 'function') {
                    dirScope.inplaceButtonYes();
                }
                hideInlineButtons();
            }
            function noClick(e) {
                e.stopPropagation();

                hideInlineButtons();
            }

            function hideInlineButtons() {
                $document.off('click', noClick);
                transcluded.removeClass('hidden');
                inlineButtonsContainer.addClass('hidden');
            }

            function showInlineButtons() {
                transcluded.addClass('hidden');
                inlineButtonsContainer.removeClass('hidden');
                $document.on('click', noClick);
            }

            function actionClick(e) {
                e.stopPropagation();

                showInlineButtons();
            }

            function cleanup() {
                yesButton.off('click', yesClick);
                noButton.off('click', noClick);
                transcluded.off('click', actionClick);
                $document.off('click', noClick);
            }
        }
    }
})();