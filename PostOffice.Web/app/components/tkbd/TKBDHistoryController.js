'use strict';
angular.module('postoffice.tkbd')

    .controller('TKBDHistoryController',
        ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter', '$state','authService',
            function ($scope, apiService, notificationService, $ngBootbox, $filter, $state, authService) {
                $scope.page = 0;
                $scope.pagesCount = 0;
                $scope.tkbds = [];
                $scope.show = false;
                $scope.keyword = '';
                $scope.search = search;
                $scope.loading = true;
                
                //check role 
                $scope.isManager = authService.haveRole('Manager');
                $scope.isAdmin = authService.haveRole('Administrator');

                if ($scope.isManager) {
                    $stateParams.id = authService.authentication.userName;
                    apiService.get('/api/applicationUser/userinfo',
                        null,
                        function (response) {
                            $stateParams.id = response.data.POID;
                            $scope.tkbd.districtId = response.data.POID;
                            getPos();
                        },
                        function (response) {
                            notificationService.displayError('Không tải được danh sách huyện/ thành phố.');
                        });
                }
                else {
                    if ($scope.isAdmin) {
                        getDistricts();
                    }
                }
                //lấy danh sách huyện / đơn vị
                $scope.getDistricts = getDistricts;
                function getDistricts() {
                    apiService.get('/api/district/getallparents',
                        null,
                        function (response) {
                            $scope.tkbd.districts = response.data;
                        }, function (response) {
                            notificationService.displayError('Không tải được danh sách huyện.');
                        }
                    );
                }

                // câp nhật danh sách bưu cục của đơn vị được chọn
                $scope.updatePos = function (item) {
                    if (item != 0 && item != null) {
                        $stateParams.id = item;
                        getPos();
                    }
                    else {
                        $scope.tkbd.pos = [];
                        $scope.tkbd.posId = 0;
                    }
                };

                // lấy danh sách bưu cục 
                $scope.getPos = getPos;
                function getPos() {
                    apiService.get('/api/po/getbydistrictid/ ' + $stateParams.id,
                        null,
                        function (response) {
                            $scope.tkbd.pos = response.data;
                        }, function (response) {
                            notificationService.displayError('Không tải được danh sách đơn vị.');
                        }
                    );
                }

                // lấy danh sách users của bưu cục được chọn
                $scope.updateUser = function (item) {
                    if (item != 0 && item != null) {
                        $stateParams.id = item;
                        getListUser();
                    }
                    else {
                        $scope.tkbd.users = [];
                        $scope.tkbd.userId = 0;
                    }
                };

                // lấy danh sách người dùng
                $scope.getListUser = getListUser;
                function getListUser() {
                    apiService.get('/api/applicationUser/getuserbypoid/' + $stateParams.id,
                        null,
                        function (response) {
                            $scope.tkbd.users = response.data;
                        }, function (response) {
                            notificationService.displayError('Không tải được danh sách nhân viên.');
                        });
                }


                function search() {
                    getTkbds();
                }                

                $scope.getTkbds = getTkbds;
                function getTkbds() {
                    var fromDate = $scope.tkbd.date.startDate.format('MM-DD-YYYY');
                    var toDate = $scope.tkbd.date.endDate.format('MM-DD-YYYY');
                    var config = {
                        params: {
                            //mm/dd/yyyy
                            fromDate: fromDate,
                            toDate: toDate,
                            districtId: $scope.tkbd.districtId || 0,
                            posId: $scope.tkbd.posId || 0,
                            userId: $scope.tkbd.userId || '',
                            serviceId: $scope.tkbd.serviceId || 0
                        }
                    }
                    apiService.get('api/transactions/stattistic', config,
                        function (result) {
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