function SensorTypes_Save() {
    var model = {
        Id: $("#SensorTypes_Id").val(),
        FaName: $("#SensorTypes_FaName").val(),
        EnName: $("#SensorTypes_EnName").val(), 
        SensorType_State: $('input[name=SensorTypes_SensorType_State]:checked').val()
    };
    $.ajax({
        url: "/BaseInfo/SensorTypes_Save/",
        type: "POST",
        cache: false,
        dataType: "json",
        data: model,
        success: function (result) {
            if (result) {
                if ($("#SensorTypes_Id").val() !== "0") {
                    messageBox("success", "ویرایش نوع سنسور انجام شد");
                    Edit_SensorType_List(result.id, result.faName, result.enName,  result.sensorType_State === 1 ? "حقیقی(فیزیکی)" : "مجازی(محاسباتی)	" );
                } else {
                    messageBox("success", "ثبت نوع سنسور انجام شد");
                    Add_SensorType_List(result.id, result.faName, result.enName, result.sensorType_State === 1 ? "حقیقی(فیزیکی)" : "مجازی(محاسباتی)");
                }
                if (model.SensorType_State === "2") {
                    SensorTypes_UpdateCombo();
                }
            } else {
                messageBox("error", "خطا رخ داده است");
            }
        }
    });
}
function SensorTypes_Get(id) {
    $.ajax({
        url: "/BaseInfo/SensorTypes_Get/",
        type: "Get",
        cache: false,
        dataType: "json",
        data: { id: id },
        success: function (result) {
            if (result) {
                $("#SensorTypes_Id").val(result.id);
                $("#SensorTypes_FaName").val(result.faName);
                $("#SensorTypes_EnName").val(result.enName); 
                $("input[name=SensorTypes_SensorType_State][value=" + result.sensorType_State + "]").prop('checked', true);
            } else {
                messageBox("error", "خطا رخ داده است");
            }
        }
    });
}
function SensorTypes_Delete(id) {

    swal({
        title: "حذف نوع سنسور",
        text: "نوع سنسور مورد نظر حذف شود؟",
        type: "error",
        showCancelButton: true,
        confirmButtonClass: 'btn-danger waves-effect waves-light',
        confirmButtonText: "حذف"
    }, function (result) {
        if (result) {
            $.ajax({
                url: "/BaseInfo/SensorTypes_Delete/",
                type: "Get",
                cache: false,
                dataType: "json",
                data: { id: id },
                success: function (result) {
                    if (result) {
                        Delete_SensorType_List(result);
                        messageBox("success", "نوع سنسور مورد نظر حذف شد");
                    } else {
                        messageBox("error", "خطا رخ داده است");
                    }
                }
            });
        } else {
            messageBox("info", "حذف نوع سنسور لغو شد");
        }
    });
}


function Add_SensorType_List(id, faName, enName, sensorType) {
    var item = '<tr id="SensorTypes_ListItem_' + id + '">' +
        '<td>' + faName + '</td>' +
        '<td>' + enName + '</td>' + 
        '<td>' + sensorType + '</td>' +
        '<td>' +
        '<span onclick="SensorTypes_Delete(' + id + ')" class="badge badge-primary listStyleButton"><i class="fa fa-trash-o"></i></span>' +
        '<span onclick="SensorTypes_Get(' + id + ')" class="badge badge-primary listStyleButton"><i class="fa fa-pencil"></i></span>' +
        '</td>' +
        '</tr>';
    $('#SensorTypes_List').append(item);
    $("#SensorTypes_Id").val(0);
    $("#SensorTypes_FaName").val("");
    $("#SensorTypes_EnName").val(""); 
    $('input[name=SensorTypes_SensorType_State]:checked').prop('checked', false);
}
function Edit_SensorType_List(id, faName, enName, sensorType) {
    
    $("#SensorTypes_List #SensorTypes_ListItem_" + id + " td:nth-child(1)").text(faName);
    $("#SensorTypes_List #SensorTypes_ListItem_" + id + " td:nth-child(2)").text(enName); 
    $("#SensorTypes_List #SensorTypes_ListItem_" + id + " td:nth-child(3)").text(sensorType);
    $("#SensorTypes_Id").val(0);
    $("#SensorTypes_FaName").val("");
    $("#SensorTypes_EnName").val(""); 
    $('input[name=SensorTypes_SensorType_State]:checked').prop('checked', false);
}
function Delete_SensorType_List(id) {
    $("#SensorTypes_List #SensorTypes_ListItem_" + id ).remove();
}

function SensorTypes_UpdateCombo(){ 
    $.ajax({
        url: "/BaseInfo/SensorTypes_UpdateCombo/",
        type: "Get",
        cache: false,
        dataType: "json",
        success: function (result) {
            if (result) {
                $("#VirtualSensorBase_SensorTypeId").empty();
                $("#VirtualSensorBase_SensorTypeId").append(' <option value="0">سنسورهای مجازی ...</option>');
                for (var i = 0; i < result.length; i++) {
                    $("#VirtualSensorBase_SensorTypeId").append('<option value="' + result[i].id + '">' + result[i].faName + '</option>');
                }
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}