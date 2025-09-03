function ModemType_Save() {
    var Id = $("#ModemType_Id").val();
    var Name = $("#ModemType_Name").val(); 
    $.ajax({
        url: "/BaseInfo/ModemType_Save/",
        type: "POST",
        cache: false,  
        dataType: "json",
        data: { id: Id, name: Name },
        success: function(result) {
            if (result) { 
                if ($("#ModemType_Id").val() !== "0") {
                    Edit_ModemType_List(result.id, result.name );
                    messageBox("success", "ویرایش نوع مودم انجام شد");
                } else {
                    Add_ModemType_List(result.id, result.name );
                    messageBox("success", "ثبت نوع مودم انجام شد");
                }
            } else {
                messageBox("error","خطا  رخ داده است");
            }
        }
    });
}
function ModemType_Get(id) {
    $.ajax({
        url: "/BaseInfo/ModemType_Get/",
        type: "Get",
        cache: false,
        dataType: "json",
        data: { id: id},
        success: function (result) {
            if (result) { 
                $("#ModemType_Id").val(result.id);
                $("#ModemType_Name").val(result.name); 
            } else {
                messageBox("error","خطا  رخ داده است");
            }
        }
    });   
}
function ModemType_Delete(id) {
    swal({
        title: "حذف نوع مودم",
        text: "نوع مودم مورد نظر حذف شود؟",
        type: "error",
        showCancelButton: true,
        confirmButtonClass: 'btn-danger waves-effect waves-light',
        confirmButtonText: "حذف"
    }, function (result) {
        if (result) {
            $.ajax({
                url: "/BaseInfo/ModemType_Delete/",
                type: "Get",
                cache: false,
                dataType: "json",
                data: { id: id },
                success: function (result) {
                    if (result) {
                        Delete_ModemType_List(result);
                        messageBox("success", "حذف نوع مودم انجام شد");
                    } else {
                        messageBox("error", "خطا رخ داده است");
                    }
                }
            }); 
        } else {
            messageBox("info", "حذف نوع مودم لغو شد");
        }
    });
}

function Add_ModemType_List(id, name) {
    var item = '<li class="list-group-item"  id="ModemTypeItem_'+id+'">' +
        '<span onclick="ModemType_Delete('+id+')" class="badge badge-primary listStyleButton"><i class="fa fa-trash-o"></i></span>' +
        '<span onclick="ModemType_Get('+id+')" class="badge badge-primary listStyleButton"><i class="fa fa-pencil"></i></span>' +
        '<text id="ModemTypeItemName_' + id +'">'+name+'</text>'+
        '</li>';
    $("#ModemType_List").append(item);
    $("#ModemType_Name").val("");
    $("#ModemType_Id").val(0);
}
function Edit_ModemType_List(id, name) {
    $("#ModemTypeItemName_" + id).text(name);
    $("#ModemType_Name").val("");
    $("#ModemType_Id").val(0);
}
function Delete_ModemType_List(id) {
    $("#" +"ModemType_List #ModemTypeItem_"+id).remove();
}