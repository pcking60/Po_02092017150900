(function (app) {
    app.controller('rootController', rootController);

    rootController.$inject = ['$state', '$scope', 'authService', 'apiService'];

    function rootController($state, $scope, authService, apiService) {
        $scope.logOut = function () {
            authService.logOut();
            $state.go('login');
        }
        $scope.authentication = authService.authentication;
        if ($scope.authentication.isAuth)
        {
            $scope.isAdmin = authService.haveRole('Administrator');
            $scope.isManager = authService.haveRole('Manager');           
        }
        
        $scope.sideBarBaseView = 'app/shared/views/sideBarBaseView.html';
        
      
        //authenticationService.validateRequest();
    }
})(angular.module('postoffice'));