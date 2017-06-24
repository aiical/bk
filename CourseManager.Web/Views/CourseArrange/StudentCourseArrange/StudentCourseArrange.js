var Bk = window.Bk || {};
Bk.StudentCourseArrange = {
    bootstrap: function () {
        this.eventBind();
        this.initData();
    },
    initData: function () {
        var nowYearMonth = Bk.Common.getUrlParam('yearMonth'), nowMonth = "";
        if (nowYearMonth != '' && nowYearMonth != null && typeof nowYearMonth != 'undefined') {
            nowMonth = nowYearMonth.split('-')[1];
            $('#Month').val(nowMonth);
        };
        $(".course-edit").on("click", function () {
            abp.notify.info('排课编辑 暂未开放，程序员正在奋力开发中');
        });
    },
    eventBind: function () {
        $('.datetime').datetimepicker({
            lang: 'ch',
            scrollTime: true,
            step: 15
        });
        //弹出modal
        $(".btn-add").click(function (e) {
            $("#addStudentCourseArrangeModal").modal("show");
        });
        //保存排课信息
        $('.save-arrange').click(function () {
            CourseArrangeValidator.init();
            if ($('.course-arrange-form').validate().form()) {
                var postData = $('#add-studentCourse-form').serializeJson();
                console.info(postData);

                //console.log(JSON.stringify(postData));
                var beginTime = postData.BeginTime, endTime = postData.EndTime;
                //验证数据 如开始时间和结束时间
                if (
                    (endTime == null || endTime == "")
                    || (beginTime == null || beginTime == "")
                ) {
                    abp.notify.warn("请先选择上课时间");
                    $('#BeginTime').focus();
                    return false;
                }

                if (endTime != null && endTime != "" && beginTime != null && beginTime != "") {
                    if (endTime < beginTime
                        ||
                        new Date(endTime).getTime() < new Date(beginTime).getTime()
                    ) {
                        endTime = null;
                        abp.notify.error("上课时间不能大于下课时间");
                        abp.ui.clearBusy();
                        $('#BeginTime').focus();
                        return;
                    }
                }
                postData = $.extend(postData, { "StudentId": $('#hidStudentId').val() });
                if ($(this).attr('crossweek') == 1) {
                    postData = $.extend(postData, { "CrossWeek": true });
                }
                abp.ui.setBusy(
                    abp.ajax({
                        context: this,
                        url: abp.appPath + 'CourseArrange/AddStudentCourseArrange',
                        type: 'POST',
                        data: JSON.stringify(postData) //abp需要进行转换
                    }).done(function (res) {
                        console.log(res);
                        if (typeof res == "string") res = res == "true" ? true : false;
                        if (res) {// != ''
                            abp.notify.success("排课成功");
                            window.location.reload();
                        } else abp.notify.info("排课失败，请联系管理员");
                    }).fail(function (error) {
                        console.info(error);
                        //abp.notify.error(error.responseText);
                    })
                );
            }
        });

        //智能搜索
        var students = [];
        $("#scStudent option").each(function () {
            if ($(this).val() != "0") {
                var item = { to: $(this).val(), name: $(this).html() };
                students.push(item);
            }
        });
        $('#keyword').autocomplete(students, {
            max: 20,    //列表里的条目数
            minChars: 0,    //自动完成激活之前填入的最小字符
            width: 200,     //提示的宽度，溢出隐藏
            scrollHeight: 400,   //提示的高度，溢出显示滚动条
            matchContains: true,    //包含匹配，就是data参数里的数据，是否只要包含文本框里的数据就显示
            autoFill: true,    //自动填充
            mustMatch: false,//表示必须匹配条目,文本框里输入的内容,必须是data参数里的数据,如果不匹配,文本框就被清空
            formatItem: function (row, i, max) {
                return row.name;
            },
            formatMatch: function (row, i, max) {
                return row.name;
            },
            formatResult: function (row) {
                return row.name;
            }
        }).result(function (event, row, formatted) {
            $("#scStudent").val(row.to);
            $("#hidStudentId").val(row.to);
        });

        $(".CourseItem").hover(function () {
            $(this).css("border-bottom", "solid 1px yellow");
        }, function () {
            $(this).css("border-bottom", "0px");
        });
    },
    actions: {
        //上月
        preMonth: function () {
            //if (Bk.StudentCourseArrange.actions.validTeacherInput($('#scSudent').val())) {
            var _year = parseInt($.trim($("#Year").val()));
            var _month = parseInt($.trim($("#Month").val()));
            //如果此时月份是1月的话，则上月为上一年的12月，年减1
            if (_month == 1) {
                $("#Month").val(12);
                $("#Year").val(_year - 1);
            } else {
                $("#Month").val(_month - 1);
            }
            Bk.StudentCourseArrange.actions.showStudentCourses();
            //}
        },
        //下月
        nextMonth: function () {
            // if (Bk.StudentCourseArrange.actions.validTeacherInput($('#scSudent').val())) {
            var _year = parseInt($.trim($("#Year").val()));
            var _month = parseInt($.trim($("#Month").val()));
            if (_month == 12) {
                $("#Month").val(1);
                $("#Year").val(_year + 1);
            }
            else {
                $("#Month").val(_month + 1);
            }
            Bk.StudentCourseArrange.actions.showStudentCourses();
            // }
        },
        //本月
        curMonth: function () {
            var now = new Date();
            $("#Year").val(now.getFullYear());
            $("#Month").val(now.getMonth() + 1);
            Bk.StudentCourseArrange.actions.showStudentCourses();
        },
        //导出
        exportExcel: function () {
            abp.notify.info('导出功能暂未开放，程序员正在奋力开发中');
            return;
            var teacherId = $("#scStudent").val();
            if (Bk.StudentCourseArrange.actions.validTeacherInput(teacherId)) {
                var yearmonth = $("#Year").val() + "-" + ($("#Month").val() + 1);
                window.open("/CourseArrange/TeacherScheduleByMonthExport?teacherId=" + teacherId + "&yearmonth=" + yearmonth);
            }
        },
        showStudentCourses: function () { //得到某个学生的课程安排
            var studentId = $("#hidStudentId").val();
            if (Bk.StudentCourseArrange.actions.validTeacherInput(studentId)) {
                var yearmonth = $("#Year").val() + "-" + (parseInt($("#Month").val()));
                console.log("/CourseArrange/StudentCourseArrange?studentId=" + studentId + "&yearMonth=" + yearmonth);
                window.location.href = "/CourseArrange/StudentCourseArrange?studentId=" + studentId + "&yearMonth=" + yearmonth;
            }
        },
        validTeacherInput: function (studentId) {
            if (studentId == "" || studentId == 0) {
                abp.notify.warn("请先选择学生");
                $("#scStudent").focus();
                return false;
            }
            return true;
        },
        //切换课程状态
        mouseover: function (obj) {
            var classname = $(obj).children(':first').attr("class");
            $(".course-arrange-item-wrapper ul li").each(function () {
                if (!$(this).hasClass(classname)) {
                    $(this).hide();
                }
            });
        },
        mouseout: function () {
            $(".course-arrange-item-wrapper ul li").show();
        },
        out: function () {
            window.location.reload();
        }

    }
}
