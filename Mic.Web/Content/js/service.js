/**
 * Created by Kest on 2019/7/7.
 */
//service
(function () {
    function Service() {
    };
    Service.prototype.ajax = function (data, cb, url, isAsync) {
        if (isAsync == null || isAsync == undefined) {
            isAsync = false;
        }
        var that = this;

        $.ajax({
            type: "post",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: data,
            async: isAsync,
            success: function (result) {

                if (cb != null && typeof (cb) == "function") {
                    cb(result);
                }
            },
            error: function (data) {
                var basePath = location.href;
                if (location.pathname.length > 0 && location.pathname != '\/') {
                    basePath = location.href.replace(location.pathname, '');
                }
                location.href = basePath + "/Home/Error500";
            }
        });
    };
    Service.prototype.ChangeModelLayer = function (flag) {
        var findObj = $("body").find("div#SystemModelLayer");
        if (flag) {
            //添加模态框
            if (findObj == null || findObj == undefined || findObj.length == 0) {
                $("body").append('<div id="SystemModelLayer" class="modal-backdrop in">'
                    + '<div class="sk-spinner sk-spinner-three-bounce block_vh_center">'
                    + '<div class="sk-bounce1"></div>'
                    + '<div class="sk-bounce2"></div>'
                    + '<div class="sk-bounce3"></div>'
                    + '</div></div>');
            }
        }
        else {
            if (findObj != null) {
                findObj.remove();
            }
        }
    };

    Service.prototype.FormatDate = function (fmt, date) {
        if (date == undefined || date == null) {
            return "-";
        }
        var o = {
            "M+": date.getMonth() + 1,                 //月份   
            "d+": date.getDate(),                    //日   
            "H+": date.getHours(),                   //小时   
            "m+": date.getMinutes(),                 //分   
            "s+": date.getSeconds(),                 //秒   
            "q+": Math.floor((date.getMonth() + 3) / 3), //季度   
            "S": date.getMilliseconds()             //毫秒   
        };
        if (/(y+)/.test(fmt))
            fmt = fmt.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(fmt))
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        return fmt;
    };
    Service.prototype.ConvertNetDate = function (dateStr) {
        var date = undefined;
        if (dateStr != null && /\/Date\((\d+)\)\//.test(dateStr)) {
            var matchDate = dateStr.match(/\/Date\((\d+)\)\//);
            if (matchDate.length >= 2) {
                date = new Date(Number(matchDate[1]));
            }
        }
        return date;
    };

    Service.prototype.ConvertSecondsTime = function (value) {
        var theTime = parseInt(value);// 秒  
        var theTime1 = 0;// 分  
        var theTime2 = 0;// 小时  
        if (theTime > 60) {
            theTime1 = parseInt(theTime / 60);
            theTime = parseInt(theTime % 60);
            if (theTime1 > 60) {
                theTime2 = parseInt(theTime1 / 60);
                theTime1 = parseInt(theTime1 % 60);
            }
        }
        var result = "";
        if (theTime < 10) {
            result += "0"+parseInt(theTime);
        }
        else result = "" + parseInt(theTime);

        if (theTime1 > 0) {
            result = "" + parseInt(theTime1) + ":" + result;
        }
        if (theTime2 > 0) {
            result = "" + parseInt(theTime2) + ":" + result;
        }
        return result;
    };


    Service.prototype.IsNullOrEmpty = function (obj) {
        if (obj == null || obj == undefined || obj == "null" || obj == "undefined" || obj == "") {
            return true;
        }
        return false;
    };


    Service.prototype.LayerConfirm = function (title, content, callback, param, property) {
        var layerObjProperty = {
            btn: ['确定', '取消'], //按钮
            area: ['auto', '180px'],
            icon: 3,
            title: title,
            skin: 'tempSkin',
            success: function () {
                $(".tempSkin .layui-layer-content").css("height", $(".tempSkin").height() - 36 * 2);
                $(".tempSkin .layui-layer-btn a").removeClass().addClass('bottom_btn');
            }
        };
        if (typeof property != 'undefined') {
            layerObjProperty = $.extend({}, layerObjProperty, property);
        }
        layer.confirm("<div style='color: #222; margin-top: 20px;margin-left:50px;'>"
            + content + "</div>", layerObjProperty, function (index) {
                if (typeof callback == 'function') {
                    callback(param);
                }
                layer.close(index);
            });
    };
    window.Service = new Service();
})();