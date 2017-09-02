(function (app) {
    app.controller('reportsController', reportsController);

    reportsController.$inject = ['$scope', 'apiService', 'notificationService', '$filter', 'authService', '$stateParams', '$injector'];

    function reportsController($scope, apiService, notificationService, $filter, authService, $stateParams, $injector) {
      
        $scope.report = {
            functionId: 0,
            date: { startDate: null, endDate: null },
            districts: [],
            pos: [],
            users: [],
            districtId: 0,
            posId: 0,
            userId: '',
            serviceId: 0,          
        };

        $stateParams.id = 0;        
        $scope.functions =
            [
                { Id: 1, Name: 'Bảng kê thu tiền tại bưu cục - tổng hợp' },
                { Id: 2, Name: 'Bảng kê thu tiền tại bưu cục - chi tiết' },
                { Id: 3, Name: 'Bảng kê thu tiền theo nhân viên' },
                { Id: 4, Name: 'Bảng kê thu tiền theo dịch vụ' },
                { Id: 5, Name: 'Bảng kê thu tiền theo nhân viên và dịch vụ' },

            ]

       

        //lấy danh sách huyện / đơn vị
        $scope.getDistricts = getDistricts;
        function getDistricts(){
            apiService.get('/api/district/getallparents',
                null,
                function (response) {
                    $scope.report.districts = response.data;
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
                $scope.report.pos = [];
                $scope.report.posId = 0;
            }
        };

        // lấy danh sách bưu cục 
        $scope.getPos = getPos;
        function getPos() {
            apiService.get('/api/po/getbydistrictid/ ' + $stateParams.id,
                null,
                function (response) {
                    $scope.report.units = response.data;
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
                $scope.report.users = [];
                $scope.report.userId = 0;
            }
        };

        // lấy danh sách người dùng
        $scope.getListUser = getListUser;
        function getListUser() {
            apiService.get('/api/applicationUser/getuserbypoid/' + $stateParams.id,
                null,
                function (response) {
                    $scope.report.users = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách nhân viên.');
                });
        }

        // lấy danh sách bưu cục
        $scope.getPos = getPos;
        function getPos() {
            apiService.get('/api/po/getbydistrictid/' + $stateParams.id,
                null,
                function (response) {
                    $scope.report.pos = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách đơn vị.');
                }
            );
        }

        // lấy danh sách dịch vụ
        $scope.getService = getService;
        function getService() {
            apiService.get('/api/service/getallparents',
                null,
                function (response) {
                    $scope.services = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách dịch vụ.');
                });
        }

        //reset các tham số
        $scope.Reset = Reset;
        function Reset() {
            $scope.report.districtId = 0;
            $scope.report.unitId = 0;
        }

       
        $scope.Report = Report;
        function Report() {
            $scope.loading = false;
            var fromDate = $scope.report.date.startDate.format('MM/DD/YYYY');
            var toDate = $scope.report.date.endDate.format('MM/DD/YYYY');
            var config = {
                params: {
                    //mm/dd/yyyy
                    fromDate: fromDate,
                    toDate: toDate,
                    districtId: $scope.report.districtId || 0,
                    functionId: $scope.report.functionId || 0,
                    unitId: $scope.report.poId || 0,
                    userId: $scope.report.userId || '',
                    serviceId: $scope.report.serviceId || 0,
                }
            }
            apiService.get('api/statistic/rp1', config,
                function (response) {
                    $scope.loading = true;
                    if (response.status = 200) {
                        window.location.href = response.data.Message;
                    }
                },
                function (response) {
                    if (response.status == 500) {
                        notificationService.displayError('Không có dữ liệu');
                    }
                    else {
                        notificationService.displayError('Không thể tải dữ liệu');
                    }
                }
            )
        }

        //check role 
        $scope.isManager = authService.haveRole('Manager');
        $scope.isAdmin = authService.haveRole('Administrator');
        if (!$scope.isAdmin && !$scope.isManager) {
            $stateParams.id = authService.authentication.userName;
            apiService.get('/api/applicationUser/userinfo',
                null,
                function (response) {
                    $stateParams.id = response.data.Id;
                    getService();
                },
                function (response) {
                    notificationService.displayError('Không tải được danh sách dịch vụ.');
                });
        }
        else {
            if (!$scope.isAdmin) {
                $stateParams.id = authService.authentication.userName;
                apiService.get('/api/applicationUser/userinfo',
                    null,
                    function (response) {
                        $stateParams.id = response.data.POID;
                        $scope.report.districtId = response.data.POID;
                        getPos();
                    },
                    function (response) {
                        notificationService.displayError('Không tải được danh sách dịch vụ.');
                    });
            }
            else {
                getDistricts();
            }
        }

        getService();
    }

})(angular.module('postoffice.statistics'));