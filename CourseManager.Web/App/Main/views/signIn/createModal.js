(function () {
    angular.module('app').controller('app.views.signIn.createModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.signIn',
        function ($scope, $uibModalInstance, $signInService) {
            var vm = this;

            vm.signIn = {
                isActive: true
            };

            vm.save = function () {
                abp.ui.setBusy();
                $signInService.createSignIn(vm.signIn)
                    .then(function () {
                        abp.notify.info(App.localize('SavedSuccessfully'));
                        $uibModalInstance.close();
                    }).finally(function () {
                        abp.ui.clearBusy();
                    });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss({});
            };
        }
    ]);
})();