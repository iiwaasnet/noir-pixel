(function() {
    'use strict';

    angular.module('np.geo')
        .service('Geo', geoService);

    geoService.$inject = ['$http', 'Config', 'Url'];

    function geoService($http, Config, Url) {
        var srv = this;
        srv.getCountries = getCountries;

        function getCountries() {
            var url = Url.buildApiUrl(Config.ApiUris.Geo.Countries);
            return $http.get(url);
        }
    }
})();