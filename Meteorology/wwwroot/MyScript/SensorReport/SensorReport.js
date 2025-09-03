var skip = 0;
$(document).ready(function () {
    $(".datepicker").pDatepicker({
        format: 'YYYY/MM/DD HH:mm:ss',
        initialValue: false,
        initialValueType: 'persian',
        autoClose: true,
        calendar: {
            leapYearMode: "algorithmic"
        },
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
    $('#Sensor_Selecter, .chb_ReportType').prop("disabled", true);
    $('#Sensor_Selecter').select2({
        placeholder: "یک یا چند سنسور را انتخاب نمایید",
        allowClear: true,
        width: 'auto'
    });

    $("#SensorReport_Project").change(function () {
        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/SensorReport/GetStation/",
            data: { Projectid: $("#SensorReport_Project").val() },
            dataType: "json",
            success: function (city) {
                $("#SensorReport_Station").empty();
                $("#SensorReport_Station").append("<option selected>انتخاب ایستگاه</option>");
                $.each(city, function () {
                    $("#SensorReport_Station").append($("<option></option>").val(this['id']).html(this['name']));
                });
            }
        });
    });

    reportState();
});
function reportState() {
    var rdoLvl = $('input[name=All_Day_Month]:checked').val();
    if (rdoLvl === "AllData") {
        $('.chb_ReportType').prop("disabled", true);
        $('.chb_ReportType').prop('checked', false);
        $('#Sensor_Selecter').prop("disabled", false);
    }
    else if (rdoLvl === "DAY" || rdoLvl === "MONTH") {
        $('.chb_ReportType').prop("disabled", false);
        $('#Sensor_Selecter').prop("disabled", false);
    }
}
$(document).on("change", "input[name=All_Day_Month], input[name=computingType],#SensorReport_Station", function () {
    SensorSelecter();
});
function SensorSelecter() {
    var computingType = $('input[name=computingType]:checked').map(function () {
        return $(this).val();
    }).get().join(',');

    $.ajax({
        type: "POST",
        url: "/SensorReport/SelecteSensors/",
        data: {
            StationId: $("#SensorReport_Station").val(),
            computingType: computingType
        },
        dataType: "json",
        success: function (sensors) {
            $("#Sensor_Selecter").empty();
            $("#Sensor_Selecter").append('<option disabled selected value="">انتخاب سنسور</option>');

            var unique = [];
            $.each(sensors, function () {
                if (!unique.includes(this.id)) {
                    unique.push(this.id);
                    $("#Sensor_Selecter").append(
                        $("<option></option>").val(this.id).text(this.faName)
                    );
                }
            });
            $("#Sensor_Selecter").select2({
                placeholder: "انتخاب سنسور",
                allowClear: true,
                width: 'auto'
            });
        }
    });
}
function SensorReport_filter() {
    $(".listPreLoader").show();
    var computing = "";
    $('input[name=computingType]:checked').each(function (i) {
        computing += $(this).val();
    });
    SensorReport_List($("#SensorReport_Station").val(), $("#FromDate").val(), $("#ToDate").val(), $('input[name=All_Day_Month]:checked').val(), computing, 0, $("#Sensor_Selecter").val());
}
function SensorReport_List(stationId, startDate, endDate, reportType, computingType, skip, sensorselected) {
    $.ajax({
        url: "/SensorReport/SensorReport_List/",
        type: "Get",
        cache: false,
        traditional: true,
        contentType: 'application/x-www-form-urlencoded',
        dataType: "html",
        data: { stationId: stationId, startDate: startDate, endDate: endDate, reportType: reportType, computingType: computingType, sensorselected: sensorselected, Skip: skip },
        success: function (result) {
            if (result) {
                $("#SensorReport_List").html(result);
                //$("#skipcount").val(skip);
                $('[data-toggle="tooltip"]').tooltip();
            } else {
                messageBox("error", "اطلاعات فرم را تکمیل نمایید");
            }
            $(".listPreLoader").hide();
        }
    });
}
function Next() {
    if ($("#rowcount").val() !== "0") {
        skip += 1;
        $(".listPreLoader").show();
        var computing = "";
        $('input[name=computingType]:checked').each(function (i) {
            computing += $(this).val();
        });
        SensorReport_List($("#SensorReport_Station").val(), $("#FromDate").val(), $("#ToDate").val(), $('input[name=All_Day_Month]:checked').val(), computing, skip);
    } else {
        messageBox("info", "صفحه آخر هستید");
    }
}
function Previous() {
    if (skip !== 0) {
        skip -= 1;
        $(".listPreLoader").show();
        var computing = "";
        $('input[name=computingType]:checked').each(function (i) {
            computing += $(this).val();
        });
        SensorReport_List($("#SensorReport_Station").val(), $("#FromDate").val(), $("#ToDate").val(), $('input[name=All_Day_Month]:checked').val(), computing, skip);
    } else {
        messageBox("info", "صفحه اول هستید");
    }

}
function ExportToExcel() {
    var computing = "";
    $('input[name=computingType]:checked').each(function () {
        computing += $(this).val();
    });

    var stationId = $("#SensorReport_Station").val();
    var stationName = $("#SensorReport_Station :selected").text();
    var startDate = $("#FromDate").val();
    var endDate = $("#ToDate").val();
    var reportType = $('input[name=All_Day_Month]:checked').val();
    var sensors = $("#Sensor_Selecter").val();

    var url = '/SensorReport/ExportToExcel'
        + '?stationId=' + stationId
        + '&stationname=' + encodeURIComponent(stationName)
        + '&startDate=' + startDate
        + '&endDate=' + endDate
        + '&reportType=' + reportType
        + '&computingType=' + computing;

    if (sensors && sensors.length > 0) {
        url += '&sensorselected=' + sensors.join(',');
    }
    window.open(url, '_blank');
}