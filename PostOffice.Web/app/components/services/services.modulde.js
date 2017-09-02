/// <reference path="~/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('postoffice.services', ['postoffice.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('services', {
                url: "/services",
                templateUrl: "/app/components/services/serviceListView.html",
                parent: 'base',
                controller: "serviceListController"
            }).state('service_add', {
                url: "/service_add",
                templateUrl: "/app/components/services/serviceAddView.html",
                parent: 'base',
                controller: "serviceAddController"
            }).state('service_edit', {
                url: "/service_edit/:id",
                templateUrl: "/app/components/services/serviceEditView.html",
                parent: 'base',
                controller: "serviceEditController"
            });
    }
})();