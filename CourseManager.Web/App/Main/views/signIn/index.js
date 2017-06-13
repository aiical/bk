(function () {
    var signInControllerId = "app.views.signInRecord.index";
    angular.module('app').controller(
        signInControllerId,
        [
            '$scope',
            '$uibModal',
            'abp.services.app.signInRecord', 'abp.services.app.categorys',
            function ($scope, $uibModal, $signInService, $categoryService) {
                var vm = this;
                vm.signInRecord = [], data = [];
                //function getSignInRecord() {
                //    $signInService.getSignInRecords({}).then(function (result) {
                //        // console.log(result);
                //        vm.signInRecord = result.data.items;
                //    });
                //}
                vm.openSignInModal = function () {
                    var modalInstance = $uibModal.open({
                        templateUrl: '/App/Main/views/signIn/createModal.cshtml',
                        controller: 'app.views.signInRecord.createModal as vm',
                        backdrop: 'static',
                        //resolve: {
                        //    'get-categories': function () {
                        //        return getCategorys();
                        //    }
                        //}
                    });
                    //modalInstance.opened.then(function () {//模态框打开之后执行的函数 一个契约，当模态窗口打开并且加载完内容时传递的变量
                    //    console.log('模态框打开');
                    //});
                    //resolve：定义一个成员并将他传递给$modal指定的控制器，相当于routes的一个reslove属性，如果需要传递一个objec对象，需要使用angular.copy()
                    modalInstance.result.then(function () {
                        getSignInRecord();
                    });
                };


                //分页
                function getSignInRecord() {
                    // 发送给后台的请求数据
                    var postData = {
                        pIndex: $scope.paginationConf.currentPage,
                        pSize: $scope.paginationConf.itemsPerPage
                    };
                    $signInService.getPagedSignInRecords(postData).then(function (result) {
                        //console.log(result);
                        //console.log(result.data.totalCount);
                        //console.log(result.data.items);
                        // 变更分页的总数
                        $scope.paginationConf.totalItems = result.data.totalCount;
                        // 变更产品条目
                        $scope.signInRecord = result.data.items;
                        vm.signInRecord = result.data.items;
                        vm.paginationConf.totalItems = result.data.totalCount;
                        // console.log($scope.paginationConf);
                    });

                }

                //配置分页基本参数
                $scope.paginationConf = {
                    currentPage: 1,
                    itemsPerPage: 10
                };

                /***************************************************************
                当页码和页面记录数发生变化时监控后台查询
                如果把currentPage和itemsPerPage分开监控的话则会触发两次后台事件。
                通过$watch currentPage和itemperPage 当他们一变化的时候，重新获取数据条目
                ***************************************************************/
                $scope.$watch('paginationConf.currentPage + paginationConf.itemsPerPage', getSignInRecord);

                //  getSignInRecord();
            }
        ]
    );
})();