$('.dropify').dropify({
    messages: {
        'default': 'فایل را به اینجا بکشید یا کلیک کنید',
        'replace': 'برای جایگزینی فایل را به اینجا بکشید یا کلیک کنید',
        'remove': 'پاک کردن',
        'error': 'با پوزش فراوان، خطایی رخ داده'
    },
    error: {
        'fileSize': 'حجم فایل بیشتر از حد مجاز است (1M).'
    }
});


$(document).ready(function () {
    $("#Statedrp").change(function () {
        $.ajax({
            type: "Get",
            url: "/Account/GetCities/",
            data: { stateId: $("#Statedrp").val() },
            async: false,
            cache: false,
            contentType: "application/json",
            dataType: "json"
        }).done(function (city) {
            $("#CityId").empty();
            $.each(city, function () {
                $("#CityId").append($("<option></option>").val(this['id']).html(this['name']));
            });
        });
    });
    
});
function SendRegisterCode() {
    $.ajax({
        url: "/Account/SendRegisterCode/",
        type: "POST",
        async: false,
        cache: false,
        dataType: "json",
        data: { userId: $("#UserId").val(), RegisterCode: $("#registerCode").val() },
        success: function (result) {
            if (result) {
                swal({
                    title: "Registered",
                    text: "حساب کاربری شما فعال شد مجدد وارد شوید",
                    type: "info",
                    showCancelButton: true,
                    confirmButtonClass: 'btn-success waves-effect waves-light',
                    confirmButtonText: "ورود مجدد"
                }, function (result) {
                    if (result) {
                        window.location.replace("/Account/Logout");
                    } else {
                        messageBox("info", "لغو ورود مجدد");
                    }
                });
            } else {
                messageBox("error", "خطا در سیستم");
            }
        }
    });
}
function User_Regisiter(id) {
    $.ajax({
        url: "/Account/UserRegister/",
        type: "Get",
        cache: false,
        dataType: "Html",
        data: { userId: id },
        success: function (result) {
            if (result) {
                $("#ModalTitle").hide();//text("فعالسازی حساب کاربری");
                $("#ModalBody").html(result);
                $("#myModal").modal();
            } else {
                messageBox("error", "خطا در سیستم");
            }
        }
    });
}
function sendsms(id) {
        $(".sendsms").hide();
    var code = $("#RegisterCode").val();
    var PhoneNumber = $("#PhoneNumber").val();
   
    $.ajax({
        url: "/Account/SendSMS/",
        type: "post",
        async: false,
        cache: false,
        dataType: "json",
        data: { userId: id, Rcode: code, Phone: PhoneNumber },
        success: function (result) {
            
            if (result) {
                messageBox("info", "کد فعال سازی ارسال شد");
               
            } else {
                messageBox("error", "خطا در سیستم");
            }
        }
    });
}