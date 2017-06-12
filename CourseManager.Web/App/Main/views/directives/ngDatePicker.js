//(function () {
//    angular.module('app').directive('ngDatePicker', function ($timeout) {
//        return {
//            require: '?ngModel',
//            restrict: 'A',
//            scope: { // 自定义控件绑定$scope分为 @，=，& 三种情况。个人认为@:单向绑定，=：双向绑定，&: 用于绑定函数，下面看看Angularjs权威指南的介绍@  本地作用域属性：使用@符号将本地作用域同DOM属性的值进行绑定。
//                //=  双向绑定：通过=可以将本地作用域上的属性同父级作用域上的属性进行双向的数据绑定。 就像普通的数据绑定一样，本地属性会反映出父数据模型中所发生的改变 &  父级作用域绑定 通过&符号可以对父级作用域进行绑定，以便在其中运行函数。意味着对这个值进行设置时会生成一个指向父级作用域的包装函数.   
//                ngModel: '=',
//                maxDate: '@',
//                minDate: '@'
//            },
//            link: function (scope, element, attr, ngModel) {
//                var _date = null, _config = {};
//                // 渲染模板完成后执行
//                $timeout(function () {
//                    // 初始化参数 
//                    _config = {
//                        elem: '#' + attr.id,
//                        format: attr.format != undefined && attr.format != '' ? attr.format : 'YYYY-MM-DD',
//                        max: attr.hasOwnProperty('maxDate') ? attr.maxDate : '',
//                        min: attr.hasOwnProperty('minDate') ? attr.minDate : '',
//                        choose: function (data) {
//                            scope.$apply(setViewValue);

//                        },
//                        clear: function () {
//                            ngModel.$setViewValue(null);
//                        }
//                    };
//                    // 初始化
//                    _date = laydate(_config);

//                    // 监听日期最大值
//                    if (attr.hasOwnProperty('maxDate')) {
//                        attr.$observe('maxDate', function (val) {
//                            _config.max = val;
//                        })
//                    }
//                    // 监听日期最小值
//                    if (attr.hasOwnProperty('minDate')) {
//                        attr.$observe('minDate', function (val) {
//                            _config.min = val;
//                        })
//                    }

//                    // 模型值同步到视图上
//                    ngModel.$render = function () {
//                        element.val(ngModel.$viewValue || '');
//                    };

//                    // 监听元素上的事件
//                    element.on('blur keyup change', function () {
//                        scope.$apply(setVeiwValue);
//                    });

//                    setVeiwValue();

//                    // 更新模型上的视图值
//                    function setViewValue() {
//                        var val = element.val();
//                        ngModel.$setViewValue(val);
//                    }
//                }, 0);
//            }
//        };
//    })
//})()

//(function () {
//    angular.module("app", [])
//        .directive("datetimepicker", function () {
//            return {
//                restrict: "EA",
//                require: "ngModel",
//                link: function (scope, element, attrs, ctrl) {

//                    var unregister = scope.$watch(function () {

//                        $(element).append("<input id='date-" + attrs.dateid + "' style='border:none;width:100%;height:100%' " +
//                            "value='" + ctrl.$modelValue + "'>");
//                        $(element).css("padding", "0");

//                        element.on('change', function () {
//                            scope.$apply(function () {
//                                ctrl.$setViewValue($("#date-" + attrs.dateid).val());
//                            });
//                        });

//                        element.on('click', function () {
//                            $("#date-" + attrs.dateid).datetimepicker({
//                                format: attrs.format || 'Y/m/d h:i',
//                                onClose: function () {
//                                    element.change();
//                                }
//                            });
//                        });

//                        element.click();

//                        return ctrl.$modelValue;
//                    }, initialize);

//                    function initialize(value) {
//                        ctrl.$setViewValue(value);
//                        unregister();
//                    }
//                }
//            }
//        });

//})()
