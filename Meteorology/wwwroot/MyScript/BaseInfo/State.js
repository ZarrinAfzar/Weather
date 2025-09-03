function State_Save() {

    $.ajax({
        url: "/BaseInfo/State_Save/",
        cache: false,  
        type: "POST", 
        async: false,
        data: { id: $("#State_Id").val(), name: $("#State_Name").val() },
        dataType: "json",
        success: function(result) {
            if (result) {
                if ($("#State_Id").val() !== "0") {
                    Edit_State_List(result.id, result.name);
                    messageBox("success","ویرایش استان انجام شد");
                } else {
                    Add_State_List(result.id, result.name);
                    messageBox("success", "ثبت استان انجام شد");
                }
                UpdateState();
            } else {
                messageBox("error","خطا  رخ داده است");
            }
        }
    });
}
function State_Get(id) {
    $.ajax({
        url: "/BaseInfo/State_Get/",
        type: "Get",
        cache: false,
        dataType: "json",
        data: { id: id},
        success: function (result) {
            if (result) { 
                $("#State_Id").val(result.id);
                $("#State_Name").val(result.name);
            } else {
                messageBox("error", "خطا رخ داده است");
            }
        }
    });   
}
function State_Delete(id) {

    swal({
        title: "حذف استان",
        text: "استان مورد نظر حذف شود؟",
        type: "error",
        showCancelButton: true,
        confirmButtonClass: 'btn-danger waves-effect waves-light',
        confirmButtonText: "حذف"
    }, function (result) {
        if (result) {
            $.ajax({
                url: "/BaseInfo/State_Delete/",
                type: "Get",
                cache: false,
                dataType: "Html",
                data: { id: id },
                success: function (result) {
                    if (result) {
                        Delete_State_List(result);
                        messageBox("success","حذف استان انجام شد");
                    } else { 
                        messageBox("error", "خطا رخ داده است");
                    }
                }
            }); 
        } else {
            messageBox("info", "حذف استان لغو شد");
        }
    });
} 
function Add_State_List(id,name) {
    var item = '<li class="list-group-item"  id="StateItem_' + id +'">' +
        '<span onclick="State_Delete('+id+')" class="badge badge-primary listStyleButton"><i class="fa fa-trash-o"></i></span>' +
        '<span onclick="State_Get('+id+')" class="badge badge-primary listStyleButton"><i class="fa fa-pencil"></i></span>' +
        '<text  id="StateItemName_' + id +'">' + name+'</text>' +
        '</li>';
    $("#State_List").append(item);
    $("#State_Name").val("");
    $("#State_Id").val(0);
}
function Edit_State_List(id, name) {
    $("#" + "State_List #StateItem_" + id + " #StateItemName_" + id).text(name);
    $("#State_Name").val("");
    $("#State_Id").val(0);
}
function Delete_State_List(id) {
    $("#"+"State_List #StateItem_"+id).remove();
}
function UpdateState() { 
    $.ajax({
        url: "/BaseInfo/State_UpdateCombo/",
        type: "Get",
        cache: false, 
        dataType: "json",
        success: function (result) {
            if (result) {
                $("#city_StateId").empty();
                $("#city_StateId").append(' <option>پروژه...</option>');
                for (var i = 0; i < result.length; i++) {
                    $("#city_StateId").append('<option value="' + result[i].id + '">' + result[i].name + '</option>');
                } 
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}