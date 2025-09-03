
function EvaporationReport_filter() {
    $(".listPreLoader").show();
   
        EvaporationReport_List($("#EvaporationReport_Station").val(), $("#FromDate").val(), $("#ToDate").val());
    
}
function EvaporationReport_List(stationId, startDate, endDate) {
    $.ajax({
        url: "/EvaporationReport/EvaporationReport_List/",
        type: "Get",
        cache: false,
        contentType: 'application/x-www-form-urlencoded',
        dataType: "html",
        data: { stationId: stationId, startDate: startDate, endDate: endDate },
        success: function (result) {
            if (result) {
                $("#EvaporationReport_List").html(result);
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
    window.open('/EvaporationReport/ExportToExcel?stationId=' + $("#EvaporationReport_Station").val() + '&stationname=' + $("#EvaporationReport_Station :selected").text() + '&startDate=' + $("#FromDate").val() + '&endDate=' + $("#ToDate").val());
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
    $("#EvaporationReport_Project").change(function () {
        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/EvaporationReport/GetStation/",
            data: { Projectid: $("#EvaporationReport_Project").val() },
            dataType: "json",
            success: function (city) {
                $("#EvaporationReport_Station").empty();
                $("#EvaporationReport_Station").append("<option selected>انتخاب ایستگاه</option>");
                $.each(city,
                    function () {
                        $("#EvaporationReport_Station").append($("<option></option>").val(this['id']).html(this['name']));
                    });
            }
        });
    });

});
