(function() {
    angular.module('app').controller('app.views.student.index', [
        '$scope', '$uibModal', 'abp.services.app.student',
        function ($scope, $uibModal, $studentService) {
            var vm = this;

            vm.students = [];

            function getStudents() {
                $studentService.getStudents({}).then(function (result) {
                    vm.students = result.data.items;
                });
            }

            vm.openStudentCreationModal = function() {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/students/createModal.cshtml',
                    controller: 'app.views.student.createModal as vm',
                    backdrop: 'static'
                });

                modalInstance.result.then(function () {
                    getStudents();
                });
            };

            getStudents();
        }
    ]);
})();