function CompareReport() {
    $(".listPreLoader").show();
    var formData = new FormData();
    var _file = $("#file").get(0).files;
    formData.append('file', _file[0]);
    formData.append('StationId', $("#CompareReport_Station").val());
    formData.append('StationName', $("#CompareReport_Station :selected").text());
    formData.append('StartDate', $("#FromDate").val());
    formData.append('EndDate', $("#ToDate").val());
    $.ajax({
        url: "/CompareReport/CompareReport_List/",
        type: "POST",
        processData: false,
        contentType: false,
        data: formData,
        success: function (result) {
            if (result) {
                $("#CompareReport_List").html(result);
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
        column7 = row.cells[6].innerText;
        column8 = row.cells[7].innerText;
        column9 = row.cells[8].innerText;
        column10 = row.cells[9].innerText;
        column11 = row.cells[10].innerText;
        column12 = row.cells[11].innerText;
        column13 = row.cells[12].innerText;
        column14 = row.cells[13].innerText;
        column15 = row.cells[14].innerText;
        column16 = row.cells[15].innerText;
        column17 = row.cells[16].innerText;
        column18 = row.cells[17].innerText;
        column19 = row.cells[18].innerText;
        column20 = row.cells[19].innerText;
        column21 = row.cells[20].innerText;
        column22 = row.cells[21].innerText;
        column23 = row.cells[22].innerText;


        /* add a new records in the array */
        rows.push(
            [
                column1,
                column2,
                column3,
                column4,
                column5,
                column6,
                column7,
                column8,
                column9,
                column10,
                column11,
                column12,
                column13,
                column14,
                column15,
                column16,
                column17,
                column18,
                column19,
                column20,
                column21,
                column22,
                column23

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
    link.setAttribute("download", $("#CompareReport_Station :selected").text() + ".csv");
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
    $("#CompareReport_Project").change(function () {
        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/CompareReport/GetStation/",
            data: { Projectid: $("#CompareReport_Project").val() },
            dataType: "json",
            success: function (city) {
                $("#CompareReport_Station").empty();
                $("#CompareReport_Station").append("<option selected>انتخاب ایستگاه</option>");
                $.each(city,
                    function () {
                        $("#CompareReport_Station").append($("<option></option>").val(this['id']).html(this['name']));
                    });
            }
        });
    });

});
