(function () {

    $(function () {
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
            abp.ui.setBusy(
                $('#LoginArea'),
                abp.ajax({
                    url: abp.appPath + 'Account/Login',
                    type: 'POST',
                    data: postData
                })
            );
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