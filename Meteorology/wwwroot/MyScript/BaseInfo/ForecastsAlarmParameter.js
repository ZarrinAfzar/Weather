function ForecastsAlarmParameter_Save() {//ForecastsAlarmParameter_Image
    var formData = new FormData();
    var _file = $("#ForecastsAlarmParameter_Image").get(0).files;
    if (_file) {
        formData.append('Icon', _file[0]);
    }
    formData.append('Id', $('#ForecastsAlarmParameter_Id').val());
    formData.append('Name', $('#ForecastsAlarmParameter_Name').val());
    formData.append('Value', $('#ForecastsAlarmParameter_Value').val());

    $.ajax({
        url: "/BaseInfo/ForecastsAlarmParameter_Save/",
        type: "POST",
        processData: false,
        contentType: false,
        data: formData,
        success: function (result) {

            if (result) { 
                if ($("#ForecastsAlarmParameter_Id").val() !== "0") {
                    Edit_ForecastsAlarmParameter_List(result.id, result.name, result.value , result.iconPath);
                    messageBox("success","ویرایش پارامتر پشتیبانی انجام شد");
                } else {
                    Add_ForecastsAlarmParameter_List(result.id, result.name , result.value , result.iconPath);
                    messageBox("success", "ثبت پارامتر پشتیبانی انجام شد");
                }
            } else { 
                messageBox("error","خطا رخ داده است");
            }
        }
    });
}
function ForecastsAlarmParameter_Get(id) {
    $.ajax({
        url: "/BaseInfo/ForecastsAlarmParameter_Get/",
        type: "Get",
        cache: false,
        dataType: "json",
        data: { id: id },
        success: function (result) {
            if (result) {
                $("#ForecastsAlarmParameter_Id").val(result.id);
                $("#ForecastsAlarmParameter_Name").val(result.name);
                $("#ForecastsAlarmParameter_Value").val(result.value);
            } else {
                messageBox("error", "خطا رخ داده است");
            }
        }
    });
}
function ForecastsAlarmParameter_Delete(id) {

    swal({
        title: "حذف پارامترهای پیش بینی",
        text: "پارامتر پیش بینی مورد نظر حذف شود؟",
        type: "error",
        showCancelButton: true,
        confirmButtonClass: 'btn-danger waves-effect waves-light',
        confirmButtonText: "حذف"
    }, function (result) {
        if (result) {
            $.ajax({
                url: "/BaseInfo/ForecastsAlarmParameter_Delete/",
                type: "Get",
                cache: false,
                dataType: "json",
                data: { id: id },
                success: function (result) {
                    if (result) {
                        Delete_ForecastsAlarmParameter_List(result);
                        messageBox("success", "پارامتر پیش بینی مورد نظر حذف شد");
                    } else {
                        messageBox("error", "خطا رخ داده است");
                    }
                }
            });
        } else {
            messageBox("info", "حذف پارامتر پیش بینی لغو شد");
        }
    });
}

function Add_ForecastsAlarmParameter_List(id, name ,value , icon) {
    var item = '<li class="list-group-item"  id="ForecastsAlarmParameterItem_' + id + '">' +
        '<span onclick="ForecastsAlarmParameter_Delete(' + id + ')" class="badge badge-primary listStyleButton"><i class="fa fa-trash-o"></i></span>' +
        '<span onclick="ForecastsAlarmParameter_Get(' + id + ')" class="badge badge-primary listStyleButton"><i class="fa fa-pencil"></i></span>' +
        '<img id="ForecastsAlarmParameterItemIcon_' + id + '" src="' + icon + '"  style="width:24px"/>' +
        '<text id="ForecastsAlarmParameterItemName_' + id + '">' + name + ' ' + value + '</text>' +
        '</li>';
    $("#ForecastsAlarmParameter_List").append(item);
    $("#ForecastsAlarmParameter_Name").val("");
    $("#ForecastsAlarmParameter_Value").val("");
    $("#ForecastsAlarmParameter_Id").val(0);
}
function Edit_ForecastsAlarmParameter_List(id, name,value, icon) {
    $("#ForecastsAlarmParameterItemName_" + id).text(name +' '+ value);
    $("#ForecastsAlarmParameterItemIcon_" + id).attr("src", icon);
    $("#ForecastsAlarmParameter_Name").val("");
    $("#ForecastsAlarmParameter_Value").val("");
    $("#ForecastsAlarmParameter_Id").val(0);
}
function Delete_ForecastsAlarmParameter_List(id) {
    $("#" + "ForecastsAlarmParameter_List #ForecastsAlarmParameterItem_" + id).remove();
}

function ForecastsAlarmParameter_Type(val) {
    if (val === 0) {
        $("#ForecastsAlarmParameter_ImageBox").show();
    } else {
        $("#ForecastsAlarmParameter_ImageBox").hide();
    }
}