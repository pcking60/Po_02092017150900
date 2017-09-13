﻿/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('postoffice.application_users', ['postoffice.common', 'ngMaterial', 'ngMessages']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('application_users', {
            url: "/application_users",
            templateUrl: "/app/components/application_users/applicationUserListView.html",
            parent: 'base',
            controller: "applicationUserListController"
        })
        .state('add_application_user', {
            url: "/add_application_user",
            parent: 'base',
            templateUrl: "/app/components/application_users/applicationUserAddView.html",
            controller: "applicationUserAddController"
        })
        .state('user_profile', {
            url: "/user_profile",
            parent: 'base',
            templateUrl: "/app/components/application_users/userProfileView.html",
            controller: "userProfileViewController"
        })
        .state('changePassword', {
            url: "/change-password",
            parent: 'base',
            templateUrl: "/app/components/application_users/changePassword.html",
            controller: "changePasswordController"
        })
        .state('edit_application_user', {
            url: "/edit_application_user/:id",
            templateUrl: "/app/components/application_users/applicationUserEditView.html",
            controller: "applicationUserEditController",
            parent: 'base'
        });
    }
})();