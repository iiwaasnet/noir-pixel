(function() {
    'use strict';

    angular.module('np.view-resolver')
        .service('ViewResolver', viewResolverService);

    viewResolverService.$inject = ['$templateFactory', 'Roles', 'User'];

    function viewResolverService($templateFactory, Roles, User) {
        var srv = this;
        srv.resolveTemplateUrl = resolveTemplateUrl;
        srv.resolveController = resolveController;

        function resolveTemplateUrl(state, stateParams) {
            switch (state) {
            case 'user':
                return $templateFactory.fromUrl('/app/src/user-home/user-home.html', stateParams);
            case 'user.profile':
                return $templateFactory.fromUrl('/app/src/user-home/profile/profile.html', stateParams);
            case 'user.photos':
                return $templateFactory.fromUrl('/app/src/user-home/photos/photos.html', stateParams);
            default:
                return $templateFactory.fromUrl('/app/src/errors/404.html', stateParams);
            }
        }

        function resolveController(state, stateParams) {
            switch (state) {
            case 'user':
                return 'UserHomeController';
            case 'user.profile':
                return 'ProfileController';
            case 'user.photos':
                return '';
            default:
                return '';
            }
        }
    }
})();