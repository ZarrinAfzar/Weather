function AlarmLog_List(projectId, stationId, fromDate, toDate) {
    $(".listPreLoader").show();
  
    $.ajax({
        url: "/AlarmLog/AlarmLog_List/",
        type: "Get",
        cache: false,
        dataType: "html",
        data: {  FromDate : fromDate , ToDate : toDate , ProjectId: projectId, StationId: stationId },
        success: function (result) {
            if (result) {
           
                $("#AlarmLog_List").html(result);
                $('[data-toggle="tooltip"]').tooltip();
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
            $(".listPreLoader").hide();
        }
    });
}
function AlarmLog_filter() {
    AlarmLog_List($("#AlarmLog_Project").val(), $("#AlarmLog_Station").val(), $("#FromDate").val(), $("#ToDate").val());
    TableManageButtons.init();
}
