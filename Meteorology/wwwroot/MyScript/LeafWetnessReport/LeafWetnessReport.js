
function LeafWetnessReport_filter() {
    $(".listPreLoader").show();
    //debugger;
    if ( $("#Max").val() == "") {
        messageBox("error", "اطلاعات فرم را تکمیل نمایید");
        $(".listPreLoader").hide();
    }
    else {
        LeafWetnessReport_List($("#LeafWetnessReport_Station").val(), $("#FromDate").val(), $("#ToDate").val(), $("#Max").val());
    }
}
function LeafWetnessReport_List(stationId, startDate, endDate, max) {
    $.ajax({
        url: "/LeafWetnessReport/LeafWetnessReport_List/",
        type: "Get",
        cache: false,
        contentType: 'application/x-www-form-urlencoded',
        dataType: "html",
        data: { stationId: stationId, startDate: startDate, endDate: endDate, max: max },
        success: function (result) {
            $("#ExportToExcel").show();
            if (result) {
                $("#LeafWetnessReport_List").html(result);
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
   
    window.open('/LeafWetnessReport/ExportToExcel?stationId=' + $("#LeafWetnessReport_Station").val() + '&stationname=' + $("#LeafWetnessReport_Station :selected").text() + '&startDate=' + $("#FromDate").val() + '&endDate=' + $("#ToDate").val() + '&max=' + $('#Max').val());
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
    $("#LeafWetnessReport_Project").change(function () {
        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/LeafWetnessReport/GetStation/",
            data: { Projectid: $("#LeafWetnessReport_Project").val() },
            dataType: "json",
            success: function (city) {
                $("#LeafWetnessReport_Station").empty();
                $("#LeafWetnessReport_Station").append("<option selected>انتخاب ایستگاه</option>");
                $.each(city,
                    function () {
                        $("#LeafWetnessReport_Station").append($("<option></option>").val(this['id']).html(this['name']));
                    });
            }
        });
    });

});
