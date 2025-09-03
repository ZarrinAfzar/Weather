function SensorSetting_List(projectId, stationId) {
    $(".listPreLoader").show();
    $.ajax({
        url: "/SensorSetting/SensorSetting_List/",
        type: "Get",
        cache: false,
        dataType: "html",
        data: { ProjectId: projectId, StationId: stationId },
        success: function (result) {
            if (result) {
                $("#SensorSetting_List").html(result);
                $("#SensorSetting_ListTitle").empty();
                if (projectId) {
                    $("#SensorSetting_ListTitle").text(" لیست کلی سنسور های پروژه " + $("#SensorSetting_Project option[value='" + projectId + "']").text());
                } else if (projectId && stationId) {
                    $("#SensorSetting_ListTitle").text(" لیست کلی سنسور های پروژه " + $("#SensorSetting_Project option[value='" + projectId + "']" + "و ایستگاه " + $("#SensorSetting_Station option[value='" + stationId + "']").text()));
                } else {
                    $("#SensorSetting_ListTitle").text("لیست کلی سنسورها");
                }
                $('[data-toggle="tooltip"]').tooltip();
            } else {
                messageBox("error", "خطا  رخ داده است");

            }
            $(".listPreLoader").hide();
        }
    });
}
function filter() {
    SensorSetting_List($("#SensorSetting_Project").val(), $("#SensorSetting_Station").val());
    TableManageButtons.init();

}
function SensorSetting_Get(sensorId) {
    $.ajax({
        url: "/SensorSetting/SensorSetting_Get/",
        type: "Get",
        cache: false,
        async: false,
        dataType: "html",
        data: { SensorId: sensorId },
        success: function (result) {
            if (result) {
                $("#ModalTitle").text("ویرایش سنسور");
                $("#ModalBody").html(result);
                $("#myModal").modal();
            } else {
                messageBox("error", "خطا  رخ داده است");

            }
        }
    });
}
function SensorSetting_Default() {
    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        url: "/SensorSetting/GetDefault/",
        data: { SensorTypeId: $("#SensorType").val() },
        dataType: "json",
        success: function (result) {
            $("#UnitId").val(result.unitId);
            $("#Digit").val(result.sensorDigit);
            $("#MaxValue").val(result.sensorMax);
            $("#MinValue").val(result.sensorMin);
        }
    });
}
function SensorSetting_Save() {

    if (+($("#MaxValue").val()) < +($("#MinValue").val())) {
        messageBox("error", "مقدار کمترین و بیشترین را برعکس وارد کرده اید");
    } else {
        $(".listPreLoader").show();
        var model = {};
        $.each($('#formBody_SensorSetting').serializeArray(), function (i, field) {
            model[field.name] = field.value || null;
        });
        $.ajax({
            url: "/SensorSetting/SensorSetting_Save/",
            type: "Post",
            cache: false,
            dataType: "json",
            data: model,
            success: function (result) {
                if (result) {
                    filter();
                    $("#myModal").modal("hide");
                    $(".listPreLoader").hide();
                    messageBox("success", "ویرایش اطلاعات سنسور انجام شد");
                    messageBox("info", "لیست سنسورها به روز شد");
                } else {
                    messageBox("error", "خطا  رخ داده است");

                }
            }
        });
    }
}
function updateRowValue(id, text, min, max) {
    $("#datatable-buttons #SensorSettingRow_" + id + " td:nth-child(3)").text(text);
    $("#datatable-buttons #SensorSettingRow_" + id + " td:nth-child(7)").text(min);
    $("#datatable-buttons #SensorSettingRow_" + id + " td:nth-child(8)").text(max);
}

$(document).ready(function () {
   
    //TableManageButtons.init();
    $("#SensorSetting_Project").change(function () {
        $.ajax({
            type: "POST",
            cache: false,
            async: true,
            url: "/SensorSetting/GetStation/",
            data: { Projectid: $("#SensorSetting_Project").val() },
            dataType: "json",
            success: function (station) {
                $("#SensorSetting_Station").empty();
                $("#SensorSetting_Station").append("<option selected>انتخاب ایستگاه</option>");
                $.each(station,
                    function () {
                        $("#SensorSetting_Station").append($("<option></option>").val(this['id']).html(this['name']));
                    });
            }
        });
    });
    $("#SensorSetting_Station").change(function () {
        filter();
    });
   
});