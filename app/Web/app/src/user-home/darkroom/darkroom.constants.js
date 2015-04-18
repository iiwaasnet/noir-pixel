(function() {
    'use strict';

    var modes  = {
        Edit: 'Edit',
        AddToPhotos: 'AddToPhotos',
        Delete: 'Delete',
        Stack: 'Stack'
    }

    angular.module('np.user-home')
        .constant('DarkroomModes', modes);
})();