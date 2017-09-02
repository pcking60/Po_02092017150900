(function (app) {
    app.controller('timeStatisticController', timeStatisticController);

    timeStatisticController.$inject = ['$scope', 'apiService', 'notificationService', '$filter', 'authService', '$stateParams'];

    function timeStatisticController($scope, apiService, notificationService, $filter, authService, $stateParams) {
        $scope.statisticResult = [];
        $scope.report = {
            date: { startDate: moment(), endDate: moment() },
            districts: [],
            pos: [],
            users: [],  
            districtId: 0,
            posId: 0,
            userId: '',
            serviceId: 0,
            totalQuantity: 0,
            totalCash: 0,
            totalDebt: 0,
            totalEarn: 0,
            totalVat: 0
        };
       
        // lấy danh sách đơn vị / huyện
        $scope.getDistricts = getDistricts;
        function getDistricts() {
            apiService.get('/api/district/getallparents',
                null,
                function (response) {
                    $scope.report.districts = response.data;
                },
                function (error) {
                    notificationService.displayError('Không lấy được danh sách đơn vị / huyện')
                });
        }
        // lấy danh sách bưu cục của đơn vị được chọn
        $scope.updatePos = function (item) {
            if(item!=0 && item!=null){
                $stateParams.id = item;                
                getPos();
            }
            else {
                $scope.report.pos = [];                
                $scope.report.posId = 0;
            }            
        };
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
        // lấy danh sách dịch vụ user đã thực hiện
        $scope.updateService = function (item) {
            if (item != 0 && item != null) {
                $stateParams.id = item;
                getService();
            }
            else {
                $scope.report.services = [];
                $scope.report.serviceId = 0;
            }
        };
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

        // lấy danh sách người dùng
        $scope.getListUser = getListUser;
        function getListUser() {
            apiService.get('/api/applicationUser/getuserbypoid/' +$stateParams.id,
                null,
                function (response) {
                    $scope.report.users = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách nhân viên.');
                });
        }
        // lấy danh sách dịch vụ
        $scope.getService = getService;
        function getService() {
            apiService.get('/api/service/getallbyuserid/' +$stateParams.id,
                null,
                function (response) {
                    $scope.services = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách dịch vụ.');
                });
        }
       
        $scope.chartdata = [];
        $scope.TimeStatistic = TimeStatistic;
        function TimeStatistic() {
            var fromDate = $scope.report.date.startDate.format('MM-DD-YYYY');
            var toDate = $scope.report.date.endDate.format('MM-DD-YYYY');
            var config = {
                params: {
                    //mm/dd/yyyy
                    fromDate: fromDate,
                    toDate: toDate,
                    districtId: $scope.report.districtId || 0,
                    posId: $scope.report.posId || 0,
                    userId: $scope.report.userId || '',
                    serviceId: $scope.report.serviceId || 0                    
                }
            }
            apiService.get('api/transactions/stattistic', config,
                function (response) {
                    $scope.statisticResult = response.data;
                    $scope.report.totalQuantity = 0;
                    $scope.report.totalCash = 0;
                    $scope.report.totalDebt = 0;
                    $scope.report.totalEarn = 0;
                    angular.forEach($scope.statisticResult, function (item) {
                        if (item.Status == true) {
                            $scope.report.totalQuantity += item.Quantity;
                            $scope.report.totalCash += item.TotalCash;
                            $scope.report.totalDebt += item.TotalDebt;
                            $scope.report.totalEarn += item.EarnMoney;
                            $scope.report.totalVat += ((item.TotalMoney + item.TotalCash + item.TotalDebt) * item.VAT / 100);
                        }                        
                    })
                    $scope.result = true;
                    console.log(response.data);                    
                },
                function (response) {
                    if (response.status == 500) {
                        notificationService.displayError('Không có dữ liệu');
                    }
                    else {
                        notificationService.displayError('Không thể tải dữ liệu');
                    }
                });
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
        else
        {
            if (!$scope.isAdmin) {
                $stateParams.id = authService.authentication.userName;
                apiService.get('/api/applicationUser/userinfo',
                    null,
                    function (response) {
                        $stateParams.id = response.data.POID;
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
    }

})(angular.module('postoffice.statistics'));