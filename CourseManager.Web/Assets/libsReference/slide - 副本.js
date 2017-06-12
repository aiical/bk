var timer = null;//定时器
var offset = 5000;//时间间隔
var index = -1; /* -1第一个，0第二个... */
var IFutureSlide = window.IFutureSlide || {};
IFutureSlide.BootStrap = {
	//大图交替轮换
	slideImage: function (i) {
		var id = 'image_' + target[i];
		$('#' + id).animate({ opacity: 1 }, 800, function () {
			$(this).find('.word').animate({ height: 'show' }, 'slow');
		}).show()
			.siblings(':visible')
			.find('.word').animate({ height: 'hide' }, 'fast', function () {
				$(this).parent().animate({ opacity: 0 }, 800).hide();
			});
	},
	//绑定小图 的a标签添加单击事件 切换大图显示
	hookThumb: function () {
		$('#thumbs li a').bind('click', function () {
			//if (timer) {
			//	clearTimeout(timer);
			//}
			$(this).parent().css({ opacity: 1 }).siblings().css({ opacity: 0.75 });
			$('.btnArrow').css({ opacity: 1 });
			var id = this.id;
			index = IFutureSlide.BootStrap.getIndex(id.substr(6));
			IFutureSlide.BootStrap.rechange(index);
			IFutureSlide.BootStrap.slideImage(index);
			timer = window.setTimeout(this.auto, offset);
			this.blur();
			return false;
		});
	},
	//前 后箭头 事件绑定
	hookBtn: function () {
		$('#thumbs li img').filter('#play_prev,#play_next').bind('click', function () {
			if (timer) {
				clearTimeout(timer);
			}
			var id = this.id;
			if (id == 'play_prev') {
				index--;
				if (index < 0) index = 6;
			} else {
				index++;
				if (index > 6) index = 0;
			}
			IFutureSlide.BootStrap.rechange(index);
			IFutureSlide.BootStrap.slideImage(index);
			timer = window.setTimeout(this.auto, offset);
		});
	},
	//大图上也需要左右箭头的时候 触发
	bighookBtn: function () {
		$('#bigpicarea p span').filter('#big_play_prev,#big_play_next').bind('click', function () {
			if (timer) {
				clearTimeout(timer);
			}
			var id = this.id;
			if (id == 'big_play_prev') {
				index--;
				if (index < 0) index = 6;
			} else {
				index++;
				if (index > 6) index = 0;
			}
			IFutureSlide.BootStrap.rechange(index);
			IFutureSlide.BootStrap.slideImage(index);
			timer = window.setTimeout(auto, offset);
		});
	},

	getIndex: function (v) {
		for (var i = 0; i < target.length; i++) {
			if (target[i] == v) return i;
		}
	},

	rechange: function (loop) {
		var id = 'thumb_' + target[loop];
		$('#thumbs li a.current').removeClass('current');
		$('#' + id).addClass('current');
	},

	auto: function () {
		index++;
		if (index > 6) {
			index = 0;
		}
		$('#thumbs li.slideshowItem').eq(index).css({ opacity: 1 }).siblings().css({ opacity: 0.75 });
		$('.btnArrow').css({ opacity: 1 });
		IFutureSlide.BootStrap.rechange(index);
		IFutureSlide.BootStrap.slideImage(index);
		timer = window.setTimeout(IFutureSlide.BootStrap.auto, offset);
	}
};