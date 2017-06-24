(function () {
    angular.module('app').controller('app.views.signInRecord.createModal', [
        '$scope',
        '$uibModalInstance',
        'abp.services.app.signInRecord',
        'abp.services.app.categorys',
        'abp.services.app.teacherCourseArrange',
        function ($scope, $uibModalInstance, $signInService, $categoryService, $teacherCourseArrangeService) {

            var vm = this, defaultSelectItem = { "CategoryName": "--请选择--", "Id": "-1" };
            vm.signInRecord = {
                isActive: true,
                beginTime: "",
                endTime: "",
                courseArranges: [],
                type: [
                    defaultSelectItem
                ],
                courseType: [
                    defaultSelectItem
                ],
                classType: [
                    defaultSelectItem
                ],
                unNormalType: [
                    defaultSelectItem
                ],
                addressType: [
                    defaultSelectItem
                ]
                //type: [
                //    { "CategoryName": "准时上课", "Id": "305ab34ea2674ed4a1c9dbcc7265d2e9" },//["迟到", "正常", "未上课"]
                //    { "CategoryName": "迟到", "Id": "4a7f3fde896544a68756762eebaa12e2" },
                //    { "CategoryName": "未上课", "Id": "21ecd0cb5cda4149b3dca7e04e1611c3" }
                //],
                //courseType: [
                //    { "CategoryName": "入门上", "Id": "0a5ac3cdba364c7dacd4424708db329e" },
                //    { "CategoryName": "入门下", "Id": "25df80ade53e4f158e63ef9f0208a259" },
                //    { "CategoryName": "基础", "Id": "48f99248288748ff851952171e28201c" },
                //    { "CategoryName": "提高", "Id": "cf922c690f91467daef62b6921da26f3" },
                //    { "CategoryName": "TSC", "Id": "18552c6ae6884ea3bd5a373d0aa89ba3" },
                //],
                //classType: [
                //    { "CategoryName": "1V1", "Id": "7be04a7f2bdb49f5a2dfd57e29a28550" },
                //    { "CategoryName": "班级课", "Id": "89e3d2b7d0df4091a21622f592713dc4" },
                //],
                //unNormalType: [
                //    { "CategoryName": "", "Id": "-1" },
                //    { "CategoryName": "学生请假", "Id": "4775c43210b14f84a722737b6895b230" },
                //    { "CategoryName": "老师请假", "Id": "c228ab60960446fe9c8e7b8c3ca5e0b1" },
                //    { "CategoryName": "其他原因", "Id": "47f65adef437459a8fe41f4c21bc9bce" }
                //]
            };
            //console.log(vm.signInRecord);
            function getCategorys() {
                $categoryService.getAllCategorys()
                    .then(function (res) {
                        console.log(res.data);
                        $.each(res.data, function (index, item) {
                            var cd = { "CategoryName": item.categoryName, "Id": item.id };
                            switch (item.categoryType) {
                                case "CourseAddressType":
                                    vm.signInRecord.addressType.push(cd);
                                    break;
                                case "SignInRecordType":
                                    vm.signInRecord.type.push(cd);
                                    break;
                                case "CourseType":
                                    vm.signInRecord.courseType.push(cd);
                                    break;
                                case "ClassType":
                                    vm.signInRecord.classType.push(cd);
                                    break;
                                case "NoCourseReasonType":
                                    vm.signInRecord.unNormalType.push(cd);
                                    break;
                                default:
                            }
                        });
                        console.log(vm.signInRecord);
                        $scope.selectedType = vm.signInRecord.type[0];//如果想要第一个值
                        $scope.selectedClassType = vm.signInRecord.classType[0]
                        $scope.selectedCourseType = vm.signInRecord.courseType[0];
                        $scope.selectedAddressType = vm.signInRecord.addressType[0];
                        $scope.selectedUnNormalType = vm.signInRecord.unNormalType[0];
                    });
            }

            function getCourseArrange() {
                var now = new Date(), year = now.getFullYear(), month = now.getMonth() + 1, day = now.getDate();
                $teacherCourseArrangeService.getTeacherCourseArrange2SignIn(
                    { "beginTime": year + "-" + month + "-" + day, "endTime": year + "-" + month + "-" + (day + 1) }
                )
                    .then(function (res) {
                        console.log(res.data);
                        var data = res.data;
                        if (data.length == 0) {
                            abp.notify.info("当前没有安排上课或已经签到哟，如需补签请联系管理员!");
                            $uibModalInstance.dismiss({});
                            return;
                        }
                        $.each(res.data, function (index, item) {
                            var courseArrangeItem = { "TimeDuration": item.timeDuration, "Id": item.id };
                            vm.signInRecord.courseArranges.push(courseArrangeItem);
                        });
                        $scope.selectCourseArrange = vm.signInRecord.courseArranges[0];//如果想要第一个值
                    });
            }
            $uibModalInstance.opened.then(function () {//模态框打开之后执行的函数 一个契约，当模态窗口打开并且加载完内容时传递的变量
                //console.log('模态框打开');
                getCategorys();//初始化分类
                getCourseArrange();
            });
            //select 的ng-change事件和原始ng-change相同  
            vm.signInRecord.selectChange = function () {
                var curSelect = $scope.selectedUnNormalType;
                if (curSelect != null) {
                    if (curSelect.Id != "-1") {
                        $('#Reason-Container').show();
                    } else $('#Reason-Container').hide();
                }
            }
            vm.save = function () {
                abp.ui.setBusy();
                if (vm.signInRecord.endTime != null && vm.signInRecord.endTime != "" && vm.signInRecord.beginTime != null && vm.signInRecord.beginTime != "") {

                    if (vm.signInRecord.endTime < vm.signInRecord.beginTime
                        ||
                        new Date(vm.signInRecord.endTime).getTime() < new Date(vm.signInRecord.beginTime).getTime()
                    ) {
                        vm.signInRecord.endTime = null;
                        abp.notify.error("上课时间不能大于下课时间");
                        abp.ui.clearBusy();
                        $('#BeginTime').focus();
                        return;
                    }
                    var now = new Date();
                    //  console.log(vm.signInRecord.endTime + "--" + now.getHours() + ":" + now.getMinutes());
                    if ((vm.signInRecord.endTime != null) &&
                        (vm.signInRecord.endTime > now.getHours() + ":" + now.getMinutes()
                            || new Date(vm.signInRecord.endTime).getTime() > new Date().getTime())) {
                        vm.signInRecord.endTime = null;
                        abp.notify.error("下课时间不能大于当前时间");
                        abp.ui.clearBusy();
                        $('#EndTime').focus();
                        return;
                    }
                }

                vm.signInRecord.type = $scope.selectedType.Id;
                vm.signInRecord.classType = $scope.selectedClassType.Id;
                vm.signInRecord.unNormalType = $scope.selectedUnNormalType.Id;
                vm.signInRecord.courseType = $scope.selectedCourseType.Id;
                vm.signInRecord.courseAddressType = $scope.selectedAddressType.Id;
                vm.signInRecord.courseArranges = $scope.selectCourseArrange.Id;
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
    ]).directive("datetimepicker", function () {
        return {
            restrict: "EA",
            require: "ngModel",
            link: function (scope, element, attrs, ctrl) {
                var unregister = scope.$watch(function () {
                    $(element).append("<input placeHolder=\"请选择准确的时间\" readonly=\"readonly\" autocomplete=\"off\" id='date-" + attrs.dateid + "' style='border:none;width:100%;height:100%' " +
                        "value='" + ctrl.$modelValue + "'>");
                    $(element).css("padding", "0");

                    element.on('change', function () {
                        scope.$apply(function () {
                            ctrl.$setViewValue($("#date-" + attrs.dateid).val());
                        });
                    });
                    element.on('click', function () {
                        $("#date-" + attrs.dateid).datetimepicker({
                            format: attrs.format || 'Y/m/d h:i',
                            step: 15,//时间按1分钟累加
                            maxDate: '1970/01/01', //签到就限定 当天签到 必须养成规范 有特殊情况可以邮件 一个月最多2次
                            minDate: '1970/01/01',
                            yearStart: 2017,
                            datepicker: true,//不显示日期选择
                            scrollTime: true,
                            minHour: 7,//每天默认最早从8点开始上课 
                            onClose: function () {
                                element.change();
                            }
                        });
                    });
                    element.click();
                    return ctrl.$modelValue;
                }, initialize);

                function initialize(value) {
                    ctrl.$setViewValue(value);
                    unregister();
                }
            }
        }
    });
})();