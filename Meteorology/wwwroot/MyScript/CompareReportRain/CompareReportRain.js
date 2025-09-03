function CompareReportRain() {
    $(".listPreLoader").show();
    var formData = new FormData();
    var _file = $("#file").get(0).files;
    formData.append('file', _file[0]);
    formData.append('StationId', $("#CompareReportRain_Station").val());
    formData.append('StationName', $("#CompareReportRain_Station :selected").text());
    formData.append('StartDate', $("#FromDate").val());
    formData.append('EndDate', $("#ToDate").val()); 
    $.ajax({
        url: "/CompareReportRain/CompareReportRain_List/",
        type: "POST",
        processData: false,
        contentType: false,
        data: formData,
        success: function (result) {
            if (result) {
                $("#CompareReportRain_List").html(result);
            } else {
                messageBox("error", "اطلاعات فرم را تکمیل نمایید");
            }

    $(".listPreLoader").hide();
        }
    });

}
function exportData() {
    /* Get the HTML data using Element by Id */
    var table = document.getElementById("tblStocks");

    /* Declaring array variable */
    var rows = [];

    //iterate through rows of table
    for (var i = 0, row; row = table.rows[i]; i++) {
        //rows would be accessed using the "row" variable assigned in the for loop
        //Get each cell value/column from the row
        column1 = row.cells[0].innerText;
        column2 = row.cells[1].innerText;
        column3 = row.cells[2].innerText;
        column4 = row.cells[3].innerText;
        column5 = row.cells[4].innerText;
        column6 = row.cells[5].innerText;
        
        /* add a new records in the array */
        rows.push(
            [
                column1,
                column2,
                column3,
                column4,
                column5,
                column6,
              
            ]
        );

    }
    csvContent = "data:application/vnd.ms-excel;charset=UTF-8,\uFEFF";
    /* add the column delimiter as comma(,) and each row splitted by new line character (\n) */
    rows.forEach(function (rowArray) {
        row = rowArray.join(",");
        csvContent += row + "\r\n";
    });

    /* create a hidden <a> DOM node and set its download attribute */
    var encodedUri = encodeURI(csvContent);
    var link = document.createElement("a");
    link.setAttribute("href", encodedUri);
    link.setAttribute("download", $("#CompareReportRain_Station :selected").text() + ".csv");
    document.body.appendChild(link);
    /* download the data file named "Stock_Price_Report.csv" */
    link.click();
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
    $("#CompareReportRain_Project").change(function () {
        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/CompareReportRain/GetStation/",
            data: { Projectid: $("#CompareReportRain_Project").val() },
            dataType: "json",
            success: function (city) {
                $("#CompareReportRain_Station").empty();
                $("#CompareReportRain_Station").append("<option selected>انتخاب ایستگاه</option>");
                $.each(city,
                    function () {
                        $("#CompareReportRain_Station").append($("<option></option>").val(this['id']).html(this['name']));
                    });
            }
        });
    });

});
