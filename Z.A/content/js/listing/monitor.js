$.listing_monitor = {    
    ready: function () {        
        $.listing_monitor.loadTree();
    },
    loadTree: function () {
        $.ajaxZ({
            url: "/Listing/GetMonitorTree/",
            callback: function (resp) {
                if (resp.ok) {
                    var setting = {
                        view: {
                            selectedMulti: false,
                            nameIsHTML:true
                        },
                        callback: {
                            onClick: $.listing_monitor.treeOnClick
                        }
                    };
                    $.fn.zTree.destroy();
                    $.listing_monitor.zTree = $.fn.zTree.init($("#div-tree"), setting, JSON.parse(resp.data));
                    $.listing_monitor.zTree.expandAll(true);
                    $.listing_monitor.getTreeTips();
                }
                else
                    bootbox.alert(resp.data);
            }
        });
    },
    getTreeTips: function () {
        var zTree = $.listing_monitor.zTree;
        $.ajaxZ({
            url: "/Listing/GetTreeTips/",
            callback: function (resp) {
                if (resp.ok) {
                    var datas = JSON.parse(resp.data);
                    $(datas).each(function (i, data) {                       
                        var node = zTree.getNodeByParam("CKDM",data._id,null);
                        if (node) {
                            $(node.children).each(function (i, snode) {
                                switch (snode.key) {
                                    case "OOS":
                                        if (data.value.OOS > 0) {
                                            snode.name = "<span title='库存不足'>" + snode.name + "<span class='label label-danger'>&nbsp" + data.value.OOS + "&nbsp</span></span>";
                                        }
                                        snode.sum = data.value.OOS;
                                        break;
                                    case "AQKCL":
                                        if (data.value.AQKCL > 0) {
                                            snode.name = "<span title='低于安全库存量'>" + snode.name + "<span  class='label label-warning'>&nbsp" + data.value.AQKCL + "&nbsp</span></span>";
                                        }
                                        snode.sum = data.value.AQKCL;
                                        break;
                                    case "Good":
                                        if (data.value.Good > 0) {
                                            snode.name = "<span title='库存充足'>" + snode.name + "<span class='label label-success'>&nbsp" + data.value.Good + "&nbsp</span></span>";
                                        }
                                        snode.sum = data.value.Good;
                                        break;
                                }
                                zTree.updateNode(snode);
                            });
                        }
                       
                    });
                }
            }
        });
    },
    treeOnClick: function (event, treeId, treeNode) {
        switch (treeNode.level) {
            case 1:
                var node = treeNode.getParentNode();
                var ckdm = node.CKDM;
                var sum = treeNode.sum;
                var pi = 0;
                var ps = 10;
                if ($(node).data[treeNode.key + ckdm]) {
                    $.listing_monitor.showTable($(node).data[treeNode.key + ckdm], pi, ps, sum, function (pi) {
                        $.listing_monitor.getTreeDetail(ckdm, treeNode.key, pi, ps, sum, node, title);
                    });
                }
                else {
                    $.listing_monitor.getTreeDetail(ckdm, treeNode.key, pi, ps,sum, node, node.name + treeNode.name + "SKU");
                }
                break;
            default:
                $("#di9v-content").empty();
                break;
        }
       
    },
    getTreeDetail: function (ckdm,key,pi,ps,sum,node,title) {
        $.ajaxZ({
            url: "/Listing/GetTreeDetail/",
            data: {
                CKDM: ckdm,
                Key: key,
                PI: pi,
                PS:ps
            },
            callback: function (resp) {
                if (resp.ok) {
                    var datas = JSON.parse(resp.data);
                    $(node).data[key+ckdm] = {datas:datas,title:title};
                    $.listing_monitor.showTable($(node).data[key + ckdm], pi, ps, sum, function (pi) {
                        $.listing_monitor.getTreeDetail(ckdm, key, pi, ps, sum, node, title);
                    });
                }
                else
                    bootbox.alert(resp.data);
            }
        });
    },
    showTable: function (data,pi,ps,sum,pageFun) {
        $("#div-content").html(kendo.template($("#tmpl-list").html())({datas:data.datas,title:data.title}));
        $("#pager").toPage(pi, ps, sum, pageFun);
    },
    viewListing: function (sku) {
        window.open("/Listing/Query?SKU=" + sku, "listing-query-"+sku);
    }
 
}