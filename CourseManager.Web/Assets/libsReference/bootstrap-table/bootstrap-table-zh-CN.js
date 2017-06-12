(function ($) {
	'use strict';
	$.extend($.fn.bootstrapTable.defaults, {
		formatLoadingMessage: function () {
			return "<img src='../../Assets/images/loading.gif' style='display:inline-block;width:60px; height:60px;' alt='正在努力地加载数据中，请稍候……'>";
		},
		formatRecordsPerPage: function (pageNumber) {
			return '每页显示 ' + pageNumber + ' 条记录';
		},
		formatShowingRows: function (pageFrom, pageTo, totalRows) {
			return '显示第 ' + pageFrom + ' 到第 ' + pageTo + ' 条记录，总共 ' + totalRows + ' 条记录';
		},
		formatSearch: function () {
			return '搜索';
		},
		formatNoMatches: function () {
			return '没有找到匹配的记录';
		}
	});
})(jQuery);