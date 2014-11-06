(function() {
    'use strict';

    angular.module('np.i18n')
        .directive('npI18n', npI18n);

    npI18n.$inject = ['Strings'];

    function npI18n(Strings) {
        var dir = {
            restrict: "A",
            link: link,
            updateText: updateText
        };

        return dir;

        function updateText(elm, token, attrs) {
            var values = token.split('|');
            if (values.length >= 1) {
                // construct the tag to insert into the element
                var localizedTemplate = Strings.getLocalizedString(values[0]);
                // update the element only if data was returned
                if ((localizedTemplate !== null) && (localizedTemplate !== undefined) && (localizedTemplate !== '')) {
                    if (values.length > 1) {
                        for (var index = 1; index < values.length; index++) {
                            var target = '{' + (index - 1) + '}';
                            localizedTemplate = localizedTemplate.replace(target, values[index]);
                        }
                    }
                    // insert the text into the element
                    if (attrs.type === 'button') {
                        elm.val(localizedTemplate);
                    } else {
                        elm.text(localizedTemplate);
                    }
                }
            }
        }

        function link(scope, elm, attrs) {
            scope.$on('stringsUpdated', function() {
                dir.updateText(elm, attrs.npI18n, attrs);
            });

            attrs.$observe('npI18n', function(value) {
                dir.updateText(elm, attrs.npI18n, attrs);
            });
        }

    }
})();