/// <reference path="~/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('postoffice.service_groups', ['postoffice.common']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $urlRouterProvider.otherwise('/service_groups');

        $stateProvider
            
            .state('service_groups', {
                url: "/service_groups",
                parent: 'base',
                templateUrl: "/app/components/service_groups/serviceGroupsListView.html",
                controller: "serviceGroupsListController"
            }).state('add_service_groups', {
                url: "/add_service_groups",
                parent: 'base',
                templateUrl: "/app/components/service_groups/serviceGroupAddView.html",
                controller:"serviceGroupAddController"
            }).state('edit_service_groups', {
                url: "/edit_service_groups/:id",
                parent: 'base',
                templateUrl: "/app/components/service_groups/serviceGroupEditView.html",
                controller: "serviceGroupEditController"
            });
    };
})();