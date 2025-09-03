
$(document).ready(function () {
    List("","");
    $("#UserSort").change(function () {
        List($("#UserNameSearch_value").val(), $(this).val());
    });
});

function UserName_Search() {
    $(".listPreLoader").show();
    if ($("#UserNameSearch_value").val()) {
        List($("#UserNameSearch_value").val(), $(UserSort).val());
    } else {
        messageBox("error", "مقدار جستجو وارد نشده است");
    }
    $(".listPreLoader").hide();
}
//_____________________________________________
function List(search, sort) {
    $(".listPreLoader").show();
    $.ajax({
        url: "/User/User_List/",
        type: "Get",
        cache: false,
        async: true,
        dataType: "Html",
        data: { valueSearch: search, sort: sort },
        success: function (result) {
            if (result) {
                $("#UserList").html(result);
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
        url: "/User/User_Get/",
        type: "Get",
        cache: false,
        dataType: "Html",
        success: function (result) {
            if (result) {
                $("#ModalTitle").text("ثبت کاربر جدید");
                $("#ModalBody").html(result);
                $("#myModal").modal();
            } else {
                messageBox("error", "خطا در سیستم");
            }
        }
    });
}
function Edit(id) {
    $.ajax({
        url: "/User/User_Get/",
        type: "Get",
        cache: false,
        dataType: "Html",
        data: { id: id },
        success: function (result) {
            if (result) {
                $("#ModalTitle").text("ویرایش کاربر");
                $("#ModalBody").html(result);
                $("#myModal").modal();
            } else {
                messageBox("error", "خطا در درخواست کاربر");
            }
        }
    });
}
function NextPage(pageNumber) {
    pageNumber = (pageNumber - 1) * 6;
    $(".listPreLoader").show();
    $.ajax({
        url: "/User/User_List/",
        type: "Get",
        cache: false,
        async: false,
        data: { skip: pageNumber },
        dataType: "Html",
        success: function (result) {
            if (result) {
                $("#UserList").html(result);
            } else {
                messageBox("error", "خطا در سیستم");
            }
        }
    });
    $(".listPreLoader").hide();
}
//_____________________________________________

function Save_User() {
    var json = {};
    // converting to formname:formvalue format
    $.each($('#formBody_User').serializeArray(), function (i, field) {
        json[field.name] = field.value || null;
    });
    $.ajax({
        url: "/User/User_Save/",
        type: "POST",
        cache: false,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: JSON.stringify(json),
        success: function (result) {
            if (result) {
                if ($("#Id").val() !== "0") {
                    messageBox("success", "ویرایش کاربر انجام شد");
                } else {
                    messageBox("success", "ثبت کاربر انجام شد");
                }
                List();
                $("#myModal").modal('hide');
            } else {
                messageBox("error", "خطا در ثبت کاربر");
            }
        }
    });
}
function Delete(id) {
    swal({
        title: "حذف کاربر",
        text: "کاربر مورد نظر حذف شود؟",
        type: "error",
        showCancelButton: true,
        confirmButtonClass: 'btn-danger waves-effect waves-light',
        confirmButtonText: "حذف"
    }, function (result) {
        if (result) {
            $.ajax({
                url: "/User/User_Delete/",
                type: "Get",
                cache: false,
                async: false,
                dataType: "json",
                data: { id: id },
                success: function (result) {
                    if (result) {
                        messageBox("success", "حذف کاربر انجام شد");
                        List();
                    } else {
                        messageBox("error", "خطا در حذف کاربر");
                    }
                }
            });
        } else {
            messageBox("info", "حذف کاربر لغو شد");
        }
    });
}

//_____________________________________________

function User_CorrespondenceAnswer(id) {
    $.ajax({
        url: "/User/User_CorrespondenceAnswer/",
        type: "Get",
        cache: false,
        dataType: "Html",
        data: { id: id },
        success: function (result) {
            if (result) {
                $("#ModalTitle").text("نظرات کاربر");
                $("#ModalBody").html(result);
                $("#myModal").modal();
            } else {
                messageBox("error", "خطا در سیستم");
            }
        }
    });
}

function User_Stations(id) {
    $.ajax({
        url: "/User/User_Stations/",
        type: "Get",
        cache: false,
        dataType: "Html",
        data: { id: id },
        success: function (result) {
            if (result) {
                $("#ModalTitle").text("ایستگاه های کاربر");
                $("#ModalBody").html(result);
                $("#myModal").modal();
                $("#User_Stattions_UserId").val(id);
                //GetStation($("#User_Stations_Project").val());
            } else {
                messageBox("error", "خطا در سیستم");
            }
        }
    });
}

function GetStation(id, userId) {
    $.ajax({
        url: "/User/GetStation/",
        type: "Get",
        cache: false,
        dataType: "json",
        data: { id: id, userId: userId },
        success: function (result) {
            if (result) {
                $("#User_Stations").empty();
                $.each(result, function () {
                    var cheked;
                    if (this['state'] == true)
                        cheked = "checked";

                    var check = '<div style="float: left;"> <input type="checkbox" ' + cheked + ' data-plugin="switchery" data-color="#188ae2" StationId="' + this['id'] + '"  Stationtext="' + this['name'] + '" data-size="small" id="UserStation" onchange="UserStationAccessChange(this)" /></div>';
                    $("#User_Stations").append('<li class="list-group-item" id="' + this['id'] + '"> <b>' + this['name'] + '</b>' + check + '</li>');
                });
                $('[data-plugin="switchery"]').each(function (idx, obj) {
                    new Switchery($(this)[0], $(this).data());
                });
            } else {
                messageBox("error", "خطا در سیستم");
            }
        }
    });
}

function User_Access(id) {
    $.ajax({
        url: "/User/User_AccessAsync/",
        type: "Get",
        cache: false,
        dataType: "Html",
        data: { id: id },
        success: function (result) {
            if (result) {
                $("#ModalTitle").text("دسترسی های کاربر");
                $("#ModalBody").html(result);
                $("#myModal").modal();
            } else {
                messageBox("error", "خطا در سیستم");
            }
        }
    });
}

function UserState(userId, state) {
    $.ajax({
        url: "/User/UserState/",
        type: "post",
        cache: false,
        async: false,
        dataType: "json",
        data: { userId: userId, state: state },
        success: function (result) {
            if (result) {
                if (state == "true") {
                    messageBox("success", "کاربر غیر فعال شد");
                } else {
                    messageBox("success", "کاربر فعال شد");
                }
                List();
            } else {
                messageBox("error", "خطا در سیستم");
            }
        }
    });
}

function User_Notification(userId, Name, LastName) {
    $.ajax({
        url: "/User/User_Notification/",
        type: "Get",
        cache: false,
        dataType: "Html",
        data: { UserId: userId },
        success: function (result) {
            if (result) {
                $("#ModalTitle").text("پیام های یک طرفه کاربر");
                $("#ModalBody").html(result);
                $("#NotificationUserName").text(Name + ' ' + LastName);
                $("#NotificationUserId").val(userId);
                $("#myModal").modal();
            } else {
                messageBox("error", "خطا در سیستم");
            }
        }
    });
}
function User_Notification_Save() {
    $.ajax({
        url: "/User/User_Notification_Save/",
        type: "POST",
        cache: false,
        dataType: "json",
        data: { UserId: $("#NotificationUserId").val(), Message: $("#NotificationUserMessage").val() },
        success: function (result) {
            if (result) {
                messageBox("success", "پیام برای کاربر ارسال شد");
                $("#myModal").modal('hide');
            } else {
                messageBox("error", "خطا در ارسال پیام");
            }
        }
    });
}