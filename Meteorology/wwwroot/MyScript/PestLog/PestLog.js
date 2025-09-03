function PestLog_List(projectId, stationId, fromDate, toDate) {
    $(".listPreLoader").show();
  
    $.ajax({
        url: "/PestLog/PestLog_List/",
        type: "Get",
        cache: false,
        dataType: "html",
        data: {  FromDate : fromDate , ToDate : toDate , ProjectId: projectId, StationId: stationId },
        success: function (result) {
            if (result) {
           
                $("#PestLog_List").html(result);
                $('[data-toggle="tooltip"]').tooltip();
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
            $(".listPreLoader").hide();
        }
    });
}
function PestLog_filter() {
    PestLog_List($("#PestLog_Project").val(), $("#PestLog_Station").val(), $("#FromDate").val(), $("#ToDate").val());
    TableManageButtons.init();
}
$(document).ready(function () {
    AlarmLog_List(0, 0, null, null);
    TableManageButtons.init();
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

    $("#AlarmLog_Project").change(function () {
        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/AlarmLog/GetStation/",
            data: { Projectid: $("#AlarmLog_Project").val() },
            dataType: "json",
            success: function (city) {
                $("#AlarmLog_Station").empty();
                $("#AlarmLog_Station").append("<option selected>اتنخاب ایستگاه</option>");
                $.each(city,
                    function () {
                        $("#AlarmLog_Station").append($("<option></option>").val(this['id']).html(this['name']));
                    });
            }
        });
    });
});
