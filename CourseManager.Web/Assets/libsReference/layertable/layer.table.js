/**
* layer组件及扩展
* @author martin
* @namespace BootStrap
* @description 以layer提供的css为基础，实现各种常用组件
* @datetime 20170514
*/
if (!window.BootStrap) {
    window.BootStrap = {};
}

BootStrap.Datagrid = {
    AllGridDataCache: [],
    layer: null,//提供layer操作类
    laypage: null, //提供layer分页类
    DataGridSetData: {
        //DataGrid列配置对象. { field: 'DT', title: '时间格式', width: 70, align: 'left', type: 'date', format: 'yyyy-MM-dd hh:mm:ss',columnStyler:function(index, row){ return 'background-color:Red;'; } }
        columns: undefined,
        //该方法类型请求远程数据，担心数据量大而默认采用post
        method: "POST",
        //顶部工具栏的DataGrid面板。可能的值：1) 一个数组，每个工具属性都和linkbutton一样。2) 字符串ID,选择器指定的工具栏。 
        toolbar: null,
        //如果为true，则在同一行中显示数据；为false，过长就会换行
        nowrap: true,
        //一个URL从远程站点请求数据
        url: null,
        /*当前页的数据*/
        data: null,
        /*所有页的数据，如果url为空，就判断这个是否有数据*/
        gridData: null,
        loadMsg: "正在努力加载数据...",
        title: null,
        //如果为true，则在DataGrid控件底部显示分页工具栏。
        pagination: true,
        rownumbers: false, /*显示行号*/
        checkbox: false, /*显示checkbox*/
        singleSelect: true,
        /*如果为true，单击复选框将永远选择行。如果为false，选择行将不选中复选框。*/
        selectOnCheck: false,
        pageIndex: 1,
        pageSize: 50,
        pageList: [20, 50, 80, 100],
        queryParams: {},
        fitColumns: false,
        hiddenColumns: null,//隐藏列
        /*返回行样式 function (index, row) { return null; }*/
        rowStyler: null,
        /*禁用行 function (index, row) { return false; }*/
        rowDisabler: null,
        /*返回过滤数据显示。该函数带一个参数'data'用来指向源数据,您可以改变源数据的标准数据格式。
        这个函数必须返回包含'total'和'rows'属性的标准数据对象。 function (data) { return null; }*/
        loadFilter: null,
        onLoadSuccess: null,
        onLoadError: null,
        /*在载入请求数据数据之前触发 function () { return true; }*/
        onBeforeLoad: null,
        /*点击一行的时候触发 function (rowIndex, rowData) { }*/
        onClickRow: null,
        /*点击一个单元格的时候触发 function (rowIndex, field, value) { }*/
        onClickCell: null,
        /*勾选一行的时候触发  function (rowIndex, rowData) { }*/
        onCheck: null,
        /*取消勾选一行的时候触发 function (rowIndex, rowData) { }*/
        onUncheck: null,
        /*勾选所有行的时候触发 function (rows) { }*/
        onCheckAll: null,
        /*取消勾选所有行的时候触发 function (rows) { }*/
        onUncheckAll: null,

        //#region  grid的状态，这些值不能手动设置
        /*是否首次加载页面*/
        isFirstLoad: true,
        isCheckOnSelectEvent: false,
        selectTrIndexColl: [],
        /*已经选中列表中的checkbox的索引集合*/
        checkedBoxIndexColl: [],
        /*全选checkbox的状态*/
        cacheGlobalBoxStatus: false,
        /*列表的TableId*/
        tableId: null,
        /*全选checkbox的ID*/
        checkAllId: "ck_all",
        /*是否是点击全选按钮*/
        isCheckAllEvent: false,
        /*构造数据，显示滚动条*/
        isForgeRowData: false,
        /*加载是否完毕*/
        isLoadOver:false
        //#endregion
    },
    TableLayerLoad: null,
    Init: function (tableId, setData) {
        if (tableId.indexOf("#") == -1) {
            BootStrap.Window.Messager.Alert({ title: "错误", msg: tableId + "格式是错误的，必须是#+id的格式" });
            return;
        }
        this.ChcheLayer(setData);
        this.LoadGridMaskLayout(tableId, setData);
        if (this.JudegIsFirstLoad(tableId)) {
            this.FirstLoadAction(tableId);
        }
        this.ResetDataBeforeLoad(tableId);
        this.UpdateSetData(tableId, setData); //更新用户设置的数据
        this.BeginLoadDataEvent(tableId); //生成列表前的事件
        this.GenerateGridBody(tableId);//生成grid布局
        this.GetGridDataByUrlOrLocal(tableId);
    },
    ChcheLayer: function (setData) {
        //缓存layer和laypage对象
        if (!BootStrap.Datagrid.layer) {
            BootStrap.Datagrid.layer = setData.layer || layer;
        }
        if (!BootStrap.Datagrid.laypage) {
            BootStrap.Datagrid.laypage = setData.laypage || laypage;
        }
        setData.layer = null;
        setData.laypage = null;
    },
    LoadGridMaskLayout: function (tId, newSetData) {
        var msg = this.DataGridSetData.loadMsg;
        if (newSetData && newSetData.loadMsg) {
            msg = newSetData.loadMsg;
        } else {
            var currGridData = this.GetGridDataById(tId);
            if (currGridData && currGridData.loadMsg) {
                msg = currGridData.loadMsg; 
            }
        }
        this.TableLayerLoad = BootStrap.Datagrid.layer.load(1, { shade: [0.6, '#ccc'], time: 30 * 1000, title:msg }); //调用遮罩层
    },
    /*根据tableId获取其对应的数据*/
    GetGridDataById: function (tId) {
        var data = BootStrap.Tools.GetDataByLabel(BootStrap.Datagrid.AllGridDataCache, "label", tId);
        if (data) return data.gridSetData;
        return null;
    },
    /*是否是第一次加载，true:是；false:否*/
    JudegIsFirstLoad: function (tId) {
        var gdCache = BootStrap.Datagrid.AllGridDataCache;
        for (var i in gdCache) {
            if (gdCache[i].label == tId) {//存在说明不是第一次加载
                return false;
            }
        }
        return true;
    },
    /*列表第一次初始化执行的方法*/
    FirstLoadAction: function (tId) {
        var grids = BootStrap.Datagrid;
        var tools = BootStrap.Tools;
        grids.AllGridDataCache.push({//缓存的数据
            label: tId,
            gridSetData: tools.Clone(grids.DataGridSetData)
        });
        var currGrid = this.GetGridDataById(tId);
        currGrid.tableId = tId;//记录ID
        currGrid.checkAllId = BootStrap.Tools.GetIdNameTrimOther(tId) + "_ck_all"; //设置全选的checkbox的ID

    },
    /*列表加载完成执行的方法*/
    EndLoadAction: function (tId) {
        var currGridData = this.GetGridDataById(tId);
        var isEixtColumn = this.IsExistColumn(currGridData);
        if (currGridData.isFirstLoad) {//第一次加载就执行
            $(tId).css("display", "none"); //隐藏table
            
        }
        if (currGridData.isForgeRowData) {
            
        }
        currGridData.isFirstLoad = false; //最后设置不是第一次加载
        currGridData.isLoadOver = true; //列表在当前页是否初始化完成
        var hideTime = 100;
        if (currGridData.pageSize >= 50) {
            hideTime = 500;
        }
        setTimeout(function () {
            
            BootStrap.Datagrid.layer.close(BootStrap.Datagrid.TableLayerLoad);
        }, hideTime);
    },
    /*执行之前重置一些数据*/
    ResetDataBeforeLoad: function (tId) {
        var currGridData = this.GetGridDataById(tId);
        currGridData.cacheGlobalBoxStatus = false;//全选按钮为false
        currGridData.checkedBoxIndexColl.length = 0;
    },
    /*更新设置数据*/
    UpdateSetData: function (tId, newSetData) {
        var currGridData = this.GetGridDataById(tId);
        currGridData = $.extend(currGridData, newSetData);
        currGridData.isLoadOver = false;
    },
    BeginLoadDataEvent: function (tId) {
        var currGridData = this.GetGridDataById(tId);
        if (!currGridData.url && !currGridData.gridData) {
            return;
        }
        this.LoadDataBeforeEvent(currGridData);
    },
    LoadDataBeforeEvent: function (currGridData) {
        if (currGridData.onBeforeLoad) {//在载入请求数据数据之前触发
            currGridData.onBeforeLoad();
        }
    },
    /*获取远程或者本地数据*/
    GetGridDataByUrlOrLocal: function (tId) {
        var currGridData = this.GetGridDataById(tId);
        if (!currGridData.queryParams) {
            currGridData.queryParams = {};
        }
        currGridData.queryParams.pageIndex = currGridData.pageIndex;
        currGridData.queryParams.pageSize = currGridData.pageSize;
        currGridData.queryParams.SkipCount = (currGridData.pageIndex-1)* currGridData.pageSize;
        currGridData.queryParams.MaxResultCount = currGridData.pageSize;
        currGridData.queryParams.isFirstLoad = currGridData.isFirstLoad;
        if (currGridData.url) {
            $.ajax({
                url: currGridData.url,
                type: currGridData.method,
                //async: false,
                data: currGridData.queryParams,
                success: function (data) {
                    if (data.success == false) {
                        BootStrap.Datagrid.layer.msg(data.errorMsg, { icon: 2, time: 2000 });
                    }
                    if (typeof data.total == "undefined") {
                        data = data.result;
                    }
                    var hasData = data && data.total > 0;
                    if (currGridData.loadFilter) { //过滤数据
                        if (hasData) {
                            data = currGridData.loadFilter(data);
                        }
                    }
                    currGridData.data = data;
                    currGridData.isForgeRowData = false;
                    //把生成html放到异步里面
                    BootStrap.Datagrid.BeginBuildHtml(tId);
                },
                error: function () {
                    if (currGridData.onLoadError) {
                        currGridData.onLoadError();
                    }
                    //把生成html放到异步里面
                    BootStrap.Datagrid.BeginBuildHtml(tId);
                }
            });
        } else {
            var rows = currGridData.gridData.rows;
            if (currGridData.gridData && rows) {
                currGridData.data = { total: currGridData.gridData.total, rows: [] };
                var begin = (currGridData.pageIndex - 1) * currGridData.pageSize;
                var end = begin + currGridData.pageSize;
                for (var i = begin; i < end; i++) {
                    currGridData.data.rows.push(rows[i]);
                }
            }
            BootStrap.Datagrid.BeginBuildHtml(tId);//生成html函数
        }
    },
    /*开始生成html*/
    BeginBuildHtml: function (tableId) {
        //把生成html放到异步里面
        setTimeout(function () {
            BootStrap.Datagrid.GenerateTableHtml(tableId); //生成列表table的html
            BootStrap.Datagrid.EndLoadDataEvent(tableId); //列表和分页展示完成后执行的函数   
            BootStrap.Datagrid.EndLoadAction(tableId); //列表加载完成执行的额外操作
        }, 0);
    },
    /*生成列表体*/
    GenerateGridBody: function (tId) {
        var currGridData = this.GetGridDataById(tId);
        if (currGridData.isFirstLoad) {
            //如果页面加载很慢,isFirstLoad的值还没被赋值false，再点击查询就可能生成多个toolbar，所以这里多做了一个toolbar是否已经存在的判断
            if ($(currGridData.tableId + "_toolbar").length > 0) {
                return;
            }
            this.AddGridBody(tId); //表头和列表的框架
            //创建各列的宽度样式类
            this.CreateWidthStyle(currGridData);
        }
    },
    /*生成html数据*/
    GenerateTableHtml: function (tId) {
        var currGridData = this.GetGridDataById(tId);
        this.GenerateGridHtml(tId);
        if (currGridData.pagination) {
            var trimId = BootStrap.Tools.GetIdNameTrimOther(tId);

            var pageParam = currGridData.queryParams || currGridData;
            BootStrap.Datagrid.laypage({
                gridid: tId
                , cont: trimId + '_page'
                , pages: parseInt((currGridData.data.total + pageParam.pageSize - 1) / pageParam.pageSize) //总页数
                //, groups: 10 //连续显示分页数
                , curr: pageParam.pageIndex
                , skip: true
                , jump: function (obj, first) {//first一个Boolean类，检测页面是否初始加载
                    var currGridData = BootStrap.Datagrid.GetGridDataById(this.gridid);
                    if (currGridData.isLoadOver && (!currGridData.isFirstLoad)) {
                        BootStrap.Datagrid.Init(this.gridid, { pageIndex: obj.curr });
                    }
                }
            });
        }

    },
    /*生成列表头html*/
    LoadGeneralHead: function (currGridData) {
        var tId = currGridData.tableId;
        var trimTid = BootStrap.Tools.GetIdNameTrimOther(tId);
        var headTemplate = '<thead><tr>%1</tr></thead>';
        var headTd = '<th tableid="' + trimTid + '" field="%3" style="text-align:center;[hidden]" class=" %1">%2</th>';
        var headHtml = [];
        var colunmData = currGridData.columns;
        if (colunmData && colunmData.length > 0) {
            if (!this.IsExistFrozen(currGridData)) {
                var rownumberData = this.GetRownumberData(tId);
                headHtml.push('<th tableid="' + trimTid + '" class="table-rownumbers-thtd" style="padding:0px;width:' + rownumberData.widthPx + 'px;"><div class="body-cell-div-height" style="padding:0;width:' + (rownumberData.widthPx - 6) + 'px;">&nbsp;</div></th>');
                if (currGridData.checkbox) {
                    headHtml.push('<th tableid="' + trimTid + '" style="width:20px;"><input type="checkbox" lay-filter="allselector" lay-skin="primary" lay-ignore="lay-ignore"><div id="' + currGridData.checkAllId + '" class="layui-unselect layui-form-checkbox" lay-skin="primary"><i class="layui-icon"></i></div></th>');
                }
            }

            var headHtmlData = this.GetHeadHtmlData(currGridData, colunmData, headTd);
            headHtml.push(headHtmlData.headHtml.join(" "));
            
            headTemplate = String.format(headTemplate, headHtml.join(" ")); //得到列的表头
            headHtml = null;
        }
        return headTemplate;
    },
    GetHeadHtmlData: function (currGridData, colunmData, headTd) {
        var headHtml = [];
        var noFitCol = currGridData.fitColumns == false;//没有自适应的时候
        $.each(colunmData[0], function (i, d) {
            if (!d) return;
            var isHid = BootStrap.Datagrid.IsHiddenColumn(currGridData, d);
            //if (isHid) {//隐藏不生成html，如果列比较多导致性能差的话
            //    return;
            //}
            var className = "grid_cell_" + d.field;
            var c = String.format(headTd, className, d.title, d.field);
            headHtml.push(c.replace("[hidden]", (isHid ? "display:none;" : "")));
            //headHtml.push(c);
        });
        return { "headHtml": headHtml };
    },
    /*获取列的默认宽度，如果fitColumns=true，如果总的设置宽度大于body的宽度，那么设置就失效，将采用平均宽度*/
    GetColumnDefWidth: function (currGridData) {
        var w = 80;
        if (currGridData.fitColumns) {
            var len = this.GetColumnsCount(currGridData);
            var cut = 0;
            if (currGridData.checkbox) cut += 25;//如果有checkbox，总宽度就要减去
            if (currGridData.rownumbers) {//如果有序号
                cut += 10 * String(currGridData.pageSize * currGridData.pageIndex).length;
            }
            w = Math.floor(($(document.body).width() - cut - 25) / len);//25是滚动条的宽度
        }
        return w;
    },
    GetColumnsCount: function (currGridData) {
        var count = 0;
        if (this.IsExistColumn(currGridData)) {
            var cols = currGridData.columns[0];
            for (var i = 0, len = cols.length; i < len; i++) {
                if (!cols[i]) continue;
                if (!BootStrap.Datagrid.IsHiddenColumn(currGridData, cols[i])) {//排除隐藏列
                    count++;
                }
            }
        }
        return count;
    },
    /*是否是隐藏列*/
    IsHiddenColumn: function (currGridData, colData) {
        return colData.hidden || (currGridData.hiddenColumns && currGridData.hiddenColumns.contains(colData.field));
    },
    /*创建各个列宽度的样式类*/
    CreateWidthStyle: function (currGridData) {
        var style = "";
        var funGetStyle = function (data) {
            var _sty = "";
            if (data && data.length > 0) {
                $.each(data[0], function (i, d) {
                    if (!d) return;
                    var noFitCol = currGridData.fitColumns == false;//没有自适应的时候
                    if (noFitCol && d.width) {
                        //var width = d.width ? d.width : BootStrap.Datagrid.GetColumnDefWidth(currGridData);
                        _sty += ".grid_cell_" + d.field + "{width:" + d.width + "px;} ";
                    }

                });
            }
            return "<style type='text/css' field='bootstrap'>" + _sty + "</style>";
        };
        style += funGetStyle(currGridData.columns);
        $(document.body).append(style); //创建样式
    },
    /*生成列*/
    GetColumnsHtmlByData: function (currGridData, colData, isFrozen) {
        var bodyTrHtml = [];
        var tds = [];
        if (currGridData.data && currGridData.data.rows) {
            var getAlign = function (val) {
                if (val) {
                    return "text-align:" + val + ";";
                } else {
                    return "text-align:left;";
                }
            };
            var rownumberData = this.GetRownumberData(currGridData.tableId); //获取rownumber的数据
            var _tId = BootStrap.Tools.GetIdNameTrimOther(currGridData.tableId); //去掉前面的#
            var isNotForgeRowData = !currGridData.isForgeRowData;
            $.each(currGridData.data.rows, function (index, data) {
                if (!data) return;
                var _rowSytle = "";//行样式
                var styles = "";//获取样式
                var disableRow = false;//是否禁用行
                if (typeof currGridData.rowStyler == "function") {
                    styles = currGridData.rowStyler(index, data) + ";";
                    _rowSytle = " style='" + styles + "'";
                }
                if (typeof currGridData.rowDisabler == "function") {
                    disableRow = currGridData.rowDisabler(index, data);
                }
                tds.length = 0;
                if (true) {
                    tds.push('<td class="table-rownumbers-thtd" style="' + styles + 'width:' + (rownumberData.widthPx) + 'px;">' +
                        (rownumberData.hasNumber && isNotForgeRowData ? String(++rownumberData.beginIndex) : "") + '</td>');//如果是构造数据也不能显示出来
                    if (currGridData.checkbox) {
                        var ck = '<td><input type="checkbox" name="" lay-skin="primary" ' + (disableRow ? "disabled" : "") + ' value="' + data.Id + '"' + ' lay-ignore="lay-ignore"><div  id="' + _tId + '_cb_' + index + '" class="layui-unselect layui-form-checkbox" lay-skin="primary" ' + (disableRow ? 'disabled="disabled"' : "")+'><i class="layui-icon"></i></div></td>';
                        tds.push(ck)
                    }
                }

                $.each(colData[0], function (i, d) {
                    if (!d) return;
                    var isHid = BootStrap.Datagrid.IsHiddenColumn(currGridData, d);//
                    //if (isHid) {//隐藏列就不生成html，因为列多的话，生成这个在ie中渲染太慢了
                    //    return;
                    //}
                    var className = "grid_cell_" + d.field;
                    var nowrapClass = currGridData.nowrap ? "dg-cell-nowrap" : "dg-cell-no-nowrap";
                    var tText = data[d.field];
                    if (d.formatter) {
                        if (isNotForgeRowData) {
                            tText = d.formatter(tText, data, index);
                        }
                    }
                    !(tText != "0" && !tText) || (tText = "&nbsp;");
                    if (d.type == 'date') {//格式化日期
                        tText = BootStrap.Tools.SetDateTimeFormat(tText, d.format);
                    }
                    var fieldName = "field='" + d.field + "'";
                    var hiddenCss = isHid ? "display:none;" : "";
                    var colSty = "";
                    if (d.columnStyler) {//单元格自定义样式
                        colSty = d.columnStyler(index, data);
                        colSty.endsWith(";") || (colSty += ";");
                    }
                    tds.push("<td  class='" + className + "' style=\"" + hiddenCss + styles + (colSty || "") + getAlign(d.align) + ";\" " + fieldName + "><div style='height:auto;' " + fieldName + " class='datagrid-cell body-cell-div-height" + " " + nowrapClass + " " + className + "'>" + tText + "</div></td>");
                });
                //if (!BootStrap.Datagrid.IsExistColumn(currGridData)) {//如果不存在columns也增加一列
                //    tds.push("<td class='datagrid-body-cell table-last-thtd' style='padding:0;'><div class='datagrid-cell body-cell-div-height datagrid-cell-nowrap' style='height:auto;'>　&nbsp;</div></td>");
                //}

                var trDefClass = index % 2 != 0 ? " class='datagrid-tr-even' " : ""; //隔行变色
                bodyTrHtml.push("<tr" + _rowSytle + trDefClass + " dataIndex='" + index + "'>" + tds.join(" ") + "</tr>");
            });
        }
        var trHtmls = bodyTrHtml.join(" ");
        tds = null; bodyTrHtml = null;
        return "<tbody>" + trHtmls + "</tbody>" ;
    },
    /*创建列表区域*/
    AddGridBody: function (tId) {
        //#region html
        var bodyHtml = ' \
    <fieldset class="layui-elem-field"  id="%1_body_panel">\
        <legend>%2</legend>\
        <div class="layui-field-box layui-form"><table id="%1_main_tb" class="layui-table admin-table"></table></div>\
        <div class="fhui-admin-pagelist"><div id="%1_page"></div></div>\
    </fieldset>';
        //#endregion
        var currGridData = this.GetGridDataById(tId);
        var table = $(tId);
        var generalData = currGridData.columns;
        var _tId = BootStrap.Tools.GetIdNameTrimOther(currGridData.tableId); //去掉前面的#
        var $bodyPanel = $(String.format('<div class="admin-main" id="%1_body_all" ></div>', _tId)).html(String.format(bodyHtml, _tId, currGridData.title));
        
        //var $bodyPanel = $(String.format(bodyHtml, _tId, currGridData.title));
        table.before($bodyPanel);
        this.CreateToolBar(currGridData); //创建toolbar
    },
    /*创建ToolBar*/
    CreateToolBar: function (currGridData) {
        if (currGridData.isFirstLoad) {
            if (currGridData.toolbar) {
                var _tId = BootStrap.Tools.GetIdNameTrimOther(currGridData.tableId);
                if (currGridData.toolbar && currGridData.toolbar.length > 0) {//第一次加载要生成toolbar
                    if (typeof currGridData.toolbar == "string") {
                        $("#" + _tId + "_body_panel").before($(currGridData.toolbar).remove());
                    } else {
                        var toolBarContent = $('<blockquote class="layui-elem-quote" id="' + _tId + '_toolbar"></blockquote>');
                        $("#" + _tId + "_body_panel").before(toolBarContent);
                        var toolBarHtml = '<a href="javascript:;" class="layui-btn layui-btn-small" style="%3" %4 %5><i class="%1">&#xe608;</i> %2</a>';
                        $.each(currGridData.toolbar, function () {/*包含的属性{text:'',align:'left',disabled:false,iconCls:'icon-filter',handler:function(){}}*/
                            var thisBar = this;
                            var align = thisBar.align == "left" ? "float:left;" : "";//是否居左
                            var disabled = thisBar.disabled === true ? "disabled" : "";
                            var barId = " id='" + (thisBar.id || "") + "'";
                            var _bar = $(String.format(toolBarHtml, thisBar.iconCls, thisBar.text, align, disabled, barId));
                            toolBarContent.append(_bar);
                            if (!thisBar.disabled) {
                                _bar.click(function () {
                                    thisBar.handler();
                                });
                            }
                        });
                    }
                }
            }

        }
    },
    GenerateGridHtml: function (tId) {
        var currGridData = this.GetGridDataById(tId);
        //生成列的表头
        this.CreateGeneralHeader(currGridData);

        var geneConHtml = "";
        //生成列的数据
        if (this.IsExistColumn(currGridData)) {
            geneConHtml = this.GetColumnsHtmlByData(currGridData, currGridData.columns, false);
        }
        var geneDg = $(tId + "_main_tb");
        var funBuildHtml = function (strHtml, eleTable) {
            if (strHtml) {
                var divPar = eleTable.parent();
                //divPar[0].innerHTML = String.format('<table id="%1" class="layui-table admin-table">%1,%2</table>', $("thead", eleTable)[0].innerHTML, strHtml);
                //eleTable.remove();

                $(eleTable).append(strHtml)
                
            }
        };
        funBuildHtml(geneConHtml, geneDg);//生成右边html
        geneConHtml = null;
        this.IsShowNoDataTip(tId, currGridData);//是否显示没有数据的提示
    },
    /*数据提示，如果没有数据就文字提示*/
    IsShowNoDataTip: function (tId, currGridData) {
        //如果没有数据
        if (!currGridData.data || !currGridData.data.total || currGridData.data.total < 1) {
            setTimeout(function () {
                
            }, 100);
        } else {
            
        }
    },
    /*创建列表头*/
    CreateGeneralHeader: function (currGridData) {
        if (this.IsExistColumn(currGridData)) {
            var generalHeadHtml = this.LoadGeneralHead(currGridData);
            $(currGridData.tableId + "_main_tb").html(generalHeadHtml);
        }
    },
    /*获取rownumber的数据*/
    GetRownumberData: function (tId) {
        var currGridData = this.GetGridDataById(tId);
        var beginIndex = (currGridData.pageIndex - 1) * currGridData.pageSize;
        var hasNumber = currGridData.rownumbers;
        var widthPx = "15"; //根据数字的位数确定rownumber列的宽度
        if (hasNumber) {
            var max = beginIndex + currGridData.pageSize;
            widthPx = (9 * String(max).length); //每个数字宽度为6像素
        }
        return { beginIndex: beginIndex, hasNumber: hasNumber, widthPx: widthPx };
    },
    EndLoadDataEvent: function (tId) {
        var currGridData = this.GetGridDataById(tId);      

        if (typeof currGridData.onLoadSuccess == "function") {
            currGridData.onLoadSuccess(currGridData.data);
        }
        if (currGridData.isForgeRowData) {//如果是构造数据，那么checkbox和行的事件就不需要绑定
            return;
        }
        this.BindGeneralDGEvent(tId, currGridData);
        if (currGridData.checkbox) {//绑定全选事件
            var ckBoxGlo = $("#" + currGridData.checkAllId);
            if (currGridData.singleSelect) {
                ckBoxGlo.click(function () { return false; });
            } else {
                ckBoxGlo.click(function (e) {//单击全选checkbox事件
                    $(this).toggleClass("layui-form-checked");
                    currGridData.isCheckAllEvent = true;
                    var isCheckAll = $(this).hasClass("layui-form-checked"); //获取全选checkbox的状态
                    currGridData.cacheGlobalBoxStatus = isCheckAll; //记录是否选中
                    var ckInTId = currGridData.tableId+"_main_tb";
                    $(ckInTId + ">tbody>tr").each(function (i) {
                        if (currGridData.rowDisabler) {
                            if (currGridData.rowDisabler(i, currGridData.data.rows[i])) {
                                return;
                            }
                        }
                        var _cb = $(this).find("td>.layui-form-checkbox:first");

                        if (_cb.hasClass("layui-form-checked") == isCheckAll) {
                            return;
                        } else {
                            if (currGridData.selectOnCheck) {//如果有涉及到checkbox事件就点击
                                BootStrap.Datagrid.JqClickCheckBox(currGridData.tableId, _cb);
                            } else {
                                if (isCheckAll) {
                                    if (!currGridData.checkedBoxIndexColl.contains(i)) {
                                        currGridData.checkedBoxIndexColl.push(i);
                                    }
                                } else {
                                    if (currGridData.checkedBoxIndexColl.contains(i)) {
                                        currGridData.checkedBoxIndexColl.removeValue(i);
                                    }
                                }
                                if (isCheckAll) {
                                    _cb.addClass("layui-form-checked");
                                } else {
                                    _cb.removeClass("layui-form-checked");
                                }
                            }
                        }
                    });
                    if (isCheckAll) { //是否全选
                        BootStrap.Datagrid.CheckAllBox(currGridData.tableId, currGridData.data.rows);
                    } else {
                        BootStrap.Datagrid.UnCheckAllBox(currGridData.tableId, currGridData.data.rows);
                    }
                    currGridData.isCheckAllEvent = false;
                    BootStrap.Tools.StopBubble(e); //阻止事件冒泡
                });
            }
        }

    },
    /*绑定常规列事件*/
    BindGeneralDGEvent: function (tId, currGridData) {
        if (this.IsExistColumn(currGridData)) {
            $(currGridData.tableId + "_main_tb>tbody>tr").each(function (trIndex) {
                var _tr = $(this);
                BootStrap.Datagrid.BandDgTrClickEvent(currGridData, _tr, false);
                if (currGridData.checkbox) {
                    BootStrap.Datagrid.BandCheckBoxEvent(currGridData, _tr, trIndex, false);
                }

            });

        }
    },
    /*绑定列表行单击事件*/
    BandDgTrClickEvent: function (currGridData, _tr, isLeftDg) {
        _tr.click(function (e) {  
            var trIndex = this.rowIndex;
            if (currGridData.rowDisabler) {//如果禁用行就返回
                if (currGridData.rowDisabler(trIndex, currGridData.data.rows[trIndex])) {
                    return;
                }
            }
            //执行选中行的处理方法
            //var isAddColor = BootStrap.Datagrid.SelectRowHandler(currGridData, trIndex, _tr, isLeftDg, null);
            if (currGridData.onClickCell) {//点击一个单元格的时候触发
                var clickTd = e.srcElement;
                if (clickTd.tagName != "TD") {
                    clickTd = BootStrap.Datagrid.GetClickTDEle(clickTd);
                    if (!clickTd) return;
                }
                var field = clickTd.getAttribute("field");
                if (field) {//如果没有field，说明不是有数据的td，
                    currGridData.onClickCell(trIndex, field, currGridData.data.rows[trIndex][field]);
                }
            }
            BootStrap.Tools.StopBubble(e); //阻止事件冒泡
        });
    },
    /*选中行肯定需要执行的操作，参数selData:{ ok: true, isChecked: true }*/
    SelectRowHandler: function (currGridData, trIndex, tr, isLeftDg, selData) {
        //执行选中行，或者非选中行方法
        var isAddColor = BootStrap.Datagrid.SelOrUnSelRowAction(currGridData, trIndex, tr, isLeftDg, selData);
        if (currGridData.onClickRow) {//单击行事件
            currGridData.onClickRow(trIndex, currGridData.data.rows[trIndex]);
        }
        return isAddColor;
    },
    /*执行选中行，或者非选中行方法,selData的数据是点击checkbox的时候触发选中事件，这个时候就不需要显示编辑 */
    SelOrUnSelRowAction: function (currGridData, trIndex, _tr, isLeftDg, selData) {
        var isAddColor;
        var dg = BootStrap.Datagrid;
        if (selData && selData.ok) {//如果是selectOnCheck触发执行，那么就以checkbox的状态来确定当前行是否需要选中
            isAddColor = selData.isChecked;
            dg.TrToggleClass3(currGridData.tableId, selData.isChecked, trIndex, _tr);
        } else {
            //先执行默认的颜色变化事件,如果isAddColor为true，说明是需要选中checkbox，否则取消选中        
            isAddColor = dg.TrToggleClass(currGridData.tableId, trIndex, _tr); //先改变行颜色，再判断时候有selectOnCheck事件
        }
        return isAddColor;
    },
    /*绑定checkbox事件*/
    BandCheckBoxEvent: function (currGridData, _tr, trIndex, isLeftDg) {
        _tr.find("td>div.layui-form-checkbox:first").click(function (e) {
            $(this).toggleClass("layui-form-checked");
            var isChecked = $(this).hasClass("layui-form-checked");
            /*selectOnCheck如果为true，当用户点击复选框就会改变行的颜色*/
            if (currGridData.selectOnCheck) {
                //执行选中行的处理方法
                var hasClass = _tr.hasClass("active");
                if (hasClass != isChecked) {
                    var selData = { ok: true, isChecked: isChecked };
                    BootStrap.Datagrid.SelectRowHandler(currGridData, trIndex, _tr, isLeftDg, selData);
                }
            }
            BootStrap.Datagrid.ClickCbHandler(isChecked, currGridData, trIndex);
            BootStrap.Tools.StopBubble(e); //阻止事件冒泡
        });
    },
    /*点击checkbox执行的操作，点击单个checkbox，不会执行用户定义的全选或者非全选方法*/
    ClickCbHandler: function (isChecked, currGridData, trIndex) {
        if (isChecked && currGridData.onCheck) {
            currGridData.onCheck(trIndex, currGridData.data.rows[trIndex]);
        }
        if (!isChecked && currGridData.onUncheck) {
            currGridData.onUncheck(trIndex, currGridData.data.rows[trIndex]);
        }
        if (isChecked) {//如果选中，就判断是否已经全选
            if (!currGridData.checkedBoxIndexColl.contains(trIndex)) {
                currGridData.checkedBoxIndexColl.push(trIndex);
            }
            //判断是否已经全选
            if (currGridData.checkedBoxIndexColl.length == currGridData.data.rows.length) {
                BootStrap.Datagrid.ChangeGlobalBoxStatus(currGridData.tableId, true, false);
            }
        } else {
            currGridData.checkedBoxIndexColl.removeValue(trIndex);
            //如果觉得获取dom元素的数据耗时，那么就先判断时候勾选过checkbox，勾选过才需要判断取消全选，如果都没有勾选过，就没必要判断是否取消全选
            BootStrap.Datagrid.ChangeGlobalBoxStatus(currGridData.tableId, false, false);
            if (currGridData.checkedBoxIndexColl.length < 1) {
                BootStrap.Datagrid.UnCheckAllBox(currGridData.tableId, currGridData.data.rows);
            }
        }
    },
    /*获取点击的td*/
    GetClickTDEle: function (parentEle) {
        if (parentEle.tagName == "TR") {
            return null;
        }
        if (parentEle.tagName != "TD") {
            return this.GetClickTDEle(parentEle.parentElement);
        }
        return parentEle;
    },
    IsExistFrozen: function (currGridData) {
        if (!currGridData) return false;
        return currGridData.frozenColumns && currGridData.frozenColumns.length > 0;
    },
    /*是否存在列*/
    IsExistColumn: function (currGridData) {
        if (!currGridData) return false;
        return currGridData.columns && currGridData.columns.length > 0;
    },
    //#region 选中行（单击行就代表选中或取消选中），并修改行的颜色
    /*获取行索引，从0开始*/
    GetRowIndex: function (tId, jqTr) {
        return parseInt(jqTr.attr("dataIndex"));
    },
    /*直接修改颜色：返回true代表做了增加样式操作，false代表不做任何操纵*/
    AddTrColor3: function (jqTr) {
        if (!jqTr.hasClass("active")) {
            jqTr.addClass("active");
            return true;
        }
        return false;
    },
    /**
    * 增加行的颜色
    * @param {Number} index，行索引
    * @param {JqueryObject} jqTr，行tr的jQuery对象
    */
    AddTrColor2: function (tId, index, jqTr) {
        var currGridData = this.GetGridDataById(tId);
        if (!currGridData.selectTrIndexColl.contains(index)) {
            currGridData.selectTrIndexColl.push(index);
        }
        var hasClass = this.AddTrColor3(jqTr);

        if (!currGridData.isCheckAllEvent && currGridData.singleSelect) {
            var dgId = jqTr.parent().parent().attr("id");
            jqTr.siblings().each(function () {
                var rIndex = this.rowIndex;
                if (currGridData.selectTrIndexColl.contains(rIndex)) {
                    BootStrap.Datagrid.RemoveTrColor2(currGridData.tableId, rIndex, $(this));
                    var clickTr = $(currGridData.tableId + "_main_tb")[0].rows[rIndex];
                    BootStrap.Datagrid.RemoveTrColor3($(clickTr));
                }
            });
        }
    },
    /*增加行的颜色*/
    AddTrColor: function (tId, jqTr) {//单独放在一个函数，是为了统计选中的行数量和快速获取某行的值
        var index = this.GetRowIndex(tId, jqTr); //第几行               
        this.AddTrColor2(tId, index, jqTr);
    },
    /**
    * 删除行的颜色
    * @param {Number} index，行索引
    * @param {JqueryObject} jqTr，行tr的jQuery对象
    */
    RemoveTrColor2: function (tId, index, jqTr) {
        var currGridData = this.GetGridDataById(tId);
        if (currGridData.selectTrIndexColl.contains(index)) {
            currGridData.selectTrIndexColl.removeValue(index);
        }
        if (this.RemoveTrColor3(jqTr)) {
            //this.RemoveCheck(currGridData.tableId, jqTr, index); //去掉选中
        }
    },
    /*直接修改颜色：返回true代表做了删除样式操作，false代表不做任何操纵*/
    RemoveTrColor3: function (jqTr) {
        if (jqTr.hasClass("active")) {
            jqTr.removeClass("active");
            return true;
        }
        return false;
    },
    /*checkbox去掉选中*/
    RemoveCheck: function (tId, jqTr, rowIndex) {
        var currGridData = this.GetGridDataById(tId);
        if (currGridData.checkedBoxIndexColl.contains(rowIndex)) {//同时去掉选中
            currGridData.checkedBoxIndexColl.removeValue(rowIndex);
            if (!currGridData.isCheckAllEvent) {//如果是全选事件，就不必要做任何操作，因为全选触发的已经是点击事件，状态会自动改变
                var cb = jqTr.find("td>.layui-form-checkbox:first");
                if (cb.hasClass("layui-form-checked")) {
                    cb.removeClass("layui-form-checked");
                    BootStrap.Datagrid.ChangeGlobalBoxStatus(tId, false, true);
                }
            }
        }
    },
    /*删除行的颜色*/
    RemoveTrColor: function (tId, jqTr) {
        var index = this.GetRowIndex(tId, jqTr);
        this.RemoveTrColor2(tId, index, jqTr);
    },
    /*切换行的颜色,返回true代表是增加颜色，false是代表删除颜色*/
    TrToggleClass: function (tId, index, jqTr) {
        if (jqTr.hasClass("active")) {
            BootStrap.Datagrid.RemoveTrColor2(tId, index, jqTr);
            return false;
        } else {
            BootStrap.Datagrid.AddTrColor2(tId, index, jqTr);
            return true;
        }
    },
    /*指定添加或删除*/
    TrToggleClass2: function (tId, isAdd, jqTr) {
        if (isAdd) {
            if (!jqTr.hasClass("active")) {
                BootStrap.Datagrid.AddTrColor(tId, jqTr);
            }
        } else {
            if (jqTr.hasClass("active")) {
                BootStrap.Datagrid.RemoveTrColor(tId, jqTr);
            }
        }
    },
    /*指定添加或删除*/
    TrToggleClass3: function (tId, isAdd, index, jqTr) {
        if (isAdd) {
            BootStrap.Datagrid.AddTrColor2(tId, index, jqTr);
        } else {
            BootStrap.Datagrid.RemoveTrColor2(tId, index, jqTr);
        }
    },
    //#endregion   
    /*jQuery点击列表前面的checkbox，如果是调用jQuery的click事件，那么就会导致先执行事件，再更新checked的值，因此，要对这个情况作处理*/
    JqClickCheckBox: function (tId, jqBox) {
        var currGridData = this.GetGridDataById(tId);
        jqBox.trigger("click");
    },
    /* 根据条件判断是否勾选checkbox */
    JudgeClickCheckBox: function (tId, jqTr, isCheck) {
        var cb = jqTr.find(".layui-form-checkbox:first");
        var currGridData = this.GetGridDataById(tId);
        if (isCheck != cb.hasClass("layui-form-checked")) {
            currGridData.isCheckOnSelectEvent = true; //记录是selectOnCheck事件
            BootStrap.Datagrid.JqClickCheckBox(tId, cb);
        }
    },
    /*
    * 改变控制全选checkbox的状态
    * @param {Boolean} isChecked，值为true，选中；fals取消
    * @param {Boolean} isActionFun，值为true，需要执行用户定义的全选或者不全选处理方法；fals 不执行用户定义的事件处理方法
    */
    ChangeGlobalBoxStatus: function (tId, isChecked, isActionFun) {
        var currGridData = this.GetGridDataById(tId);
        if (isChecked) {
            if (!currGridData.cacheGlobalBoxStatus) {//如果是不选中
                currGridData.cacheGlobalBoxStatus = true; //记录已经选中
                var globalBox = $("#" + currGridData.checkAllId);
                if (!globalBox.hasClass("layui-form-checked")) {
                    globalBox.addClass("layui-form-checked");
                    BootStrap.Datagrid.CheckAllBox(currGridData.tableId, currGridData.data.rows); //执行用户定义的全选后的事件
                }
            }
        } else {
            if (currGridData.cacheGlobalBoxStatus) {//如果是已经选中才执行
                currGridData.cacheGlobalBoxStatus = false;
                var globalBox = $("#" + currGridData.checkAllId);
                if (globalBox.hasClass("layui-form-checked")) {
                    globalBox.removeClass("layui-form-checked");
                    BootStrap.Datagrid.UnCheckAllBox(currGridData.tableId, currGridData.data.rows); //执行用户定义的全选后的事件
                }
            }
        }

    },
    /*全选checkbox时执行的事件*/
    CheckAllBox: function (tId, rows) {
        var currGridData = this.GetGridDataById(tId);
        if (currGridData.onCheckAll) {
            currGridData.onCheckAll(rows);
        }
    },
    /*取消全选checkbox时执行的事件*/
    UnCheckAllBox: function (tId, rows) {
        var currGridData = this.GetGridDataById(tId);
        if (currGridData.onUncheckAll) {
            currGridData.onUncheckAll(rows);
        }
    },
    /*返回datagrid的Id*/
    GetDgIdByFroData: function (currGridData) {
        var filter = currGridData.tableId+"_main_tb";
        return filter;
    },

    //#region 更新缓存的列表rows数据
    UpdateFieldDataByRowIndex: function (tId, rowIndex, field, value) {
        var currGridData = this.GetGridDataById(tId);
        var len = currGridData.data.rows.length;
        if (len <= rowIndex) {
            BootStrap.Window.Messager.Show({ title: "错误", msg: "超出行索引" });
        } else {
            currGridData.data.rows[rowIndex][field] = value;
        }
    },
    //#endregion
    //#region 获取datagrid各个值的方法
    Options: function (tId) {
        return this.GetGridDataById(tId);
    },
    /* 返回列字段，以数组形式返回*/
    GetColumnFields: function (tId) {
        var currGridData = this.GetGridDataById(tId);
        var fields = [];
        $.each(currGridData.columns[0], function (i, d) {
            if (!d) return;
            fields.push(d.field); //记录列的顺序
        });
        return fields;
    },
    /* 返回指定列属性*/
    GetColumnOption: function (tId, field) {
        var currGridData = this.GetGridDataById(tId);
        return this.GetColumnDataByField(currGridData, field);
    },
    /*根据字段，获取这个列的设置*/
    GetColumnDataByField: function (currGridData, field) {
        var columnData = null;
        var funGetData = function (d) {
            for (var i in d) {
                if (d[i].field == field) return BootStrap.Tools.Clone(d[i]);
            }
            return null;
        };
        if (!columnData && this.IsExistColumn(currGridData)) {
            columnData = funGetData(currGridData.columns[0]);
        }
        return columnData;
    },
    /*加载和显示第一页的所有行。如果指定第二个参数，必须是Json数据，它将取代'queryParams'属性*/
    Load: function (tId) {
        var currGridData = this.GetGridDataById(tId);
        var param = {};
        if (arguments) {
            if (arguments.length > 1) {
                param = arguments[1];
            }
        }
        currGridData.pageIndex = 1;
        var isNull = BootStrap.Tools.JudgeJsonIsNull(param);
        if (!isNull) {
            if (!currGridData.queryParams) {
                currGridData.queryParams = {};
            }
            for (var key in param) {
                if (key) {
                    var val = param[key];
                    if ((typeof val == "string") && val.length > 100) {//查询条件输入大于100字符就删除掉多余的
                        param[key] = val.substring(0, 99);
                    }
                }
            }
            currGridData.queryParams = param;
        }
        BootStrap.Datagrid.Init(currGridData.tableId, {});
    },
    /*重载行。等同于'load'方法，但是它将保持在当前页。*/
    Reload: function (tId, param) {
        BootStrap.Datagrid.Init(this.GetGridDataById(tId).tableId, param);
    },
    /*返回加载完毕后的数据*/
    GetData: function (tId) {
        return BootStrap.Tools.Clone(this.GetGridDataById(tId).data);
    },
    /*修改grid的属性，value是数组*/
    SetGridData: function (tId, attr, value) {
        var currGridData = this.GetGridDataById(tId);
        currGridData[attr] = value;
    },
    /*返回当前页的所有行。*/
    GetRows: function (tId) {
        return BootStrap.Tools.Clone(this.GetGridDataById(tId).data.rows);
    },
    /*根据索引集合获取行的数据*/
    GetByRowIndexs: function (tId, indexColl) {
        var checkRow = [];
        if (indexColl) {
            indexColl.delRepeat();
            $.each(indexColl, function (i, d) {
                if (!d && d != 0) return;
                var _d = BootStrap.Tools.Clone(BootStrap.Datagrid.GetGridDataById(tId).data.rows[d]);
                checkRow.push(_d);
            });
        }
        return checkRow;
    },
    /*根据索引获取某行的数据*/
    GetByRowIndex: function (tId, index) {
        if (typeof index != "number") return null;
        if (index) return BootStrap.Datagrid.GetGridDataById(tId).data.rows[index];
        return null;
    },
    /*返回复选框被选中复选框的所有行。*/
    GetChecked: function (tId) {
        return this.GetByRowIndexs(tId, this.GetGridDataById(tId).checkedBoxIndexColl);
    },
    /*清除所有勾选的行*/
    ClearChecked: function (tId) {
        var currGridData = this.GetGridDataById(tId);
        var filter = this.GetDgIdByFroData(currGridData);
        $(filter + " tr").each(function (i) {
            if (currGridData.checkedBoxIndexColl.contains(i)) {
                var cb = $(this).find(".layui-form-checkbox:first");
                if (cb.hasClass("layui-form-checked")) {
                    BootStrap.Datagrid.JqClickCheckBox(currGridData.tableId, cb); //清除选中，同时触发选中事件
                }
            }
        });
        currGridData.checkedBoxIndexColl.length = 0;
    },
    /*勾选一行，行索引从0开始。*/
    CheckRow: function (tId, index) {
        if (index < 0) return;
        var currGridData = this.GetGridDataById(tId);
        if (!currGridData.checkedBoxIndexColl.contains(index)) {
            currGridData.checkedBoxIndexColl.push(index); //记录选中
            var ck = $(tId + "_cb_" + index);
            if (ck.length > 0 && !ck.hasClass("layui-form-checked")) {
                //这里还有问题，如果设置selectOnCheck，行的颜色没有改变
                BootStrap.Datagrid.JqClickCheckBox(tId, ck);
            }
        }
    },
    /*取消选中行,index从0开始*/
    UncheckRow: function (tId, index) {
        if (index < 0) return;
        var currGridData = this.GetGridDataById(tId);
        if (currGridData.checkedBoxIndexColl.contains(index)) {
            currGridData.checkedBoxIndexColl.removeValue(index); //剔除选中
            var ck = $(tId + "_cb_" + index);
            if (ck.length > 0 && ck.hasClass("layui-form-checked")) {
                BootStrap.Datagrid.JqClickCheckBox(tId, ck);
            }
        }
    },
    /* 设置页面额外数据 setJsonData={displayMsg:"额外数据"} */
    SetPagerDisplayMsg: function (tId, setJsonData) {
        
    },
    /*隐藏列*/
    HideColumn: function (tId, field) {
        this.UpdateColumnDisplay(tId, field, true);
    },
    /*显示列*/
    ShowColumn: function (tId, field) {
        this.UpdateColumnDisplay(tId, field, false);
    },
    UpdateColumnDisplay: function (tId, field, isHide) {
        if (!field) return;
        if (tId.indexOf("#") < 0) {
            BootStrap.Window.Messager.Alert({ showSpeed: 4000, title: "提示", msg: "ID要在前面加上'#'符号" });
        }
        var currGridData = this.GetGridDataById(tId);
        var column = BootStrap.Tools.GetDataByLabel(currGridData.columns[0], "field", field);
        if (column && ((isHide && !column.hidden) || (!isHide && column.hidden))) {
            column.hidden = isHide;//设置列为隐藏或显示
            BootStrap.Datagrid.Reload(tId, {});//重新生成html即可，不需要设置display，那样效率太差
            
        }
    }
    //#endregion
};

