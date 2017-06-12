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
                vm.signInRecord = [], data=[];
                function getSignInRecord() {
                    $signInService.getSignInRecords({}).then(function (result) {
                        // console.log(result);
                        vm.signInRecord = result.data.items;
                    });
                }
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

                getSignInRecord();
            }
        ]
    );
})();