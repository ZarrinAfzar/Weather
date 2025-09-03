
function GapReport_filter() {
    $(".listPreLoader").show();
  
        GapReport_List( $("#FromDate").val(), $("#ToDate").val());
   
}
function GapReport_List(startDate, endDate) {
    
    $.ajax({
        url: "/GapReport/GapReport_List/",
        type: "Get",
        cache: false,
        contentType: 'application/x-www-form-urlencoded',
        dataType: "html",
        data: {  startDate: startDate, endDate: endDate },
        success: function (result) {
            if (result) {
                $("#GapReport_List").html(result);
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
   
    window.open('/GapReport/ExportToExcel?startDate=' + $("#FromDate").val() + '&endDate=' + $("#ToDate").val());
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
