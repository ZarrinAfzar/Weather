var handleDataTableButtons = function () { 
    "use strict";
    0 !== $(".table").length && $(".table").DataTable({
        dom: "Bfrtip", 
        buttons: [
        {
                extend: "copy",
                text: '<i class="fa fa-copy"></i>',
                className: "btn-sm"
        }, {
                extend: "excel",
                text: '<i class="fa fa-file-excel-o"></i>',
                className: "btn-sm"
        }, {
                extend: "pdf",
                text: '<i class="fa fa-file-pdf-o"></i>',
                className: "btn-sm"
        }, {
                extend: "print",
                text: '<i class="fa fa-print"></i>',
                className: "btn-sm"
            }],
        language: {
            "paginate": {
                "previous": "صفحه قبل",
                "next": "صفحه بعد"
            },
            "search": "جستجو :",
            "info": "نمایش _START_ تا _END_ از _TOTAL_ سطر",
            "infoEmpty": "نمایش 0 تا 0 از 0 سطر"
        },
        responsive: !0
    });
}


TableManageButtons = function() {
    "use strict";
    return {
        init: function() {
            handleDataTableButtons();
        }
    }
}();