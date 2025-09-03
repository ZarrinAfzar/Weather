
function StationStatusReport_filter() {
    $(".listPreLoader").show();
    
  
        StationStatusReport_List( $("#FromDate").val());

}
function StationStatusReport_List(startDate) {
    $(".listPreLoader").show();
   
    $.ajax({
               url: "/StationStatusReport/StationStatusReport_List/",
        type: "Get",
        cache: false,
        dataType: "html",
        data: { startDate: startDate },
        success: function (result) {
            if (result) {
                $("#StationStatusReport_List").html(result);
                //$("#skipcount").val(skip);
                $('[data-toggle="tooltip"]').tooltip();
            } else {
                messageBox("error", "داده ای یافت نشد");
            }
            $(".listPreLoader").hide();
        }
    });
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
});
function ExportToExcel() {

    window.open('/StationStatusReport/ExportToExcel?startDate=' + $("#FromDate").val());
}
