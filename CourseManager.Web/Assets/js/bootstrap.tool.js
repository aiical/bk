/**
* 初始化这里用到的扩展函数，非jquery，
* 有时间把这些扩展改成jquery扩展方法
*/
String.format = function (str) {
	var args = arguments;
	var len = args.length > 9 ? "[10-" + args.length + "]|[1-9]" : "[1-" + args.length + "]";
	var re = new RegExp("%(" + len + ")", "g");
	return String(str).replace(re,
	    function ($1, $2) {
	    	return args[$2];
	    });
};
String.formatMoreThanTenParams = function () {
	if (arguments.length == 0)
		return null;
	var str = arguments[0];
	for (var i = 1; i < arguments.length; i++) {
		var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
		str = str.replace(re, arguments[i]);
	}
	return str;
};

String.htmlEncode = function (str) {
	if (typeof str == "string") {
		return str.replace(/</g, '&lt;').replace(/>/g, '&gt;');
	}
	return str;
};
String.wrap = function (str) {
    return str.replace(/\r\n/g, '<br />').replace(/\n/g, "<br />");
};

//序列号
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

//jquery 1.9之后去掉了这个
if ($.browser === undefined) {
	$.browser = $.browser || {};
	var agent = navigator.userAgent.toLowerCase();
	$.browser.mozilla = /firefox/.test(agent);
	$.browser.webkit = /webkit/.test(agent);
	$.browser.opera = /opera/.test(agent);
	$.browser.msie = /msie/.test(agent);
}

/**
* Jquery对象方法扩展类
*/
(function ($) {
	$.fn.extend({
		/**
		* 判断是否是指定的html标签
		* @param {String} tn，html标签名称
		*/
		isTag: function (tn) {
			if (!tn) return false;
			return $(this)[0].tagName.toLowerCase() == tn ? true : false;
		},
		/**
		* 判断当前元素是否已经绑定某个事件
		* @param {Object} type
		*/
		isBind: function (type) {
			var _events = $(this).data("events");
			return _events && type && _events[type];
		},
		/**
		* 输出firebug日志
		* @param {Object} msg
		*/
		log: function (msg) {
			return this.each(function () {
				if (console) console.log("%s: %o", msg, this);
			});
		}
	});

	/**
	* 扩展String方法
	*/
	$.extend(String.prototype, {
		isPositiveInteger: function () {
			return (new RegExp(/^[1-9]\d*$/).test(this));
		},
		isInteger: function () {
			return (new RegExp(/^\d+$/).test(this));
		},
		isNumber: function () {
			return (new RegExp(/^-?(?:\d+|\d{1,3}(?:,\d{3})+)(?:\.\d+)?$/).test(this));
		},
		trim: function () {
			return this.replace(/(^\s*)|(\s*$)|\r|\n/g, "");
		},
		startsWith: function (pattern) {
			return this.indexOf(pattern) === 0;
		},
		endsWith: function (pattern) {
			var d = this.length - pattern.length;
			return d >= 0 && this.lastIndexOf(pattern) === d;
		},
		replaceSuffix: function (index) {
			return this.replace(/\[[0-9]+\]/, '[' + index + ']').replace('#index#', index);
		},
		trans: function () {
			return this.replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&quot;/g, '"');
		},
		encodeTXT: function () {
			return (this).replaceAll('&', '&amp;').replaceAll("<", "&lt;").replaceAll(">", "&gt;").replaceAll(" ", "&nbsp;");
		},
		replaceAll: function (os, ns) {
			return this.replace(new RegExp(os, "gm"), ns);
		},
		replaceTm: function ($data) {
			if (!$data) return this;
			return this.replace(RegExp("({[A-Za-z_]+[A-Za-z0-9_]*})", "g"), function ($1) {
				return $data[$1.replace(/[{}]+/g, "")];
			});
		},
		replaceTmById: function (_box) {
			var $parent = _box || $(document);
			return this.replace(RegExp("({[A-Za-z_]+[A-Za-z0-9_]*})", "g"), function ($1) {
				var $input = $parent.find("#" + $1.replace(/[{}]+/g, ""));
				return $input.val() ? $input.val() : $1;
			});
		},
		isFinishedTm: function () {
			return !(new RegExp("{[A-Za-z_]+[A-Za-z0-9_]*}").test(this));
		},
		skipChar: function (ch) {
			if (!this || this.length === 0) { return ''; }
			if (this.charAt(0) === ch) { return this.substring(1).skipChar(ch); }
			return this;
		},
		isValidPwd: function () {
			return (new RegExp(/^([_]|[a-zA-Z0-9]){6,32}$/).test(this));
		},
		isValidMail: function () {
			return (new RegExp(/^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/).test(this.trim()));
		},
		isSpaces: function () {
			for (var i = 0; i < this.length; i += 1) {
				var ch = this.charAt(i);
				if (ch != ' ' && ch != "\n" && ch != "\t" && ch != "\r") { return false; }
			}
			return true;
		},
		isPhone: function () {
			return (new RegExp(/(^([0-9]{3,4}[-])?\d{3,8}(-\d{1,6})?$)|(^\([0-9]{3,4}\)\d{3,8}(\(\d{1,6}\))?$)|(^\d{3,8}$)/).test(this));
		},
		isUrl: function () {
			return (new RegExp(/^[a-zA-z]+:\/\/([a-zA-Z0-9\-\.]+)([-\w .\/?%&=:]*)$/).test(this));
		},
		isExternalUrl: function () {
			return this.isUrl() && this.indexOf("://" + document.domain) == -1;
		},
		/*是否包含中文*/
		containsChina: function () {
			return new RegExp(/.*[\u4e00-\u9fa5]+.*$/).test(this);
		}
	});
	$.extend(Array.prototype, {
		contains: function (obj) {
			var i = this.length;
			while (i--) {
				if (this[i] === obj) {
					return true;
				}
			}
			return false;
		},
		containsJson: function (obj, key) {
			var i = this.length;
			while (i--) {
				if (this[i][key] === obj) {
					return true;
				}
			}
			return false;
		},
		removeValue: function (obj) {
			var i = this.length;
			while (i--) {
				if (this[i] === obj) {
					this.splice(i, 1);
					return this;
				}
			}
			return this;
		},
		delRepeat: function () {
			var newArray = [];
			var provisional = {};
			for (var i = 0, item; (item = this[i]) != null; i++) {
				if (!provisional[item]) {
					newArray.push(item);
					provisional[item] = true;
				}
			}
			return newArray;
		}
	});
})(jQuery);

if (!window.BootStrap) {
	window.BootStrap = {};
}
/**
* bootstrap工具类
* @namespace BootStrap.Tools
* @description 给BootStrap组件提供各种公共方法
*/
BootStrap.Tools = {
	/*阻止事件冒泡*/
    StopBubble: function (e) {
        e.preventDefault();
		if (e && e.stopPropagation)
			e.stopPropagation();
		
	},
	/**
	* 清除iframe，并释放内存
	* @param {JqueryObject} tabId，标签页的Id
	*/
	ClearIframe: function (jqFrame) {
		if (jqFrame && jqFrame.length > 0) {
			jqFrame[0].contentWindow.document.write('');
			jqFrame[0].contentWindow.close();
			jqFrame.remove();
			this.IeGc();
		}
	},
	//IE回收内存
	IeGc: function () {
		if ($.browser.msie) {
			CollectGarbage();
		}
	},
	ShowMaskLayout: function (opacity) {
		var maskLayout = $(".mask_layout");
		if (maskLayout.length < 1) {
			maskLayout = $("<div class=\"mask_layout\" style=\"display: none;\"></div>");
			$(document.body).append(maskLayout);
		}
		if (maskLayout.css("display") == "none") {
			maskLayout.css({
				"position": "absolute",
				"top": "0px",
				"left": "0px",
				"margin-left": "0px",
				"margin-top": "0px",
				"background-color": "#CFCFCF",
				"height": function () { return $(document.body).height() + 20; },
				"filter": "alpha(opacity=80)",
				"opacity": opacity ? opacity : "0.8",
				"overflow": "hidden",
				"width": function () { return $(document.body).width() + 22; },
				"z-index": "10"
			});
			maskLayout.show();
			//maskLayout.fadeIn(200);
		}
	},
	HideMaskLayout: function () {
		var maskLayout = $(".mask_layout");
		if (maskLayout.length < 1) {
			return;
		}
		if (maskLayout.css("display") == "none") {
			return;
		}
		maskLayout.hide();
		//$(".mask_layout").fadeOut(200);
	},
	/**
	* 在固定区域显示加载图
	* @param {Number} opacity，透明度，小数表示
	* @param {Number} top，位置类型：absolute，relative
	*/
	ShowMaskLayout2: function (opacity, top) {
		var newTop = String(top);
		if (newTop.indexOf("px") < 0) {
			newTop += "px";
		}
		var maskLayout = $(".grid_mask_layout");
		if (maskLayout.length < 1) {
			maskLayout = $("<div class=\"grid_mask_layout\" style=\"display: none;\"></div>");
			$(document.body).append(maskLayout);
		}
		if (maskLayout.css("display") == "none") {
			maskLayout.css({
				"position": "absolute",
				"top": newTop,
				"background-color": "#000000",
				"height": function () { return $(document).height() - top; },
				"filter": "alpha(opacity=" + (parseInt(opacity) * 100) + ")",
				"opacity": opacity ? opacity : "0.8"
			});
			maskLayout.show();
			//maskLayout.fadeIn(200);
		}
	},
	/*隐藏指定区域的遮罩图*/
	HideMaskLayout2: function () {
		var maskLayout = $(".grid_mask_layout");
		if (maskLayout.css("display") == "none") {
			return;
		}
		maskLayout.hide();
		//$(".mask_layout").fadeOut(200);
	},
	ShowDragMask: function (zIndex) {
		zIndex || (zIndex = 4);
		var dm = $("body>.drag-mask");//显示遮罩层
		if (dm.length < 1) {
			var proxy = $("<div class='drag-mask' style=''></div>").css({ "width": BootStrap.Tools.GetScreenWidth(), "height": BootStrap.Tools.GetScreenHeight() });
			$("body").append(proxy);
		} else {
			dm.css({ "z-index": String(zIndex) }).show();
		}
	},
	HiddenDragMask: function () {
		$("body>.drag-mask").css({ "z-index": "-1" }).hide();//隐藏遮罩层
	},
	GetStyleSheetByArrry: function (styleSheetArr) {
		var style = [];
		$.each(styleSheetArr, function () {
			style.push(this.key + this.value);
		});
		return style.join(" ");
	},
	/*创建样式*/
	CreateStyle: function (styleSheetArr) {
		if ($.browser.msie) {
			var sheet = document.createStyleSheet();
			$.each(styleSheetArr, function () {
				sheet.addRule(this.key, this.value);
			});
		} else {
			var styleStr = this.GetStyleSheetByArrry(styleSheetArr);
			var style = document.createElement('style');
			style.type = 'text/css';
			style.innerHTML = styleStr; // "body{ background-color:blue }";
			document.getElementsByTagName('HEAD').item(0).appendChild(style);
		}
	},
	/*创建样式*/
	CreateStyle2: function (strStyleSheet) {
		$(document.body).append(strStyleSheet);
	},
	/*获取json数据的所有key*/
	ReadJsonKeys: function (jsonData) {
		var keys = [];
		if (!jsonData) {
			return keys;
		}
		for (var key in jsonData) {
			//alert("key：" + key + ",value：" + jsonData[key]);
			keys.push(key);
		}
		return keys;
	},
	/*
	* 判断Json是否为空
	* @param {object} jsonData，json数据
	* return true：为空；false不为空
	*/
	JudgeJsonIsNull: function (jsonData) {
		if (!jsonData) {
			return true;
		}
		for (var key in jsonData) {
			if (key) return false;
		}
		return true;
	},
	/*根据标识获取对应的数据*/
	GetDataByLabel: function (arrJsonData, label, value) {
		var data;
		for (var i in arrJsonData) {
			if (arrJsonData[i][label] == value) {
				data = arrJsonData[i];
				data.index = i;
				break;
			}
		}
		return data;
	},
	/**
	* Ajax获取数据
	* @param {setJsonData} Ajax的设置数据，格式：{url:"/home/getdata", type:"POST", params:{id:1}}
	* @return 返回结果是json数据 {result: true,data:{}}
	*/
	GetDataByAjax: function (setJsonData) {
		var returnData = null;
		var isSuccess = false;
		if (!setJsonData.params) {
			setJsonData.params = {};
		}
		$.ajax({
			url: setJsonData.url,
			type: setJsonData.type,
			async: false,
			data: setJsonData.params,
			success: function (data) {
				returnData = data;
				isSuccess = true;
			},
			error: function () {

			}
		});
		return {
			result: isSuccess,
			data: returnData
		};
	},
	/*克隆数据，防止被污染*/
	Clone: function (obj) {
		if (typeof (obj) != 'object') return obj;
		if (obj == null) return obj;
		if ((obj instanceof Array) && obj.length != undefined) {
			//浅复制
			return obj.concat();
		}
		var newObj = new Object();
		for (var i in obj)
			newObj[i] = this.Clone(obj[i]);
		return newObj;
	},
	/*获取浏览器的高度*/
	GetScreenHeight: function () {
		return (document.documentElement && document.documentElement.clientHeight || window.innerHeight || document.body.clientHeight);
	},
	/*获取浏览器的宽度*/
	GetScreenWidth: function () {
		var docEle = document.documentElement;
		return (docEle && docEle.clientWidth || window.innerWidth || document.body.clientWidth);
	},
	/*获取Id，去掉特殊字符：# .（类标记） 等*/
	GetIdNameTrimOther: function (id) {
		if (!id) return "";
		return String(id).replace("#", "").replace(".", "");
	},
	/*设置日期格式，类似C#设置日期格式一样*/
	SetDateTimeFormat: function (dtValue, dtFormat) {
		function getFormatDate(date, dateformat) {
			if (isNaN(date)) return null;
			var format = dateformat;
			var o = {
				"M+": date.getMonth() + 1,
				"d+": date.getDate(),
				"h+": date.getHours(),
				"m+": date.getMinutes(),
				"s+": date.getSeconds(),
				"q+": Math.floor((date.getMonth() + 3) / 3),
				"S": date.getMilliseconds()
			};
			if (/(y+)/.test(format)) {
				format = format.replace(RegExp.$1, (date.getFullYear() + "")
			  .substr(4 - RegExp.$1.length));
			}
			for (var k in o) {
				if (new RegExp("(" + k + ")").test(format)) {
					format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k]
				  : ("00" + o[k]).substr(("" + o[k]).length));
				}
			}
			return format;
		}
		if (!dtValue) return "";
		// /Date(1328423451489)/
		if (typeof (dtValue) == "string" && /^\/Date/.test(dtValue)) {
			dtValue = dtValue.replace(/^\//, "new ").replace(/\/$/, "");
			dtValue = eval(dtValue);
		}
		if (dtValue instanceof Date) {
			dtFormat = dtFormat || "yyyy-MM-dd";
			return getFormatDate(dtValue, dtFormat);
		}
		else {
			return String(dtValue);
		}
	}
};