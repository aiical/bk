Array.prototype.contains = function (obj) {
	var i = this.length;
	while (i--) {
		if (this[i] === obj) {
			return true;
		}
	}
	return false;
};

var HighChart = {
	ChartDataFormate: {
		FormateNOGroupData: function (data) {
			var categories = [];
			var datas = [];
			for (var i = 0; i < data.length; i++) {
				categories.push(data[i].name || "");
				datas.push([data[i].name, data[i].value || 0]);
			}
			return { category: categories, data: datas };
		},
		FormatGroupData: function (data) {//处理分组数据，数据格式：name：XXX，group：XXX，value：XXX用于折线图、柱形图（分组，堆积）
			var names = new Array();
			var groups = new Array();
			var series = new Array();
			for (var i = 0; i < data.length; i++) {
				if (!names.contains(data[i].name))
					names.push(data[i].name);
				if (!groups.contains(data[i].group))
					groups.push(data[i].group);
			}
			for (var i = 0; i < groups.length; i++) {
				var temp_series = {};
				var temp_data = new Array();
				for (var j = 0; j < data.length; j++) {
					for (var k = 0; k < names.length; k++)
						if (groups[i] == data[j].group && data[j].name == names[k])
							temp_data.push(data[j].value);
				}
				temp_series = { name: groups[i], data: temp_data };
				series.push(temp_series);
			}
			return { category: names, series: series };
		},
		FormatBarLineData: function (data, name, name1) {//数据类型：name：XXX，value：XXX，处理结果主要用来展示X轴为日期的具有增幅的趋势的图
			var category = [];
			var series = [];
			var s1 = [];
			var s2 = [];
			for (var i = 1; i < data.length; i++) {
				if (!category.contains(data[i].name))
					category.push(data[i].name);
				s1.push(data[i].value);
				var growth = 0;
				if (data[i].value != data[i - 1].value)
					if (data[i - 1].value != 0)
						growth = Math.round((data[i].value / data[i - 1].value - 1) * 100);
					else
						growth = 100;
				s2.push(growth);
			}
			series.push({ name: name, color: '#4572A7', type: 'column', yAxis: 1, data: s1, tooltip: { valueStuffix: '' } });
			series.push({ name: name1, color: '#89A54E', type: 'spline', yAxis: 1, data: s2, tooltip: { valueStuffix: '%' } });
			return { series: series };
		}
	},
	ChartOptionTemplates: {
		Pie: function (data, name, title) {
			var pie_datas = HighChart.ChartDataFormate.FormateNOGroupData(data);
			var option = {
				chart: {
					plotBackgroundColor: null,
					plotBorderWidth: null,
					plotShadow: false
				},
				title: {
					text: title || ''
				},
				tooltip: {
					pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
				},
				plotOptions: {
					pie: {
						allowPointSelect: true,
						cursor: 'pointer',
						dataLabels: {
							enabled: false,
							format: '<b>{point.name}</b>: {point.percentage:.1f} %',
							style: {
								color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
							}
						},
						showInLegend: true
					}
				},
				series: [{
					type: 'pie',
					name: name || '',
					data: pie_datas.data
				}]
			};
			return option;
		},
		DrillDownPie: function (data, name, title) {
			var drilldownpie_datas;
			var option = {
				chart: {
					type: 'pie'
				},
				title: {
					text: title || ''
				},
				subtitle: {
					text: '请点击饼图项看详细占比'
				},
				plotOptions: {
					series: {
						dataLabels: {
							enabled: true,
							format: '{point.name}: {point.y:.1f}%'
						}
					}
				},
				tooltip: {
					headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
					pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b> of total<br/>'
				},
				series: [{
					name: name || '',
					colorByPoint: true,
					data: drilldownpie_datas.fir_data
				}],
				drilldown: {
					series: drilldownpie_datas.series
				}
			};
			return option;
		},
		Line: function (data, name, title) {
			var line_datas = HighChart.ChartDataFormate.FormatGroupData(data);
			var option = {
				title: {
					text: title || '',
					x: -20
				},
				subtitle: {
					text: '',
					x: -20
				},
				xAxis: {
					categories: line_datas.category
				},
				yAxis: {
					title: {
						text: name || ''
					},
					plotLines: [{
						value: 0,
						width: 1,
						color: '#808080'
					}]
				},
				tooltip: {
					valueSuffix: ''
				},
				legend: {
					layout: 'horizontal',
					align: 'center',
					verticalAlign: 'bottom'
				},
				series: line_datas.series
			};
			return option;
		},
		Bars: function (data, is_stack, is_stack_percent, name, title) {
			var bars_datas = HighChart.ChartDataFormate.FormatGroupData(data);
			var option = {
				chart: {
					type: 'column'
				},
				title: {
					text: title || ''
				},
				subtitle: {
					text: ''
				},
				credits: {
					enabled: false
				},
				xAxis: {
					categories: bars_datas.category
				},
				yAxis: {
					//min: 0,
					title: {
						text: name
					}
				},
				tooltip: {
					headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
					pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name};</td>' +
					    '<td style="padding:0"><b>{point.y:.1f}</b></td></tr>',
					footerFormat: '</table>',
					shared: true,
					useHTML: true
				},
				plotOptions: {
					column: {
						pointPadding: 0.2,
						borderWidth: 0
					}
				},
				series: bars_datas.series
			};
			var stack_option = {};
			if (is_stack && !is_stack_percent) {
				stack_option = {
					tooltip: {
						formatter: function () {
							return '<b>' + this.x + '</b><br/>' +
						  this.series.name + ': ' + this.y + '<br/>' +
						  'Total: ' + this.point.stackTotal;
						}
					},
					plotOptions: {
						column: {
							stacking: 'normal',
							dataLabels: {
								enabled: true,
								color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'
							}
						}
					}
				};
			}
			if (!is_stack && is_stack_percent) {
				stack_option = {
					tooltip: {
						pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.percentage:.0f}%)<br/>',
						shared: true
					},
					plotOptions: {
						column: {
							stacking: 'percent'
						}
					}
				};
			}
			return $.extend({}, option, stack_option);
		},
		Pyramid: function (data, name, title) {
			var pyramid_datas = HighChart.ChartDataFormate.FormateNOGroupData(data);
			var option = {
				chart: {
					type: 'pyramid',
					marginRight: 100
				},
				title: {
					text: title || '',
					x: -50
				},
				plotOptions: {
					series: {
						dataLabels: {
							enabled: true,
							format: '<b>{point.name}</b> ({point.y:,.0f})',
							color: 'black',
							softConnector: true
						}
					}
				},
				legend: {
					enabled: false
				},
				series: [{
					name: name || '',
					data: pyramid_datas.data
				}]
			};
			return option;
		},
		BarLine: function (data, name, name1, title) {
			var barline_datas = HighChart.ChartDataFormate.FormatBarLineData(data);
			var option = {
				chart: {
					zoomType: 'xy'
				},
				title: {
					text: title || ''
				},
				subtitle: {
					text: ''
				},
				xAxis: [{
					categories: barline_datas.category
				}],
				yAxis: [{ // Primary yAxis
					labels: {
						format: '{value}',
						style: {
							color: '#89A54E'
						}
					},
					title: {
						text: name || '',
						style: {
							color: '#89A54E'
						}
					}
				}, { // Secondary yAxis
					title: {
						text: name1 || '',
						style: {
							color: '#4572A7'
						}
					},
					labels: {
						format: '{value}',
						style: {
							color: '#4572A7'
						}
					},
					opposite: true
				}],
				tooltip: {
					shared: true
				},
				legend: {
					layout: 'horizontal',
					align: 'center',
					verticalAlign: 'bottom'
				},
				series: [{
					name: 'Rainfall',
					color: '#4572A7',
					type: 'column',
					yAxis: 1,
					data: [49.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4],
					tooltip: {
						valueSuffix: ' mm'
					}

				}, {
					name: 'Temperature',
					color: '#89A54E',
					type: 'spline',
					data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6],
					tooltip: {
						valueSuffix: '°C'
					}
				}]
			};
		}
	},
	RenderChart: function (option, container) {
		container.highcharts(option);
	}
};