
function GCDReport_filter() {
    $(".listPreLoader").show();
    //debugger;
    if ($("#Min").val() == "" || $("#Max").val() == "") {
        messageBox("error", "اطلاعات فرم را تکمیل نمایید");
        $(".listPreLoader").hide();
    }
    else {
        GCDReport_List($("#GCDReport_Station").val(), $("#FromDate").val(), $("#ToDate").val(), $("#Min").val(), $("#Max").val());
    }
}
function GCDReport_List(stationId, startDate, endDate, min, max) {
    $.ajax({
        url: "/GCDReport/GCDReport_List/",
        type: "Get",
        cache: false,
        contentType: 'application/x-www-form-urlencoded',
        dataType: "html",
        data: { stationId: stationId, startDate: startDate, endDate: endDate, min: min, max: max },
        success: function (result) {
            if (result) {
                $("#GCDReport_List").html(result);
                //$("#skipcount").val(skip);
                $('[data-toggle="tooltip"]').tooltip();
            } else {
                messageBox("error", "اطلاعات فرم را تکمیل نمایید");
            }
            $(".listPreLoader").hide();
        }
    });
}

function ExportToExcel() {
    var computing = "";
    $('input[name=computingType]:checked').each(function (i) {
        computing += $(this).val();
    });
    window.open('/GCDReport/ExportToExcel?stationId=' + $("#GCDReport_Station").val() + '&stationname=' + $("#GCDReport_Station :selected").text() + '&startDate=' + $("#FromDate").val() + '&endDate=' + $("#ToDate").val() + '&min=' + $('#Min').val() + '&max=' + $('#Max').val());
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
    $("#GCDReport_Project").change(function () {
        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/GCDReport/GetStation/",
            data: { Projectid: $("#GCDReport_Project").val() },
            dataType: "json",
            success: function (city) {
                $("#GCDReport_Station").empty();
                $("#GCDReport_Station").append("<option selected>انتخاب ایستگاه</option>");
                $.each(city,
                    function () {
                        $("#GCDReport_Station").append($("<option></option>").val(this['id']).html(this['name']));
                    });
            }
        });
    });

});
