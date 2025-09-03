$(document).ready(function () {
    AlarmReport_List();
    TableManageButtons.init();
    $("#AlarmReport_Project").change(function () {
        AlarmReport_List($("#AlarmReport_Project").val());
        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/AlarmReport/GetStation/",
            data: { Projectid: $("#AlarmReport_Project").val() },
            dataType: "json",
            success: function (result) {

                $("#AlarmReport_Station").empty();
                $("#AlarmReport_Station").append("<option selected>انتخاب ایستگاه</option>");
                $.each(result,
                    function () {
                        $("#AlarmReport_Station").append($("<option></option>").val(this['id']).html(this['name']));
                    });

            }
        });
    });
    $("#AlarmReport_Station").change(function () {

        AlarmReport_List($("#AlarmReport_Project").val(), $("#AlarmReport_Station").val());

    });
});
function AlarmReport_List(projectId, stationId) {
    $.ajax({
        url: "/AlarmReport/AlarmReport_List/",
        type: "Get",
        cache: false,
        async: false,
        dataType: "html",
        data: { ProjectId: projectId, StationId: stationId },
        success: function (result) {
            if (result) {
                $("#AlarmReport_List").empty();
                $("#AlarmReport_List").html(result);
                $('[data-toggle="tooltip"]').tooltip();
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}
function PestAlarmLog(Id) {
    $.ajax({
        url: "/AlarmReport/PestAlarmLog_Get/",
        type: "Get",
        cache: false,
        async: false,
        dataType: "html",
        data: { id: Id },
        success: function (result) {
            if (result) {
                $("#ModalTitle").text("'گزارش آلارم آفات");
                $("#ModalBody").html(result);
                $("#myModal").modal();
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}
function SensorAlarmLog(Id) {
    $.ajax({
        url: "/AlarmReport/SensorAlarmLog_Get/",
        type: "Get",
        cache: false,
        async: false,
        dataType: "html",
        data: { id: Id },
        success: function (result) {
            if (result) {
                $("#ModalTitle").text("گزارش آلارم سنسور");
                $("#ModalBody").html(result);
                $("#myModal").modal();
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}
function SmsSendLog(Id) {
    $.ajax({
        url: "/AlarmReport/SmsSendLog_Get/",
        type: "Get",
        cache: false,
        async: false,
        dataType: "html",
        data: { id: Id },
        success: function (result) {
            if (result) {
                $("#ModalTitle").text("گزارش پیامک های آلارم");
                $("#ModalBody").html(result);
                $("#myModal").modal();
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}