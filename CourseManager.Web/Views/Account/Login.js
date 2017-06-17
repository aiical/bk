(function () {

    $(function () {
        var loginResultEnum = {
            InvalidUserNameOrEmailAddress:2,
            InvalidPassword :3,
            UserIsNotActive : 4,
        };
        $('#LoginButton').click(function (e) {
            e.preventDefault();
            var postData = JSON.stringify({
                tenancyName: $('#TenancyName').val(),
                usernameOrEmailAddress: $('#EmailAddressInput').val(),
                password: $('#PasswordInput').val(),
                rememberMe: $('#RememberMeInput').is(':checked'),
                returnUrlHash: $('#ReturnUrlHash').val()
            });
            //console.log(postData);
            //return;
            var headers = {};
            headers["x-xsrf-token"] = abp.security.antiForgery.getToken();
            abp.ui.setBusy(
                $('#LoginArea'),
                abp.ajax({
                    url: abp.appPath + 'Account/Login',
                    type: 'POST',
                    headers: headers, //可能会出现Empty or invalid anti forgery header token.
                    /*1.在Controller的Action里标注：[DisableAbpAntiForgeryTokenValidation]
                    2.在ajax的header里添加键值‘x-xsrf - token’:abp.security.antiForgery.getToken()*/
                    data: postData
                })
                //.done(function (data) {
                //    console.log(data);
                //    if (data.success) {
                //        window.location.href = res.targetUrl;
                //    } else {
                //        var returnCode = res.result;
                //        switch (returnCode) {
                //            case loginResultEnum.InvalidPassword:
                //            case loginResultEnum.InvalidUserNameOrEmailAddress:
                //                abp.notify.error(App.localize("InvalidUserNameOrPassword"));
                //                return;
                //            case loginResultEnum.UserIsNotActive:
                //                abp.notify.error(App.localize("UserIsNotActiveAndCanNotLogin"));
                //                return;
                //            default:
                //                return;
                //        }
                //    }
                //    }).fail(function (error) {
                //        console.info(error);
                //    })
                )
        });

        $('a.social-login-link').click(function () {
            var $a = $(this);
            var $form = $a.closest('form');
            $form.find('input[name=provider]').val($a.attr('data-provider'));
            $form.submit();
        });

        $('#ReturnUrlHash').val(location.hash);

        $('#LoginForm input:first-child').focus();
    });

})();