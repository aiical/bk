(function () {
    angular.module('app').controller('app.views.signIn.createModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.signIn',
        function ($scope, $uibModalInstance, signInService) {
            var vm = this;

            vm.signIn = {
                isActive: true
            };

            vm.save = function () {
                signInService.createRecord(vm.signIn)
                    .then(function () {
                        abp.notify.info(App.localize('SavedSuccessfully'));
                        $uibModalInstance.close();
                    });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };
        }
    ]);
})();