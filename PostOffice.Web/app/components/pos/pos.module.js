/// <reference path="~/Assets/admin/libs/angular/angular.js" />

angular.module('postoffice.pos', ['postoffice.common'])

    .config(['$stateProvider', '$urlRouterProvider',function( $stateProvider, $urlRouterProvider) {

        $urlRouterProvider.otherwise('/pos');

        $stateProvider
            .state('pos', {
                url: "/pos",
                templateUrl: "/app/components/pos/poView.html",
                parent: 'base',
                controller: "poController"
            }).state('add_po', {
                url: "/add_po",
                parent: 'base',
                templateUrl: "/app/components/pos/poAddView.html",
                controller: "poAddController"
            }).state('edit_po', {
                url: "/edit_po/:id",
                templateUrl: "/app/components/pos/poEditView.html",
                parent: 'base',
                controller: "poEditController"
            });
    }])
