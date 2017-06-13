(function () {
    angular.module('app').controller('app.views.teacherClassHourStatistics.index', [
        '$scope', 'abp.services.app.classHourStatistics',
        function ($scope, $teacherClassHourStatisticsService) {
            var vm = this;

            $scope.Data = {};
            $scope.Data.chartConfig = {//此处配置的信息是针对某表的详情信息
                title: {//标题
                    text: "7天消费"
                },
                yAxis: {//y轴信息
                    title: {
                        text: '金额'
                    },
                    labels: {
                        format: '{value}$'
                    }
                },
                xAxis: {//x轴信息
                    categories: ["04-01", "04-02", "04-03", "04-04", "04-05", "04-06", "04-07"]
                },
                series: [180, 160, 155, 165, 170, 150, 150]//数据表内容
            }
            vm.teacherClassHourStatistics = [];



            function getTeacherClassHourStatistics() {
                // 发送给后台的请求数据
                var postData = {

                };
                $studentService.getPagedStudents(postData).then(function (result) {
                    //console.log(result);
                    //console.log(result.data.totalCount);
                    //console.log(result.data.items); 
                    vm.teacherClassHourStatistics = result.data.items;
                });

            }
            //getTeacherClassHourStatistics();
        }
    ])
})();