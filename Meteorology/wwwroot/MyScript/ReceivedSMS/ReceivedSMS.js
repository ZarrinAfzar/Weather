var skip = 0;
function ReceivedSMS_filter() {
    $(".listPreLoader").show();
    ReceivedSMS_List($("#ReceivedSMS_Station").val(), $("#FromDate").val(), $("#ToDate").val(), $("#ReceivedSMS_Type").val());
}
function ReceivedSMS_List(stationId, startDate, endDate,type) {
    $.ajax({
        url: "/ReceivedSMS/ReceivedSMS_List/",
        type: "Get",
        cache: false,
        contentType: 'application/x-www-form-urlencoded',
        dataType: "html",
        data: { stationId: stationId, startDate: startDate, endDate: endDate ,type:type},
        success: function (result) {
            if (result) {
                $("#ReceivedSMS_List").html(result);
                //$("#skipcount").val(skip);
                $('[data-toggle="tooltip"]').tooltip();
            } else {
                messageBox("error", "اطلاعات فرم را تکمیل نمایید");
            }
            $(".listPreLoader").hide();
        }
    });
}
function ReceivedSMS_Delete(id) {
    swal({
        title: "حذف پیامک",
        text: "پیامک مورد نظر حذف شود؟",
        type: "error",
        showCancelButton: true,
        confirmButtonClass: 'btn-danger waves-effect waves-light',
        confirmButtonText: "حذف"
    }, function (result) {
        if (result) {
            $.ajax({
                url: "/ReceivedSMS/ReceivedSMS_Delete/",
                type: "Get",
                cache: false,
                async: false,
                dataType: "json",
                data: { id: id },
                success: function (result) {
                    if (result) {
                        StationList();
                        DetailStation();
                        messageBox("success", "حذف پیامک انجام شد");
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
function ReceivedSMS_Accepted(id) {
    swal({
        title: "تایید پیامک",
        text: "پیامک مورد نظر تایید شود؟",
        type: "error",
        showCancelButton: true,
        confirmButtonClass: 'btn-danger waves-effect waves-light',
        confirmButtonText: "تایید"
    }, function (result) {
        if (result) {
            $.ajax({
                url: "/ReceivedSMS/ReceivedSMS_Activated/",
                type: "Get",
                cache: false,
                async: false,
                dataType: "json",
                data: { id: id, activated:true},
                success: function (result) {
                    if (result) {
                        messageBox("success", "تغییر وضعیت تایید انجام شد");
                        ReceivedSMS_filter();
                    } else {
                        messageBox("error", "خطا رخ داده است");
                    }
                }
            });
        } else {
            messageBox("info", " تایید پیامک لغو شد");
        }
    });
    
}

function ReceivedSMS_DeAccepted(id) {
    swal({
        title: "تایید پیامک",
        text: "پیامک مورد نظر تایید نمی شود؟",
        type: "error",
        showCancelButton: true,
        confirmButtonClass: 'btn-danger waves-effect waves-light',
        confirmButtonText: "تایید"
    }, function (result) {
        if (result) {
            $.ajax({
                url: "/ReceivedSMS/ReceivedSMS_Activated/",
                type: "Get",
                cache: false,
                async: false,
                dataType: "json",
                data: { id: id, activated:false },
                success: function (result) {
                    if (result) {
                        messageBox("success", "تغییر وضعیت تایید انجام شد");
                        ReceivedSMS_filter();
                    } else {
                        messageBox("error", "خطا رخ داده است");
                    }
                }
            });
        } else {
            messageBox("info", " تایید پیامک لغو شد");
        }
    });
   
}
function ReceivedSMS_EditValue(Id) {
    $.ajax({
        url: "/ReceivedSMS/ReceivedSMS_EditValue/",
        type: "Get",
        cache: false,
        async: false,
        dataType: "html",
        data: { Id: Id },
        success: function (result) {
            if (result) {
                $("#ModalTitle").text("ویرایش پیامک");
                $("#ModalBody").html(result);
                $("#myModal").modal();
            } else {
                messageBox("error", "خطا  رخ داده است");

            }
        }
    });
}
function ReceivedSMS_Save() {

        $(".listPreLoader").show();
        var model = {};
        $.each($('#formBody_ReceivedSMS').serializeArray(), function (i, field) {
            model[field.name] = field.value || null;
        });
        $.ajax({
            url: "/ReceivedSMS/ReceivedSMS_Save/",
            type: "Post",
            cache: false,
            dataType: "json",
            data: model,
            success: function (result) {
                if (result) {
                    $("#myModal").modal("hide");
                    $(".listPreLoader").hide();
                    messageBox("success", "ویرایش پیامک انجام شد");
                    ReceivedSMS_filter();
                    messageBox("info", "لیست پیامک ها به روز شد");
                } else {
                    messageBox("error", "خطا  رخ داده است");

                }
            }
        });
 }
function ExportToExcel() {
  
    window.open('/ReceivedSMS/ExportToExcel?stationId=' + $("#ReceivedSMS_Station").val() + '&stationname=' + $("#ReceivedSMS_Station :selected").text() + '&startDate=' + $("#FromDate").val() + '&endDate=' + $("#ToDate").val()+'&type='+$("#ReceivedSMS_Type").val());
}
$(document).ready(function () {
    $(".datepicker").pDatepicker({
        format: 'YYYY/MM/DD HH:mm:ss',
        initialValue: false,
        initialValueType: 'persian',
        autoClose: true,
        timePicker: {
            enabled: true,
            meridiem: {
                enabled: false
            }
        },
        navigator: {
            scroll: {
                enabled: false
            }
        },
    });
    $("#ReceivedSMS_Project").change(function () {
        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/ReceivedSMS/GetStation/",
            data: { Projectid: $("#ReceivedSMS_Project").val() },
            dataType: "json",
            success: function (city) {
                $("#ReceivedSMS_Station").empty();
                $("#ReceivedSMS_Station").append("<option selected>انتخاب ایستگاه</option>");
                $.each(city,
                    function () {
                        $("#ReceivedSMS_Station").append($("<option></option>").val(this['id']).html(this['name']));
                    });
            }
        });
    });
  
});
