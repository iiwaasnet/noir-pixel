(function() {
    'use strict';

    var events = {
        Auth: {
            SignedIn: 'SignedIn',
            SignedOut: 'SignedOut'
        }
    };

    angular.module('np.events')
        .constant('Events', events);
})();