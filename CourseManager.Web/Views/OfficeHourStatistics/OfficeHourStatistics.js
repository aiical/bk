var Bk = window.Bk || {};
Bk.OfficeHourStatistics = {
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
        Bk.OfficeHourStatistics.actions.generateCharts($('#chart-show'));
        $('#btnSearch').click(function () {
            Bk.OfficeHourStatistics.actions.generateCharts($('#chart-show'));
        })
    },
    actions: {
        generateCharts: function ($chartContainer) { //参考API:https://www.hcharts.cn/docs/basic-series
            var postData = $('#queryForm').serializeJson(),
                title = "老师坐班时间统计";
            if (postData.BeginTime == '' || postData.EndTime == '') {
                abp.notify.error("请先选择时间周期");
                return;
            }
            if (postData.BeginTime > postData.EndTime) {
                abp.notify.error("开始时间不能大于结束时间");
                return;
            }
            //console.log(postData.EndTime);
            //console.log(DateTimeUtil.dateToStr('yyyy-MM-dd', new Date()));
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
            if (postData.BeginTime != '' || postData.EndTime != '') {
                title = postData.BeginTime + "~" + postData.EndTime + (postData.TeacherName == "" ? "胡盼" : postData.TeacherName) + title;
            };

            for (var i = startTimeDay; i < endTimeDay; i++) {
                days.push(i);
            }
            $.each(days, function (index, item) {
                xCategories.push(startTimeMonth + "月" + item + "日");
            })
            console.log(xCategories);
            console.log(postData);
            var pointStartVal = Date.UTC(startDateTime.getFullYear(), startDateTime.getMonth(), startDateTime.getDate());
            abp.ui.setBusy(
                $('#chart-show'),
                abp.ajax({
                    url: abp.appPath + 'OfficeHourStatistics/GetTeacherOfficeHourStatistics',
                    type: 'POST',
                    data: JSON.stringify(postData) //abp需要进行转换
                }).done(function (res) {
                    //console.log(res);
                    var result = res.returnData, totalClassHours = result.total;
                    $chartContainer.highcharts({
                        chart: {
                            type: 'column'
                        },
                         title: {
                            text: title + "--" + "(当月共坐班" + totalClassHours + "个小时)"
                        },

                        credits: {
                            enabled: false
                        },
                         xAxis: {
                            type: 'datetime',//格式化X 轴时间显示
                             labels: {
                                 formatter: function () {
                                     return Highcharts.dateFormat('%m-%d', this.value);
                                 }
                             } 
                        },
                         yAxis: [{ 
                            title: {
                                text: '时间段(h)',
                                style: {
                                    color: Highcharts.getOptions().colors[0]
                                }
                            } 
                        }],
                         tooltip: {
                            xDateFormat: '%m-%d',
                            shared: true
                        },
                        plotOptions: {
                            series: {
                                pointStart: pointStartVal,
                                pointInterval: 24 * 3600 * 1000
                            }
                        },
                        //plotOptions: {
                        //    column: {
                        //        dataLabels: {
                        //            enabled: true,
                        //            // verticalAlign: 'top', // 竖直对齐方式，默认是 center
                        //            inside: true
                        //        }
                        //    }
                        //},
                        series: [{
                            name: postData.TeacherName == "" ? "胡老师" : postData.TeacherName,
                            data: result.durations
                        }]
                    });
                })
            );
        }
    }
}
