$.collector_nc = {
    ready: function () {
        $("#btnNC_New").click(function () {
            var div = $("<div></div>");
            div.html(kendo.template($("#tmpl-newnc-step1").html()));
            $.collector_nc.WinNewNC = div.kendoWindow({
                modal: true,
                visible: false,
                resizable: true,
                title: "新采集",
                width: 300,
                height: 300,
                open: function (e) {
                    var now = new Date();
                    $("#txtNC_Name").val("NC"+now.getFullYear()+now.getMonth()+now.getDay()+now.getHours()+now.getMinutes()+now.getSeconds());
                    $("#btnNC_NextStep").click(function () {
                        $.collector_nc.NC = {
                            Name: $("#txtNC_Name").val(),
                            Minutes: $("input:radio[name='rdb-nc-execdt']").filter(":checked").val()
                        };
                        if ($.collector_nc.NC.Name == "") {
                            bootbox.alert("随便写个名称吧");
                            return;
                        }
                        $.collector_nc.WinNewNC.content(kendo.template($("#tmpl-newnc-step2").html()));
                        $("input:radio[name='chk-newnc']").each(function (i, rdb) {
                            $(rdb).unbind("click");
                            $(rdb).bind("click", function () {
                                $("#btnNC_OK").fadeIn();
                                var value = $(rdb).val();
                                $("#div-nc-setting").html(kendo.template($("#tmpl-" + value).html()));
                                switch (value) {
                                    case "amazon":
                                        $("#ddlAmazonSite").kendoDropDownList({
                                            optionLabel: "请选择站点",
                                            dataTextField: "text",
                                            dataValueField: "value",
                                            height: 300,
                                            dataSource: [
                                                { text: "Amazon.com(美国)", value: "US" },
                                                { text: "Amazon.de(德国)", value: "DE" },
                                                { text: "Amazon.co.uk(英国)", value: "UK" },
                                                { text: "Amazon.co.fr(法国)", value: "FR" },
                                                { text: "Amazon.in(印度)", value: "IN" },
                                                { text: "Amazon.jp(日本)", value: "JP" },
                                                { text: "Amazon.cn(中国)", value: "CN" },
                                                { text: "Amazon.ca(加拿大)", value: "CA" },
                                                 { text: "Amazon.co.es(西班牙)", value: "ES" },
                                                 { text: "Amazon.co.it(意大利)", value: "IT" }
                                            ]
                                        });

                                }
                            });
                        });
                    });
                },
                close: function (d) {
                    div.empty();
                }
            }).data("kendoWindow");
            $.collector_nc.WinNewNC.center().open();
        });
        $.collector_nc.loadNC();
    },
    newNC: function () {
        $.collector_nc.NC.For = $("input:radio[name='chk-newnc']").filter(":checked").val();
        $.collector_nc.NC.Value = $("#nc-value").val();
        switch ($.collector_nc.NC.For) {
            case "ebay":
                $.collector_nc.NC.Key = "ITEMID";
                break;
            case "amazon":
                $.collector_nc.NC.Key = "ASIN";
                $.collector_nc.NC.For += "-" + $("#ddlAmazonSite").val();
                break;
        }
        if ($.collector_nc.NC.Value == "") {
            bootbox.alert("木有数据啊!!!");
            return;
        }
        $("#btnNC_OK").ajaxZ({
            type:"POST",
            url: "/Collector/NewNC/",
            data: $.collector_nc.NC,
            callback: function (resp) {
                if (resp.ok) {
                    $.collector_nc.WinNewNC.close();
                    $.collector_nc.loadNC();
                }
                else
                    bootbox.alert(resp.data);
            }
        });
    },
    loadNC: function () {
        $.ajaxZ({
            url: "/Collector/GetMyNC/",
            callback: function (resp) {
                if (resp.ok) {
                    var setting = {
                        view: {
                            selectedMulti: false,
                            nameIsHTML: true
                        },
                        callback: {
                            onClick: $.collector_nc.onTreeClick
                        }
                    };
                    var data = JSON.parse(resp.data);
                    var tree = $.fn.zTree.init($("#tree"), setting, data.datas);
                    var nodes = tree.getNodesByFilter(function (node) {
                        return node.level == 0;
                    });
                    $(nodes).each(function (i, node) {
                        tree.expandNode(node, true, false, false);
                    });
                    $("#div-nc-sum").html("30天内采集任务:"+data.sum);
                }
                else {
                    $("#tree").html(resp.data);
                }
            }
        });
    },
    onTreeClick: function (event, treeId, treeNode) {
        if (treeNode.level == 2) {
            $("#div-nc-detail").fadeIn();
            $.ajaxZ({
                url: "/Collector/GetNC/" + treeNode.id,
                callback: function (resp) {
                    if (resp.ok) {
                        var data = JSON.parse(resp.data);
                        $("#div-nc-detail").html(kendo.template($("#tmpl-nc-detail").html())(data));
                        $(data.Logs).each(function (i, log) {
                            $("#ullogs").append("<li>"+log+"</li>");
                        });
                        if (data.Result.Ok) {
                            $("#btnDo").hide();
                        }
                    }
                    else {
                        bootbox.alert(resp.data);
                    }
                }
            });
        }
        else {
            $("#div-nc-detail").empty();
            $("#div-nc-detail").fadeOut();
        }
    },
    deleteNC: function (id) {
        bootbox.confirm("确定要删除吗？", function (result) {
            if (result) {
                $.ajaxZ({
                    url: "/Collector/DeleteNC/" + id,
                    callback: function (resp) {
                        if (resp.ok) {
                            $.collector_nc.loadNC();
                            $("#div-nc-detail").empty();
                            $("#div-nc-detail").fadeOut();
                        }
                        else {
                            bootbox.alert(resp.data);
                        }
                    }
                });
            }
        });
    },
    doNC: function (id) {
        bootbox.confirm("确定要执行吗?", function (result) {
            if (result) {

            }
        });
    }
}