
(function () {
    angular.module('postoffice.statistics', ['postoffice.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('statistic_revenue', {
                url: "/statistic_revenue",
                parent: 'base',
                templateUrl: "/app/components/statistic/revenueStatisticView.html",
                controller: "revenueStatisticController"
            })
            .state('timeStatistic', {
                url: "/timeStatistic",
                parent: 'base',
                templateUrl: "/app/components/statistic/timeStatisticView.html",
                controller: "timeStatisticController"
            })
            .state('reports', {
                url: "/reports",
                parent: "base",
                templateUrl: "app/components/statistic/reportsView.html",
                controller: "reportsController"
            })
        ;
            
    }
})();