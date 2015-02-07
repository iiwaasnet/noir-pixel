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
                Wall: {
                    Name: 'userHome.profile.wall'
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
        },
        Errors: {
            NotFound: {
                Name: '404'
            }
        }
    };

    angular.module('np.const')
        .constant('States', states);
})();