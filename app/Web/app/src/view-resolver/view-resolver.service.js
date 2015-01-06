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
            case 'userHome':
                return $templateFactory.fromUrl('/app/src/user-home/user-home.html', stateParams);
            case 'userHome.profile':
                return $templateFactory.fromUrl('/app/src/user-home/profile/profile.html', stateParams);
            case 'userPublic.photos':
                return $templateFactory.fromUrl('/app/src/user-home/photos/photos.html', stateParams);
            default:
                return $templateFactory.fromUrl('/app/src/errors/404.html', stateParams);
            }
        }

        function resolveController(state, stateParams) {
            switch (state) {
            case 'userHome':
                return 'UserHomeController';
            case 'userHome.profile':
                return 'ProfileController';
            case 'userPublic.photos':
                return '';
            default:
                return '';
            }
        }
    }
})();