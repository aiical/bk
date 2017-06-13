var Bk = window.Bk || {};
Bk.ClassHourStatistics = {
    bootstrap: function () {
        this.eventBind();
        this.initData();
        this.initStyle();
    },
    initData: function () {

    },
    initStyle: function () {

    },
    eventBind: function () {
        $('.datetime').datetimepicker({
            lang: 'ch',
            format: 'Y-m-d',
            formatDate: 'Y-m-d',
            scrollInput: false,
            timepicker: false,//关闭时间选项
        });
        this.actions.generateCharts($('#chart-show'));
        $('#btnSearch').click(function () {
            this.actions.generateCharts($('#chart-show'));
            //console.log(postData);
        })
    },
    actions: {
        generateCharts: function ($chartContainer) {
            var postData = $('#queryForm').serializeJson(),
                title = "老师课时走势";
            if (postData.BeginTime != '' || postData.EndTime != '') {
                title = postData.BeginTime + "~" + postData.EndTime + postData.TeacherName + title;
            };
            $.ajax({
                type: "POST",
                dataType: "html",
                cache: false,
                url: "/ClassHourStatistics/GetTeacherClassHourStatistics",
                data: {},
                success: function (res) {
                    //console.log(res);
                    var result = JSON.parse(res).result;
                  
                    //$.each(result, function (index, item) {
                    //    var newDate = new Date(Date.parse(item.name));//把字符串类型专程Date类型
                    //    var tms = Date.UTC(newDate.getFullYear(), newDate.getMonth(), newDate.getDate(), newDate.getHours(), newDate.getMinutes());//进行Date.UTC处理
                    //    item.name = tms;
                    //})
                    console.log(result);
                    $chartContainer.highcharts({
                        chart: {
                            zoomType: 'x'
                        },
                        title: {
                            text: title
                        },
                        subtitle: {
                            text: document.ontouchstart === undefined ?
                                '鼠标拖动可以进行缩放' : '手势操作进行缩放'
                        },
                        xAxis: {
                            type: 'datetime',
                            dateTimeLabelFormats: {
                                millisecond: '%H:%M:%S.%L',
                                second: '%H:%M:%S',
                                minute: '%H:%M',
                                hour: '%H:%M',
                                day: '%m-%d',
                                week: '%m-%d',
                                month: '%Y-%m',
                                year: '%Y'
                            }
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
                            title: {
                                text: '课时'
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
                                    radius: 2
                                },
                                lineWidth: 1,
                                states: {
                                    hover: {
                                        lineWidth: 1
                                    }
                                },
                                threshold: null
                            }
                        },
                        series: [{
                            type: 'area',
                            name: '当天工时',
                            data: result
                        }]
                    });
                }
            });
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
        }
    }
}
