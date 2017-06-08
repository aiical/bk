(function () {
    var signInControllerId = "app.views.signInRecord.index";
    angular.module('app').controller(
        signInControllerId,
        [
            '$scope',
            '$uibModal',
            'abp.services.app.signInRecord',
            function ($scope, $uibModal, $signInService) {
                var vm = this;
                vm.signInRecord = [];
                function getSignInRecord() {
                    $signInService.getSignInRecords({}).then(function (result) {
                        console.log(result);
                        vm.signInRecord = result.data.items;
                    });
                }

                vm.openSignInModal = function () {
                    var modalInstance = $uibModal.open({
                        templateUrl: '/App/Main/views/signIn/createModal.cshtml',
                        controller: 'app.views.signInRecord.createModal as vm',
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