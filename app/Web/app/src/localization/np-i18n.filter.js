(function() {
    'use strict';

    angular.module('np.i18n')
        .filter('npi18n', npI18n);

    npI18n.$inject = ['Strings'];

    function npI18n(Strings) {
        return translate;

        function translate(item) {
            var values = item.split('|');

            if (values.length >= 1) {
                var localizedTemplate = Strings.getLocalizedString(values[0].trim());
                if ((localizedTemplate !== null) && (localizedTemplate !== undefined) && (localizedTemplate !== '')) {
                    if (values.length > 1) {
                        for (var index = 1; index < values.length; index++) {
                            var target = '{' + (index - 1) + '}';
                            localizedTemplate = localizedTemplate.replace(target, values[index].trim());
                        }
                    }
                    return localizedTemplate;
                }
            }

            return '';
        }
    }
})();