(function() {
    angular.module('app').controller('app.views.users.index', [
        '$scope', '$uibModal', 'abp.services.app.user',
        function ($scope, $uibModal, studentService) {
            var vm = this;

            vm.users = [];

            function getStudents() {
                studentService.getStudents({}).then(function (result) {
                    vm.users = result.data.items;
                });
            }

            vm.openUserCreationModal = function() {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/Main/views/users/createModal.cshtml',
                    controller: 'app.views.users.createModal as vm',
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