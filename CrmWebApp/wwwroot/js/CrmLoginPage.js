(function () {
    var app = angular.module('App', ['ui.router']);

    app.run(function ($rootScope, $location, $state) {
        if (loggedInUser == null) {
            $state.transitionTo('login');
        }
    });

    app.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        $urlRouterProvider.otherwise('/home');

        $stateProvider
            .state('login', {
                url: '/login',
                templateUrl: 'login.html',
                controller: 'LoginController'
            })
            .state('home', {
                url: '/home',
                templateUrl: 'home.html',
                controller: 'HomeController'
            });
    }]);

    app.controller('LoginController', function ($scope, $rootScope, $stateParams, $state) {
        loggedInUser == null;
        $('#login-door').toggle(true);

        if (error != null)
            $scope.error = error;
    });

    app.controller('HomeController', function ($scope, $rootScope, $stateParams, $state) {
        $('#login-door').toggle(loggedInUser == null);
        $scope.fullName = loggedInUser.fullName;
    });
})();