
function Alarms_List(projectId, stationId) {
    $(".listPreLoader").show();
    $.ajax({
        url: "/Alarms/Alarms_List/",
        type: "Get",
        cache: false,
        async: false,
        dataType: "Html",
        data: { ProjectId: projectId, StationId: stationId },
        success: function (result) {
            if (result) {
                $("#Alarms_List").html(result);
                $('[data-toggle="tooltip"]').tooltip();
            } else {
                messageBox("error", "خطا در سیستم");
            }
        }
    });
    $(".listPreLoader").hide();
}
function Alarms_Get(id) {
    $(".listPreLoader").show();
    $.ajax({
        url: "/Alarms/Alarms_Get/",
        type: "Get",
        cache: false,
        async: false,
        dataType: "Html",
        data: { id: id },
        success: function (result) {
            if (result) {
                if (id > 0) {
                    $("#ModalTitle").text("ویرایش آلارم");
                } else {
                    $("#ModalTitle").text("ثبت آلارم");
                }
                $("#ModalBody").html(result);
                $("#myModal").modal();
            } else {
                messageBox("error", "خطا در سیستم");
            }
        }
    });
    $("#Alarms_Project").change();
    $(".listPreLoader").hide();
}
function Alarms_Save() {
    $("#Alarm_formBody").validate({

        rules: {
            StationId: "required",
            AlarmType: "required",
            AlarmName: "required",
            AlarmMessage: "required",
            AlarmStartDateConvertor: "required",
            AlarmEndDateConvertor: "required"
        },
        messages: {
            StationId: "ایستگاه را انتخاب کنید",
            AlarmType: "نوع آلارم را انتخاب کنید",
            AlarmName: "نام آلارم را وارد کنید",
            AlarmMessage: "متن پیام را وارد کنید",
            AlarmStartDateConvertor: "تاریخ شروع را انتخاب کنید",
            AlarmEndDateConvertor: "تاریخ پایان را انتخاب کنید"
        },
        errorPlacement: function (label, element) {
            label.addClass('arrow');
            label.insertAfter(element);
        },
        wrapper: 'span',
        submitHandler: function () {

            var model = {};
            $.each($('#Alarm_formBody').serializeArray(), function (i, field) {
                model[field.name] = field.value || null;
            });
            $.ajax({
                url: "/Alarms/Alarms_Save/",
                type: "POST",
                cache: false,
                async: false,
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                data: JSON.stringify(model),
                success: function (result) {
                    if (result) {
                        if (model.Id == 0) {
                            messageBox("success", "ثبت آلارم انجام شد");
                        } else {
                            messageBox("success", "ویرایش آلارم انجام شد");
                        }
                        Alarms_filter();
                        $("#myModal").modal('hide');
                    } else {
                        messageBox("error", "خطا در ثبت آلارم");
                    }
                }
            });
        }
    });
}
function Alarms_Delete(id) {
    swal({
        title: "حذف آلارم",
        text: "آلارم مورد نظر حذف شود؟",
        type: "error",
        showCancelButton: true,
        confirmButtonClass: 'btn-danger waves-effect waves-light',
        confirmButtonText: "حذف"
    }, function (result) {
        if (result) {
            $.ajax({
                url: "/Alarms/Alarms_Delete/",
                type: "Get",
                cache: false,
                async: false,
                dataType: "json",
                data: { id: id },
                success: function (result) {
                    if (result) {
                        messageBox("success", "حذف آلارم انجام شد");
                        Alarms_filter();
                    } else {
                        messageBox("error", "خطا در حذف آلارم");
                    }
                }
            });
        } else {
            messageBox("info", "حذف آلارم لغو شد");
        }
    });

}

function Alarms_NextPage(pageNumber, e) {
    $(".pagination").removeClass("acctivePage");
    $(e).children().addClass("acctivePage");
    pageNumber = (pageNumber - 1) * 6;
    Alarms_List(pageNumber, $("#Alarms_Project").val(), $("#Alarms_Station").val())
}
function Alarms_filter() {
    Alarms_List($("#Alarms_Project").val(), $("#Alarms_Station").val());
}

$(document).ready(function () {
    Alarms_List();
    $("#Alarms_Project").change(function () {

        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/Alarms/GetStation/",
            data: { Projectid: $("#Alarms_Project").val() },
            dataType: "json",
            success: function (station) {
                $("#Alarms_Station").empty();
                $("#Alarms_Station").append("<option selected value='0'>اتنخاب ایستگاه</option>");
                $.each(station,
                    function () {
                        $("#Alarms_Station").append($("<option></option>").val(this['id']).html(this['name']));
                    });
            }
        });
        Alarms_filter();
    });
    $("#Alarms_Station").change(function () {
        Alarms_filter();
    });
});
//_____________________________________________

function Alarms_Station(id) {
    $.ajax({
        url: "/Alarms/Alarms_Station/",
        type: "Get",
        cache: false,
        dataType: "Html",
        data: { id: id },
        success: function (result) {
            if (result) {
                $("#ModalTitle").text("جزئیات ایستگاه");
                $("#ModalBody").html(result);
                $("#myModal").modal();
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}

//_____________________________________________

function AlarmTell_Add(stationId) {
    
    $.ajax({
        url: "/Alarms/AlarmTell_Add/",
        type: "Get",
        cache: false,
        dataType: "json",
        data: { StationTellid: stationId, AlarmId: $("#AlarmId").val() },
        success: function (result) {
            if (result) {
                Alarms_filter();
                AddToAlarmTellList(stationId, result);
                messageBox("success", "به تلفن های آلارم اضافه شد");
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}
function AlarmTell_Delete(id) {
    $.ajax({
        url: "/Alarms/AlarmTell_Delete/",
        type: "Get",
        cache: false,
        dataType: "json",
        data: { id: id },
        success: function (result) {

            if (result) {
                Alarms_filter();
                AddToStationTellList(result, id);
                messageBox("success", "حذف تلفن انجام شد");
            } else {
                messageBox("error", "خطا رخ داده است");
            }
        }
    });
}
function AlarmTell_List(AlarmId, StationId) {
    $.ajax({
        url: "/Alarms/AlarmTell_List/",
        type: "Get",
        cache: false,
        async: false,
        dataType: "Html",
        data: { AlarmId: AlarmId, StationId: StationId },
        success: function (result) {
            if (result) {
                $("#ModalTitle").text(" ");
                $("#ModalBody").html(result);
                $("#myModal").modal();
                $("#AlarmTell_AlarmId").val(AlarmId);
            } else {
                messageBox("error", "خطا رخ داده است");
            }
        }
    });
}

function AddToStationTellList(stationId, alarmId) {
    var innerli = $("#AlarmTellList").find("#TellItem_" + alarmId);
    innerli["0"].innerHTML = innerli["0"].innerHTML.replace("AlarmTell_Delete(" + alarmId + ")", "AlarmTell_Add(" + stationId + ")");
    innerli["0"].innerHTML = innerli["0"].innerHTML.replace("delete", "plus");
    $("#StationTellList").append('<li class="list-group-item" id="TellItem_' + stationId + '">' + innerli["0"].innerHTML + '</li>');
    $("#AlarmTellList li#TellItem_" + alarmId).remove();
}
function AddToAlarmTellList(stationId, alarmId) {
    var innerli = $("#StationTellList").find("#TellItem_" + stationId);
    innerli["0"].innerHTML = innerli["0"].innerHTML.replace("AlarmTell_Add(" + stationId + ")", "AlarmTell_Delete(" + alarmId + ")");
    innerli["0"].innerHTML = innerli["0"].innerHTML.replace("plus", "delete");
    $("#AlarmTellList").append('<li class="list-group-item" id="TellItem_' + alarmId + '">' + innerli["0"].innerHTML + '</li>');
    $("#StationTellList li#TellItem_" + stationId).remove();
}

