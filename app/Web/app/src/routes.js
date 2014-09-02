npApp.config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
    $routeProvider
        .when('/', {
            templateUrl: '/app/src/Home/Home.html',
            controller: 'HomeController as homeCtrl'
        })
        .when('/list', {
            templateUrl: '/app/src/List/List.html',
            controller: 'ListController as listCtrl'
        })
    .otherwise({
        redirectTo: '/'
    });

    $locationProvider.html5Mode(true);
}]);