(function() {
    'use strict';

    angular.module('np.photos')
        .service('Photos', photosService);

    photosService.$inject = ['$http', '$q', 'Config', 'Url'];

    function photosService($http, $q, Config, Url) {
        var srv = this;
        srv.getPendingPhotos = getPendingPhotos;

        function getPendingPhotos(offset, count) {
            var url = Url.buildApiUrl(Config.ApiUris.Photos.GetPending);

            return $http.get(url, {
                params: {
                    offset: offset,
                    count: count
                }
            });
        }
    }
})();