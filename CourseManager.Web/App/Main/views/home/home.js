(function () {
    var controllerId = 'app.views.home';
    angular.module('app').controller(controllerId, [
        '$scope', 'abp.services.app.teacherCourseArrange', 'abp.services.app.signInRecord', function ($scope, $teacherCourseArrangeService, $signInRecordService) {
            var vm = this;
            vm.schedule = [];
            vm.todaySignInRecord = "";
            function getSchedule() {
                var postData = {
                    "beginTime": $('#beginTime').val(),
                    "endTime": $('#endTime').val()
                };
                $teacherCourseArrangeService.getArranages(postData).then(function (result) {
                    vm.schedule = result.data.items;
                });
                $signInRecordService.generateHomeSignRecordDescription(postData).then(function (result) {
                    console.info(result);
                    vm.todaySignInRecord = result.data;
                })
            }
            getSchedule();
        }
    ]);
})();