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
    $("#WindChart_Project").change(function () {
        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/WindChart/GetStation/",
            data: { Projectid: $("#WindChart_Project").val() },
            dataType: "json",
            success: function (station) {
                $("#WindChart_Station").empty();
                $("#WindChart_Station").append("<option selected>اتنخاب ایستگاه</option>");
                $.each(station,
                    function () {
                        $("#WindChart_Station").append($("<option></option>").val(this['id']).html(this['name']));
                    });
            }
        });
    });
    $("#WindChart_Station").change(function () {
        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/WindChart/GetWindSpeedSensor/",
            data: { Stationid: $("#WindChart_Station").val() },
            dataType: "json",
            success: function (sensor) {
                $("#WindChart_SpeedSensor").empty();
                $("#WindChart_SpeedSensor").append("<option selected>انتخاب سنسور</option>");
                $.each(sensor,
                    function () {
                        $("#WindChart_SpeedSensor").append($("<option></option>").val(this['id']).html(this['faName']));

                    });
            }
        });
        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/WindChart/GetWindDirectSensor/",
            data: { Stationid: $("#WindChart_Station").val() },
            dataType: "json",
            success: function (sensor) {
                $("#WindChart_DirectionSensor").empty();
                $("#WindChart_DirectionSensor").append("<option selected>انتخاب سنسور</option>");
                $.each(sensor,
                    function () {
                        $("#WindChart_DirectionSensor").append($("<option></option>").val(this['id']).html(this['faName']));
                    });
            }
        });
    });
});

function WindChart_filter() {
    $.ajax({
        type: "Get",
        cache: false,
        async: false,
        url: "/WindChart/WindChart_filter/",
        data: { Stationid: $("#WindChart_Station").val(), SpeedSensor: $("#WindChart_SpeedSensor").val(), DirectionSensor: $("#WindChart_DirectionSensor").val(), FromDate: $("#FromDate").val(), ToDate: $("#ToDate").val() },
        dataType: "html",
        success: function (result) {
            $("#WindChart_View").empty();
            $("#WindChart_View").html(result);

            $(function () {
                Highcharts.setOptions({
                    colors: ['#4000ff', '#00bfff', '#4CAF50', '#40ff00', '#ffff00', '#ffad33', '#ff3300', '#ff3300']
                });
                var chartdata = {
                    table: 'freq',
                    startRow: 1,
                    endRow: 17,
                    endColumn: 7
                }
                var chartype = {
                    polar: true,
                    type: 'column',
                    zoomType: 'xy'
                }
                var chartitle = {
                    text: 'نمودار گلباد سرعت و جهت باد'
                }
                var chartsubtitle = {
                    text: 'Source: or.water.usgs.gov'
                }
                var chartpane = {
                    size: '90%'
                }
                var chartlegend = {
                    align: 'right',
                    verticalAlign: 'top',
                    y: 100,
                    layout: 'vertical'
                }
                var chartxaxis = {
                    tickmarkPlacement: 'on'
                }
                var chartyaxis = {
                    min: 0.5,
                    endOnTick: false,
                    showLastLabel: true,
                    title: {
                        text: 'Frequency (%)'
                    },
                    labels: {
                        formatter: function () {
                            return this.value + '%';
                        }
                    },
                    reversedStacks: false
                }
                var chartooltip = {
                    valueSuffix: '%'
                }
                var chartplotoptions = {
                    series: {
                        //colors: [
                        //    '#40ff00',
                        //    '#80ff00',
                        //    '#bfff00',
                        //    '#ffff00',
                        //    '#ffbf00',
                        //    '#ff8000',
                        //    '#ff4000'
                        //],
                        stacking: 'normal',
                        shadow: false,
                        groupPadding: 0,
                        pointPlacement: 'on'
                    }
                }
                $('#container').highcharts({
                    data: chartdata,
                    chart: chartype,
                    title: "",//chartitle,
                    //subtitle:chartsubtitle,
                    pane: chartpane,
                    legend: chartlegend,
                    xAxis: chartxaxis,
                    yAxis: chartyaxis,
                    tooltip: chartooltip,
                    plotOptions: chartplotoptions
                });
            });
            Highcharts.chart('containerBar', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: '_______________________________'
                },
                subtitle: {
                    text: 'نمودار سرعت باد'
                },
                xAxis: {
                    type: 'category'
                },
                yAxis: {
                    title: {
                        text: ''
                    }

                },
                legend: {
                    enabled: false
                },
                plotOptions: {
                    series: {
                        borderWidth: 0,
                        dataLabels: {
                            enabled: true,
                            format: '{point.y:.1f}%'
                        }
                    }
                },

                tooltip: {
                    headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b> of total<br/>'
                },

                "series": [
                    {
                        "name": "SPEED",
                        "colorByPoint": true,
                        "data": [
                            {
                                "name": "0.5 -> 1 m/s",
                                "y": +($("#speed1").val())
                            },
                            {
                                "name": "1 -> 2.1 m/s",
                                "y": +($("#speed2").val())
                            },
                            {
                                "name": "2.1 -> 3.6 m/s",
                                "y": +($("#speed3").val())
                            },
                            {
                                "name": "3.6 -> 5.7 m/s",
                                "y": +($("#speed4").val())
                            },
                            {
                                "name": "5.7 -> 8.8 m/s",
                                "y": +($("#speed5").val())
                            },
                            {
                                "name": "8.8 -> 11.1 m/s",
                                "y": +($("#speed6").val())
                            },
                            {
                                "name": " > 11.1 m/s",
                                "y": +($("#speed7").val())
                            }
                        ]
                    }
                ]
            });
        }
    });
}