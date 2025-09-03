function City_Save() {
    var Id = $("#City_Id").val();
    var Name = $("#city_Name").val();
    var StateId = $("#city_StateId").val(); 
    $.ajax({
        url: "/BaseInfo/City_Save/",
        type: "POST",
        cache: false,  
        dataType: "json",
        data: { id: Id, name: Name, stateId: StateId },
        success: function(result) {
            if (result) {
                if ($("#City_Id").val() !== "0") {
                    Edit_City_List(result.id, result.name, $("#city_StateId option:selected").text());
                    messageBox("success","ویرایش شهر انجام شد");
                } else {
                    Add_City_List(result.id, result.name, $("#city_StateId option:selected").text());
                    messageBox("success","ثبت شهر انجام شد");
                }
            } else {
                messageBox("error", "خطا  رخ داده است");

            }
        }
    });
}
function City_Get(id) {
    $.ajax({
        url: "/BaseInfo/City_Get/",
        type: "Get",
        cache: false,
        dataType: "json",
        data: { id: id},
        success: function (result) {
            if (result) { 
                $("#City_Id").val(result.id);
                $("#city_Name").val(result.name); 
                $("#city_StateId").val(result.stateId);
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });   
}
function City_Delete(id) {

    swal({
        title: "حذف شهرستان",
        text: "شهرستان مورد نظر حذف شود؟",
        type: "error",
        showCancelButton: true,
        confirmButtonClass: 'btn-danger waves-effect waves-light',
        confirmButtonText: "حذف"
    }, function (result) {
        if (result) {
            $.ajax({
                url: "/BaseInfo/City_Delete/",
                type: "Get",
                cache: false,
                dataType: "json",
                data: { id: id },
                success: function (result) {
                    if (result) {
                        Delete_City_List(result);
                        messageBox("success", "حذف شهرستان انجام شد");
                    } else {
                        messageBox("error", "خطا رخ داده است");
                    }
                }
            }); 
        } else {
            messageBox("info", "حذف شهرستان لغو شد");
        }
    });
}

function Add_City_List(id, name, stateName) {
    var item = '<li class="list-group-item"  id="CityItem_'+id+'">' +
        '<span onclick="City_Delete('+id+')" class="badge badge-primary listStyleButton"><i class="fa fa-trash-o"></i></span>' +
        '<span onclick="City_Get('+id+')" class="badge badge-primary listStyleButton"><i class="fa fa-pencil"></i></span>' +
        '<p style="margin: 0;" id="CityItemName_' + id + '"><b> ' + name + '</b> <text>' + stateName + '</text></p>'+
        '</li>';
    $("#City_List").append(item);
    $("#city_Name").val("");
    $("#City_Id").val(0);
}
function Edit_City_List(id, name, stateName) {
    $("#CityItemName_" + id + " b").text(name);
    $("#CityItemName_" + id + " text").text(stateName);
    $("#city_Name").val("");
    $("#City_Id").val(0);
}
function Delete_City_List(id) {
    $("#" +"City_List #CityItem_"+id).remove();
}