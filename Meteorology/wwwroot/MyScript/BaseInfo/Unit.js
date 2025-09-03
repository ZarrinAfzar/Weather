function Unit_Save() {
    $.ajax({
        url: "/BaseInfo/Unit_Save/",
        cache: false,
        type: "POST",
        async: false,
        data: { id: $("#Unit_Id").val(), FaName: $("#Unit_FaName").val(), EnName: $("#Unit_EnName").val() },
        dataType: "json",
        success: function (result) {
            if (result) {
                if ($("#Unit_Id").val() !== "0") {
                    Edit_Unit_List(result.id, result.faName, result.enName);
                    messageBox("success", "ویرایش واحد اندازه گیری انجام شد");
                } else {
                    Add_Unit_List(result.id, result.faName, result.enName);
                    messageBox("success", "ثبت واحد اندازه گیری انجام شد");
                }
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}
function Unit_Get(id) {
    $.ajax({
        url: "/BaseInfo/Unit_Get/",
        type: "Get",
        cache: false,
        dataType: "json",
        data: { id: id },
        success: function (result) {
            if (result) {
                $("#Unit_Id").val(result.id);
                $("#Unit_EnName").val(result.enName);
                $("#Unit_FaName").val(result.faName);
            } else {
                messageBox("error", "خطا رخ داده است");
            }
        }
    });
}
function Unit_Delete(id) {

    swal({
        title: "حذف واحد اندازه گیری",
        text: "واحد اندازه گیری مورد نظر حذف شود؟",
        type: "error",
        showCancelButton: true,
        confirmButtonClass: 'btn-danger waves-effect waves-light',
        confirmButtonText: "حذف"
    }, function (result) {
        if (result) {
            $.ajax({
                url: "/BaseInfo/Unit_Delete/",
                type: "Get",
                cache: false,
                dataType: "Html",
                data: { id: id },
                success: function (result) {
                    if (result) {
                        Delete_Unit_List(result);
                        messageBox("success", "حذف واحد اندازه گیری انجام شد");
                    } else {
                        messageBox("error", "خطا رخ داده است");
                    }
                }
            });
        } else {
            messageBox("info", "حذف واحد اندازه گیری لغو شد");
        }
    });
}
function Add_Unit_List(id, faName , enName) {
    var item = '<li class="list-group-item"  id="UnitItem_' + id + '">' +
        '<span onclick="Unit_Delete(' + id + ')" class="badge badge-primary listStyleButton"><i class="fa fa-trash-o"></i></span>' +
        '<span onclick="Unit_Get(' + id + ')" class="badge badge-primary listStyleButton"><i class="fa fa-pencil"></i></span>' +
        '<text  id="UnitItemName_' + id + '">' + faName +'('+ enName +')'+'</text>' +
        '</li>';
    $("#Unit_List").append(item);
    $("#Unit_EnName").val("");
    $("#Unit_FaName").val("");
    $("#Unit_Id").val(0);
}
function Edit_Unit_List(id, faName, enName) {
    $("#" + "Unit_List #UnitItem_" + id + " #UnitItemName_" + id).text(faName + '(' + enName +')');
    $("#Unit_EnName").val("");
    $("#Unit_FaName").val("");
    $("#Unit_Id").val(0);
}
function Delete_Unit_List(id) {
    $("#" + "Unit_List #UnitItem_" + id).remove();
}
 