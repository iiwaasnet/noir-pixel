(function() {
    'use strict';

    var modes  = {
        Edit: 'Edit',
        AddToPortfolio: 'AddToPortfolio',
        Delete: 'Delete',
        Stack: 'Stack'
    }

    angular.module('np.user-home')
        .constant('DarkroomModes', modes);
})();