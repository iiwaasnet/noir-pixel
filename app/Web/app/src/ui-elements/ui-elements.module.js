(function() {
    'use strict';

    angular.module('np.ui-elements', [])
        .config(config);

    config.$inject = ['progressArcDefaultsProvider'];

    function config(progressArcDefaultsProvider) {
        var beauBlue = '#BCD4E6';
        var bkColor = 'rgba(252, 251, 243, 0.3)';

        progressArcDefaultsProvider
            .setDefault('background', bkColor)
            .setDefault('strokeWidth', 5)
            .setDefault('stroke', beauBlue)
            .setDefault('size', 40);
    }
})();