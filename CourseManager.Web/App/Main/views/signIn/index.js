(function () {
    var signInControllerId = "app.views.signIn.index";
    angular.module('app').controller(
        signInControllerId,
        [
            '$scope',
            '$uibModal',
            'abp.services.app.signIn',
            function ($scope, $uibModal, $signInService) {
                console.log(this);
                var vm = this;
                vm.signIn = [];
                function getSignInRecord() {
                    $signInService.getSignInRecord({}).then(function (result) {
                        console.log(result);
                        vm.signIn = result.data.items;
                    });
                }

                vm.openSignInModal = function () {
                    alert(1);
                    var modalInstance = $uibModal.open({
                        templateUrl: '/App/Main/views/signIn/createModal.cshtml',
                        controller: 'app.views.signIn.createModal as vm',
                        backdrop: 'static'
                    });

                    modalInstance.result.then(function () {
                        getSignInRecord();
                    });
                };

                getSignInRecord();
            }
        ]
    );
})();