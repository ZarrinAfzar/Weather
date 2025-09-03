
function RainReport_filter() {
    $(".listPreLoader").show();
   
        RainReport_List($("#RainReport_Station").val(), $("#FromDate").val(), $("#ToDate").val());
    
}
function RainReport_List(stationId, startDate, endDate) {
    $.ajax({
        url: "/RainReport/RainReport_List/",
        type: "Get",
        cache: false,
        contentType: 'application/x-www-form-urlencoded',
        dataType: "html",
        data: { stationId: stationId, startDate: startDate, endDate: endDate },
        success: function (result) {
            if (result) {
                $("#RainReport_List").html(result);
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
    window.open('/RainReport/ExportToExcel?stationId=' + $("#RainReport_Station").val() + '&stationname=' + $("#RainReport_Station :selected").text() + '&startDate=' + $("#FromDate").val() + '&endDate=' + $("#ToDate").val());
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
    $("#RainReport_Project").change(function () {
        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/RainReport/GetStation/",
            data: { Projectid: $("#RainReport_Project").val() },
            dataType: "json",
            success: function (city) {
                $("#RainReport_Station").empty();
                $("#RainReport_Station").append("<option selected>انتخاب ایستگاه</option>");
                $.each(city,
                    function () {
                        $("#RainReport_Station").append($("<option></option>").val(this['id']).html(this['name']));
                    });
            }
        });
    });

});
