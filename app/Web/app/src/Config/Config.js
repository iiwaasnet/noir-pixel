angular.module('npConfig')
    .service('Config', [
        '$q', '$http', 'Const', function($q, $http, Const) {

            var service = this,
                settings = {};

            service.getConfig = function(name) {
                var config = resolveConfig();

                //return config;
            };

            function resolveConfig() {
                var deferred = $q.defer();

                if (Object.keys(settings).length === 0) {
                    $http.get(Const.configApiUri)
                        .success(function(data) {
                            settings = buildSettings(data);
                            deferred.resolve(settings);
                        });
                } else {
                    deferred.resolve(settings);
                }

                return deferred.promise;
            }

            function buildSettings(data) {
                var tmp = {};
                data.forEach(function(el) {
                    tmp[el.name] = angular.fromJson(el.data);
                });

                return tmp;
            }
        }
    ]);