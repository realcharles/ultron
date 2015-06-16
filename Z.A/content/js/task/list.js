$.task_list = {
    ready: function () {
        $.task_list.refresh();
        //setInterval($.task_list.refresh, 5*60000);
    },
    refresh: function () {
        $.ajaxZ({
            url: "/Task/GetTasks/",
            callback: function (resp) {
                if (resp.ok) {
                    $("#list").html(kendo.template($("#tmpl-list").html())(JSON.parse(resp.data)));
                }
                else {
                    bootbox.alert(resp.data);
                }
            }
        });
    },
    run: function (id) {
        bootbox.confirm("确定要运行[" + id + "]吗?", function (result) {
            if (result) {
                $.ajaxZ({
                    url: "/Task/Run/" + id,
                    callback: function () {
                        $.task_list.refresh();
                    }
                });
            }
        });
    }
}