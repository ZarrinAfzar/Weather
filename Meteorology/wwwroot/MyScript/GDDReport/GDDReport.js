
function GDDReport_filter() {
    $(".listPreLoader").show();
    //debugger;
    if ($("#Min").val() == "" || $("#Max").val() == "") {
        messageBox("error", "اطلاعات فرم را تکمیل نمایید");
        $(".listPreLoader").hide();
    }
    else {
        GDDReport_List($("#GDDReport_Station").val(), $("#FromDate").val(), $("#ToDate").val(), $("#Min").val(), $("#Max").val(), $('input[name=DDType]:checked').val());
    }
}
function GDDReport_List(stationId, startDate, endDate, min, max, DDType) {
    $.ajax({
        url: "/GDDReport/GDDReport_List/",
        type: "Get",
        cache: false,
        contentType: 'application/x-www-form-urlencoded',
        dataType: "html",
        data: { stationId: stationId, startDate: startDate, endDate: endDate, Tmin: min, Tmax: max, DDType:DDType},
        success: function (result) {
            $("#ExportToExcel").show();
            if (result) {
                $("#GDDReport_List").html(result);
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
       window.open('/GDDReport/ExportToExcel?stationId=' + $("#GDDReport_Station").val() + '&stationname=' + $("#GDDReport_Station :selected").text() + '&startDate=' + $("#FromDate").val() + '&endDate=' + $("#ToDate").val() + '&tmin=' + $('#Min').val() + '&tmax=' + $('#Max').val()+ '&dDType=' + $('input[name=DDType]:checked').val() );
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
    $("#GDDReport_Project").change(function () {
        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/GDDReport/GetStation/",
            data: { Projectid: $("#GDDReport_Project").val() },
            dataType: "json",
            success: function (city) {
                $("#GDDReport_Station").empty();
                $("#GDDReport_Station").append("<option selected>انتخاب ایستگاه</option>");
                $.each(city,
                    function () {
                        $("#GDDReport_Station").append($("<option></option>").val(this['id']).html(this['name']));
                    });
            }
        });
    });

});
