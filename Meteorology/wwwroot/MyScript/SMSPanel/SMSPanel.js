
$(document).ready(function () {
    $(".select2").select2();

    SMSPanel_List("", "", "");
    $("#SMSPanelSort").change(function () {
        SMSPanel_List($("#OperatorNameSearch").val(), $("#MessageSearch_value").val(), $(this).val());
    });

    $(document).on('input', '#Tel', function () {
        this.value = this.value.replace(/\D/g, '').slice(0, 11);
        $(this).removeClass('is-invalid');
        $(this).next('.text-danger').remove();
    });
    $(document).on('blur', '#Tel', function () {
        const value = $(this).val();
        const isValid = /^09\d{9}$/.test(value);

        if (!isValid) {
            if ($(this).next('.text-danger').length === 0) {
                $(this).after('<div class="text-danger mt-1">شماره تلفن باید 11 رقم باشد و با 09 شروع شود.</div>');
                $('.btn.btn-primary').prop('disabled', true);
            }
            $(this).addClass('is-invalid');
        } else {
            $(this).removeClass('is-invalid');
            $(this).next('.text-danger').remove();
            $('.btn.btn-primary').prop('disabled', false);
        }
    });
});
function UserName_Search() {
    if ($("#OperatorNameSearch").val()) {
        SMSPanel_List($("#OperatorNameSearch").val(), $("#MessageSearch_value").val(), $(SMSPanelSort).val());
    } else {
        messageBox("error", "مقدار جستجو وارد نشده است");
    }
}
function Message_Search() {
    if ($("#MessageSearch_value").val()) {
        SMSPanel_List($("#OperatorNameSearch").val(), $("#MessageSearch_value").val(), $(SMSPanelSort).val());
    } else {
        messageBox("error", "مقدار جستجو وارد نشده است");
    }
}
function Send() {
    var UserGeter = $("#UserGeter").val();
    var Message = $("#MessageText").val();
    $.ajax({
        url: "/SMSPanel/SendMessage/",
        type: "POST",
        cache: false,
        dataType: "json",
        data: { OperatorGeter: UserGeter, Message: Message },
        success: function (result) {
            if (result) {
                $("#UserGeter").val("");
                $("#MessageText").val("");
                messageBox("success", "ارسال پیام انجام شد");
                SMSPanel_List();
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}
function SMSPanel_List(userNameSearch, valueSearch, sort) {
    $.ajax({
        url: "/SMSPanel/SMSPanel_List/",
        type: "Get",
        cache: false,
        dataType: "Html",
        data: { operatorNameSearch: userNameSearch, valueSearch: valueSearch, sort: sort },
        success: function (result) {
            if (result) {
                $("#SMSPanelList").empty();
                $("#SMSPanelList").html(result);
            } else {
                alert("خطا رخ داده است");
            }
        }
    });
}
function SMSPanelMessage(Id) {
    $.ajax({
        url: "/SMSPanel/SMSPanelMessage/",
        type: "POST",
        cache: false,
        dataType: "json",
        data: { SMSPanelId: Id },
        success: function (result) {
            if (result) {
                $("#ModalTitle").text("متن پیام");
                $("#ModalBody").html("<b>" + result + "</b>");
                $("#myModal").modal();
                SMSPanel_List();
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
                url: "/SMSPanel/SMSPanelDelete/",
                type: "Get",
                cache: false,
                async: false,
                dataType: "json",
                data: { id: id },
                success: function (result) {
                    if (result) {
                        SMSPanel_List();
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
function ManagerTel_filter() {
    $.ajax({
        url: "/SMSPanel/ManagerTel_list",
        type: "GET",
        cache: false,
        dataType: "html",
        success: function (result) {
            if (result) {
                $("#ManagerTel_List").empty().html(result);
            } else {
                alert("خطا رخ داده است");
            }
        },
        error: function (xhr, status, error) {
            alert("خطا در دریافت اطلاعات از سرور: " + error);
        }
    });
}

function ManagerTel_Get(id) {
    $(".listPreLoader").show();
    $.ajax({
        url: "/SMSPanel/ManagerTel_Get/",
        type: "Get",
        cache: false,
        async: false,
        dataType: "html",
        data: { id: id },
        success: function (result) {
            if (result) {
                $("#modalBody").html(result);
                $('#formModal').modal('show');
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        },
        error: function () {
            messageBox("error", "خطا در دریافت اطلاعات از سرور");
        },
        complete: function () {
            $(".listPreLoader").hide();
        }
    });
}
function ManagerTel_Create() {
    $("#modalBody").load("/SMSPanel/ManagerTel_Create", function () {
        $("#formModal").modal('show');

        $("#formManagerTelCreate").submit(function (e) {
            e.preventDefault();

            var form = $(this);

            $.ajax({
                url: form.attr("action"),
                type: form.attr("method"),
                data: form.serialize(),
                dataType: "json",
                success: function (response) {
                    if (response.success) {
                        messageBox("success", "ثبت تلفن انجام شد");
                        ManagerTel_filter();
                        $("#formModal").modal('hide');
                    } else {
                        alert("خطا در ذخیره‌سازی");
                    }
                },
                error: function () {
                    alert("خطا در ارسال درخواست به سرور.");
                }
            });
        });
    });
}
function ManagerTel_Edit(form) {
    $(".listPreLoader").show();
    /*$("#formErrors").hide().empty();*/

    $.ajax({
        url: "/SMSPanel/ManagerTel_Edit/",
        type: "POST",
        data: $(form).serialize(),
        dataType: "json",
        success: function (response) {
            if (response.success) {
                $('#formModal').modal('hide');
                messageBox("success", "ویرایش با موفقیت انجام شد");
                ManagerTel_filter();
            } else {
                if (response.errors && response.errors.length > 0) {
                    let htmlErrors = "<ul>";
                    response.errors.forEach(function (err) {
                        htmlErrors += "<li>" + err + "</li>";
                    });
                    htmlErrors += "</ul>";
                    $("#formErrors").html(htmlErrors).show();
                } else {
                    messageBox("error", "خطایی رخ داده است");
                }
            }
        },
        error: function () {
            messageBox("error", "خطا در دریافت اطلاعات از سرور");
        },
        complete: function () {
            $(".listPreLoader").hide();
        }
    });
    return false;
}
function ManagerTel_Delete(id) {

    swal({
        title: "حذف تلفن",
        text: "تلفن مورد نظر حذف شود؟",
        type: "error",
        showCancelButton: true,
        confirmButtonClass: 'btn-danger waves-effect waves-light',
        confirmButtonText: "حذف",
        cancelButtonText: "لغو"
    }, function (result) {
        if (result) {
            $.ajax({
                url: "/SMSPanel/ManagerTel_Delete/",
                type: "Get",
                cache: false,
                dataType: "json",
                data: { id: id },
                success: function (result) {
                    if (result) {
                        //Delete_ManagerTel_List(result);
                        messageBox("success", "حذف تلفن انجام شد");
                        ManagerTel_filter();
                    } else {
                        messageBox("error", "خطا رخ داده است");
                    }
                }
            });
        } else {
            messageBox("info", "حذف تلفن لغو شد");
        }
    });
}