function CompareReportJame() {
    $(".listPreLoader").show();
    var formData = new FormData();
    var _ELfile = $("#ELfile").get(0).files;
    formData.append('ELfile', _ELfile[0]);
    var _MEfile = $("#MEfile").get(0).files;
    formData.append('MEfile', _file[0]);
    formData.append('StartDate', $("#FromDate").val());
    formData.append('EndDate', $("#ToDate").val());
    $.ajax({
        url: "/CompareReportJame/CompareReportJame_List/",
        type: "POST",
        processData: false,
        contentType: false,
        data: formData,
        success: function (result) {
            if (result) {
                $("#CompareReportJame_List").html(result);
            } else {
                messageBox("error", "اطلاعات فرم را تکمیل نمایید");
            }

            $(".listPreLoader").hide();
        }
    });

}
function ExportToExcel() {
    $(".listPreLoader").show();
    var formData = new FormData();
    var _ELfile = $("#ELfile").get(0).files;
    formData.append('ELfile', _ELfile[0]);
    var _MEfile = $("#MEfile").get(0).files;
    formData.append('MEfile', _file[0]);
    formData.append('StartDate', $("#FromDate").val());
    formData.append('EndDate', $("#ToDate").val());
    $.ajax({
        url: "/CompareReportJame/CompareReportJame_List/",
        type: "POST",
        processData: false,
        contentType: false,
        data: formData,
        success: function (result) {
            if (result) {
                window.open(result).text();
            } else {
                messageBox("error", "اطلاعات فرم را تکمیل نمایید")
            }

            $(".listPreLoader").hide();
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
        },
    });

});

