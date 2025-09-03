$(document).ready(function () {
    PestAlarmReport_List();
    TableManageButtons.init();
    $("#PestAlarmReport_Project").change(function () {
        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/PestAlarmReport/GetStation/",
            data: { Projectid: $("#PestAlarmReport_Project").val() },
            dataType: "json",
            success: function (city) {
                $("#PestAlarmReport_Station").empty();
                $("#PestAlarmReport_Station").append("<option selected>اتنخاب ایستگاه</option>");
                $.each(city,
                    function () {
                        $("#PestAlarmReport_Station").append($("<option></option>").val(this['id']).html(this['name']));
                    });
            }
        });
    });
    $("#PestAlarmReport_Station").change(function () {
        PestAlarmReport_filter();
    });
});
function PestAlarmReport_List(projectId, stationId) {
    $.ajax({
        url: "/PestAlarmReport/PestAlarmReport_List/",
        type: "Get",
        cache: false,
        async: false,
        dataType: "html",
        data: { ProjectId: projectId, StationId: stationId },
        success: function (result) {
            if (result) {
                $("#PestAlarmReport_List").html(result);
                $('[data-toggle="tooltip"]').tooltip();
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}

function PestAlarmReport_filter() {
    PestAlarmReport_List($("#PestAlarmReport_Project").val(), $("#PestAlarmReport_Station").val() );
    TableManageButtons.init();
}