function VirtualSensorBase_Save() {
    var model = {};
    $.each($('#VirtualSensorBase_Form').serializeArray(), function (i, field) {
        model[field.name] = field.value || null;
    });
    $.ajax({
        url: "/BaseInfo/VirtualSensorBase_Save/",
        type: "POST",
        cache: false,
        async: false,
        dataType: "json",
        data: model, 
        success: function (result) {
            if (result) {
                result.sensorType = $("#VirtualSensorBase_SensorTypeId :selected").text(); 
                if ($("#VirtualSensorBase_Id").val() !== "0") {
                    messageBox("success", "ویرایش پارامتر های محاسباتی انجام شد");
                    Edit_VirtualSensorBase_List(result);
                } else {
                    messageBox("success", "ثبت پارامتر های محاسباتی انجام شد");
                    Add_VirtualSensorBase_List(result);
                }
            } else {
                messageBox("error", "خطا رخ داده است");
            }
        }
    });
}
function VirtualSensorBase_Get(id) {
    $.ajax({
        url: "/BaseInfo/VirtualSensorBase_Get/",
        type: "Get",
        cache: false,
        dataType: "json",
        data: { id: id },
        success: function (result) {
            if (result) {
                
                $("#VirtualSensorBase_Id").val(result.id);
                $("#VirtualSensorBase_SensorTypeId").val(result.sensorTypeId);
                $("#VirtualSensorBase_Name1").val(result.parameterName1);
                $("#VirtualSensorBase_Name2").val(result.parameterName2);
                $("#VirtualSensorBase_Name3").val(result.parameterName3);
                $("#VirtualSensorBase_Name4").val(result.parameterName4);
                $("#VirtualSensorBase_Name5").val(result.parameterName5);
                $("#VirtualSensorBase_Value1").val(result.parameterValue1);
                $("#VirtualSensorBase_Value2").val(result.parameterValue2);
                $("#VirtualSensorBase_Value3").val(result.parameterValue3);
                $("#VirtualSensorBase_Value4").val(result.parameterValue4);
                $("#VirtualSensorBase_Value5").val(result.parameterValue5);
                $("#VirtualSensorBase_Type1").val(result.parameterType1);
                $("#VirtualSensorBase_Type2").val(result.parameterType2);
                $("#VirtualSensorBase_Type3").val(result.parameterType3);
                $("#VirtualSensorBase_Type4").val(result.parameterType4);
                $("#VirtualSensorBase_Type5").val(result.parameterType5);
            } else {
                messageBox("error", "خطا رخ داده است");
            }
        }
    });
}
function VirtualSensorBase_Delete(id) {
    swal({
        title: "حذف پارامتر های محاسباتی",
        text: "پارامتر های محاسباتی مورد نظر حذف شود؟",
        type: "error",
        showCancelButton: true,
        confirmButtonClass: 'btn-danger waves-effect waves-light',
        confirmButtonText: "حذف"
    }, function (result) {
        if (result) {
            $.ajax({
                url: "/BaseInfo/VirtualSensorBase_Delete/",
                type: "Get",
                cache: false,
                dataType: "json",
                data: { id: id },
                success: function (result) {
                    if (result) {
                        Delete_VirtualSensorBase_List(result);
                        messageBox("success", "پارامتر های محاسباتی مورد نظر حذف شد");
                    } else {
                        messageBox("error", "خطا رخ داده است");
                    }
                }
            });
        } else {
            messageBox("info", "حذف پارامتر های محاسباتی لغو شد");
        }
    });
}


function Add_VirtualSensorBase_List(model) {
    var item = '<tr id="VirtualSensorBase_ListItem_' + model.id + '">' +
        '<td>' + model.sensorType + '</td>' +
        '<td>' +  model.parameterName1 + ' - ' + model.parameterType1Title + ' - ' + model.parameterValue1   + '</td>' +
        '<td>' +  model.parameterName2 + ' - ' + model.parameterType2Title + ' - ' + model.parameterValue2   + '</td>' +
        '<td>' +  model.parameterName3 + ' - ' + model.parameterType3Title + ' - ' + model.parameterValue3   + '</td>' + 
        '<td>' +  model.parameterName4 + ' - ' + model.parameterType4Title + ' - ' + model.parameterValue4   + '</td>' +
        '<td>' +  model.parameterName5 + ' - ' + model.parameterType5Title + ' - ' + model.parameterValue5   + '</td>' +
        '<td>' +
        '<span onclick="VirtualSensorBase_Delete(' + model.id + ')" class="badge badge-primary listStyleButton"><i class="fa fa-trash-o"></i></span>' +
        '<span onclick="VirtualSensorBase_Get(' + model.id + ')" class="badge badge-primary listStyleButton"><i class="fa fa-pencil"></i></span>' +
        '</td>' +
        '</tr>';
    item = replaceAll(item,'null', '');
    $('#VirtualSensorBase_List').append(item);
    VirtualSensorBase_resetForm();
}
function Edit_VirtualSensorBase_List(model) {
    $("#VirtualSensorBase_List #VirtualSensorBase_ListItem_" + model.id + " td:nth-child(1)").text(model.sensorType);
    $("#VirtualSensorBase_List #VirtualSensorBase_ListItem_" + model.id + " td:nth-child(2)").text(replaceAll(model.parameterName1 + ' - ' + model.parameterType1Title + ' - ' + model.parameterValue1, 'null', ''));
    $("#VirtualSensorBase_List #VirtualSensorBase_ListItem_" + model.id + " td:nth-child(3)").text(replaceAll(model.parameterName2 + ' - ' + model.parameterType2Title + ' - ' + model.parameterValue2, 'null', ''));
    $("#VirtualSensorBase_List #VirtualSensorBase_ListItem_" + model.id + " td:nth-child(4)").text(replaceAll(model.parameterName3 + ' - ' + model.parameterType3Title + ' - ' + model.parameterValue3, 'null', ''));
    $("#VirtualSensorBase_List #VirtualSensorBase_ListItem_" + model.id + " td:nth-child(5)").text(replaceAll(model.parameterName4 + ' - ' + model.parameterType4Title + ' - ' + model.parameterValue4, 'null', ''));
    $("#VirtualSensorBase_List #VirtualSensorBase_ListItem_" + model.id + " td:nth-child(6)").text(replaceAll(model.parameterName5 + ' - ' + model.parameterType5Title + ' - ' + model.parameterValue5, 'null', ''));
    VirtualSensorBase_resetForm();
}
function Delete_VirtualSensorBase_List(id) {
    $("#VirtualSensorBase_List #VirtualSensorBase_ListItem_" + id ).remove();
}
function VirtualSensorBase_resetForm() {
    $("#VirtualSensorBase_Id").val(0);
    $("#VirtualSensorBase_SensorTypeId").val(0);
    $("#VirtualSensorBase_Name1").val("");
    $("#VirtualSensorBase_Name2").val("");
    $("#VirtualSensorBase_Name3").val("");
    $("#VirtualSensorBase_Name4").val("");
    $("#VirtualSensorBase_Name5").val("");
    $("#VirtualSensorBase_Value1").val("");
    $("#VirtualSensorBase_Value2").val("");
    $("#VirtualSensorBase_Value3").val("");
    $("#VirtualSensorBase_Value4").val("");
    $("#VirtualSensorBase_Value5").val("");
    $("#VirtualSensorBase_Type1").val(0);
    $("#VirtualSensorBase_Type2").val(0);
    $("#VirtualSensorBase_Type3").val(0);
    $("#VirtualSensorBase_Type4").val(0);
    $("#VirtualSensorBase_Type5").val(0);
}

function replaceAll(str, find, replace) {
    return str.replace(new RegExp(find, 'g'), replace);
}
