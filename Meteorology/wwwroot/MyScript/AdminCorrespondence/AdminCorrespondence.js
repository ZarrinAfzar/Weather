function changestate(Id) { 
    $.ajax({
        url: "/AdminCorrespondence/changestate/",
        type: "Get",
        cache: false,  
        dataType: "json",
        data: { msgId: Id},
        success: function(result) {
            if (result) {
                messageBox("success", "تغییر وضعیت انجام شد");
            } else {
                messageBox("error", "خطا  رخ داده است");
            }
        }
    });
}