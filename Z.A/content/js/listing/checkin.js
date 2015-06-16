$.listing_checkin = {
    ready: function () {
        var tabStrip = $("#tabStrip").kendoTabStrip().data("kendoTabStrip");
        $("#tabStrip").show();
        var tabToActivate = $("#tab1");
        tabStrip.activateTab(tabToActivate);
        $("#win-checkin").html(kendo.template($("#tmpl-checkin").html()));
        $("#win-checkin").kendoWindow({
            modal: true,
            visible: false,
            resizable: true,
            title: "登记",
            width: 300,
            height: 430,
            open: function (e) {
                $("input:radio[name='chk-checkin']").each(function (i, rdb) {
                    $(rdb).unbind("click");
                    $(rdb).bind("click", function () {
                        $("#btnCheckIn").fadeIn();
                        var value = $(rdb).val();
                        $("#div-checkin-setting").html(kendo.template($("#tmpl-" + value).html()));
                        switch (value) {
                            case "wish":
                                $("#ddlWishAccount").kendoDropDownList({
                                    optionLabel: "请选择账号",
                                    dataTextField: "text",
                                    dataValueField: "value",
                                    height: 300,
                                    dataSource: [
                                        { text: "WishDIS", value: "WishDIS" },
                                        { text: "WishSZ", value: "WishSZ" },
                                        { text: "WishBIX", value: "WishBIX" },
                                        { text: "WishFCT", value: "WishFCT" }
                                    ]
                                });
                                break;
                            case "amazon":
                                $("#ddlAccount").kendoDropDownList({
                                    optionLabel: "请选择账号",
                                    dataTextField: "text",
                                    dataValueField: "value",
                                    height: 300,
                                    dataSource: [
                                        { text: "Amazon.com(美国)", value: "Amazon_US" },
                                        { text: "Amazon.de(德国)", value: "Amazon_DE" },
                                        { text: "Amazon.co.uk(英国)", value: "Amazon_UK" },
                                        { text: "Amazon.co.fr(法国)", value: "Amazon_FR" },
                                        { text: "Amazon.in(印度)", value: "Amazon_IN" },
                                        { text: "Amazon.jp(日本)", value: "Amazon_JP" },
                                        { text: "Amazon.cn(中国)", value: "Amazon_CN" },
                                        { text: "Amazon.ca(加拿大)", value: "Amazon_CA" },
                                         { text: "Amazon.co.es(西班牙)", value: "Amazon_ES" },
                                         { text: "Amazon.co.it(意大利)", value: "Amazon_IT" }
                                    ]
                                });
                                $("#ddlIDKey").kendoDropDownList({
                                    //optionLabel: "请选择类型",
                                    dataTextField: "text",
                                    dataValueField: "value",
                                    height: 300,
                                    dataSource: [
                                        { text: "ASIN", value: "ASIN" },
                                        //{ text: "GCID", value: "GCID" },
                                        //{ text: "UPC", value: "UPC" },
                                        //{ text: "EAN", value: "EAN" },
                                        //{ text: "ISBN", value: "ISBN" },
                                        //{ text: "JAN", value: "JAN" }
                                    ]
                                });
                                break;
                        }
                    });
                });
            },
            close: function (d) {
                //$("#win-checkin").empty();
                $("#checkin-value").val("");
            }
        });

        $.ajaxZ({
            url: "/Listing/GetAccountNum/",
            callback: function (resp) {
                if (resp.ok) {
                    data = JSON.parse(resp.data);
                    for (var i = 0; i < data.length; i++) {
                        switch (data[i]._id) {
                            case "ebay":
                                $("#spaneBay").html(" "+data[i].value.count);
                                break;
                            case "amazon":
                                $("#spanAmazon").html(" "+data[i].value.count);
                                break;
                            case "wish":
                                $("#spanWish").html(" " + data[i].value.count);
                                break;
                            case "aliexpress":
                                $("#spanAliExpress").html(" " + data[i].value.count);
                                break;
                            default:

                        }
                    }

                }
                else {

                }
            }
        })

        $.ajaxZ({
            url: "/Listing/GetAllAccountNum/",
            callback: function (resp) {
                if (resp.ok) {
                    resp.data = JSON.parse(resp.data);
                    $("#btnTodayInsertNum").val(resp.data.TodayInsertNum);
                    $("#btnNotConfirmedNum").val(resp.data.NotConfirmedNum);
                    $("#btnTotalNum").val(resp.data.TotalNum);
                }
            }
        });
        var pi = 0;
        var ps = 10;
        $.listing_checkin.getNotConfirmedList(pi, ps, $("#btnNotConfirmedNum").val());
    },
    checkin: function () {
        var For = $("input:radio[name='chk-checkin']").filter(":checked").val();
        var IDKey = "";
        var IDValue = "";
        var AccountID = "";
        var confirmmsg = "";
        switch (For) {
            case "ebay":
                AccountID = "";
                IDKey = "ITEMID";
                IDValue = $("#checkin-value").val();
                confirmmsg = "<br/>平台:" + For + "<br/>ItemID:" + IDValue;
                if (IDValue == "") {
                    bootbox.alert("您一定是姿式不对!!!");
                    return;
                }
                break;
            case "amazon":
                AccountID = $("#ddlAccount").val();
                IDKey = $("#ddlIDKey").val();
                IDValue = $("#checkin-value").val();
                confirmmsg = "<br/>平台:" + For + "<br/>账号:" + AccountID + "<br/>" + IDKey + ":" + IDValue;
                if (AccountID == undefined || AccountID == "" || IDKey == undefined || IDKey == "" || IDValue == "") {
                    bootbox.alert("您一定是姿式不对!!!");
                    return;
                }
                break;
            case "aliexpress":
                AccountID = "";
                IDKey = "ProductID";
                IDValue = $("#checkin-value").val();
                confirmmsg = "<br/>平台:" + For + "<br/>ProductID:" + IDValue;
                if (IDValue == "") {
                    bootbox.alert("您一定是姿式不对!!!");
                    return;
                }
                break;
            case "wish":
                AccountID = $("#ddlWishAccount").val();
                IDKey = "ProductID";
                IDValue = $("#checkin-value").val();
                confirmmsg = "<br/>平台:" + For + "<br/>账号:" + AccountID + "<br/>" + IDKey + ":" + IDValue;
                if (AccountID == undefined || AccountID == "" || IDValue == "") {
                    bootbox.alert("您一定是姿式不对!!!");
                    return;
                }
                break;
        }
        if (bootbox.confirm("请确认以下信息:" + confirmmsg, function (result) {
            if (result) {
                var url = $("#checkin-value").data["ID"] == null ? "/Listing/DoCheckIn/" : "/Listing/DoCheckIn?Id=" + $("#checkin-value").data["ID"];
                $("#btnCheckIn").ajaxZ({
                url:url,
                data: {
                    For: For,
                    IDKey: IDKey,
                    IDValue: IDValue,
                    AccountID: AccountID,
                    Id: $("#checkin-value").data["ID"]
                },
                callback: function (resp) {
                    if (resp.ok) {
                        $("#win-checkin").data("kendoWindow").close();
                        bootbox.alert("姿式不错，操作成功!");
                        $.listing_checkin.getNotConfirmedList(0, 10, $("#btnNotConfirmedNum").val());
                    }
                    else
                        bootbox.alert(resp.data);
                    }
                });
            }
        }));
    },  
    openDialog: function () {
        var win = $("#win-checkin").data("kendoWindow");
        win.center().open();
    },
    getNotConfirmedList: function (pi, ps,sum) {
        $.ajaxZ({
            url: "/Listing/GetNotConfirmedList",
            data: {
                PI: pi,
                PS: ps
            },
            callback: function (resp) {
                if (resp.ok) {
                    var datas = JSON.parse(resp.data);
                    $("#div-content").html(kendo.template($("#tmpl-list").html())({ datas: datas }));
                    $("#pager").toPage(pi, ps, $("#btnNotConfirmedNum").val(), function (pi) {
                        $.listing_checkin.getNotConfirmedList(pi, ps, $("#btnNotConfirmedNum").val());
                    });
                }
                else
                    bootbox.alert(resp.data);
            }
        });
    },
    openEditor: function (Id) {
        $("#checkin-value").data["ID"] = Id;
        $.listing_checkin.openDialog();
        $.ajaxZ({
            url: "/Listing/GetCheckInById",
            data: {Id:Id},
            callback: function (resp) {
                var data = JSON.parse(resp.data)[0];
                $("input[value=" + data.For + "]")[0].click();
                switch (data.For) {
                    case "wish":
                        var ddlWishAccount = $("#ddlWishAccount").data("kendoDropDownList");
                        ddlWishAccount.value(data.AccountID);
                        break;
                    case "amazon":
                        var ddlAccount = $("#ddlAccount").data("kendoDropDownList");
                        ddlAccount.value(data.AccountID);
                        var ddlIDKey = $("#ddlIDKey").data("kendoDropDownList");
                        ddlIDKey.value(data.IDKey);
                        break;
                }
                $("#checkin-value").val(data.IDValue);
            }
        });
    },
    delect: function (Id) {
        bootbox.confirm("确认删除?", function (result) {
            if (result) {
                $.ajaxZ({
                    url: "/Listing/DelectCheckInById",
                    data: {Id:Id},
                    callback: function (resp) {
                        if (resp.ok) {
                            bootbox.alert("姿式不错，操作成功!");
                            $.listing_checkin.getNotConfirmedList(0, 10, $("#btnNotConfirmedNum").val());
                        }
                        else
                            bootbox.alert(resp.data);
                    }
                });
            }
        });
    },
    getTodayInsertList: function (pi,ps) {
        $.ajaxZ({
            url: "/Listing/GetTodayInsertList",
            data: {
                PI: pi,
                PS: ps
            },
            callback: function (resp) {
                if (resp.ok) {
                    var datas = JSON.parse(resp.data);
                    $("#div-content").html(kendo.template($("#tmpl-list").html())({ datas: datas }));
                    $("#pager").toPage(pi, ps, $("#btnTodayInsertNum").val(), function (pi) {
                        $.listing_checkin.gettodayinsertlist(pi, ps, $("#btnTodayInsertNum").val());
                    });
                    var tabStrip = $("#tabStrip").data("kendoTabStrip");
                    tabStrip.select("li:last");
                }
                else
                    bootbox.alert(resp.data);
            }
        });
    },
    btnNotConfirmedNumClick: function (pi, ps, sum) {
        var tabStrip = $("#tabStrip").data("kendoTabStrip");
        tabStrip.select("li:last");
        $.listing_checkin.getNotConfirmedList(0, 10);
    }
};
