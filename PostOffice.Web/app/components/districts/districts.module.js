/// <reference path="/Assets/admin/libs/angular/angular.js" />
'use strict';
angular.module('postoffice.districts', ['postoffice.common'])
    .config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) { 
    
        $stateProvider
            .state('districts', {
            url: "/districts",
            templateUrl: "/app/components/districts/districtListView.html",
            parent: 'base',
            controller: "districtListController"
            })
            .state('add_district', {
                url: "/add_district",
                parent: 'base',
                templateUrl: "/app/components/districts/districtAddView.html",
                controller: "districtAddController"
            })
            .state('edit_district', {
                url: "/edit_district/:id",
                templateUrl: "/app/components/districts/districtEditView.html",
                controller: "districtEditController",
                parent: 'base',
            });
    }])

 

