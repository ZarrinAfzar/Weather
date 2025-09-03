
function VirtualSensorDetail_GetNew(id) {
    $.ajax({
        type: "Get",
        cache: false,
        async: false,
        url: "/VirtualSensorDetail/VirtualSensorDetail_GetNew/",
        data: { Id: id },
        dataType: "Html",
        success: function (result) {
            $("#VirtualSensorDetail_Get").html(result);
            $(".date").pDatepicker({
                format: 'YYYY/MM/DD',
                initialValue: false,
                initialValueType: 'persian',
                autoClose: true,
                navigator: {
                    scroll: {
                        enabled: false
                    }
                }
            });
            $(".time").pDatepicker({
                format: 'HH:mm:ss',
                initialValue: false,
                initialValueType: 'persian',
                onlyTimePicker :true,
                autoClose: true,
                timePicker: {
                    enabled: true,
                    meridiem: {
                        enabled: false
                    }
                },
                datePicker: {
                    enabled: false
                },
                navigator: {
                    scroll: {
                        enabled: false
                    }
                }
            });
        }
    });
}
function VirtualSensorDetail_GetNew_Save() { 
    $.ajax({
        url: "/VirtualSensorDetail/VirtualSensorDetail_GetNew_Save/",
        type: "POST",
        cache: false,
        async: false,
        //contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: $("#formBody_VirtualSensorBase").serialize(),
        success: function (result) {
            if (result) {
                messageBox("success", "ثبت پارامتر محاسباتی انجام شد");
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}
function VirtualSensorDetail_GetEdit(id) {
    $.ajax({
        type: "Get",
        cache: false,
        async: false,
        url: "/VirtualSensorDetail/VirtualSensorDetail_GetEdit/",
        data: { Id: id },
        dataType: "Html",
        success: function (result) {
            $("#VirtualSensorDetail_Get").html(result);
            $(".date").pDatepicker({
                format: 'YYYY/MM/DD',
                initialValue: false,
                initialValueType: 'persian',
                autoClose: true, 
                navigator: {
                    scroll: {
                        enabled: false
                    }
                }
            });
            $(".time").pDatepicker({
                format: 'HH:mm:ss',
                initialValue: false,
                initialValueType: 'persian',
                onlyTimePicker :true,
                autoClose: true,
                timePicker: {
                    enabled: true,
                    meridiem: {
                        enabled: false
                    }
                },
                datePicker: {
                    enabled: false 
                },
                navigator: {
                    scroll: {
                        enabled: false
                    }
                }
            });
        }
    });
}
function VirtualSensorDetail_GetEdit_Save() { 
    $.ajax({
        url: "/VirtualSensorDetail/VirtualSensorDetail_GetEdit_Save/",
        type: "POST",
        cache: false,
        async: false,
        dataType: "json",
        data: $("#formBody_VirtualSensorDetail").serialize(),
        success: function (result) {
            if (result) {
                messageBox("success", "ویرایش پارامتر محاسباتی انجام شد");
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}
function VirtualSensorBase_List() {
    $.ajax({
        url: "/VirtualSensorDetail/VirtualSensorBase_List/",
        type: "Get",
        cache: false,
        async: false,
        dataType: "Html",
        success: function (result) {
            if (result) {
                $("#VirtualSensorBase_List").html(result);
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}
$(document).ready(function () {

    VirtualSensorBase_List();

    $("#VirtualSensorDetail_Project").change(function () {
        $.ajax({
            type: "POST",
            cache: false,
            async: true,
            url: "/VirtualSensorDetail/GetStation/",
            data: { projectId: $("#VirtualSensorDetail_Project").val() },
            dataType: "json",
            success: function (city) {
                $("#VirtualSensorDetail_Station").empty();
                $("#VirtualSensorDetail_Station").append("<option selected>اتنخاب ایستگاه</option>");
                $.each(city,
                    function () {
                        $("#VirtualSensorDetail_Station").append($("<option></option>").val(this['id']).html(this['name']));
                    });
            }
        });
    });
    $("#VirtualSensorDetail_Station").change(function () {
        $(".listPreLoader").show();
        $.ajax({
            type: "POST",
            cache: false,
            url: "/VirtualSensorDetail/GetVirtualDetail/",
            data: { stationId: $("#VirtualSensorDetail_Station").val() },
            dataType: "json",
            success: function (city) {
                $("#VirtualSensorDetail_VirtualSensorDetail").empty();
                $("#VirtualSensorDetail_VirtualSensorDetail").append("<option selected>اتنخاب پارامتر محاسباتی</option>");
                $.each(city,
                    function () {
                        $("#VirtualSensorDetail_VirtualSensorDetail").append($("<option></option>").val(this['id']).html(this['title']));
                    });
                    $(".listPreLoader").hide();
            }
        });
    });
    $("#VirtualSensorDetail_VirtualSensorDetail").change(function () {
        VirtualSensorDetail_GetEdit($(this).val());
    });

    
});


//.done(function () {
//    $.ajax({
//        type: "Get",
//        cache: false,
//        url: "/VirtualSensorDetail/VirtualSensorDetail_List/",
//        data: { stationId: $("#VirtualSensorDetail_Station").val() },
//        dataType: "Html",
//        success: function (result) {
//            $("#VirtualSensorDetail_List").html(result);
//        }
//    });
//})