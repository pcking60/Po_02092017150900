'use strict';
angular.module('postoffice.tkbd')

    .controller('TKBDHistoryController',
        ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter', '$state',
            function ($scope, apiService, notificationService, $ngBootbox, $filter, $state) {
                $scope.page = 0;
                $scope.pagesCount = 0;
                $scope.tkbds = [];
                $scope.getTkbds = getTkbds;
                $scope.keyword = '';
                $scope.search = search;
                $scope.loading = true;
                
                function search() {
                    getTkbds();
                }
                

                function getTkbds(page) {
                    page = page || 0;
                    var config = {
                        params: {
                            page: page,
                            pageSize: 20
                        }
                    }
                    apiService.get('/api/tkbd/getallhistory', config, function (result) {
                        if (result.data.TotalCount == 0) {
                            notificationService.displayWarning("Không tìm thấy bản ghi nào!");
                        }
                        $scope.loading = false;
                        $scope.tkbds = result.data.Items;
                        $scope.page = result.data.Page;
                        $scope.pagesCount = result.data.TotalPages;
                        $scope.totalCount = result.data.TotalCount;
                    },
                    function () {
                        $scope.loading = false;
                        console.log('Load list TKBD History failed');
                    });
                }
                $scope.getTkbds();
            }]);