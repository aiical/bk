(function () {
    angular.module('app').controller('app.views.student.createModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.student',
        function ($scope, $uibModalInstance, $studentService) {
            var vm = this;
            vm.student = {
                isActive: true
            };
            vm.save = function () {
                $studentService.createStudent(vm.student)
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