(function () {
    'use strict';

    var app = angular.module('app', [
        'ngAnimate',
        'ngSanitize',

        'ui.router',
        'ui.bootstrap',
        'ui.jq',

        'abp'
    ]);

    //Configuration for Angular UI routing.
    app.config([
        '$stateProvider', '$urlRouterProvider', '$locationProvider', '$qProvider',
        function ($stateProvider, $urlRouterProvider, $locationProvider, $qProvider) {
            $locationProvider.hashPrefix('');
            $urlRouterProvider.otherwise('/');
            $qProvider.errorOnUnhandledRejections(false);

            if (abp.auth.hasPermission('Pages.Users')) {
                $stateProvider
                    .state('users', {
                        url: '/users',
                        templateUrl: '/App/Main/views/users/index.cshtml',
                        menu: 'Users' //Matches to name of 'Users' menu in CourseManagerNavigationProvider
                    });
                $urlRouterProvider.otherwise('/users');
            }

            //if (abp.auth.hasPermission('Pages.Tenants')) {
            //    $stateProvider
            //        .state('tenants', {
            //            url: '/tenants',
            //            templateUrl: '/App/Main/views/tenants/index.cshtml',
            //            menu: 'Tenants' //Matches to name of 'Tenants' menu in CourseManagerNavigationProvider
            //        });
            //    $urlRouterProvider.otherwise('/tenants');
            //}
            //配置路由
            $stateProvider
                .state('home', {
                    url: '/',
                    templateUrl: '/App/Main/views/home/home.cshtml',
                    menu: 'Home' //Matches to name of 'Home' menu in CourseManagerNavigationProvider
                })
                .state('signIn', {
                    url: '/signIn',
                    templateUrl: '/App/Main/views/signIn/index.cshtml',
                    menu: 'SignIn'
                })
                .state('absentCheckIn', {
                    url: '/absentCheckIn',
                    templateUrl: '/App/Main/views/signIn/index.cshtml',
                    menu: 'AbsentCheckIn'
                });
                //.state('about', {
                //    url: '/about',
                //    templateUrl: '/App/Main/views/about/about.cshtml',
                //    menu: 'About' //Matches to name of 'About' menu in CourseManagerNavigationProvider
                //});
        }
    ]);
})();