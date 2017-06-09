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
                courseType: [
                    { "CategoryName": "入门上", "Id": "0a5ac3cdba364c7dacd4424708db329e" },
                    { "CategoryName": "入门下", "Id": "25df80ade53e4f158e63ef9f0208a259" },
                    { "CategoryName": "基础", "Id": "48f99248288748ff851952171e28201c" },
                    { "CategoryName": "提高", "Id": "cf922c690f91467daef62b6921da26f3" },
                    { "CategoryName": "TSC", "Id": "18552c6ae6884ea3bd5a373d0aa89ba3" },
                ],
                classType: [
                    { "CategoryName": "1V1", "Id": "7be04a7f2bdb49f5a2dfd57e29a28550" },
                    { "CategoryName": "班级课", "Id": "89e3d2b7d0df4091a21622f592713dc4" },
                ],
                unNormalType: [
                    { "CategoryName": "", "Id": "-1" },
                    { "CategoryName": "学生请假", "Id": "4775c43210b14f84a722737b6895b230" },
                    { "CategoryName": "老师请假", "Id": "c228ab60960446fe9c8e7b8c3ca5e0b1" },
                    { "CategoryName": "其他原因", "Id": "47f65adef437459a8fe41f4c21bc9bce" }
                ]
            };
            //  $scope.selectedType = '305ab34ea2674ed4a1c9dbcc7265d2e9';//id的值，区分类型
            $scope.selectedType = vm.signInRecord.type[0];//如果想要第一个值
            $scope.selectedClassType = vm.signInRecord.classType[0]
            $scope.selectedCourseType = vm.signInRecord.courseType[0];
            $scope.selectedUnNormalType = vm.signInRecord.unNormalType[0];
            //console.log(vm.signInRecord.type);
            vm.save = function () {
                abp.ui.setBusy();
                vm.signInRecord.type = $scope.selectedType.Id;
                vm.signInRecord.classType = $scope.selectedClassType.Id;
                vm.signInRecord.unNormalType = $scope.selectedUnNormalType.Id;
                vm.signInRecord.courseType = $scope.selectedCourseType.Id;
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