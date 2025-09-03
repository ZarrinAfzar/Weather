
$(document).ready(function () {
    $(".select2").select2();
    
    Notification_List("","","");
    $("#NotificationSort").change(function () {
        Notification_List($("#UserNameSearch_value").val(), $("#MessageSearch_value").val(), $(this).val());
    });
});

function UserName_Search() {
    if ($("#UserNameSearch_value").val()) {
        Notification_List($("#UserNameSearch_value").val(), $("#MessageSearch_value").val(), $(NotificationSort).val());
    } else {
        messageBox("error", "مقدار جستجو وارد نشده است");
    }
}

function Message_Search() {
    if ($("#MessageSearch_value").val()) {
        Notification_List($("#UserNameSearch_value").val(),$("#MessageSearch_value").val(), $(NotificationSort).val());
    } else {
        messageBox("error", "مقدار جستجو وارد نشده است");
    }
}

function Send() {
    var UserGeter = $("#UserGeter").val();
    var Message = $("#MessageText").val();
    $.ajax({
        url: "/Notification/SendMessage/",
        type: "POST",
        cache: false,
        dataType: "json",
        data: { UserGeter: UserGeter, Message: Message },
        success: function (result) {
            if (result) {
                $("#UserGeter").val("");
                $("#MessageText").val("");
                messageBox("success", "ارسال پیام انجام شد");
                Notification_List();
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}

function Notification_List(userNameSearch, valueSearch, sort) {
    $.ajax({
        url: "/Notification/Notification_List/",
        type: "Get",
        cache: false,
        dataType: "Html",
        data: { userNameSearch: userNameSearch, valueSearch: valueSearch, sort: sort },
        success: function (result) {
            if (result) {
                $("#NotificationList").empty();
                                $("#NotificationList").html(result);
            } else {
                alert("خطا رخ داده است");
            }
        }
    });
}

function NotificationMessage(Id) {
    $.ajax({
        url: "/Notification/NotificationMessage/",
        type: "POST",
        cache: false,
        dataType: "json",
        data: { NotificationId: Id },
        success: function (result) {
            if (result) {
                $("#ModalTitle").text("متن پیام");
                $("#ModalBody").html("<b>" + result + "</b>");
                $("#myModal").modal();
                Notification_List();   
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}
function Delete(id) {
    swal({
        title: "حذف پیام",
        text: "پیام مورد نظر حذف شود؟",
        type: "error",
        showCancelButton: true,
        confirmButtonClass: 'btn-danger waves-effect waves-light',
        confirmButtonText: "حذف"
    }, function (result) {
        if (result) {
            $.ajax({
                url: "/Notification/NotificationDelete/",
                type: "Get",
                cache: false,
                async: false,
                dataType: "json",
                data: { id: id },
                success: function (result) {
                    if (result) {
                        Notification_List();                                     
                        messageBox("success", "حذف پیام انجام شد");
                    } else {
                        messageBox("error", "خطا رخ داده است");
                    }
                }
            });
        } else {
            messageBox("info", "حذف ایستگاه لغو شد");
        }
    });

}