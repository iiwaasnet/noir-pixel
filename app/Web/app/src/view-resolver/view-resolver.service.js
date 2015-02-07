(function() {
    'use strict';

    angular.module('np.view-resolver')
        .service('ViewResolver', viewResolverService);

    viewResolverService.$inject = ['$templateFactory', 'States', 'Roles', 'User'];

    function viewResolverService($templateFactory, States, Roles, User) {
        var srv = this;
        srv.resolveTemplateUrl = resolveTemplateUrl;
        srv.resolveController = resolveController;

        function resolveTemplateUrl(state, stateParams) {
            var isSelf = User.isSelf(stateParams.userName);

            if (state === States.UserHome.Name) {
                return $templateFactory.fromUrl('/app/src/user-home/user-home.html', stateParams);
            }
            if (state === States.UserHome.Profile.Name && isSelf) {
                return $templateFactory.fromUrl('/app/src/user-home/profile/profile.html', stateParams);
            }
            if (state === States.UserHome.Profile.Public.Name && isSelf) {
                return $templateFactory.fromUrl('/app/src/user-home/profile/profile-public.html', stateParams);
            }
            if (state === States.UserHome.Profile.Wall.Name && isSelf) {
                return $templateFactory.fromUrl('/app/src/user-home/profile/profile-wall.html', stateParams);
            }
            if (state === States.UserHome.Profile.Private.Name && isSelf) {
                return $templateFactory.fromUrl('/app/src/user-home/profile/profile-private.html', stateParams);
            }
            if (state === States.UserPublic.Photos.Name) {
                return $templateFactory.fromUrl('/app/src/user-home/photos/photos.html', stateParams);
            }
            return $templateFactory.fromUrl('/app/src/errors/404.html', stateParams);

        }

        function resolveController(state, stateParams) {
            var isSelf = User.isSelf(stateParams.userName);

            if (state === States.UserHome.Name) {
                return 'UserHomeController';
            }
            if (state === States.UserHome.Profile.Name && isSelf) {
                return 'ProfileController';
            }
            if (state === States.UserHome.Profile.Public.Name && isSelf) {
                return 'ProfilePublicController';
            }
            if (state === States.UserHome.Profile.Wall.Name && isSelf) {
                return 'ProfileWallController';
            }
            if (state === States.UserHome.Profile.Private.Name && isSelf) {
                return 'ProfilePrivateController';
            }
            return '';
        }
    }
})();