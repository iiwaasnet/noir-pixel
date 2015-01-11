(function() {
    'use strict';

    var states = {
        Home: {
            Name: 'home'
        },
        UserHome: {
            Name: 'userHome',
            Profile: {
                Name: 'userHome.profile',
                Public: {
                    Name: 'userHome.profile.public'
                },
                Private: {
                    Name: 'userHome.profile.private'
                }
            }
        },
        ExternalSignIn: {
            Name: 'externalSignIn'
        },
        ExternalRegister: {
            Name: 'externalRegister'
        },
        NotAuthorized: {
            Name: 'notAuthorized'
        }
    };

    angular.module('np.const')
        .constant('States', states);
})();