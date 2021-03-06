﻿(function() {
    'use strict';

    angular.module('np.photos')
        .service('Photos', photosService);

    photosService.$inject = ['$http', '$q', 'Config', 'Url'];

    function photosService($http, $q, Config, Url) {
        var srv = this;
        srv.getPendingPhotos = getPendingPhotos;
        srv.getPhotoForEdit = getPhotoForEdit;
        srv.getPhotoGenres = getPhotoGenres;
        srv.updatePhoto = updatePhoto;
        srv.deletePhoto = deletePhoto;

        function deletePhoto(shortId) {
            var url = Url.buildApiUrl(Config.ApiUris.Photos.DeletePhoto).formatNamed({ shortId: shortId });

            return $http.delete(url);
        }

        function updatePhoto(shortId, description) {
            var url = Url.buildApiUrl(Config.ApiUris.Photos.UpdatePhoto).formatNamed({ shortId: shortId });

            return $http.post(url, description);
        }

        function getPhotoGenres() {
            var url = Url.buildApiUrl(Config.ApiUris.Photos.GetPhotoGenres);

            return $http.get(url);
        }

        function getPendingPhotos(offset, count) {
            var url = Url.buildApiUrl(Config.ApiUris.Photos.GetPending);

            return $http.get(url, {
                params: {
                    offset: offset,
                    count: count
                }
            });
        }

        function getPhotoForEdit(shortId) {
            var url = Url.buildApiUrl(Config.ApiUris.Photos.GetPhotoForEdit).formatNamed({ shortId: shortId });

            return $http.get(url);
        }
    }
})();