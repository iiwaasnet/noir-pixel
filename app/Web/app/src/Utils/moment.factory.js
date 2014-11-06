(function() {
    'use strict';

    angular.module('np.utils')
        .constant('moment', moment)
        .factory('Moment', momentFactory);

    momentFactory.$inject = ['moment'];

    function momentFactory(moment) {
        return moment;
    }
})();