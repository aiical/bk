/**
* Uploadify 扩展
* @namespace BootStrap.Uploadify
* @description 封装初始化Uploadify组件的公共方法，包括上传常规文件，和上传需要缩略图的图片，使用方式请参考 /Demo/Home/UploadTest页面
*/
var BootStrap = window.BootStrap || {};
BootStrap.Uploadify = {

	/**
	* 上传文件独有的数据
	*/
	UploadFileData: {
		uploader: '/Uploader/UploadFile',
	},
	CommonData: {
		buttonInput: '<button class="btn btn-default glyphicon glyphicon-plus" >%1</button>',
		swf: '/Assets/lib/uploadify/uploadify.swf',
		onUploadSuccess: function (file, data) {
		}
	},
	DefaultSetData: {
		IsNeedShowUploadProgress: false,
		//提交到后台的额外数据
		formData: { flag: "", random: new Date().getTime() },
		//多选
		multi: false,
		auto: true,
		swf: "",
		//fileTypeDesc: '请选择合法文件',
		fileTypeExts: '*.*',//*.gif;*.jpg;*.jpeg;*.png
		//上传按钮显示的文字
		buttonText: '新增',
		//增加的样式
		style: {},
		//增加的css类
		className: '',
		//上传成功执行的方法
		onSuccess: function (file, data) {
		},
        autoSetCss:true
	},
	/**
	* 初始化上传文件组件
	* @param {String} id，初始化input控件的ID
	* @param {Object} setData，上传控件设置数据，Json格式
	*/
	UploadFile: function (id, setData) {
		if (typeof (setData) != 'object') {
			console.log("参数错误");
		}
		var defaultData = this.DataInit(id, setData);//数据初始化
		defaultData = $.extend(defaultData, this.UploadFileData);//初始化上传文件固定的数据
		var uploadInput = this.ModifyInput(id, defaultData);//修改元素样式
		uploadInput.uploadify(defaultData);
		if (defaultData.autoSetCss === true) {
		    this.setUploadCss(id, setData.IsNeedShowUploadProgress);
		}
	},
	setUploadCss: function (id, isNeedShowUploadProgress) {
		var container = $("#" + id);
		if (container.length > 0) {
			var width = container.width() + "px";
			var height = container.height() + "px";
			$('.swfupload', container).attr('height', height);
			$('.uploadify,.swfupload,.uploadify-button', container).css({ "width": width, "height": height, "cursor": "pointer" });
			if (!isNeedShowUploadProgress) {
				container.siblings(".uploadify-queue").css({ "display": "none" });
			}
		}
	},
	/**
	    * 设置数据的初始化调整
	    * @param {Object} setData，上传控件设置数据，Json格式
	    */
	DataInit: function (id, setData) {//数据初始化
		var defaultData = $.extend(BootStrap.Tools.Clone(BootStrap.Uploadify.DefaultSetData), setData);//更新用户设置的数据
		defaultData = $.extend(defaultData, BootStrap.Uploadify.CommonData);//更新公共的固定数据
		defaultData.buttonText = String.format(defaultData.buttonInput, defaultData.buttonText);//设置按钮
		defaultData.formData || (defaultData.formData = {});
		defaultData.formData.inputId = id;//把上传控件的name属性传到后台，方便在成功返回的时候获取
		defaultData.formData.inputName = $("#" + id).attr("name") || "";//把上传控件的name属性传到后台，方便在成功返回的时候获取
		defaultData.formData.multi = defaultData.multi;
		defaultData.formData.exts = defaultData.fileTypeExts;//把格式传到后台判断，正常情况下，前端上传组件已经限制了
		defaultData.onUploadSuccess = function (file, data) {
			var jsonData = data;
			if (typeof (data) != 'object') {
				jsonData = eval("(" + data + ")");
			}
			if (jsonData.inputId && jsonData.inputName) { //生成input，保存文件路径
				var tempId = jsonData.inputName + "Collect";
				var uploadFileCollect = $("#" + tempId);
				var uploadAreasDiv = $("#" + jsonData.inputId);
				if (uploadFileCollect.length < 1) {//建立一个div，存放上传返回的路径
					uploadFileCollect = $("<div style='display:none;' id='" + tempId + "'></div>");
					uploadAreasDiv.append(uploadFileCollect);
				}
				//返回的路径放到一个input中
				var newFileHtml = '<input type="hidden" name="' + jsonData.inputName + '" value="' + jsonData.uploadPath + '"/>';
				if (jsonData.multi) {
					uploadFileCollect.append(newFileHtml);
				} else {
					uploadFileCollect.html(newFileHtml);
				}
				//显示上传的文件
				var queueDiv = $("#" + jsonData.inputName + "-queue");
				if (queueDiv.length > 0) {
					var ul = $(">ul", queueDiv);
					if (ul.length < 1) {
						ul = $("<ul>");
						queueDiv.append(ul);
					}
					ul.append(String.format("<li tag='%1'>%2</li>", jsonData.uploadPath, jsonData.fileName));
				}
			} else {
				console.log("控件必须有ID和name属性");
			}
			defaultData.onSuccess(file, jsonData);
		};
		return defaultData;
	},
	/**
	* 修改上传组件的样式
	* @param {String} id，初始化input控件的ID
	* @param {Object} setData，上传控件设置数据，Json格式
	*/
	ModifyInput: function (id, settingData) {//修改元素样式
		var uploadInput = $("#" + id);
		if (uploadInput.length < 1) {
			console.log("id参数有误（不需要#符号），或者元素id=" + id + "不存在");
			return uploadInput;
		}
		if (BootStrap.Tools.JudgeJsonIsNull(settingData.style)) {
			uploadInput.css(settingData.style);
		}
		if (settingData.className) {
			uploadInput.addClass(settingData.className);
		}
		return uploadInput;
	}
};