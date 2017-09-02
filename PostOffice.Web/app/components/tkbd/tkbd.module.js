/// <reference path="/Assets/admin/libs/angular/angular.js" />
'use strict';
angular.module('postoffice.tkbd', ['postoffice.common'])
    .config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('tkbds', {
                url: "/tkbd",
                templateUrl: "/app/components/tkbd/tkbdListView.html",
                parent: 'base',
                controller: "tkbdListController"
            }).state('tkbd_import', {
                url: "/tkbd_import",
                templateUrl: "/app/components/tkbd/tkbdImportListView.html",
                parent: 'base',
                controller: "tkbdImportListController"
            }).state('tkbdhistory', {
                url: "/tkbdhistory",
                templateUrl: "/app/components/tkbd/TKBDHistory.html",
                parent: 'base',
                controller: "TKBDHistoryController"
            });
    }])