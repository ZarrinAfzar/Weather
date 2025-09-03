
function DataErorrReport_filter() {
    $(".listPreLoader").show();
    //debugger;
    if ($("#Min").val() == "" || $("#Max").val() == "") {
        messageBox("error", "اطلاعات فرم را تکمیل نمایید");
        $(".listPreLoader").hide();
    }
    else {
        DataErorrReport_List($("#FromDate").val(), $("#ToDate").val());
    }
}
function DataErorrReport_List( startDate, endDate) {
    $.ajax({
        url: "/DataErorrReport/DataErorrReport_List/",
        type: "Get",
        cache: false,
        contentType: 'application/x-www-form-urlencoded',
        dataType: "html",
        data: {  startDate: startDate, endDate: endDate },
        success: function (result) {
            if (result) {
                $("#DataErorrReport_List").html(result);
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
    window.open('/DataErorrReport/ExportToExcel?startDate=' + $("#FromDate").val() + '&endDate=' + $("#ToDate").val());
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
    $("#DataErorrReport_Project").change(function () {
        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/DataErorrReport/GetStation/",
            data: { Projectid: $("#DataErorrReport_Project").val() },
            dataType: "json",
            success: function (city) {
                $("#DataErorrReport_Station").empty();
                $("#DataErorrReport_Station").append("<option selected>انتخاب ایستگاه</option>");
                $.each(city,
                    function () {
                        $("#DataErorrReport_Station").append($("<option></option>").val(this['id']).html(this['name']));
                    });
            }
        });
    });

});
