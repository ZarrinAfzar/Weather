function PestAlarmDetail_List(projectId, stationId) {
    $.ajax({
        url: "/PestAlarmDetail/PestAlarmDetail_List/",
        type: "Get",
        cache: false,
        async: false,
        dataType: "html",
        data: { ProjectId: projectId, StationId: stationId },
        success: function (result) {
            if (result) {
                $("#PestAlarmDetail_List").html(result);
                $('[data-toggle="tooltip"]').tooltip();
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}

function PestAlarmDetail_filter() {
    PestAlarmDetail_List($("#PestAlarmDetail_Project").val(), $("#PestAlarmDetail_Station").val());
}
$(document).ready(function () {
    PestAlarmDetail_filter();
    $("#PestAlarmDetail_Project").change(function () {
        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/PestAlarmDetail/GetStation/",
            data: { Projectid: $("#PestAlarmDetail_Project").val() },
            dataType: "json",
            success: function (city) {
                $("#PestAlarmDetail_Station").empty();
                $("#PestAlarmDetail_Station").append("<option selected>انتخاب ایستگاه</option>");
                $.each(city,
                    function () {
                        $("#PestAlarmDetail_Station").append($("<option></option>").val(this['id']).html(this['name']));
                    });
            }
        });
        PestAlarmDetail_filter();

    });
    $("#PestAlarmDetail_Station").change(function () {
        PestAlarmDetail_filter();
    });
});
function PestAlarmDetail_Get(Id) {
    $.ajax({
        url: "/PestAlarmDetail/PestAlarmDetail_Get/",
        type: "Get",
        cache: false,
        async: false,
        dataType: "html",
        data: { id: Id },
        success: function (result) {
            if (result) {
                $("#ModalTitle").text("ویرایش آلارم آفات");
                $("#ModalBody").html(result);
                $(".level").hide();
                if ($("#CountLevel").val()) {
                    var levelCount = $("#CountLevel").val();
                    for (var i = 1; i <= levelCount; i++) {
                        $("#ch_level" + i).prop('checked', true);
                        $("#Level" + i).show();
                    }
                }
                $("#myModal").modal();
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}
function PestAlarmDetail_Save() {
    var lv2 = Boolean($('#ch_level2:checked').val());
    var lv3 = Boolean($('#ch_level3:checked').val());
    var lv4 = Boolean($('#ch_level4:checked').val());
    var lv5 = Boolean($('#ch_level5:checked').val());
    var pd2 = parseInt($('#PDDLevel2').val());
    var pd3 = parseInt($('#PDDLevel3').val());
    var pd4 = parseInt($('#PDDLevel4').val());

    $("#formBody_PestAlarmDetail").validate({

        rules: {

            TempBase: {
                required: true,
                number: true
            },
              
            TempMax: {
                required: true,
                number: true
            },
            LevelName1: 'required',
            PDDLevel2: {
                required: lv2,
                number:true
            },
            LevelName2: {
                required: lv2
            },
            PDDLevel3: {
                required: lv3,
                number: true,
                min: pd2+1
            },
            LevelName3: {
                required: lv3
            },
            PDDLevel4: {
                required: lv4,
                number: true,
                min: pd3+1
            },
            LevelName4: {
                required: lv4
            },
            PDDLevel5: {
                required: lv5,
                number: true,
                min:pd4+1
            },
            LevelName5:{
                required: lv5
            }
        },
        messages: {
            TempBase: {
                required: "دمای پایه را انتخاب کنید",
                number: "دما به درجه سانتی گراد است"
            },

            TempMax: {
                required: "دمای حداکثر را انتخاب کنید",
                number: "دما به درجه سانتی گراد است"
            },

            LevelName1: "نام مرحله اول را مشخص کنید",
            PDDLevel2: {
                required: "مقدار گذر به مرحله دوم را مشخص کنید",
                number:"PDD را به عدد وارد کنید"
            },
            LevelName2: {
                required: "نام مرحله دوم را مشخص کنید",
            },
            PDDLevel3: {
                required: "مقدار گذر به مرحله سوم را مشخص کنید",
                number: "PDD را به عدد وارد کنید",
                min:"مقدار نباید از مرحله قبل کمتر باشد"
            },
            LevelName3: {
                required: "نام مرحله سوم را مشخص کنید",
            },
            PDDLevel4: {
                required: "مقدار گذر به مرحله چهارم را مشخص کنید",
                number: "PDD را به عدد وارد کنید",
                min: "مقدار نباید از مرحله قبل کمتر باشد"
            },
            LevelName4: {
                required: "نام مرحله چهارم را مشخص کنید",
            },
            PDDLevel5: {
                required:  "مقدار گذر به مرحله پنجم را مشخص کنید",
                number: "PDD را به عدد وارد کنید",
                min: "مقدار نباید از مرحله قبل کمتر باشد"
            },
            LevelName5: {
                required: "نام مرحله پنجم را مشخص کنید",
            }
        },
        errorPlacement: function (label, element) {
            label.addClass('arrow');
            label.insertAfter(element);
        },
        wrapper: 'span',
        submitHandler: function () {

            var model = {};
            $.each($('#formBody_PestAlarmDetail').serializeArray(), function (i, field) {
                model[field.name] = field.value || null;
            });
            $.ajax({
                url: "/PestAlarmDetail/PestAlarmDetail_Save/",
                type: "Post",
                cache: false,
                async: false,
                dataType: "json",
                data: model,
                success: function (result) {
                    if (result) {
                        PestAlarmDetail_filter();
                        $("#myModal").modal("hide");
                        messageBox("success", "ویرایش اطلاعات آلارم آفت انجام شد");
                        messageBox("info", "لیست آلارم سنسورها به روز شد");
                    } else {
                        messageBox("error", "خطا  رخ داده است");

                    }
                }
            });
        }
    });
}
function SetLevel(e, level) {
    if ($(e).is(":checked")) {
        $("#CountLevel").val(level);
        $("#Level" + level).slideDown();
    } else {
        $("#CountLevel").val(level - 1);
        for (var i = 5; i > level - 1; i--) {
            $("#PDDLevel" + i).val('');
            $("#LevelName" + i).val('');
            $("#Level" + i).slideUp();
        }
    }
}
