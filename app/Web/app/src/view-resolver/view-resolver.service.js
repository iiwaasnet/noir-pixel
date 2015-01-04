(function() {
    'use strict';

    angular.module('np.view-resolver')
        .service('ViewResolver', viewResolverService);

    viewResolverService.$inject = ['Roles'];

    function viewResolverService(Roles) {
        var srv = this;
        srv.resolveTemplateUrl = resolveTemplateUrl;
        srv.resolveController = resolveController;

        function resolveTemplateUrl(state) {
        }

        function resolveController(state) {
        }
    }
})();