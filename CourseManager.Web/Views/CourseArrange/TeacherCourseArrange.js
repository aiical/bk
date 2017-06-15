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
        _this = this;
        var classname = "";
        $(".colorarea span span").each(function () {
            classname = $(this).attr("class");
            len = $(".MonthArea ul").find("." + classname).length;
            if (len > 0) {
                //$(this).html($(this).html()+"<span class='spancount'>"+len+"</span>");
                var op = $(this).parent().children('.named');
                op.html(op.html() + "(" + len + ")")
            }
        });

        $(".classedit").on("click", function () {
            _this.Url = "/StudentCourse/StuArrangeCourseEditPop?id=" + $(this).attr("tag");
            ymPrompt.win({ message: _this.Url, handler: _this.callBack, width: 800, height: 400, title: '更新排课', iframe: true })
        });

        $(".MonthArea .Status_Rest").on("click", function () {
            var Id = $(this).attr("tag");
            Url = "/Teacher/TeacherArrangeWorkEditPop?Id=" + Id;
            ymPrompt.win({ message: Url, handler: _this.callBack, width: 800, height: 400, title: '教师排班', iframe: true })
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
            //format: 'Y-m-d H:i',
            //formatDate: 'Y-m-d',
            scrollTime: true,
            step:10
        });
        $("body").on("click", function () {
            $("#area").css("display", "none");
        });
        //弹出modal
        $(".btn-add").click(function (e) {
            $("#addTeacherCourseArrangeModal").modal("show");
        });
        //保存排课信息
        $('#btn-save').click(function () {
            var postData = $('add-teacherCourse-form').serializeJson();
            console.info(postData);
            abp.ui.setBusy(
                abp.ajax({
                    url: abp.appPath + 'CourseArrange/AddTeacherCourseArrange',
                    type: 'POST',
                    data: JSON.stringify(postData) //abp需要进行转换
                }).done(function (res) {
                    console.log(res);
                    var result = res.returnData, totalClassHours = result.total;

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
                return row.name + '[' + row.to + ']';
                //return i + '/' + max + ':"' + row.name + '"[' + row.to + ']';
            },
            formatMatch: function (row, i, max) {
                return row.name + row.to;
            },
            formatResult: function (row) {
                return row.name;
            }
        }).result(function (event, row, formatted) {
            $("#scTeacher").val(row.to);
            $("#studentspan").html(row.name);
            Bk.TeacherCourseArrange.actions.showTeacherCourses();

        });

        $(".CourseItem").hover(function () {
            $(this).css("border-bottom", "solid 1px yellow");
        }, function () {
            $(this).css("border-bottom", "0px");
        });
    },
    actions: {
        //modal
        beginPost: function (modalId) {
            var $modal = $(modalId);

            abp.ui.setBusy($modal);
        },

        hideForm: function (modalId) {
            var $modal = $(modalId);

            var $form = $modal.find("form");
            abp.ui.clearBusy($modal);
            $modal.modal("hide");
            //创建成功后，要清空form表单
            $form[0].reset();
            $table.bootstrapTable('refresh');
        },

        //上月
        preMonth: function () {
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
        },
        //下月
        nextMonth: function () {
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
        },
        //本月
        curMonth: function () {
            var now = new Date();
            $("#Year").val(now.getFullYear());
            $("#Month").val(now.getMonth());
            Bk.TeacherCourseArrange.actions.showTeacherCourses();
        },
        //导出
        exportExcel: function () {
            var Id = $("#scTeacher").val();
            var areaId = $("#hidarea").val();
            var areaname = $("#hidareaname").val();
            var yearmonth = $("#Year").val() + "-" + $("#Month").val();
            window.open("/CourseArrange/TeacherScheduleByMonthExport?UId=" + Id + "&yearmonth=" + yearmonth);
        },
        showTeacherCourses: function () {
            var Id = $("#scTeacher").val();
            var yearmonth = $("#Year").val() + "-" + $("#Month").val();
            window.location.href = "/CourseArrange/TeacherCourseArrange?TeacherId=" + Id + "&yearMonth=" + yearmonth;
        },
        arrangeclick: function (obj, time, _col) {
            $("#arragelist").css("display", "block");
            $("#nowday").val(time);
            var teacherId = $("#scTeacher").val();
            $.ajax({
                url: "/Education/GetStuNames",
                data: { TID: teacherId },
                type: "POST",
                dataType: "json",
                cache: false,
                success: function (data) {
                    var details = data;
                    var studentname = details.split('|');
                    var option = "<option value=''>请选择</option>";
                    for (var i = 0; i < studentname.length - 1; i++) {
                        var stuId = studentname[i].split(',')[0];
                        var stuName = studentname[i].split(',')[1];

                        option += "<option value=" + stuId + ">" + stuName + "</option>";
                        $("#scStudent").html(option);
                    }
                }
            });
            if (_col < 4) {
                $("#arragelist").css({ left: $(obj).offset().left + $(obj).outerWidth(), top: $(obj).offset().top }).show();
            } else {
                $("#arragelist").css({ left: $(obj).offset().left - $("#arragelist").outerWidth(), top: $(obj).offset().top }).show();
            }
        },
        //arrangeadd: function (_obj, date) {
        //    _this = this;
        //    _this.TeacherId = $("#scTeacher").val();
        //    //if (_this.TeacherId == "" || _this.TeacherId == "0") {
        //    //    alert("请选择教师！");
        //    //    return;
        //    //}
        //    _this.Url = "/CourseArrange/StudentArrangeCourseAddByTea?AreaId=" + _this.AreaId + "&TId=" + _this.TeacherId + "&sdate=" + date;

        //    ymPrompt.win({ message: _this.Url, handler: _this.callBack, width: 800, height: 400, title: '排课', iframe: true })
        //},
        callBack: function (json) {
            if (json.status == 1 || json._Status) {
                showteacher("");
            }
            //showteacher("");
        },
        mouseover: function (obj) {
            var classname = $(obj).children(':first').attr("class");
            $(".MonthArea ul li").each(function () {
                if (!$(this).hasClass(classname)) {
                    $(this).hide();
                }
            });
        },
        mouseout: function () {
            $(".MonthArea ul li").show();
        },
        out: function () {
            window.location.reload();
        }

    }
}
