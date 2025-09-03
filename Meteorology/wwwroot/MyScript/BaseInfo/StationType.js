function StationType_Save() {//StationType_Image
    var formData = new FormData();
    var _file = $("#StationType_Image").get(0).files;
    if (_file) {
        formData.append('Icon', _file[0]);
    }
    formData.append('Id', $('#StationType_Id').val());
    formData.append('Name', $('#StationType_Name').val());

    $.ajax({
        url: "/BaseInfo/StationType_Save/",
        type: "POST",
        processData: false,
        contentType: false,
        //cache: false,
        //dataType: "json",
        data: formData,
        success: function (result) {

            if (result) { 
                if ($("#StationType_Id").val() !== "0") {
                    Edit_StationType_List(result.id, result.name, result.iconPath);
                    messageBox("success","ویرایش نوع ایستگاه انجام شد");
                } else {
                    Add_StationType_List(result.id, result.name, result.iconPath);
                    messageBox("success", "ثبت نوع ایستگاه انجام شد");
                }
            } else { 
                messageBox("error","خطا رخ داده است");
            }
        }
    });
}
function StationType_Get(id) {
    $.ajax({
        url: "/BaseInfo/StationType_Get/",
        type: "Get",
        cache: false,
        dataType: "json",
        data: { id: id },
        success: function (result) {
            if (result) {
                $("#StationType_Id").val(result.id);
                $("#StationType_Name").val(result.name);
            } else {
                messageBox("error", "خطا رخ داده است");
            }
        }
    });
}
function StationType_Delete(id) {

    swal({
        title: "حذف نوع ایستگاه",
        text: "نوع ایستگاه مورد نظر حذف شود؟",
        type: "error",
        showCancelButton: true,
        confirmButtonClass: 'btn-danger waves-effect waves-light',
        confirmButtonText: "حذف"
    }, function (result) {
        if (result) {
            $.ajax({
                url: "/BaseInfo/StationType_Delete/",
                type: "Get",
                cache: false,
                dataType: "json",
                data: { id: id },
                success: function (result) {
                    if (result) {
                        Delete_StationType_List(result);
                        messageBox("success", "نوع ایستگاه مورد نظر حذف شد");
                    } else {
                        messageBox("error", "خطا رخ داده است");
                    }
                }
            });
        } else {
            messageBox("info", "حذف نوع ایستگاه لغو شد");
        }
    });
}

function Add_StationType_List(id, name, icon) {
    var item = '<li class="list-group-item"  id="StationTypeItem_' + id + '">' +
        '<span onclick="StationType_Delete(' + id + ')" class="badge badge-primary listStyleButton"><i class="fa fa-trash-o"></i></span>' +
        '<span onclick="StationType_Get(' + id + ')" class="badge badge-primary listStyleButton"><i class="fa fa-pencil"></i></span>' +
        '<img id="StationTypeItemIcon_' + id + '" src="' + icon + '"  style="width:24px"/>' +
        '<text id="StationTypeItemName_' + id + '">' + name + '</text>' +
        '</li>';
    $("#StationType_List").append(item);
    $("#StationType_Name").val("");
    $("#StationType_Id").val(0);
}
function Edit_StationType_List(id, name, icon) {
    $("#StationTypeItemName_" + id).text(name);
    $("#StationTypeItemIcon_" + id).attr("src", icon);
    $("#StationType_Name").val("");
    $("#StationType_Id").val(0);
}
function Delete_StationType_List(id) {
    $("#" + "StationType_List #StationTypeItem_" + id).remove();
}