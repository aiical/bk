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

            abp.ui.setBusy(
                abp.ajax({
                    url: abp.appPath + 'PayCalculation/GetPayCalculation',
                    type: 'POST',
                    data: JSON.stringify(postData) //abp需要进行转换
                }).done(function (res) {
                    //console.log(res);
                    var outSideFee, basicFee, earlyFee, extraFee;
                    outSideFee = $('#outside-fee').val();
                    basicFee = $('#basic-fee').val();
                    earlyFee = $('#early-fee').val();
                    extraFee = $('#exta-fee').val();
                    $('#total-fee').val(outSideFee + "+" + basicFee + "+" + earlyFee + "+" + extraFee + "=6680元");
                    // var result = res.returnData, totalClassHours = result.total;
                })
            );
        }
    }
}
