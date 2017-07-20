var Bk = window.Bk || {};
Bk.PayCalculation = {
    bootstrap: function () {
        this.eventBind();
    },
    eventBind: function () {
        $('.datetime').datetimepicker({
            lang: 'ch',
            format: 'Y-m-d',
            formatDate: 'Y-m-d',
            scrollInput: false,
            timepicker: false,//关闭时间选项
        });
        Bk.PayCalculation.actions.generateSalary();
        $('#btnSearch').click(function () {
            Bk.PayCalculation.actions.generateSalary();
        })
    },
    actions: {
        generateSalary: function () {
            var postData = $('#queryForm').serializeJson();
            if (postData.BeginTime == '' || postData.EndTime == '') {
                abp.notify.error("请先选择时间周期");
                return;
            }
            if (postData.BeginTime > postData.EndTime) {
                abp.notify.error("开始时间不能大于结束时间");
                return;
            }
            if (postData.EndTime > DateTimeUtil.dateToStr('yyyy-MM-dd', new Date())) {
                abp.notify.error("结束时间不能大于当前时间");
                return;
            }
            var xCategories = [], days = [];
            var startDateTime = new Date(postData.BeginTime), startTimeMonth = startDateTime.getMonth() + 1, startTimeDay = startDateTime.getDate();
            var endDateTime = new Date(postData.EndTime), endTimeMonth = endDateTime.getMonth() + 1, endTimeDay = endDateTime.getDate() + 1;
            if (endTimeMonth > startTimeMonth) {
                abp.notify.error("时间筛选周期为1个月 请选择开始时间所在月时间");
                return;
            }
            //  console.log(postData);
            abp.ui.setBusy(
                abp.ajax({
                    url: abp.appPath + 'PayCalculation/GetPayCalculation',
                    type: 'POST',
                    data: JSON.stringify(postData) //abp需要进行转换
                }).done(function (res) {
                   // console.log(res.returnData.result);
                    var data = res.returnData.result, earlyLateNum = data.earlyCourseTimes + data.nightCourseTimes, unit = "元";
                   // console.log(data);
                    $('#ShowTeacherName').val(data.teacherName);
                    $('#BasicSalary').val(data.basicSalary + unit);
                    $('#One2OneFee').val(data.one2OneDuration * 50 + unit);
                    $('#ClassFee').val(data.classDuration * 70 + unit);
                    $('#TotalClassHours').val(data.totalDuration);
                    $('#EarlyLateNum').val(earlyLateNum);
                    $('#EarlyLateBonus').val(earlyLateNum * 5 + unit);
                    $('#AssignmentTimes').val(data.assignmentTimes);
                    $('#AssignmentBonus').val(data.assignmentTimes * 10 + unit);
                    $('#RenewNum').val(data.renewNum);
                    $('#RenewFee').val(data.renewFee + unit);
                    $('#ExtraSalary').val(data.studentAbsentFees + unit + "-" + data.studentAbsentFeeDes);
                    $('#AllOfficeHoursSalary').val(data.allOfficeHoursBonus + unit);

                    if (data.basicSalary >= 4000 && data.totalDuration >= 40) {//工资达到了底薪标准 且工时达到了40个小时标准 则有其他福利
                        $('#TotalSalary').val(
                            data.basicSalary + "+"
                            + data.allOfficeHoursBonus + "+"
                            + data.earlyCourseTimes * 5 + "+"
                            + data.assignmentTimes * 10 + "+"
                            + (data.renewNum < 1 ? 0 : data.renewFee) + "+"
                            + data.studentAbsentFees + "="
                            + parseInt((data.basicSalary + data.allOfficeHoursBonus + (data.earlyCourseTimes * 5) + (data.assignmentTimes * 10) + (data.renewNum < 1 ? 0 : data.renewFee))
                            + data.studentAbsentFees || 0)
                            + unit
                        );
                    }
                    else {
                        $('#TotalSalary').val(data.basicSalary + unit + "  (因为工时未超过40个小时 所以只能拿到底薪) ");
                    }
                    // var result = res.returnData, totalClassHours = result.total;
                }).fail(function (error) {
                    console.log(error);
                })
            );
        }
    }
}
