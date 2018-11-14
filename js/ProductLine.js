$(function () {

    layui.use(['table', 'layer'], function () {
        var table = layui.table, layer = layui.layer;
        $("#Search").click(function () {
            var NameSearch = $("#Name").val().trim();
            table.render({
                elem: '#tw'
                , url: '/SystemSettings/SeProductLine?Name=' + NameSearch           
                , page: true
                , limit: 15
                , cols: [[
                    , { field: 'id', width: 42, title: '序号' }
                    , { field: 'line', width: 235, title: '线名',edit: Text}
                    , { field: 'CreatedBy', width: 210, title: '创建人' }
                    , { field: 'CreatedTime', width: 235, title: '创建时间' }
                    , { field: 'UpdatedBy', width: 210, title: '更新人', edit: Text }
                    , { field: 'UpdateTime', width: 235, title: '更新时间' }
                    , { field: 'StatusID', width: 210, title: '状态' }
                    , { field: 'right', width: 178, align: 'center',toolbar: '#barDemo'}
                ]]
            });
        });
        table.on('tool(demo)', function (obj) { //注：tool 是工具条事件名，test 是 table 原始容器的属性 lay-filter="对应的值"
            var data = obj.data //获得当前行数据
                ,layEvent = obj.event; //获得 lay-event 对应的值
            var name = data.line
                , id = data.id           
            if (layEvent === 'detail') {
                layer.msg('查看操作');
            } else if (layEvent === 'del') {
                layer.confirm('真的删除行么', function (index) {
                    $.ajax({
                        url: "/SystemSettings/DlProduct",
                        data: {  lineID: id },
                        type: "post",
                        dataType: "text",
                        success: function (data) {
                            if (data == "true") {
                                layer.msg('删除成功');
                            }
                        }
                    });
                    obj.del(); //删除对应行（tr）的DOM结构
                    layer.close(index);
                    //向服务端发送删除指令
                });
            } else if (layEvent === 'edit') {
             
                $.ajax({
                    url: "/SystemSettings/UpProduct",
                    data: { line: name, lineID: id },
                    type: "post",
                    dataType: "text",
                    success: function (data) {
                        if (data=="true") {
                            layer.msg('修改成功');
                            $("#Search").click()
                        }
                    }
                });
                
            }
        });

    });
})