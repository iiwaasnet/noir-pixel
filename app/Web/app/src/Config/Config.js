angular.module('npConfig')
    .service('Config', [
        '$q', '$http', 'Const', function($q, $http, Const) {

            var service = this,
                settings = {};

            service.getConfig = function(name) {
                var config = resolveConfig()[name];

                return config;
            };

            function resolveConfig() {
                var deferred = $q.defer();

                if (Object.keys(settings).length === 0) {
                    $http.get(Const.configApiUri)
                        .success(function(data) {
                            deferred.resolve(data);
                        });
                } else {
                    deferred.resolve(settings);
                }

                return deferred.promise;
            }
        }
    ]);