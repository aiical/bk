var CourseArrangeValidator = function () {
    var handleSubmit = function () {
        $('.course-arrange-form').validate({
            errorElement: 'span',
            errorClass: 'help-block',
            focusInvalid: true,
            rules: {
                StudentId: {
                    required: true
                },
                Address: {
                    required: true
                },
                Remark: {
                    required: true
                },
                ClassType: {
                    required: true
                },
                CourseType: {
                    required: true
                },
                CourseAddressType: {
                    required: true
                }
            },
            messages: {
                StudentId: {
                    required: "请认真填写完整的学生名"
                },
                Address: {
                    required: "请认真填写具体上课地点"
                },
                Remark: {
                    required: "请按模板填写备注信息"
                },
                ClassType: {
                    required: "请选择上课类型"
                },
                CourseType: {
                    required: "请选择课程"
                },
                CourseAddressType: {
                    required: "请选择上课地点"
                },
                BeginTime: {
                    required: "请选择上课时间"
                },
                EndTime: {
                    required: "请选择下课时间"
                },
            },

            highlight: function (element) {
                $(element).closest('.form-group').addClass('has-error');
            },

            success: function (label) {
                label.closest('.form-group').removeClass('has-error');
                label.remove();
            },

            errorPlacement: function (error, element) {
                element.parent('div').append(error);
            },

            submitHandler: function (form) {
                form.submit();
            }
        });
    }
    return {
        init: function () {
            handleSubmit();
        }
    };
}();