var Bk = window.Bk || {};
Bk.Common = {
    getUrlParam: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
        var r = window.location.search.substr(1).match(reg); //匹配目标参数
        if (r != null) { //返回参数值
            return unescape(r[2]);
        }
        return null;
    },
    replaceSpecialChar: function (source) {
        return source == '' || typeof source == 'undefined'
            ? ""
            : source.replace("\r\n", "<br/>").replace("\n", "<br/>").replace(" ", "&nbsp;");
    },
    resolveNullData: function (obj, def) {
        if (obj == null || obj == 'undefined' || obj == '') {
            if (def == null || def == 'undefined') {
                return "";
            } else {
                return def.trim();
            }
        }
        else {
            if (typeof obj === 'string') return obj.trim();
            return obj;
        }
    },
    truncatChar: function (source, length, replaceStr) {
        if (source == null) return "";
        if (source.length <= length) return source;
        return source.substring(0, length) + replaceStr;
    },
    //格式化显示json日期格式
    formatJsonDate: function (jsonDate) {
        var date = new Date(jsonDate);
        var formatDate = date.toDateString();
        return formatDate;
    }
}
String.prototype.trimEnd = function (trimStr) {
    if (!trimStr) { return this; }
    var temp = this;
    while (true) {
        if (temp.substr(temp.length - trimStr.length, trimStr.length) != trimStr) {
            break;
        }
        temp = temp.substr(0, temp.length - trimStr.length);
    }
    return temp;
};
(function ($) {
    $.fn.serializeJson = function () {
        var serializeObj = {};
        var array = this.serializeArray();
        $(array).each(function () {
            if (serializeObj[this.name]) {
                if ($.isArray(serializeObj[this.name])) {
                    serializeObj[this.name].push(this.value);
                } else {
                    serializeObj[this.name] = [serializeObj[this.name], this.value];
                }
            } else {
                serializeObj[this.name] = this.value;
            }
        });
        return serializeObj;
    };
})(jQuery);



