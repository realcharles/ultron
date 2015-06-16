jQuery.extend({
    loading: function (msg) {
        bootbox.setDefaults({
            size: "medium"
        });
        if (msg) {
            $.loadingForm = bootbox.dialog({
                message: "<div style='text-align:center;'>" + msg + "</div><div style='text-align:center;'><img src='../../content/image/loading.gif' /></div>"
            });
        }
        else {
            $.loadingForm=bootbox.dialog({
                message: "<div style='text-align:center;'><img src='../../content/image/loading.gif' /></div>"
            });
        }
        return $.loadingForm;
    },
    loaded: function (loadingForm) {
        if (loadingForm)
            loadingForm.modal('hide');
        else if ($.loadingForm)
            $.loadingForm.modal('hide');
        else
            bootbox.hideAll();
    },   
    ajaxZ: function (option) {
        if (!option.type)
            option.type = "GET";
        var loadingForm;
        var txt = "";
        $.ajax({
            type:option.type,
            url: option.url,
            data: option.data,
            dataType:"json",
            beforeSend: function () {
                if (option.sender) {
                    txt = $(option.sender).text();
                    $(option.sender).attr("disabled", true);
                    $(option.sender).text("处理中...");
                }
                else
                    loadingForm = $.loading();
                if (option.before)
                    option.before.call();
            }
        }).done(function (resp) {
            if (option.sender) {
                $(option.sender).attr("disabled", false);
                $(option.sender).text(txt);
            }
            else
                $.loaded(loadingForm);
            if (option.callback)
                option.callback.call(this, resp);
        });
    },
    getUrlParam: function (name) {
        var reg
         = new RegExp("(^|&)" +
         name + "=([^&]*)(&|$)");
        var r
         = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    },
    rnd: function (max) {
        var now = new Date();
        return now.getSeconds() % max;
    },
    //随机数
    random: function (min, max) {
        return Math.floor(Math.random() * (max - min)) + min;
    },
    RGB2Color: function (r, g, b) {
        return '#' + $.byte2Hex(r) + $.byte2Hex(g) + $.byte2Hex(b);
    },
    //随机生成RGB颜色
    byte2Hex: function (n) {
        var nybHexString = "0123456789ABCDEF";
        return String(nybHexString.substr((n >> 4) & 0x0F, 1)) + nybHexString.substr(n & 0x0F, 1);
    },
    // 比较时间的方法，d1时间比d2时间大，则返回true 
    compare:function(d1, d2) {   
        return Date.parse(d1.replace(/-/g, "/")) > Date.parse(d2.replace(/-/g, "/"));
    }
});

jQuery.fn.extend({    
    ajaxZ: function (option) {
        option.sender = this;
        $.ajaxZ(option);
    },
    toPage: function (pi, ps, sum, pageFN) {
        var max = 10;
        var count = Math.ceil(sum / ps);
        if (count <= 1)
            return;
        var nav = $("<nav></nav>");
        var ul = $("<ul class='pagination'></ul>");
        var pre = $("<li><a href='#' aria-label='Previous'><span aria-hidden='true'>&laquo;</span></a></li>");
        var next = $("<li><a href='#' aria-label='Next'><span aria-hidden='true'>&raquo;</span></a></li>");
        if (pi == 0)
            pre.addClass("disabled");      
        if (pi == count-1)
            next.addClass("disabled");
        ul.append(pre);
        
        for (var i = 0; i < count; i++) {
            if (i >= max)
                continue;
            var li = $("<li pi='" + i + "'><a href='#'>" + (i + 1) + "</a></li>");
            if (i == pi)
                li.addClass("active");
            else {
                $(li).unbind('click');
                $(li).bind('click', function () {
                    pageFN.call(this, parseInt($(this).attr("pi")));
                });
            }
            ul.append(li);
        }
        if (i >= max) {
            var miss = $("<li><a href='#'>...</a></li>");
            ul.append(miss);
            var last = $("<li pi='" + (count - 1) + "'><a href='#'>" + (count - 1) + "</a></li>");
            $(last).unbind('click');
            $(last).bind('click', function () {
                pageFN.call(this, parseInt($(this).attr("pi")));
            });
            ul.append(last);
        }
        ul.append(next);
        nav.html(ul);

        if (pi - 1 >= 0) {
            $(pre).unbind('click');
            $(pre).bind('click', function () {
                pageFN.call(this, pi - 1);
            });
        }
        if (pi + 1 < count) {
            $(next).unbind('click');
            $(next).bind('click', function () {
                pageFN.call(this, pi + 1);
            });
        }
        this.html(nav);
    }
});