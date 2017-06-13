(function () {
    angular.module('app').directive('myChart', function () {
        var obj = {
            restrict: 'AE',
            replace: true,
            template: '<div></div>',
            scope: {
                config: '=',
                cWidth: '=',
                cHeight: '=',
                dWidth: '='
            },
            link: function (scope, element) {
                element[0].style.width = scope.dWidth ? scope.dWidth + 'px' : '1000px';//默认元素宽度1000,同时支持自定义
                setChart = function () {
                    var defaultOptions = {//所有图形的基础配置
                        chart: {
                            renderTo: element[0],//图形所在的元素
                            width: scope.cWidth || 1000,//图形宽度
                            height: scope.cHeight || 600 //图形高度
                        }
                    };
                    config = angular.extend(defaultOptions, scope.config);//新建图形的配置由新属性继承于基础配置
                    new HighCharts.Chart(config);//生成图形
                };
                //监视数据属性存在时,生成图形
                scope.$watch("config.series", function () {
                    setChart()
                });
            }
        };
        return obj;
    });
})();
