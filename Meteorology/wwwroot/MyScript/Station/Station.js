
//_____________________________________________
$(document).ready(function () {
    $(".select2").select2();
    StationList();
    $("#StationFilterBox").slideDown();
    $("#Station_ProjectList").change(function () {
        StationListByProject($("#Station_ProjectList").val());
    });
    DetailStation();
});
function StationList(Ids) {
    $(".listPreLoader").show();
    $.ajax({
        url: "/Station/Station_List/",
        type: "POST",
        cache: false,
        async: false,
        dataType: "Html",
        data: { projectIds: Ids },
        success: function (result) {
            if (result) {
                $("#StationList").html(result);
            } else {
                messageBox("error", "خطا رخ داده است");
            }
        }
    });
    $(".listPreLoader").hide();
}
function DetailStation(id) {
    $(".listPreLoader").show();
    $.ajax({
        url: "/Station/Station_Get/",
        type: "Get",
        cache: false,
        async: false,
        dataType: "Html",
        data: { id: id },
        success: function (result) {
            if (result) {
                $("#DetailStation").empty();
                $("#DetailStation").html(result);
                $('[data-toggle="tooltip"]').tooltip();
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
    $(".listPreLoader").hide();
}
function Delete(id) {
    swal({
        title: "حذف ایستگاه",
        text: "ایستگاه مورد نظر حذف شود؟",
        type: "error",
        showCancelButton: true,
        confirmButtonClass: 'btn-danger waves-effect waves-light',
        confirmButtonText: "حذف"
    }, function (result) {
        if (result) {
            $.ajax({
                url: "/Station/Station_Delete/",
                type: "Get",
                cache: false,
                async: false,
                dataType: "json",
                data: { id: id },
                success: function (result) {
                    if (result) {
                        StationList();
                        DetailStation();
                        messageBox("success", "حذف ایستگاه انجام شد");
                    } else {
                        messageBox("error", "خطا رخ داده است");
                    }
                }
            });
        } else {
            messageBox("info", "حذف ایستگاه لغو شد");
        }
    });

}
function StationListByProject(projectsId) {
    StationList(projectsId);
}
//_____________________________________________

function Save_Station() {
    $('#DataLoggerId').removeAttr('disabled');
    $('#ModemTypeId').removeAttr('disabled');
    $('#ProjectId').removeAttr('disabled');
    $('#StationTypeId').removeAttr('disabled');

    $("#formBody_Station").validate({
        rules: {
            SerialNumber: {
                required: true,
                maxlength: 15,
                minlength: 15
            },
            DataLoggerId: "required",
            ModemTypeId: "required",
            ProjectId: "required",
            StationTypeId: "required",
            Name: "required",
            Latitude: {
                latCoord: true
            },
            Longitude: {
                longCoord: true
            },
            AboveSeaLevel: {
                digits: true
            }
            
            
        },
        messages: {
            SerialNumber: {
                required: "شماره سریال ایستگاه را وارد کنید",
                maxlength: "تعداد ارقام باید 15 باشد",
                minlength: "تعداد ارقام باید 15 باشد"
            },
            DataLoggerId: "دیتالاگر را انتخاب کنید",
            ModemTypeId: "نوع مودم را انتخاب کنید",
            ProjectId: "پروژه ایستگاه را انتخاب کنید",
            StationTypeId: "نوع ایستگاه را انتخاب کنید",
            Name: "نام ایستگاه را وارد کنید",
            AboveSeaLevel: {
                digits: "ارتفاع از سطح دریا به متر است"
            }
           
          
        },
        errorPlacement: function (label, element) {
            label.addClass('arrow');
            label.insertAfter(element);
        },
        wrapper: 'span',
        submitHandler: function () {
            var model = {};
            $.each($('#formBody_Station').serializeArray(), function (i, field) {
                model[field.name] = field.value || null;
            });
            model["UpdateTime"] = $("#UpdateTime").is(':checked') ? 'true' : "false";
            model["UpdateSetting"] = $("#UpdateSetting").is(':checked') ? 'true' : "false";
            $(".listPreLoader").show();
            $.ajax({
                url: "/Station/Station_Save/",
                type: "POST",
                cache: false,
                //async: false,
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                data: JSON.stringify(model),
                success: function (result) {
                    if (result) {
                        if ($("#Id").val() !== "0") {
                            StationList();
                            DetailStation($("#Id").val());

                            $('#sendNewSms').attr('disabled', true);
                            $('#DataLoggerId').attr('disabled', true);
                            $('#ModemTypeId').attr('disabled', true);
                            $('#ProjectId').attr('disabled', true);
                            $('#StationTypeId').attr('disabled', true);



                        } else {
                            StationList();
                            DetailStation();
                            messageBox("success", "ثبت ایستگاه انجام شد");
                        }
                    } else {
                        messageBox("error", "خطا  رخ داده است");
                    }
                    $(".listPreLoader").hide();
                }
            });
        }
    });
}
$.validator.addMethod('latCoord', function (value, element) {
    return this.optional(element) ||
        value.length >= 4 && /^(?=.)-?((8[0-5]?)|([0-7]?[0-9]))?(?:\.[0-9]{1,20})?$/.test(value);
}, 'عرض جغرافیایی صحیح نیست');
$.validator.addMethod('longCoord', function (value, element) {
    return this.optional(element) ||
        value.length >= 4 && /^(?=.)-?((0?[8-9][0-9])|180|([0-1]?[0-7]?[0-9]))?(?:\.[0-9]{1,20})?$/.test(value);
}, 'طول جغرافیایی صحیح نیست');
//_____________________________________________
function StationTel_Get(id) {
    $.ajax({
        url: "/Station/StationTel_Get/",
        type: "Get",
        cache: false,
        dataType: "json",
        data: { id: id },
        success: function (result) {
            if (result) {
                $("#StationTel_Id").val(result.id);
                $("#StationTel_StationId").val(result.stationId);
                $("#StationTel_Name").val(result.name);
                $("#StationTel_LastName").val(result.lastName);
                $("#StationTel_Post").val(result.post);
                $("#StationTel_Tel").val(result.tel);
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}

function StationTel_Save() {
    $("#formbody_StationTel").validate({
        rules: {
            StationTel_Name: "required",
            StationTel_Tel: {
                required: true,
                digits: true
            }
        },
        messages: {
            StationTel_Name: "نام  را وارد کنید",
            StationTel_Tel: {
                required: "شماره تلفن را وارد کنید",
                digits: "تلفن باید به عدد وارد شود"
            }
        },
        errorPlacement: function (label, element) {
            label.addClass('arrow');
            label.insertAfter(element);
        },
        wrapper: 'span',
        submitHandler: function () {
            $(".listPreLoader").show();
            $.ajax({
                url: "/Station/StationTel_Save/",
                type: "POST",
                cache: false,
                dataType: "json",
                data: { id: $("#StationTel_Id").val(), Stationid: $("#StationTel_StationId").val(), Name: $("#StationTel_Name").val(), LastName: $("#StationTel_LastName").val(), Post: $("#StationTel_Post").val(), Tel: $("#StationTel_Tel").val() },
                success: function (result) {
                    if (result) {
                        if ($("#StationTel_Id").val() !== "0") {
                            Edit_StationTel_List(result.id, result.name, result.lastName, result.post, result.tel);
                            messageBox("success", "ویرایش تلفن انجام شد");
                        } else {
                            Add_StationTel_List(result.id, result.name, result.lastName, result.post, result.tel);
                            messageBox("success", "ثبت تلفن انجام شد");
                        }
                    } else {
                        messageBox("error", "خطا  رخ داده است");
                    }
                    $(".listPreLoader").hide();
                }
            });
        }
    });

}
function StationTel_List(Stationid) {
    $.ajax({
        url: "/Station/StationTel_List/",
        type: "Get",
        cache: false,
        async: false,
        dataType: "Html",
        data: { StationId: Stationid },
        success: function (result) {
            if (result) {
                $("#ModalTitle").text("تلفن های ایستگاه");
                $("#ModalBody").html(result);
                $("#myModal").modal();
                $("#StationTel_StationId").val(Stationid);
            } else {
                messageBox("error", "خطا رخ داده است");
            }
        }
    });
}
function StationTel_Delete(id) {

    swal({
        title: "حذف تلفن",
        text: "تلفن مورد نظر حذف شود؟",
        type: "error",
        showCancelButton: true,
        confirmButtonClass: 'btn-danger waves-effect waves-light',
        confirmButtonText: "حذف"
    }, function (result) {
        if (result) {
            $.ajax({
                url: "/Station/StationTel_Delete/",
                type: "Get",
                cache: false,
                dataType: "json",
                data: { id: id },
                success: function (result) {
                    if (result) {
                        Delete_StationTel_List(result);
                        messageBox("success", "حذف تلفن انجام شد");
                    } else {
                        messageBox("error", "خطا رخ داده است");
                    }
                }
            });
        } else {
            messageBox("info", "حذف تلفن لغو شد");
        }
    });
}

function Add_StationTel_List(id, Name, Lastname, post, Tel) {
    var item = '<li class="list-group-item"  id="StationTelItem_' + id + '">' +
        '<span onclick="StationTel_Get(' + id + ')" class="badge badge-primary telButton"><i class="zmdi zmdi-edit"></i></span>' +
        '<span onclick="StationTel_Delete(' + id + ')" class="badge badge-primary telButton"><i class="zmdi zmdi-delete"></i></span>' +
        '<p  id="StationTelItemName_' + id + '" style="margin-bottom: 0px;text-align: right"> <b> ' + Name + ' ' + Lastname + '</b> <text class="text-primary">(' + post + ') </text><text class="text-primary">  تلفن : ' + Tel + '</text></p>' +
        '</li>';
    $("#StationTelList").append(item);
    $("#StationTel_Name").val("");
    $("#StationTel_LastName").val("");
    $("#StationTel_Post").val("");
    $("#StationTel_Tel").val("");
    $("#StationTel_Id").val(0);
}
function Edit_StationTel_List(id, Name, Lastname, post, Tel) {
    $("#StationTelItemName_" + id + " b").text(Name + " " + Lastname);
    $("#StationTelItemName_" + id + " text").text("(" + post + ") تلفن : " + Tel);
    $("#StationTel_Name").val("");
    $("#StationTel_LastName").val("");
    $("#StationTel_Post").val("");
    $("#StationTel_Tel").val("");
    $("#StationTel_Id").val(0);
}
function Delete_StationTel_List(id) {
    $("#" + "StationTelList #StationTelItem_" + id).remove();
}

function StationFile_Get(id) {
    $.ajax({
        url: "/Station/StationFile_Get/",
        type: "Get",
        cache: false,
        dataType: "json",
        data: { id: id },
        success: function (result) {
            if (result) {
                $("#StationFile_Id").val(result.id);
                $("#StationFile_StationId").val(result.stationId);
                $("#Title").val(result.title);
                $("#Description").val(result.description);
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}
function StationFile_Save() {
    var formData = new FormData();
    var _file = $("#file").get(0).files;
    formData.append('file', _file[0]);
    formData.append('Id', $('#StationFile_Id').val());
    formData.append('StationId', $('#StationFile_StationId').val());
    formData.append('Title', $('#Title').val());
    formData.append('Description', $('#Description').val());
    $.ajax({
        url: "/Station/StationFile_Save/",
        type: "POST",
        processData: false,
        contentType: false,
        data: formData,
        success: function (result) {
            if (result) {
                if ($("#StationFile_Id").val() !== "0") {
                    Edit_StationFile_List(result.id, result.title, result.description, result.fileAddress);
                    messageBox("success", "ویرایش فایل انجام شد");
                } else {
                    Add_StationFile_List(result.id, result.title, result.description, result.fileAddress);
                    messageBox("success", "ثبت فایل انجام شد");
                }
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}
function StationFile_List(Stationid) {
    $.ajax({
        url: "/Station/StationFile_List/",
        type: "Get",
        cache: false,
        async: false,
        dataType: "Html",
        data: { StationId: Stationid },
        success: function (result) {
            if (result) {
                $("#ModalTitle").text("فایل های ایستگاه");
                $("#ModalBody").html(result);
                $("#myModal").modal();
                $("#StationFile_StationId").val(Stationid);
            } else {
                messageBox("error", "خطا رخ داده است");
            }
        }
    });
}
function StationFile_Delete(id) {
    swal({
        title: "حذف فایل",
        text: "فایل مورد نظر حذف شود؟",
        type: "error",
        showCancelButton: true,
        confirmButtonClass: 'btn-danger waves-effect waves-light',
        confirmButtonText: "حذف"
    }, function (result) {
        if (result) {
            $.ajax({
                url: "/Station/StationFile_Delete/",
                type: "Get",
                cache: false,
                dataType: "json",
                data: { id: id },
                success: function (result) {
                    if (result) {
                        Delete_StationFile_List(result);
                        messageBox("success", "حذف فایل انجام شد");
                    } else {
                        messageBox("error", "خطا رخ داده است");
                    }
                }
            });
        } else {
            messageBox("info", "حذف فایل لغو شد");
        }
    });
}

function Add_StationFile_List(id, Title, Description, FileName) {
    var item = '<li class="list-group-item" id="StationFileItem_' + id + '">' +
        ' <span class="badge badge-primary telButton"  onclick="StationFile_Get(' + id + ')"><i class="zmdi zmdi-edit"></i></span>' +
        ' <span class="badge badge-primary telButton"  onclick="StationFile_Delete(' + id + ')"><i class="zmdi zmdi-delete"></i></span>' +
        ' <a href="/StationFiles/' + FileName + '" target="_blank" class="badge badge-primary telButton"><i class="zmdi zmdi-download"></i></a>' +
        ' <b style="margin-bottom: 0px;text-align: right"> ' + Title + '</b>' +
        ' <p style="margin-bottom: 0px;text-align: right">' + Description + ' </p>' +
        '</li>';
    $("#StationFileList").append(item);
    $("#Description").val("");
    $("#Title").val("");
    $("#file").val(null);
    $("#StationFile_Id").val(0);
}
function Edit_StationFile_List(id, Title, Description, FileName) {
    $("#StationFileItem_" + id + " b").text(Title);
    $("#StationFileItem_" + id + " p").text(Description);
    $("#StationFileItem_" + id + " a").attr("href", "~/StationFiles/" + FileName);
    $("#Description").val("");
    $("#Title").val("");
    $("#file").val(null);
    $("#StationFile_Id").val(0);
}
function Delete_StationFile_List(id) {
    $("#StationFileList #StationFileItem_" + id).remove();
}
//______________________________________________

function StationGetProjectInfo(stationId) {
    $.ajax({
        url: "/Station/StationGetProjectInfo/",
        type: "Get",
        cache: false,
        dataType: "Html",
        data: { id: stationId },
        success: function (result) {
            if (result) {
                $("#ModalTitle").text("پروژه ایستگاه");
                $("#ModalBody").html(result);
                $("#myModal").modal();
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}
function StationSms(stationId) {
    $.ajax({
        url: "/Station/StationSms/",
        type: "Get",
        cache: false,
        dataType: "Html",
        data: { StationId: stationId },
        success: function (result) {
            if (result) {
                $("#ModalTitle").text("پیامک ایستگاه");
                $("#ModalBody").html(result);
                $("#myModal").modal();
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}
function StationSmsUpdate(stationId) {
    $("#formBody_StationSmscharge").validate({
        rules: {
            StationSmscharge: {
                required: true,
                digits: true
            }
        },
        messages: {
            StationSmscharge: {
                required: "تعداد پیامک را وارد کنید",
                digits: "تعداد باید به عدد باشد"
            }
        },
        errorPlacement: function (label, element) {
            label.addClass('arrow');
            label.insertAfter(element);
        },
        wrapper: 'span',
        submitHandler: function () {
            if (+($("#StationSmscharge").val()) > +($("#ProjectSmsCount").val())) {
                $("#formBody_StationSmscharge").validate().cancelSubmit = true;
                messageBox("error", "پروژه مورد نظر به مقدار کافی پیامک ندارد");
            } else {
                $(".listPreLoader").show();
                $.ajax({
                    url: "/Station/StationSmsUpdate/",
                    type: "Post",
                    cache: false,
                    //async: false,
                    dataType: "json",
                    data: { StationId: stationId, SmsCount: $("#StationSmscharge").val() },
                    success: function (result) {
                        if (result) {
                            DetailStation(stationId);
                            messageBox("success", "تعداد پیامک ایستگاه ثبت شد");
                            $("#myModal").modal('hide');
                        } else {
                            messageBox("error", "خطا  رخ داده است");
                        }
                        $(".listPreLoader").hide();
                    }
                });
            }
        }
    });
}
