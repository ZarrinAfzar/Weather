function DataLogger_Save() {
    var Id = $("#DataLogger_Id").val();
    var Name = $("#DataLogger_Name").val(); 
    $.ajax({
        url: "/BaseInfo/DataLogger_Save/",
        type: "POST",
        cache: false,  
        dataType: "json",
        data: { id: Id, name: Name },
        success: function(result) {
            if (result) {
                if ($("#DataLogger_Id").val() !== "0") {
                    Edit_DataLogger_List(result.id, result.name );
                    messageBox("success","ویرایش دیتالاگر انجام شد");
                } else {
                    Add_DataLogger_List(result.id, result.name );
                    messageBox("success","ثبت دیتالاگر انجام شد");
                }
            } else {
                messageBox("error","خطا  رخ داده است");
            }
        }
    });
}
function DataLogger_Get(id) {
    $.ajax({
        url: "/BaseInfo/DataLogger_Get/",
        type: "Get",
        cache: false,
        dataType: "json",
        data: { id: id},
        success: function (result) {
            if (result) { 
                $("#DataLogger_Id").val(result.id);
                $("#DataLogger_Name").val(result.name); 
            } else {
                messageBox("error","خطا  رخ داده است");
            }
        }
    });   
}
function DataLogger_Delete(id) {
    swal({
        title: "حذف دیتالاگر",
        text: "دیتالاگر مورد نظر حذف شود؟",
        type: "error",
        showCancelButton: true,
        confirmButtonClass: 'btn-danger waves-effect waves-light',
        confirmButtonText: "حذف"
    }, function (result) {
        if (result) {
            $.ajax({
                url: "/BaseInfo/DataLogger_Delete/",
                type: "Get",
                cache: false,
                dataType: "json",
                data: { id: id },
                success: function (result) {
                    if (result) {
                        Delete_DataLogger_List(result);
                        messageBox("success","حذف دیتالاگر انجام شد");
                    } else {
                        messageBox("error", "خطا رخ داده است");
                    }
                }
            }); 
        } else {
            messageBox("info", "حذف دیتالاگر لغو شد");
        }
    });
}

function Add_DataLogger_List(id, name) {
    var item = '<li class="list-group-item"  id="DataLoggerItem_'+id+'">' +
        '<span onclick="DataLogger_Delete('+id+')" class="badge badge-primary listStyleButton"><i class="fa fa-trash-o"></i></span>' +
        '<span onclick="DataLogger_Get('+id+')" class="badge badge-primary listStyleButton"><i class="fa fa-pencil"></i></span>' +
        '<text id="DataLoggerItemName_' + id +'">'+name+'</text>'+
        '</li>';
    $("#DataLogger_List").append(item);
    $("#DataLogger_Name").val("");
    $("#DataLogger_Id").val(0);
}
function Edit_DataLogger_List(id, name) {
    $("#DataLoggerItemName_" + id).text(name);
    $("#DataLogger_Name").val("");
    $("#DataLogger_Id").val(0);
}
function Delete_DataLogger_List(id) {
    $("#" +"DataLogger_List #DataLoggerItem_"+id).remove();
}