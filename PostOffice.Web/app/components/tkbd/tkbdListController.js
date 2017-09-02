'use strict';
angular.module('postoffice.tkbd')

    .controller('tkbdListController',
        ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter', '$state',
            function ($scope, apiService, notificationService, $ngBootbox, $filter, $state) {
                $scope.page = 0;
                $scope.pagesCount = 0;
                $scope.tkbds = [];
                $scope.getTkbds = getTkbds;
                $scope.keyword = '';
                $scope.search = search;
                $scope.loading = true;
                $scope.update = update;
                //function deleteMulti() {
                //    var listId = [];
                //    $.each($scope.selected, function (i, item) {
                //        listId.push(item.ID);
                //    });
                //    var config = {
                //        params: {
                //            checkedDistricts: JSON.stringify(listId)
                //        }
                //    }
                //    $ngBootbox.confirm('Bạn có chắc xóa không?').then(
                //        function () {
                //            apiService.del('/api/district/deletemulti', config, function (result) {
                //                notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi.');
                //                search();
                //            }, function (error) {
                //                notificationService.displayError('Xóa không thành công');
                //            });
                //        }, function () {
                //            console.log('Command was cancel');
                //        });

                //}
                //$scope.isAll = false;
                //function selectAll() {
                //    if ($scope.isAll === false) {
                //        angular.forEach($scope.districts, function (item) {
                //            item.checked = true;
                //        });
                //        $scope.isAll = true;
                //    } else {
                //        angular.forEach($scope.districts, function (item) {
                //            item.checked = false;
                //        });
                //        $scope.isAll = false;
                //    }
                //}

                //$scope.$watch("districts", function (n, o) {
                //    var checked = $filter("filter")(n, { checked: true });
                //    if (checked.length) {
                //        $scope.selected = checked;
                //        $('#btnDelete').removeAttr('disabled');
                //    } else {
                //        $('#btnDelete').attr('disabled', 'disabled');
                //    }
                //}, true);
                function search() {
                    getTkbds();
                }
                //$scope.deleteDistrict = deleteDistrict;
                //function deleteDistrict(id) {
                //    $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
                //        var config = {
                //            params: {
                //                id: id
                //            }
                //        }
                //        apiService.del('/api/district/delete', config,
                //        function () {
                //            notificationService.displaySuccess('Xóa dữ liệu thành công');
                //            search();
                //        }
                //        , function () {
                //            notificationService.displayError('Xóa dữ liệu thất bại');
                //        });
                //    },
                //    function () {
                //        console.log('Command was cancel');
                //    });
                //}

                function update() {
                    apiService.get('/api/tkbd/update', null, function (result) {
                        console.log(result.data.TotalCount);
                        $scope.loading = false;
                        $scope.tkbds = result.data.Items;
                        $scope.page = result.data.Page;
                        $scope.pagesCount = result.data.TotalPages;
                        $scope.totalCount = result.data.TotalCount;
                        $state.reload();
                    },
                    function () {
                        $scope.loading = false;
                        console.log('Load tkbds failed');
                    });
                }

                function getTkbds(page) {
                    page = page || 0;
                    var config = {
                        params: {
                            page: page,
                            pageSize: 20
                        }
                    }
                    apiService.get('/api/tkbd/getall', config, function (result) {
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
                        console.log('Load list TKBD failed');
                    });
                }
                $scope.getTkbds();
            }]);