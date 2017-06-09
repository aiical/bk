var Bk = window.Bk || {};
Bk.Common = {
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


