var Bk = window.Bk || {};
Bk.TeacherCourseArrange = {
    TeacherId: "",
    sDate: "",
    Url: "",
    AreaId: 0,
    bootstrap: function () {
        this.eventBind();
        this.initData();
        this.initStyle();
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
    initStyle: function () {
        var areaId = $("#hidarea").val(), areaname = $("#hidareaname").val(), employeeareaId = $("#hidemployeeareaId").val();
        if (areaId != 0) {
            $("#scCity").text(areaname);
        }
        if (employeeareaId != 0) {
            $("#scTeacher").value = employeeareaId;
            $("#keyword").val($("#scTeacher option[value=" + employeeareaId + "]").text());
        }
        else {
            $("#scTeacher").selectedIndex = 0;
        }
    },
    eventBind: function () {
        $('.datetime').datetimepicker({
            lang: 'ch',
            scrollTime: true,
            step: 10
        });
        $("body").on("click", function () {
            $("#area").css("display", "none");
        });
        //弹出modal
        $(".btn-add").click(function (e) {
            if ($('#hidTeacherId').val() > 0) {
                $("#addTeacherCourseArrangeModal").modal("show");
            }
        });
        //保存排课信息
        $('#btn-save').click(function () {
            var postData = $('#add-teacherCourse-form').serializeJson();
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
            postData = $.extend(postData, { "TeacherId": $('#hidTeacherId').val() });
            abp.ui.setBusy(
                abp.ajax({
                    context: this,
                    url: abp.appPath + 'CourseArrange/AddTeacherCourseArrange',
                    type: 'POST',
                    data: JSON.stringify(postData) //abp需要进行转换
                }).done(function (res) {
                    console.log(res);
                    // if (typeof res == "string") res = res == "true" ? true : false;
                    if (res != '') {
                        abp.notify.success("排课成功");
                        window.location.reload();
                    }
                }).fail(function (error) {
                    console.info(error);
                    //abp.notify.error(error.responseText);
                })
            );
        });

        //智能搜索
        var students = [];
        $("#scTeacher option").each(function () {
            if ($(this).val() > 0) {
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
            autoFill: false,    //自动填充
            //mustMatch: true,//表示必须匹配条目,文本框里输入的内容,必须是data参数里的数据,如果不匹配,文本框就被清空
            formatItem: function (row, i, max) {
                return "老师：" + row.name;
                // return row.name + '[' + row.to + ']';
            },
            formatMatch: function (row, i, max) {
                return "老师：" + row.name;
                // return row.name + row.to;
            },
            formatResult: function (row) {
                return row.name;
            }
        }).result(function (event, row, formatted) {
            $("#scTeacher").val(row.to);
            $("#hidTeacherId").val(row.to);
            //Bk.TeacherCourseArrange.actions.showTeacherCourses();
        });

        $(".CourseItem").hover(function () {
            $(this).css("border-bottom", "solid 1px yellow");
        }, function () {
            $(this).css("border-bottom", "0px");
        });
    },
    actions: {
        //modal
        //beginPost: function (modalId) {
        //    var $modal = $(modalId);

        //    abp.ui.setBusy($modal);
        //},

        //hideForm: function (modalId) {
        //    var $modal = $(modalId);

        //    var $form = $modal.find("form");
        //    abp.ui.clearBusy($modal);
        //    $modal.modal("hide");
        //    //创建成功后，要清空form表单
        //    $form[0].reset();
        //    $table.bootstrapTable('refresh');
        //},

        //上月
        preMonth: function () {
            //if (Bk.TeacherCourseArrange.actions.validTeacherInput($('#scTeacher').val())) {
            var _year = parseInt($.trim($("#Year").val()));
            var _month = parseInt($.trim($("#Month").val()));
            //如果此时月份是1月的话，则上月为上一年的12月，年减1
            if (_month == 1) {
                $("#Month").val(12);
                $("#Year").val(_year - 1);
            } else {
                $("#Month").val(_month - 1);
            }
            Bk.TeacherCourseArrange.actions.showTeacherCourses();
            //}
        },
        //下月
        nextMonth: function () {
            // if (Bk.TeacherCourseArrange.actions.validTeacherInput($('#scTeacher').val())) {
            var _year = parseInt($.trim($("#Year").val()));
            var _month = parseInt($.trim($("#Month").val()));
            if (_month == 12) {
                $("#Month").val(1);
                $("#Year").val(_year + 1);
            }
            else {
                $("#Month").val(_month + 1);
            }
            Bk.TeacherCourseArrange.actions.showTeacherCourses();
            // }
        },
        //本月
        curMonth: function () {
            var now = new Date();
            $("#Year").val(now.getFullYear());
            $("#Month").val(now.getMonth() + 1);
            Bk.TeacherCourseArrange.actions.showTeacherCourses();
        },
        //导出
        exportExcel: function () {
            abp.notify.info('导出功能暂未开放，程序员正在奋力开发中');
            return;
            var teacherId = $("#scTeacher").val();
            if (Bk.TeacherCourseArrange.actions.validTeacherInput(teacherId)) {
                var yearmonth = $("#Year").val() + "-" + ($("#Month").val() + 1);
                window.open("/CourseArrange/TeacherScheduleByMonthExport?teacherId=" + teacherId + "&yearmonth=" + yearmonth);
            }
        },
        showTeacherCourses: function () { //得到某个老师的课程安排
            var teacherId = $("#scTeacher").val();
            //因为现在系统主要给自己用 如果没有选择老师 默认加载胡老师 亦即id为1的数据   如果开放给其他人用 则需先选择老师 
            teacherId = (teacherId == "" || teacherId == 0) ? 1 : teacherId;
            if (Bk.TeacherCourseArrange.actions.validTeacherInput(teacherId)) {
                var yearmonth = $("#Year").val() + "-" + (parseInt($("#Month").val()));
                console.log("/CourseArrange/TeacherCourseArrange?TeacherId=" + teacherId + "&yearMonth=" + yearmonth);
                window.location.href = "/CourseArrange/TeacherCourseArrange?teacherId=" + teacherId + "&yearMonth=" + yearmonth;
            }
        },
        validTeacherInput: function (teacherId) {
            if (teacherId == "" || teacherId == 0) {
                abp.notify.warn("请先选择老师");
                $('#scTeacher').focus();
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
