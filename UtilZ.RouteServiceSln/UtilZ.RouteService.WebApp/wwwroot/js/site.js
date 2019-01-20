function clearTable(id, flag) {
    //$("#tbModules  tr:not(:first)").html("");//清空table表格除首行外的所有数据方法一
    //$("#tbModules  tr:not(:first)").empty();//清空table表格除首行外的所有数据方法二
    //上面两种jquery的方法，只是把td删除了，tr还在，结果显示效果上就出现了空区域，jquery有时候也不是万能的

    var tb = window.document.getElementById(id);
    if (tb == undefined || tb == null) {
        return;
    }

    var endIndex = 0;
    if (flag) {
        endIndex = 1;
    }

    for (var i = tb.rows.length - 1; i >= endIndex; i--) {
        var row = tb.rows[i];
        row.parentNode.removeChild(row);
    }
}