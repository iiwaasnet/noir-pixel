(function() {
    'use strict';

    var events = {
        Auth: {
            SignedIn: 'SignedIn',
            SignedOut: 'SignedOut'
        },
        Profile: {
            Updated: 'Updated'
        }
    };

    angular.module('np.events')
        .constant('Events', events);
})();