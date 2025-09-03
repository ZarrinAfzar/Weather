$(document).ready(function () {
    $("#ExportToExcel").hide();
    $("#DDChart_Project").change(function () {
        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/DDChart/GetStation/",
            data: { Projectid: $("#DDChart_Project").val() },
            dataType: "json",
            success: function (station) {
                $("#DDChart_Station").empty();
                $("#DDChart_Station").append("<option selected>انتخاب ایستگاه</option>");
                $.each(station,
                    function () {
                        $("#DDChart_Station").append($("<option></option>").val(this['id']).html(this['name']));
                    });
            }
        });
    });
    $("#DDChart_Station").change(function () {
        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/DDChart/GetAlarms/",
            data: { Stationid: $("#DDChart_Station").val() },
            dataType: "json",
            success: function (Alarm) {
                $("#DDChart_Alarms").empty();
                $("#DDChart_Alarms").append("<option selected>انتخاب آفت</option>");

                $.each(Alarm,
                    function () {
                        $("#DDChart_Alarms").append($("<option></option>").val(this['id']).html(this['alarmName']));
                    });
            }
        });
    });
});
function ExportToExcel() {
   
    window.open('/DDChart/ExportToExcel?stationname=' + $("#DDChart_Station :selected").text() + '&dDType=' + $('input[name=DDType]:checked').val() + '&alarmId=' + $("#DDChart_Alarms").val() );
}
function DDChart_filter() {
    $(".listPreLoader").show();
    $.ajax({
        url: "/DDChart/DDChartValue/",
        type: "Get",
        cache: false,
        dataType: "json",
        data: {
            DDType: $('input[name=DDType]:checked').val(),
            AlarmId: $("#DDChart_Alarms").val(),
        },

        success: function (result) {
            $("#ExportToExcel").show();
            if (result) {
                var JSON = [];
                if (result.data)
                    JSON.push(
                        {
                            name: $("#DDChart_Alarms:selected").text(),
                            data: result.data
                        });
                Highcharts.chart('trend_bl', {
                    chart: {
                        zoomType: 'xy'
                    },
                    title: {
                        text: 'نمودار درجه روز رشد '
                    },
                    xAxis: {
                        step: "60",
                        categories: result.date
                    },
                    yAxis: {
                        title: {
                            text: ' '
                        },
                        min: 0,
                        max: result.max
                    },
                    legend: {
                        layout: 'vertical',
                        align: 'right',
                        verticalAlign: 'middle'
                    },
                    exporting: {
                        filename: $("#DDChart_Alarms :selected").text()
                    },
                    series: JSON,
                });
            } else {
                messageBox("error", "اطلاعات فرم را تکمیل نمایید");
            }
            $(".listPreLoader").hide();
        }
    });

}
