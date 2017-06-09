(function () {
    angular.module('app').controller('app.views.signInRecord.createModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.signInRecord',
        function ($scope, $uibModalInstance, $signInService) {
            var vm = this;

            vm.signInRecord = {
                isActive: true,
                type: [
                    { "CategoryName": "准时上课", "Id": "305ab34ea2674ed4a1c9dbcc7265d2e9" },//["迟到", "正常", "未上课"]
                    { "CategoryName": "迟到", "Id": "4a7f3fde896544a68756762eebaa12e2" },
                    { "CategoryName": "未上课", "Id": "21ecd0cb5cda4149b3dca7e04e1611c3" }
                ],
                unNormalType: [
                    { "CategoryName": "", "Id": "-1" },
                    { "CategoryName": "学生请假", "Id": "c9749429307348e08f1dc8aba035eaed" },
                    { "CategoryName": "老师请假", "Id": "fcf2b5489d0b40219f3dc038a59c1709" },
                    { "CategoryName": "其他原因", "Id": "97ccd334df4748c7975e465d72ce50fb" }
                ]
            };
            //  $scope.selectedType = '305ab34ea2674ed4a1c9dbcc7265d2e9';//id的值，区分类型
            $scope.selectedType = vm.signInRecord.type[0];//如果想要第一个值
            $scope.selectedUnNormalType = vm.signInRecord.unNormalType[0];
            //console.log(vm.signInRecord.type);
            vm.save = function () {
                abp.ui.setBusy();
                vm.signInRecord.type = $scope.selectedType.Id;
                vm.signInRecord.unNormalType = $scope.selectedUnNormalType.Id;
                console.log(vm.signInRecord);
                $signInService.createSignInRecord(vm.signInRecord)
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