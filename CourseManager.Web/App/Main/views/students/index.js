(function() {
    angular.module('app').controller('app.views.student.index', [
        '$scope', '$uibModal', 'abp.services.app.student',
        function ($scope, $uibModal, $studentService) {
            var vm = this;
            vm.students = [];
            function getStudents() {
                $studentService.getStudentsAsync({}).then(function (result) {
                    console.log(result);
                    vm.students = result.data.items;
                });
            }
           
            vm.openStudentCreationModal = function (id) {
                $scope.id = id;
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/students/createModal.cshtml',
                    controller: 'app.views.student.createModal as vm',
                    backdrop: 'static',
                    //,resolve: {//这是一个入参,这个很重要,它可以把主控制器中的参数传到模态框控制器中
                    //    items: function () {//items是一个回调函数
                    //        return id;//这个值会被模态框的控制器获取到
                    //    }
                    //},
                    scope: $scope
                });
                //console.log(modalInstance);
                modalInstance.result.then(function () {
                    getStudents();
                });
            };
            getStudents();
        }
    ]);
})();