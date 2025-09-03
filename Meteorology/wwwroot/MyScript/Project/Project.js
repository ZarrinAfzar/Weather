
//_____________________________________________
$(document).ready(function () {
    List("", 0);
    $("#ProjectSort").change(function () {
        List("", $(this).val());
    });
});
function List(search,sort) {
    $(".listPreLoader").show();
    $.ajax({
        url: "/Project/Project_List/",
        type: "Get",
        cache: false,
        //async: false, 
        dataType: "Html",
        data: { valueSearch: search, sort: sort},
        success: function (result) {
            if (result) { 
                $("#List").html(result); 
                $('[data-toggle="tooltip"]').tooltip();
            } else { 
                messageBox("error", "خطا در سیستم");
            }
        }
    });
    $(".listPreLoader").hide();
}
function New() {
    $.ajax({
        url: "/Project/Get_Project/",
        type: "Get",
        cache: false,
        dataType: "Html",
        success: function (result) {
            if (result) {
                $("#ModalTitle").text("ثبت پروژه جدید");
                $("#ModalBody").html(result);
                $("#myModal").modal();
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}
function Edit(id) {
    $.ajax({
        url: "/Project/Get_Project/",
        type: "Get",
        cache: false,
        dataType: "Html",
        data: { id: id},
        success: function (result) {
            if (result) {
                $("#ModalTitle").text("ویرایش پروژه");
                $("#ModalBody").html(result);
                $("#myModal").modal();
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}
function View(id) {
    $.ajax({
        url: "/Project/Project_View/",
        type: "Get",
        cache: false,
        dataType: "Html",
        data: { id: id },
        success: function (result) {
            if (result) {
                $("#ModalTitle").text("جزئیات پروژه");
                $("#ModalBody").html(result);
                $("#myModal").modal();
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}
function NextPage(pageNumber, e) {
    $(".pagination").removeClass("acctivePage");
    $(e).children().addClass("acctivePage");
    pageNumber = (pageNumber - 1) * 6;
    $(".listPreLoader").show();
    $.ajax({
        url: "/Project/Project_List/",
        type: "Get",
        cache: false,
        async: false,
        data: { skip: pageNumber },
        dataType: "Html",
        success: function (result) {
            if (result) {
                $("#List").html(result);
            } else {
                messageBox("error", "خطا در سیستم");
            }
        }
    });
    $(".listPreLoader").hide();
}
function Project_Search() {
    if ($("#ProjectSearch_value").val()) {
        List($("#ProjectSearch_value").val(), 0);
    } else {
        messageBox("error", "مقدار جستجو وارد نشده است");
    }

}

//_____________________________________________

function Save() {
    $("#formBody").validate({
        rules: {
            Name: "required"
        },
        messages: {
            Name: "نام پروژه را وارد کنید"
        },
        errorPlacement: function (label, element) {
            label.addClass('arrow');
            label.insertAfter(element);
        },
        wrapper: 'span',
        submitHandler: function () {
            var json = {};
            $.each($('#formBody').serializeArray(), function (i, field) {
                json[field.name] = field.value || null;
            });
            $(".listPreLoader").show();
            $.ajax({
                url: "/Project/Project_Save/",
                type: "POST",
                cache: false,
                //async: false,
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                data: JSON.stringify(json),
                success: function (result) {
                    if (result) {
                        messageBox("success", "ثبت پروژه انجام شد");
                        List();
                        $("#myModal").modal('hide');
                    } else {
                        messageBox("error", "خطا در ثبت پروژه");
                    }
                    $(".listPreLoader").hide();
                }
            });
        }
    });
} 
function Delete(id) {
    swal({
        title: "حذف پروژه",
        text: "پروژه مورد نظر حذف شود؟",
        type: "error",
        showCancelButton: true,
        confirmButtonClass: 'btn-danger waves-effect waves-light',
        confirmButtonText: "حذف"
    }, function (result) {
        if (result) {
            $.ajax({
                url: "/Project/Project_Delete/",
                type: "Get",
                cache: false,
                async: false,
                dataType: "json",
                data: { id: id },
                success: function (result) {
                    if (result) {
                        messageBox("success", "حذف پروژه انجام شد");
                        List();
                    } else {
                        messageBox("error", "خطا در حذف پروژه");
                    }
                }
            });
        } else {
            messageBox("info", "حذف پروژه لغو شد");
        }
    });

}
 
//_____________________________________________

function SmsRequest(id) {
    $.ajax({
        url: "/Project/Project_SmsRequest/",
        type: "Get",
        cache: false,
        dataType: "Html",
        data: { id: id},
        success: function (result) {
            if (result) {
                $("#ModalTitle").text("شارژ پیامک");
                $("#ModalBody").html(result);
                $("#myModal").modal();
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}
function SmsRequest_Save() {
    var old = ($("#OldsmsCount").val());
    var newval = ($("#SmsCount").val());
    if (newval != 0) {
        $.ajax({
            url: "/Project/SmsRequest_Save/",
            type: "POST",
            cache: false,
            async: false,
            dataType: "json",
            data: { ProjectId: $("#projectId").val(), oldCount: old, smsCount: newval },
            success: function (result) {
                if (result) {
                    messageBox("success", "شارژ پیامک انجام شد");
                    List();
                    $("#myModal").modal('hide');
                } else {
                    messageBox("error", "خطا در شارژ پیامک");
                }
            }
        });
    }
   
}
function SmsSetToStation() {
    $.ajax({
        url: "/Project/Project_SmsSetToStation/",
        type: "Get",
        cache: false,
        dataType: "Html",
        success: function (result) {
            if (result) { 
                $("#ModalTitle").text("اختصاص پیامک به ایستگاه ها");
                $("#ModalBody").html(result);
                $("#myModal").modal();
            } else {
                messageBox("error", "خطا  رخ داده است");
            } 
        }
    });
}

