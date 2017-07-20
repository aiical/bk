var Bk = window.Bk || {};
Bk.ClassHourStatistics = {
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
        Bk.ClassHourStatistics.actions.generateCharts($('#chart-show'));
        $('#btnSearch').click(function () {
            Bk.ClassHourStatistics.actions.generateCharts($('#chart-show'));
        })
    },
    actions: {
        generateCharts: function ($chartContainer) { //参考API:https://www.hcharts.cn/docs/basic-series
            var postData = $('#queryForm').serializeJson(),
                title = "老师课时走势";
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
            //console.log(xCategories);
            console.log(postData);
            abp.ui.setBusy(
                $('#chart-show'),
                abp.ajax({
                    url: abp.appPath + 'ClassHourStatistics/GetTeacherClassHourStatistics',
                    type: 'POST',
                    data: JSON.stringify(postData) //abp需要进行转换
                }).done(function (res) {
                    console.log(res);
                    var result = res.returnData, totalClassHours = result.total;
                    $chartContainer.highcharts({
                        chart: {
                            zoomType: 'x'
                        },
                        title: {
                            text: title + "--" + "【当月共上课" + totalClassHours + "个小时】【1v1课时：" + result.one2oneDuration + "】【班级课课时：" + result.classDuration +"】"
                        },
                        subtitle: {
                            text: document.ontouchstart === undefined ?
                                '鼠标拖动可以进行缩放' : '手势操作进行缩放'
                        },
                        xAxis: {
                            type: "category",
                            categories: xCategories
                        },
                        credits: {
                            enabled: false
                        },
                        tooltip: {
                            dateTimeLabelFormats: {
                                millisecond: '%H:%M:%S.%L',
                                second: '%H:%M:%S',
                                minute: '%H:%M',
                                hour: '%H:%M',
                                day: '%Y-%m-%d',
                                week: '%m-%d',
                                month: '%Y-%m',
                                year: '%Y'
                            }
                        },
                        yAxis: {
                            min: 0,
                            max: 14,
                            floor: 0,
                            ceiling: 14,
                            title: {
                                text: '课时(h)'
                            }
                        },
                        legend: {
                            enabled: true
                        },
                        plotOptions: {
                            area: {
                                fillColor: {
                                    linearGradient: {
                                        x1: 0,
                                        y1: 0,
                                        x2: 0,
                                        y2: 1
                                    },
                                    stops: [
                                        [0, Highcharts.getOptions().colors[0]],
                                        [1, Highcharts.Color(Highcharts.getOptions().colors[0]).setOpacity(0).get('rgba')]
                                    ]
                                },
                                marker: {
                                    radius: 3 //线上锚点
                                },
                                lineWidth: 1,
                                states: {
                                    hover: {
                                        lineWidth: 2
                                    }
                                },
                                threshold: null
                            }
                        },
                        series: [
                            {
                            type: 'area',
                            lineWidth: 1, //线条宽度
                            name: '上课课时',
                            data: result.durations// [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0]//result.durations //[1, 2, 3, 4, 5, 6, 7, 9, 0, 3, 2, 10, 0]
                            },
                            {
                                type: 'area',
                                color: 'orange',
                                lineWidth: 1, //线条宽度
                                name: '未上课',
                                data: result.absentCourseDurations
                            },
                            {
                                type: 'area',
                                color:'green',
                                lineWidth: 1, //线条宽度
                                name: '1v1课时',
                                data: result.one2OneDurations
                            },
                            {
                                type: 'area',
                                color: 'red',
                                lineWidth: 1, //线条宽度
                                name: '班级课课时',
                                data: result.classCourseDurations
                            }
                        ]
                    });
                    // abp.notify.success();
                })
            );
            //data	显示在图表中的数据列，可以为数组或者JSON格式的数据。如：data: [0, 5, 3, 5]，或
            //data: [{ name: 'Point 1', y: 0 }, { name: 'Point 2', y: 5 }]
            //            series中data封装格式例子：
            //            data: [
            //                [Date.UTC(2010, 1, 1, 10, 20), 120],
            //                [Date.UTC(2010, 1, 1, 10, 20), 120],
            //                [Date.UTC(2010, 1, 1, 10, 20), 120],
            //                ....
            //],var someDate = new Date(Date.parse(tm));//把字符串类型专程Date类型
            //            var tms = Date.UTC(someDate.getFullYear(), someDate.getMonth(), someDate.getDate(), someDate.getHours(), someDate.getMinutes());//进行Date.UTC处理
        },
        getDiffTime: function (beginTime, endTime, type) {
            var date1 = new Date(beginTime)
            var date2 = new Date(endTime)
            var s1 = date1.getTime(), s2 = date2.getTime();
            var total = (s2 - s1) / 1000;

            var day = parseInt(total / (24 * 60 * 60));//计算整数天数
            var afterDay = total - day * 24 * 60 * 60;//取得算出天数后剩余的秒数
            var hour = parseInt(afterDay / (60 * 60));//计算整数小时数
            var afterHour = total - day * 24 * 60 * 60 - hour * 60 * 60;//取得算出小时数后剩余的秒数
            var min = parseInt(afterHour / 60);//计算整数分
            var afterMin = total - day * 24 * 60 * 60 - hour * 60 * 60 - min * 60;//取得算出分后剩余的秒数
            switch (type) {
                case "day":
                    return day;
                case "hour":
                    return hour;
                case "min":
                    return min;
                default:
            }
        }
    }
}
