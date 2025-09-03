var myLine;
function generateChartBox(lbls, data1, data2, min, max, step) {

    !function ($) {
        "use strict";

        var ChartJs = function () { };

        ChartJs.prototype.respChart = function (selector, type, data, options) {
            // get selector by context
            var ctx = selector.get(0).getContext("2d");
            // pointing parent container to make chart js inherit its width
            var container = $(selector).parent();

            // enable resizing matter
            $(window).resize(generateChart);

            // this function produce the responsive Chart JS
            function generateChart() {
                // make chart width fit with its container
                var ww = selector.attr('width', $(container).width());
                //switch (type) {
                //    case 'Line':
                //        new Chart(ctx, { type: 'line', data: data, options: options });
                //        break;
                //    case 'Doughnut':
                //        new Chart(ctx, { type: 'doughnut', data: data, options: options });
                //        break;
                //    case 'Pie':
                //        new Chart(ctx, { type: 'pie', data: data, options: options });
                //        break;
                //    case 'Bar':
                //        new Chart(ctx, { type: 'bar', data: data, options: options });
                //        break;
                //    case 'Radar':
                //        new Chart(ctx, { type: 'radar', data: data, options: options });
                //        break;
                //    case 'PolarArea':
                //        new Chart(ctx, { data: data, type: 'polarArea', options: options });
                //        break;
                //}
                // Initiate new chart or Redraw
                myLine = new Chart(ctx, { type: 'polarArea', data: data, options: options });
            };
            // run function - render chart at first load
            generateChart();
        },
            ChartJs.prototype.init = function () {
                //creating lineChart
                var lineChart = {
                    labels: lbls,
                    datasets: new Array()
                };
                if (data1 !== null) {
                    lineChart.datasets.push(
                        {
                            label: "نمودار سمت راست",
                            fill: false,
                            lineTension: 0.1,
                            backgroundColor: "#10c469",
                            borderColor: "#10c469",
                            borderCapStyle: 'butt',
                            borderDash: [],
                            borderDashOffset: 0.0,
                            borderJoinStyle: 'miter',
                            pointBorderColor: "#10c469",
                            pointBackgroundColor: "#fff",
                            pointBorderWidth: 1,
                            pointHoverRadius: 5,
                            pointHoverBackgroundColor: "#10c469",
                            pointHoverBorderColor: "#eef0f2",
                            pointHoverBorderWidth: 2,
                            pointRadius: 1,
                            bezierCurve: false,
                            pointHitRadius: 10,
                            data: [148.141, 130, 122, 107, 84, 65, 44, 22]
                        });
                }
                var lineOpts = {
                    bezierCurve: false,
                    scales: {
                        yAxes: [{
                            ticks: {
                                max: 150,
                                min: 10,
                            }
                        }]
                    },
                    animation: {
                        onComplete: done
                    }
                };
            this.respChart($("#lineChart"), 'polarArea', lineChart, lineOpts);
            },
            $.ChartJs = new ChartJs, $.ChartJs.Constructor = ChartJs;
    }(window.jQuery),
        //initializing
        function ($) {
            "use strict";
            $.ChartJs.init();
        }(window.jQuery);
}

function done() {
    var url = myLine.toBase64Image();
    document.getElementById("SaveChart").href = url;
}
