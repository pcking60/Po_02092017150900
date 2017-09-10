'use strict';
angular.module('postoffice.tkbd')

    .controller('TKBDHistoryController',
        ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter', '$state',
            function ($scope, apiService, notificationService, $ngBootbox, $filter, $state) {
                $scope.page = 0;
                $scope.pagesCount = 0;
                $scope.tkbds = [];
                
                $scope.keyword = '';
                $scope.search = search;
                $scope.loading = true;
                
                function search() {
                    getTkbds();
                }                

                $scope.getTKBD1Day = function getTKBD1Day(page) {
                    page = page || 0;
                    var config = {
                        params: {
                            page: page,
                            pageSize: 20
                        }
                    }
                    apiService.get('/api/tkbd/gettkbd1day', config, function (result) {
                        if (result.data.TotalCount == 0) {
                            notificationService.displayWarning("Không tìm thấy bản ghi nào!");
                        }
                        
                        $scope.tkbds = result.data.Items;
                        $scope.page = result.data.Page;
                        $scope.pagesCount = result.data.TotalPages;
                        $scope.totalCount = result.data.TotalCount;
                        $scope.loading = false;
                    },
                    function () {
                        $scope.loading = false;
                        console.log('Load list TKBD History failed');
                    });
                }

                $scope.getTKBD7Day = function getTKBD7Day(page) {
                    page = page || 0;
                    var config = {
                        params: {
                            page: page,
                            pageSize: 20
                        }
                    }
                    apiService.get('/api/tkbd/gettkbd7day', config, function (result) {
                        if (result.data.TotalCount == 0) {
                            notificationService.displayWarning("Không tìm thấy bản ghi nào!");
                        }

                        $scope.tkbds = result.data.Items;
                        $scope.page = result.data.Page;
                        $scope.pagesCount = result.data.TotalPages;
                        $scope.totalCount = result.data.TotalCount;
                        $scope.loading = false;
                    },
                    function () {
                        $scope.loading = false;
                        console.log('Load list TKBD History failed');
                    });
                }

                $scope.getTKBD30Day = function getTKBD30Day(page) {
                    page = page || 0;
                    var config = {
                        params: {
                            page: page,
                            pageSize: 20
                        }
                    }
                    apiService.get('/api/tkbd/gettkbd30day', config, function (result) {
                        if (result.data.TotalCount == 0) {
                            notificationService.displayWarning("Không tìm thấy bản ghi nào!");
                        }

                        $scope.tkbds = result.data.Items;
                        $scope.page = result.data.Page;
                        $scope.pagesCount = result.data.TotalPages;
                        $scope.totalCount = result.data.TotalCount;
                        $scope.loading = false;
                    },
                    function () {
                        $scope.loading = false;
                        console.log('Load list TKBD History failed');
                    });
                }

            }]);