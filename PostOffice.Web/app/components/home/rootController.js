(function (app) {
    app.controller('rootController', rootController);

    rootController.$inject = ['$state', '$scope', 'authService', 'apiService'];

    function rootController($state, $scope, authService, apiService) {

        $scope.logOut = function () {
            authService.logOut();
            $state.go('login');
        }
        $scope.authentication = authService.authentication;
        var userName = $scope.authentication.userName;

        if ($scope.authentication.isAuth)
        {
            $scope.isAdmin = authService.haveRole('Administrator');
            $scope.isManager = authService.haveRole('Manager');
            $scope.isCounter = authService.haveRole('Counter');
            
            getUserInfo();
        }
        
        function getUserInfo() {
            apiService.get('/api/applicationUser/getuserinfo/' + userName, null,
                function (result) {
                    $scope.userInfo = result.data;
                },
                function () {
                    console.log('Can not load user info');
                });
        }


        $scope.sideBarBaseView = 'app/shared/views/sideBarBaseView.html';
        
      
        //authenticationService.validateRequest();
    }
})(angular.module('postoffice'));