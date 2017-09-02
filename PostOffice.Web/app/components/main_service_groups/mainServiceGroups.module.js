/// <reference path="~/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('postoffice.main_service_groups', ['postoffice.common']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $urlRouterProvider.otherwise('/main_service_groups');

        $stateProvider
            
            .state('main_service_groups', {
                url: "/main_service_groups",
                parent: 'base',
                templateUrl: "/app/components/main_service_groups/mainServiceGroupsListView.html",
                controller: "mainServiceGroupsListController"
            }).state('add_main_service_groups', {
                url: "/add_main_service_groups",
                parent: 'base',
                templateUrl: "/app/components/main_service_groups/mainServiceGroupAddView.html",
                controller:"mainServiceGroupAddController"
            }).state('edit_main_service_groups', {
                url: "/edit_main_service_groups/:id",
                parent: 'base',
                templateUrl: "/app/components/main_service_groups/mainServiceGroupEditView.html",
                controller: "mainServiceGroupEditController"
            });
    };
})();