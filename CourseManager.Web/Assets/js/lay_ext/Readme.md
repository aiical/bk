	1.可以在使用组建之前 全局化配置一些参数
    //layui.config({
    //    dir: '/res/layui/' //layui.js 所在路径（注意，如果是script单独引入layui.js，无需设定该参数。），一般情况下可以不用这个参数
    //    , version: false //一般用于更新组件缓存，默认不开启。设为true即让浏览器不缓存。也可以设为一个固定的值，如：201610
    //    , debug: false //用于开启调试模式，默认false，如果设为true，则JS模块的节点会保留在页面
    //    , base: '' //设定扩展的Layui组件的所在目录，一般用于外部组件扩展
    //});

	2.layui.define([mods],callback)
	通过这个方法可以定义一个Layui模块，参数mods是可选的，用于声明该模块所依赖的模块，callback指的是 模块加载完毕后的回到函数，它返回一个exports参数，用于输出该模块的接口

	exp:
	layui.define(function(exports){
		//执行相关逻辑
		exports('test',function(){
			console.log('hello world!');
		})
	})


	layui.use([mods],callback)
	layui的内置模块并非默认就加载的，他必须在你执行该方法后才会加载，它的参数跟上面的define方法完全一样
	注：mods 必须是一个合法的模块名，不能包含目录，如果需要加载目录建议使用extend建立别名
	layui.use(['laypage','layedit'],function(){
		var laypage =layui.laypage,layedit=layui.layedit;
		//do...
	})
	该方法的函数其实返回了所加载的模块接口，所以其实也可以不通过layui对象赋值获得接口 
	如：
	layui.use(['laypage','layedit'],function(laypage,layedit){
		//使用分页 
		laypage();
		//初始化编辑器
		layedit.build();
	})

	layui.link(cssPath)
	注：该方法并不是使用Layui所必须的，它一般只是用于动态加载你的外部css文件


	layui.device(key)参数key是可以选的

	var device = layui.device();

	方法/属性
	layui.cache	静态属性。获得一些配置及临时的缓存信息
	layui.getStyle(node, name)	获得一个原始DOM节点的style属性值，如：layui.getStyle(document.body, 'font-size')
	layui.img(url, callback, error)	图片预加载
	layui.extend(options)	拓展一个模块别名，如：layui.extend({test: '/res/js/test'})
	layui.router()	获得location.hash路由，目前在Layui中没发挥作用。对做单页应用会派上用场。
	layui.hint()	向控制台打印一些异常信息，目前只返回了error方法：layui.hint().error('出错啦')
	layui.each(obj, fn)	对象（Array、Object、DOM对象等）遍历，可用于取代for语句
	layui.stope(e)	阻止事件冒泡
	layui.onevent(modName, events, callback)	自定义模块事件，属于比较高级的应用。有兴趣的同学可以阅读layui.js源码以及form模块
	layui.event(modName, events, params) 执行自定义模块事件，搭配onevent使用


	Layui部分组件依赖jQuery（比如layer），但是你并不用去额外加载jQuery。Layui已经将jQuery最稳定的一个版本改为Layui的内部模块，当你去使用layer的时候，它会首先判断你的页面是否已经引入了jQuery，如果没有，则加载内部的jQuery模块，如果有，则不会加载。
