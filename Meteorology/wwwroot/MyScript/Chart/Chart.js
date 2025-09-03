
function Chart_filter() {
    $(".listPreLoader").show();
    $.ajax({
        url: "/Chart/ChartValue/",
        type: "Get",
        cache: false,
        dataType: "json",
        data: {
            stationId: $("#Chart_Station").val(),
            startDate: $("#FromDate").val(),
            endDate: $("#ToDate").val(),
            DateType: $('input[name=DateType]:checked').val(),
            computingType_Right: $('input[name=Right_computingType]:checked').val(),
            computingType_Left: $('input[name=Left_computingType]:checked').val(),
            Sensor_Right: $("#Chart_RightSensor").val(),
            Sensor_Left: $("#Chart_LeftSensor").val()
        },
        success: function (result) {
            if (result) {
                $(".listPreLoader").hide();
                debugger;
                var JSON = [];
                if (result.data1)
                    JSON.push(
                        {
                            name: $("#Chart_RightSensor :selected").text(),
                            data: result.data1
                        });
                if (result.data2)
                    JSON.push(
                        {
                            name: $("#Chart_LeftSensor :selected").text(),
                            data: result.data2
                        });

                Highcharts.chart('trend_bl', {
                    chart: {
                        zoomType: 'xy',
                        style: {
                            fontSize: '10px'
                        }
                        },
                        title: {
                            text: ' '
                        },
                        xAxis: {
                            step: "60",
                            categories: result.date
                        },
                        yAxis: {
                            title: {
                                text: ' '
                            },
                            min: result.min,
                            max: result.max
                        },
                        legend: {
                            layout: 'vertical',
                            align: 'right',
                            verticalAlign: 'middle'
                        },
                        exporting: {
                            filename: $("#Chart_Station :selected").text()
                        },
                        series: JSON,
                    });
            } else {
                messageBox("error", "اطلاعات ورودی فرم را تکمیل نمایید");
            }

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
        }
    });
    //$("#lineChart").hide();
    $("#Chart_Project").change(function () {
        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/Chart/GetStation/",
            data: { Projectid: $("#Chart_Project").val() },
            dataType: "json",
            success: function (station) {
                $("#Chart_Station").empty();
                $("#Chart_Station").append("<option selected>انتخاب ایستگاه</option>");
                $.each(station,
                    function () {
                        $("#Chart_Station").append($("<option></option>").val(this['id']).html(this['name']));
                    });
            }
        });
    });
    $("#Chart_Station").change(function () {
        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/Chart/GetSensor/",
            data: { Stationid: $("#Chart_Station").val() },
            dataType: "json",
            success: function (sensor) {
                $("#Chart_LeftSensor").empty();
                $("#Chart_RightSensor").empty();
                $("#Chart_LeftSensor").append("<option selected>اتنخاب سنسور</option>");
                $("#Chart_RightSensor").append("<option selected>اتنخاب سنسور</option>");
                $.each(sensor,
                    function () {
                        $("#Chart_LeftSensor").append($("<option></option>").val(this['id']).html(this['faName']));
                        $("#Chart_RightSensor").append($("<option></option>").val(this['id']).html(this['faName']));
                    });
            }
        });
    });
    reportState();
});

function reportState() {
    var DateTypeState = $('input[name=DateType]:checked').val();

    if (DateTypeState === "AllData") {
        $('.Rchb_ReportType').prop("disabled", true);
        $('.Rchb_ReportType').prop('checked', false);
        $('.Lchb_ReportType').prop("disabled", true);
        $('.Lchb_ReportType').prop('checked', false);
    }
    else if (DateTypeState === "InDay" || DateTypeState === "InMonth") {
        $('.Rchb_ReportType').prop("disabled", false);
        $('.Lchb_ReportType').prop("disabled", false);
    }
    else {
        $('.Rchb_ReportType').prop("disabled", true);
        $('.Rchb_ReportType').prop('checked', false);
        $('.Lchb_ReportType').prop("disabled", true);
        $('.Lchb_ReportType').prop('checked', false);
    }

}

