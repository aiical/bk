(function () {
    angular.module('app').controller('app.views.student.createModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.student',
        function ($scope, $uibModalInstance, $studentService) {
            var vm = this;
            vm.student = {
                isActive: true
            };
            if (typeof $scope.id != 'undefined')
                getStudent($scope.id);

            function getStudent(id) {
                $studentService.getStudent({ "Id": $scope.id })
                    .then(function (result) {
                        console.log(result);
                        vm.student = result.data;
                    });
            }
            vm.save = function () {
                console.log(vm.student);
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