/**
* layer上传组件h5
* @namespace BootStrap
* @description 以layer提供的css为基础，实现h5上传
*/
if (!window.BootStrap) {
    window.BootStrap = {};
}

BootStrap.Uploadify = {
    /**
	* 上传文件独有的数据
	*/
    UploadFileData: {
        uploaderUrl: '/Uploader/UploadFile',
        html: '<dl class="allClassify">\
                <dt class="materialImg">\
                    <img class="img-album img-responsive" alt="-150X150" src="%4" data-holder-rendered="true">\
                    <input id="img_index_%3" type="hidden" name="%1" value="">\
                </dt>\
                <dd>%2</dd>\
                <a href="javascript:void(0);" class="delmaterial" onclick="return BootStrap.Uploadify.delImg(this)"></a>\
                <div class="layui-progress layui-progress-big" lay-showpercent="true" lay-filter="progress_%3"><div class="layui-progress-bar layui-bg-red" lay-percent="0%"></div></div>\
            </dl>',
        textHtml: '<span class="layui-upload-icon"><i class="layui-icon"></i>%1</span>',
        containerHtml:'<div class="Template"><div class="uploadingImg"><div class="uploadify"><div class="site-demo-upbar"></div></div></div></div>'
    },
    //文件索引值
    FileIndex: 0,
    //layer的进度条组件
    LayerElement:null,
    DefaultSetData: {
        isShowProgress: true,
        //多选
        multi: false,
        //fileTypeDesc: '请选择合法文件',
        fileTypeExts: 'image/jpeg,image/png',//*.gif,*.jpg,*.jpeg,*.png
        //上传按钮显示的文字
        buttonText: '上传图片',
        //增加的样式
        style: {},
        //增加的css类
        className: '',
        maxSize: 7,//单位是MB
        maxCount: 100,//支持上传文件数量
        element: null, //layer的element组件，element = layui.element();
        //上传成功执行的方法
        onSuccess: function (file, data) {
        }
    },
    CacheData:[],
    init: function (id, options) {
        if (!this.validateBrower()) {
            return;
        }
        if (id.indexOf("#") == 0) {
            id=id.replace("#","");
        }
        var settingData = this.dataInit(id, options);
        this.buildContainer(id, settingData);
        this.setInput(id, settingData);
        document.getElementById(id).addEventListener("change", function () {
            for (var i = 0; i < this.files.length; i++) {
                BootStrap.Uploadify.uploadFiles(this.id,this.files[i]);
            }
        }, false);
    },
    validateBrower: function (setData) {
        var isFalse = typeof FormData === 'undefined';
        if (isFalse) {
            var msg = "抱歉，你的浏览器不支持html5上传，请更换更高版本或谷歌浏览器";
            if (layer && layer.open) {
                layer.open({
                    content: msg, skin: 'msg', time: 3
                });
            } else {
                alert(msg)
            }
        }
        return !isFalse;
    },
    dataInit: function (id, setData) {//数据初始化
        var defaultData = $.extend(BootStrap.Tools.Clone(BootStrap.Uploadify.DefaultSetData), setData);//更新用户设置的数据
        var $input = $("#" + id);//maxcount
        defaultData.maxCount = parseInt($input.attr("maxcount") || "100");//数量以在页面设置的数量为主
        defaultData.formData || (defaultData.formData = {});
        defaultData.formData.inputId = id;//把上传控件的name属性传到后台，方便在成功返回的时候获取
        defaultData.formData.inputName = $input.attr("name") || "";//把上传控件的name属性传到后台，方便在成功返回的时候获取
        defaultData.formData.multi = defaultData.multi;
        defaultData.formData.exts = defaultData.fileTypeExts;//把格式传到后台判断，正常情况下，前端上传组件已经限制了
        this.setLayer(defaultData);
        if (!BootStrap.Tools.GetDataByLabel(BootStrap.Uploadify.CacheData, "key", id)) {
            BootStrap.Uploadify.CacheData.push({ "key": id, "value": defaultData });
        }
        return defaultData;
    },
    setLayer: function (setData) {
        if (!BootStrap.Uploadify.LayerElement) {
            BootStrap.Uploadify.LayerElement = setData.element || layui.element();
        }
    },
    buildContainer: function (id, setData) {
        var $inputButton = $("#" + id).parent();
        if ($inputButton.parents(".uploadify").length < 1) {
            var container = $(BootStrap.Uploadify.UploadFileData.containerHtml);
            $inputButton.before(container);
            var siteUpBar = $(".site-demo-upbar", container);
            siteUpBar.append($("span", $inputButton).remove());
            siteUpBar.prepend($inputButton.remove());
        }        
    },
    setInput: function (id, settingData) {//修改元素样式
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
        uploadInput.parent().append(String.format(BootStrap.Uploadify.UploadFileData.textHtml, settingData.buttonText));
        if (settingData.multi) {
            uploadInput.attr("multiple", "multiple");
        }
        if (settingData.fileTypeExts) {
            uploadInput.attr("accept", settingData.fileTypeExts);
        }
        return uploadInput;
    },
    setStyle: function (eachBlock) {
        
    },
    //上传文件入口
    uploadFiles: function (id, blob) {
        var fileSize = BootStrap.Uploadify.convertSize(blob.size);
        var setData = BootStrap.Tools.GetDataByLabel(BootStrap.Uploadify.CacheData, "key", id).value;
        if (setData.maxSize && setData.maxSize < fileSize) {
            layer.open({
                content: "抱歉，文件大小不能超过" + setData.maxSize+"MB", skin: 'msg', time: 2000
            });
            return;
        }
        if (setData.maxCount && setData.maxCount != 100) {
            var maxCount = $("#" + id).parents(".uploadingImg").siblings(".allImg").children("dl").length;
            if (maxCount >= setData.maxCount) {
                layer.open({
                    content: "最多只能上传" + maxCount+"个", skin: 'msg', time: 2000
                });
                BootStrap.Uploadify.clearInputFile(setData);//清空上传文件
                return;
            }
        }
        BootStrap.Uploadify.showImg(id, blob);
        if (BootStrap.Uploadify.isImage(blob) && fileSize > 1.5) { //   //图片大于1.5M的时候才进行压缩
            BootStrap.Uploadify.uploadFilesWidthCompress(id,blob);
        } else {
            BootStrap.Uploadify.uploadFilesDefault(id,blob);
        }
    },
    //默认上传方式：没有压缩
    uploadFilesDefault: function (id, blob) {
        var fileIndex = BootStrap.Uploadify.FileIndex;
        var fd = new FormData();
        fd.append("files", blob);
        fd.append("videoCoverDpi", "500X330");
        fd.append("fileName", BootStrap.Uploadify.getFileNameByBlob(blob));
        BootStrap.Uploadify.setCommonFormData(id, fd);
        var xhr = new XMLHttpRequest();
        xhr.target = "progress_" + String(fileIndex);
        xhr.open("POST", BootStrap.Uploadify.UploadFileData.uploaderUrl, true);
        xhr.onloadstart = function () {
            BootStrap.Uploadify.Progress.init(xhr.target);
        }
        xhr.onload = function (e) {
            if (this.status == 200) {
                //ReleaseDynamic.HasUploadedSize += (blob.size || 0);//上传完成就统计上传的总字节
                BootStrap.Uploadify.uploadSuccess(this.responseText);
            }
        };
        xhr.onerror = function (e) {
            BootStrap.Uploadify.uploadError();
        };
        xhr.timeout = 1000 * 60 * 60;
        xhr.upload.onprogress = function (e) {
            var _data = BootStrap.Tools.GetDataByLabel(BootStrap.Uploadify.Progress.loadData, "id", xhr.target);
            if (_data) {
                _data.progress = (e.loaded / e.total) * 100;
                console.log(xhr.target + "-" + String(e.loaded) + "-" + String(_data.progress));
            }
        };
        //xhr.loadEnd = function (e) {
        //    ReleaseDynamic.HasUploadedSize += e.total;
        //};
        xhr.ontimeout = function (e) {
            layer.open({
                content: "超时了", skin: 'msg', time: 10
            });

        };
        xhr.send(fd);
    },
    //压缩上传
    uploadFilesWidthCompress: function (id,blob) {
        var reader = new FileReader();
        var fileName = BootStrap.Uploadify.getFileNameByBlob(blob);
        var fileType = blob.type || ("image/" + fileName.substr(fileName.lastIndexOf('.') + 1).toLowerCase());
        var fileSize = BootStrap.Uploadify.convertSize(blob.size);
        var fileIndex = BootStrap.Uploadify.FileIndex;
        reader.onload = function (e) {
            var res = this.result
            var img = new Image();//高度限制4000像素
            img.onload = function () {
                var cvs = document.createElement('canvas'),
                    ctx = cvs.getContext('2d');
                cvs.width = img.width;
                cvs.height = img.height;
                ctx.clearRect(0, 0, cvs.width, cvs.height);
                ctx.drawImage(img, 0, 0, img.width, img.height);
                var dataUrl = cvs.toDataURL(fileType, (fileSize>4?0.7:0.8));//'image/jpeg'//'image/jpeg'
                // 上传
                var sendData = dataUrl.replace("data:" + fileType + ";base64,", '');
                var fd = new FormData();
                fd.append("base64String", sendData);
                fd.append("filename", fileName);
                BootStrap.Uploadify.setCommonFormData(id, fd);
                var xhr = new XMLHttpRequest();
                xhr.target = "progress_" + String(fileIndex);
                xhr.open("POST", BootStrap.Uploadify.UploadFileData.uploaderUrl, true);
                xhr.onloadstart = function () {
                    BootStrap.Uploadify.Progress.init(xhr.target);
                };
                xhr.onload = function (e) {
                    if (this.status == 200) {
                        BootStrap.Uploadify.uploadSuccess(this.responseText);
                    }
                };
                xhr.onerror = function (e) {
                    BootStrap.Uploadify.uploadError();
                };
                xhr.timeout = 1000 * 60 * 60;
                xhr.upload.onprogress = function (e) {
                    var _data = BootStrap.Tools.GetDataByLabel(BootStrap.Uploadify.Progress.loadData, "id", xhr.target);
                    if (_data) {
                        _data.progress = (e.loaded / e.total)*100;
                        console.log(xhr.target + "-" + String(e.loaded)+"-" + String(_data.progress));
                    }
                };
                xhr.send(fd);
            }
            img.src = res;
        };
        reader.readAsDataURL(blob);//调用readAsDataURL()方法时，将file对象返回的数据块
    },
    setCommonFormData: function (id, formData) {
        var setData = BootStrap.Tools.GetDataByLabel(BootStrap.Uploadify.CacheData, "key", id).value;
        if (setData && setData.formData) {
            for (var k in setData.formData) {
                var v = setData.formData[k];
                if (v) {
                    formData.append(k, v);
                }
            }
        }
    },
    uploadError: function () {
        layer.open({
            content: "上传出错了", skin: 'msg', time: 3
        });
    },
    uploadSuccess: function (result) {
        var jsonData = (typeof result === "string" ? eval("(" + result + ")") : result);
        if (jsonData.isSuccess) {
            if (jsonData.inputId && jsonData.inputName) { //生成input，保存文件路径
                var setData = BootStrap.Tools.GetDataByLabel(BootStrap.Uploadify.CacheData, "key", jsonData.inputId).value;
                if (setData.onSuccess) {
                    setData.onSuccess(jsonData);
                }
                //jsonData.uploadPath.replace(/:/g, ",")
                var currImgBock = $("#img_index_" + setData.FileIndex);
                var fPaths = jsonData.uploadPath.split(':');
                if (jsonData.multi) {
                    currImgBock.val(fPaths.join(","));
                } else {
                    currImgBock.val(fPaths[0]);
                }     
                
            } else {
                console.log("控件必须有ID和name属性");
            }


            ///////////////////
        } else {
            BootStrap.Uploadify.uploadError();
        }
        BootStrap.Uploadify.clearInputFile(setData);
    },
    clearInputFile: function (setData) {
        $("#" + setData.formData.inputId).val("");
    },
    getFileNameByBlob: function (blob) {
        var fname = blob.name;
        if (blob.name.indexOf('.') < 0) {//如果文件名称没有后缀，得给fileName变量把后缀加上，辣鸡安卓经常没有后缀
            if (blob.name.toUpperCase().indexOf("VIDEO") > -1) {
                fname = blob.name + ".mp4";
            } else {
                fname = blob.name + ".jpg";
            }
        }
        return fname;
    },
    isImage: function (blob) {
        return blob.type.indexOf("image/") > -1 || blob.name.toUpperCase().indexOf("IMAGE") == 0
    },
    convertSize: function (origin) {
        if (origin)
            return parseFloat(origin / (1024 * 1024));
        return 0;
    },
    delImg: function (o) {
        $(o).parents("dl").remove();
        return false;
    },
    showImg: function (id, blob) {
        BootStrap.Uploadify.FileIndex++;
        BootStrap.Uploadify.Progress.loadData.push({ "id": "progress_" + String(BootStrap.Uploadify.FileIndex), "progress": 0 });//初始化滚动条数据
        var setData = BootStrap.Tools.GetDataByLabel(BootStrap.Uploadify.CacheData, "key", id).value;
        setData.FileIndex = BootStrap.Uploadify.FileIndex;
        var imgUrl = BootStrap.Uploadify.isImage(blob) ? URL.createObjectURL(blob) : "/Assets/images/filetype/" + (blob.name.substr(blob.name.lastIndexOf('.') + 1)) + ".png";
        var imgBlock = String.format(BootStrap.Uploadify.UploadFileData.html, setData.formData.inputName, blob.name, BootStrap.Uploadify.FileIndex, imgUrl);
        var container = $("#" + setData.formData.inputId).parents(".uploadingImg");
        var allImgBlock = container.siblings(".allImg");
        if (!allImgBlock.length) {
            allImgBlock = $('<div class="allImg cf"></div>');
            container.parent().append(allImgBlock);
        }
        allImgBlock.append(imgBlock);
    },
    Progress: {
        loadData:[],
        interval:0,
        init: function (id) {
            //模拟loading
            BootStrap.Uploadify.LayerElement.progress(id, '1%');
            var timer = setInterval(function () {
                var _d = BootStrap.Tools.GetDataByLabel(BootStrap.Uploadify.Progress.loadData, "id", id);
                if (_d.progress >= 100) {
                    _d.progress = 100;
                    clearInterval(timer);
                }
                BootStrap.Uploadify.LayerElement.progress(id, _d.progress + '%');
            }, 1000);
        },
        setProgree: function (id) {

        }
    }
}