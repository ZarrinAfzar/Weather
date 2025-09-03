function ForecastsAlarmDetail_List(projectId, stationId, alarmId) {
    $.ajax({
        url: "/ForecastsAlarmDetail/ForecastsAlarmDetail_List/",
        type: "Get",
        cache: false,
        async: false,
        dataType: "html",
        data: { ProjectId: projectId, StationId: stationId, AlarmId: alarmId },
        success: function (result) {
            if (result) {
                $("#ForecastsAlarmDetail_List").html(result);
                $('[data-toggle="tooltip"]').tooltip();
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}

function ForecastsAlarmDetail_filter() {
    ForecastsAlarmDetail_List($("#ForecastsAlarmDetail_Project").val(), $("#ForecastsAlarmDetail_Station").val(), $("#ForecastsAlarmDetail_Alarms").val());
    TableManageButtons.init();
}

function ForecastsAlarmDetail_Get(Id) {
    $.ajax({
        url: "/ForecastsAlarmDetail/ForecastsAlarmDetail_Get/",
        type: "Get",
        cache: false,
        async: false,
        dataType: "html",
        data: { id: Id },
        success: function (result) {
            if (result) {
                if (Id) {
                    $("#ModalTitle").text("ویرایش آلارم پیش بینی ها");
                } else {
                    $("#ModalTitle").text("ثبت آلارم پیش بینی ها");
                }
                $("#ModalBody").html(result);
                $("#myModal").modal();
            } else {
                messageBox("error", "خطا  رخ داده است");

            }
        }
    });
}
function ForecastsAlarmDetail_Save() {
    var model = {};
    $.each($('#formBody_ForecastsAlarmDetail').serializeArray(), function (i, field) {
        model[field.name] = field.value || null;
    });
    $.ajax({
        url: "/ForecastsAlarmDetail/ForecastsAlarmDetail_Save/",
        type: "Post",
        cache: false,
        async: false,
        dataType: "json",
        data: model,
        success: function (result) {
            if (result) {
                ForecastsAlarmDetail_filter();
                $("#myModal").modal("hide");
                if (model.Id > 0) {
                    messageBox("success", "ویرایش اطلاعات آلارم پیش بینی ها انجام شد");
                } else {
                    messageBox("success", "ثبت اطلاعات آلارم پیش بینی ها انجام شد");
                }
                messageBox("info", "لیست آلارم پیش بینی ها به روز شد");
            } else {
                messageBox("error", "خطا  رخ داده است");

            }
        }
    });
}
 
function ForecastsAlarmDetail_Delete(Id) {
    swal({
        title: "حذف آلارم پیش بینی ",
        text: "آلارم پیش بینی  مورد نظر حذف شود؟",
        type: "error",
        showCancelButton: true,
        confirmButtonClass: 'btn-danger waves-effect waves-light',
        confirmButtonText: "حذف"
    }, function (result) {
        if (result) {
            $.ajax({
                url: "/ForecastsAlarmDetail/ForecastsAlarmDetail_Delete/",
                type: "Get",
                cache: false,
                async: false,
                dataType: "json",
                data: { id: Id },
                success: function (result) {
                    if (result) {
                        messageBox("success", "حذف آلارم پیش بینی  انجام شد");
                        ForecastsAlarmDetail_List();
                    } else {
                        messageBox("error", "خطا در حذف آلارم پیش بینی");
                    }
                }
            });
        } else {
            messageBox("info", "حذف آلارم پیش بینی  لغو شد");
        }
    });
}
function ForecastsAlarmDetail_GetParameter(e) {
    if (e[e.selectedIndex].value !== "0") {
        $.ajax({
            url: "/ForecastsAlarmDetail/ForecastsAlarmDetail_GetParameter/",
            type: "Get",
            cache: false,
            async: false,
            dataType: "json",
            data: { id: e[e.selectedIndex].value },
            success: function (result) {
                if (result) {
                    $("#ParameterImage").attr("src", result.icon);
                    $("#ParameterValue").text(result.value);
                } else {
                    messageBox("error", "خطا  رخ داده است");

                }
            }
        });
    } else {
        $("#ParameterImage").attr("src", "/images/stationTypeIcon.png");
        $("#ParameterValue").text("");
    }
}