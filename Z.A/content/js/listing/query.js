$.listing_query = {
    ready: function () {
        if (($.getUrlParam("SKU")||$.getUrlParam("sku")) != null) {
            $("#txtSKU").val($.getUrlParam("SKU") || $.getUrlParam("sku"));
            this.loadData($("#txtSKU").val());
        }
    },
    loadData: function (SKU) {
        //$.ajaxZ({
        //    url: "/Listing/QueryBySKU/",
        //    callback: function (resp) {
        //        if (resp.ok) {

        //        }
        //        else
        //            bootbox.alert(resp.data);
        //    }
        //});                    
        $("#ebayCount").html("(0)");
        $("#amazonCount").html("(0)");
        $("#wishCount").html("(0)");
        $("#aliExpressCount").html("(0)");
        $.ajaxZ({
            url: "/Listing/ListingItemCollect",
            data: { SKU:SKU  },
            callback: function (resp) {
                if (resp.ok) {
                    var ebayCount = 0;
                    var amazonCount = 0;
                    var wishCount = 0;
                    data = JSON.parse(resp.data);
                    var ebayData = [];
                    var amazonData = [];
                    var wishData = [];
                    $.each(data, function (index, value) {
                        $.each(value.datas, function (i,v) {
                            var obj = {};
                            obj.category = v._id;
                            obj.value = v.value.Count;
                            if (value.LX=="eBay") {
                                ebayCount += v.value.Count;
                                ebayData.push(obj);
                            }
                            if (value.LX == "Amazon") {
                                amazonCount += v.value.Count;
                                amazonData.push(obj);
                            }
                            if (value.LX =="Wish") {
                                wishCount += v.value.Count;
                                wishData.push(obj);
                            }
                        })
                    });
                    $("#ebayCount").html("(" + ebayCount + ")");
                    $("#amazonCount").html("(" + amazonCount + ")");
                    $("#wishCount").html("("+ wishCount +")");
                    var tabStrip = $("#tabStrip").kendoTabStrip().data("kendoTabStrip");
                    $("#tabStrip").show();
                    var tabToActivate = $("#tab1");
                    tabStrip.activateTab(tabToActivate);
                    var totalData = [{ category: "EBay", value: ebayCount }, { category: "Amazon", value: amazonCount }, { category: "Wish", value: wishCount }];
                    if (totalData.length>0) {
                        $.listing_query.createTotalChart(totalData);
                    }
                    if (ebayCount > 0) {
                        if ($("#ebayTitle").length==0) {
                            var p = $("#chart_ebay").parent();
                            p.before("<div style='width:100%; text-align:center' id='ebayTitle'><h5>EBay分布</h5></div>");
                        }
                        $.listing_query.createChart("#chart_ebay",ebayData);
                        var ebayTabStrip = $("#ebayTabStrip").kendoTabStrip().data("kendoTabStrip");
                        $("#ebayTabStrip").show();
                        var ebayTabToActivate = $("#ebayZXD");
                        ebayTabStrip.activateTab(ebayTabToActivate);
                        $.listing_query.getEbayZXD(0, 10);
                        $.listing_query.getEbayLSD();
                    }
                    if (amazonCount > 0) {
                        if ($("#amazonTitle").length == 0) {
                            var p = $("#chart_amazon").parent();
                            p.before("<div style='width:100%; text-align:center' id='amazonTitle'><h5>Amazon分布</h5></div>");
                        }
                        $.listing_query.createChart("#chart_amazon", amazonData);
                        var amazonTabStrip = $("#amazonTabStrip").kendoTabStrip().data("kendoTabStrip");
                        $("#amazonTabStrip").show();
                        var amazonTabToActivate = $("#amazonZXD");
                        amazonTabStrip.activateTab(amazonTabToActivate);
                        $.listing_query.getAmazonZXD(0, 10);
                        //$.listing_query.getEbayLSD();
                    }
                    if (wishCount > 0) {
                        if ($("#wishTitle").length == 0) {
                            var p = $("#chart_wish").parent();
                            p.before("<div style='width:100%; text-align:center' id='wishTitle'><h5>Wish分布</h5></div>");
                        }
                        $.listing_query.createChart("#chart_wish", wishData);
                        var wishTabStrip = $("#wishTabStrip").kendoTabStrip().data("kendoTabStrip");
                        $("#wishTabStrip").show();
                        var wishTabToActivate = $("#wishZXD");
                        wishTabStrip.activateTab(wishTabToActivate);
                        $.listing_query.getWishZXD(0, 10);
                        //$.listing_query.getEbayLSD();
                    }
                }
            }
        });
        
    },
    createTotalChart: function (totalData) {
        $("#chart_all").kendoChart({
            series: [{
                type: "pie",
                data: totalData,
            }],
            legend: {
                position: "right",
                labels: {
                    color: "white",
                    font: "20px sans-serif",
                    template: "#: text # : #: value #"
                }
            },
            chartArea: {
                background: "",
                height: 300,
                width: 450
            },
            tooltip: {
                visible: true,
                template: "#= category # : #= value #"
            }
        });
    },
    createChart: function (id,data) {
        $(id).kendoChart({
            series: [{
                type: "pie",
                data: data,
            }],
            legend: {
                position: "right",
                labels: {
                    color: "white",
                    font: "20px sans-serif",
                    template: "#: text # : #: value #"
                }
            },
            chartArea: {
                background: "",
                height: 300,
                width:500
            },
            tooltip: {
                visible: true,
                template: "#= category # : #= value #"
            }
        });
    },
    search:function(){
        if ($("#txtSKU").val() == "" || $("#txtSKU").val() == null) {
            return false;
        }
        else {
            var p = $("#chart_ebay").parent();
            $("#chart_ebay").remove();
            p.append("<div id='chart_ebay'></div>");
            p = $("#chart_amazon").parent();
            $("#chart_amazon").remove();
            p.append("<div id='chart_amazon'></div>");
            p = $("#chart_wish").parent();
            $("#chart_wish").remove();
            p.append("<div id='chart_wish'></div>");
            p = $("#divEBayZXD").parent();
            $("#divEBayZXD").remove();
            p.append("<div id='divEBayZXD'></div>");
            p = $("#divEBayLSD").parent();
            $("#divEBayLSD").remove();
            p.append("<div id='divEBayLSD'></div>");
            p = $("#divAmazonZXD").parent();
            $("#divAmazonZXD").remove();
            p.append("<div id='divAmazonZXD'></div>");
            p = $("#divAmazonLSD").parent();
            $("#divAmazonLSD").remove();
            p.append("<div id='divAmazonLSD'></div>");
            p = $("#divWishZXD").parent();
            $("#divWishZXD").remove();
            p.append("<div id='divWishZXD'></div>");
            p = $("#divWishLSD").parent();
            $("#divWishLSD").remove();
            p.append("<div id='divWishLSD'></div>");
            this.loadData($("#txtSKU").val());
        }
    },
    getEbayZXD: function (pi,ps,sum) {
        $.ajaxZ({
            url: "/Listing/GetListingItem",
            data: {
                saleSite: "EBay",
                sku: $("#txtSKU").val(),
                pi: pi,
                ps: ps
            },
            callback: function (resp) {
                if (resp.ok) {
                    var data = JSON.parse(resp.data);
                    $("#divEBayZXD").html(kendo.template($("#tmpl-ebaylist").html())({ datas: data._data }));
                    $("#pager").toPage(pi,ps,data._total , function (pi) {
                        $.listing_query.getEbayZXD(pi, ps,data._total);
                    });
                    $.each(data._data, function (i, v) {
                        var id = "#" + v.SKU;
                        $(id).kendoDropDownList({
                            dataTextField: "text",
                            dataValueField: "value",
                            dataSource: JSON.parse(v.NextStatus),
                            select: function (e) {
                                var item = e.item;
                                var text = item.text();
                                switch (text) {
                                    case "修改价格&库存":
                                        var html = "<table class='table'>";
                                        html += "<thead><tr><th>SKU</th><th>单价</th><th>库存量</th></tr></thead><tbody>";
                                        if (v.SKU == "多属性") {
                                            $.each(v.Variations.Variation, function (index, value) {
                                                html += "<tr><td><label class='lblSKU' accountID=" + v.UserID + " currency='" + value.StartPrice.CurrencyID + "' name='" + value.SKU + "'>" + value.SKU + "</label></td><td><input type='text' class='price' name='txtPrice" + value.SKU + "'/></td><td><input type='text' class='kcl' name='txtKCL" + value.SKU + "'/></td></tr>";
                                            });
                                        }
                                        else {
                                            html += "<tr><td><label class='lblSKU' accountID=" + v.UserID + " currency='" + v.CurrencyID + "' name='" + v.SKU + "'>" + v.SKU + "</label></td><td><input type='text' class='price' name='txtPrice" + v.SKU + "'/></td><td><input type='text' class='kcl' name='txtKCL" + v.SKU + "'/></td></tr>";
                                        }
                                        html += "</tbody></table><div style='width:220px;padding-left:45%'><button onclick='$.listing_query.save(" + v.ItemID + ")'>保存</button></div>";
                                        $("#UpdateEBayDialog").html(html);
                                        var dialog = $("#UpdateEBayDialog").kendoWindow({
                                            width: 520,
                                            modal: true,
                                            resizable: false,
                                            title: "修改价格&库存",
                                        }).data("kendoWindow");
                                        $(".price").kendoNumericTextBox({});
                                        $(".kcl").kendoNumericTextBox({});
                                        dialog.center().open();
                                        break;
                                    case "下架":
                                        break;
                                    case "查看日志":
                                        break;
                                    default:

                                }
                            }
                        });
                    })

                }
            }
        });
    },
    getEbayLSD:function(){
        $.ajaxZ({
            url: "/Listing/GetListingItem",
            data: { saleSite: "EBay", sku: $("#txtSKU").val(),statu:"Ended" },
            callback: function (resp) {
                if (resp.ok) {
                    datas = JSON.parse(resp.data);
                    $("#divEBayLSD").html(kendo.template($("#tmpl-ebaylist").html())({ datas: datas }));
                }
            }
        })
    },
    getAmazonZXD: function (pi, ps, sum) {
        $.ajaxZ({
            url: "/Listing/GetListingItem",
            data: {
                saleSite: "Amazon",
                sku: $("#txtSKU").val(),
                pi: pi,
                ps: ps
            },
            callback: function (resp) {
                if (resp.ok) {
                    var data = JSON.parse(resp.data);
                    $("#divAmazonZXD").html(kendo.template($("#tmpl-amazonlist").html())({ datas: data._data }));
                    $("#amazonpager").toPage(pi, ps, data._total, function (pi) {
                        $.listing_query.getAmazonZXD(pi, ps, data._total);
                    });
                    $.each(data._data, function (i, v) {
                        var id = "#" + v.SKU;
                        $(id).kendoDropDownList({
                            dataTextField: "text",
                            dataValueField: "value",
                            dataSource: [{ text: "修改价格&库存", value: "0" }, { text: "下架", value: "1" }, { text: "查看日志", value: "2" }],
                            select: function (e) {
                                var item = e.item;
                                var text = item.text();
                                switch (text) {
                                    case "修改价格&库存":
                                        alert("修改价格&库存");
                                        break;
                                    case "下架":
                                        alert("下架");
                                        break;
                                    case "查看日志":
                                        alert("查看日志");
                                        break;
                                    default:
                                }
                            }
                        });
                    })
                }
            }
        });
    },
    getWishZXD: function (pi, ps, sum) {
        $.ajaxZ({
            url: "/Listing/GetListingItem",
            data: {
                saleSite: "Wish",
                sku: $("#txtSKU").val(),
                pi: pi,
                ps: ps
            },
            callback: function (resp) {
                if (resp.ok) {
                    var data = JSON.parse(resp.data);
                    $("#divWishZXD").html(kendo.template($("#tmpl-wishlist").html())({ datas: data._data }));
                    $("#wishpager").toPage(pi, ps, data._total, function (pi) {
                        $.listing_query.getWishZXD(pi, ps, data._total);
                    });
                    $.each(data._data, function (i, v) {
                        var id = "#" + v.SKU;
                        $(id).kendoDropDownList({
                            dataTextField: "text",
                            dataValueField: "value",
                            dataSource: [{ text: "修改价格&库存", value: "0" }, { text: "下架", value: "1" }, { text: "查看日志", value: "2" }],
                            select: function (e) {
                                var item = e.item;
                                var text = item.text();
                                switch (text) {
                                    case "修改价格&库存":
                                        alert("修改价格&库存");
                                        break;
                                    case "下架":
                                        alert("下架");
                                        break;
                                    case "查看日志":
                                        alert("查看日志");
                                        break;
                                    default:
                                }
                            }
                        });
                    })
                }
            }
        });
    },
    variationsDetail: function (e) {
        if ($(e).children()[0].innerHTML == "多属性") {
            if ($("#variationsDetail").length>0) {
                $(e).parent().next().remove();
            }
            else {
                $.ajaxZ({
                    url: "/Listing/GetVariationsByItemID",
                    data: { itemID: e.id },
                    callback: function (resp) {
                        if (resp.ok) {
                            datas = JSON.parse(resp.data);
                            var html = "<tr><td colSpan='12'><div id='variationsDetail'></div></td></tr>";
                            $(e).parent().after(html);
                            $("#variationsDetail").html(kendo.template($("#tmpl-ebayvariations").html())({ datas: datas }));
                        }
                    }
                });
            }
        }
        
    },
    relationshipsDetail: function (e) {
        if ($(e).children()[0].innerHTML == "多属性单") {
            if ($("#relationshipsDetail").length > 0) {
                $(e).parent().next().remove();
            }
            else {
                $.ajaxZ({
                    url: "/Listing/GetRelationshipItems",
                    data: { ASIN: $(e).attr("asin") },
                    callback: function (resp) {
                        if (resp.ok) {
                            datas = JSON.parse(resp.data);
                            var html = "<tr><td colSpan='9'><div id='relationshipsDetail'></div></td></tr>";
                            $(e).parent().after(html);
                            $("#relationshipsDetail").html(kendo.template($("#tmpl-amazonRelationships").html())({ datas: datas }));
                        }
                    }
                });
            }
        }
    },
    variantsDetail:function(e){
        if ($(e).children()[0].innerHTML == "多属性单") {
            if ($("#variationsDetail").length > 0) {
                $(e).parent().next().remove();
            }
            else {
                $.ajaxZ({
                    url: "/Listing/GetVariantsItems",
                    data: { id: $(e).attr("data") },
                    callback: function (resp) {
                        if (resp.ok) {
                            datas = JSON.parse(resp.data);
                            var html = "<tr><td colSpan='9'><div id='variationsDetail'></div></td></tr>";
                            $(e).parent().after(html);
                            $("#variationsDetail").html(kendo.template($("#tmpl-wishVariants").html())({ datas: datas }));
                        }
                    }
                });
            }
        }
    },
    save: function (ItemID) {
        var List = [];
        $.each($(".lblSKU"), function (i, v) {
            var obj = {};
            obj.SKU = v.innerHTML;
            obj.Price = $("input[name=txtPrice" + v.innerHTML + "]").val();
            obj.KCL = $("input[name=txtKCL" + v.innerHTML + "]").val();
            obj.BZDM = $(this).attr("currency");
            obj.ItemID = ItemID;
            obj.AccountID = $(this).attr("accountID");
            List.push(obj);
        });
        $.ajaxZ({
            url: "/Listing/ReviseInventoryStatus",
            data: {Items:JSON.stringify(List)},
            callback: function () {

            }
        });
    }
}