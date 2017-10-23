var editor;
var remove;
var isExpiredDate = "";
$(function () {
    
    DisplayTasks();

    var url = window.location.href;
    url = url.split('/');
    if (url[url.length - 1] === 'Finished' || url[url.length - 1] === 'Removed') {
        appendBreadcrumb();
    }

    $.datepicker.formatDate('dd MM, yy');

});

function appendBreadcrumb() {
    var html = '<li><i class="fa fa-angle-right"></i><span>' + resourceStatus + '</span></li>';
    $('.page-breadcrumb').append(html)
}

function DisplayTasks() {

    $('table#TasksTable').dataTable({
        "language": {
            //"url": "/Scripts/datatables-locals/datatables." + lang + ".json"
        },
        buttons: ['excelHtml5', 'csvHtml5', 'pdfHtml5', 'print'],
        'bPaginate': true,
        "bProcessing": false,
        "bServerSide": true,
        "lengthMenu": [[20, 50, 100, 200, 500, 10000], [20, 50, 100, 200, 500, resourceAll]],
        "dom": 'Blfrtip',
        "pageLength": 20,
        'responsive': true,
        "oLanguage": { "sSearch": "" },
        "order": [[0, "asc"]],
        "sAjaxSource": "/Tasks/GetTasksEditableAsync?_=" + new Date().getTime(),
        "fnServerParams": function (aoData) {

            //aoData.push({ name: 'languageID', value: $('#LanguageID').val() });
        },
        "fnServerData": function (sSource, aoData, fnCallback, oSettings) {
            oSettings.jqXHR = $.ajax({
                "dataType": 'json',
                "type": "GET",
                "url": sSource,
                "cache": false,
                "data": aoData,
                "beforeSend": function () {
                    Metronic.blockUI();
                },
                "success": function (response) {
                    fnCallback(response);
                    Metronic.unblockUI();
                },
                "error": function (response) {
                    showMessageDialog(response.statusText);
                    Metronic.unblockUI();
                }
            });
        },
        aoColumns: [

            { "mData": "PriorityName" },
            { "mData": "Title" },
            { "mData": "Description" },
            { "mData": "ToDataTableToDateFormat" },

                 {
                     "mData": function (data, type, full) {

                         if (data.isExpired) {
                             isExpiredDate = "disabled";
                         }
                         else {
                             isExpiredDate = "";
                         }

                         return '<button type="button" class="btn btn-info btn-block finishTask"  onclick="FinishTask(' + data.Id + ')"' + isExpiredDate + '>Finish <span class="fa fa-check"></span></button>';
                     }, "sClass": "center-alignment vertical-align-middle noBorderRight", "bSortable": false, 'sWidth': '10%'
                 },
                    {
                        "mData": function (data, type, full) {
                            return '<button type="button" class="btn btn-danger btn-block deleteTask"  onclick="DeleteTask(' + data.Id + ')">Trash <span class="glyphicon glyphicon-trash"></span></button>';
                        }, "sClass": "center-alignment vertical-align-middle noBorderRight", "bSortable": false, 'sWidth': '10%'
                    }
        ],
        'columnDefs': [
        {
            targets: [1],
            data: null,
            render: function (data, type, full, meta) {
               
                return '<a href="#" data-pk="' + full.Id + '" class="editable" data-name="' + resourceTitle + '">' + data + '</a>';
            }
        },
             {
                 targets: [2],
                 data: null,
                 render: function (data, type, full, meta) {
                     return '<a href="#" data-pk="' + full.Id + '" class="editable" data-name="' + resourceDescription + '">' + data + '</a>';
                 }
             },
                  {
                      targets: [3],
                      data: null,
                      render: function (data, type, full, meta) {
                          return '<a href="#" data-pk="' + full.Id + '" class="editable" data-name="' + "ToDate" + '">' + data + '</a>';
                      }
                  }
        ],
        "drawCallback": function (settings) {
            GenerateEditableFields();
            RegisterSearchOverride();

            $('#TasksTable_filter').addClass("form-group");
            $('input').addClass("form-control");
            $('input').attr('placeholder', 'Search a task ...');
            $('.dataTables_filter').css('margin-left', '0em;');
        }
    });

    //$('#TasksTable').dataTable({
    //    dom: "Bfrtip",
    //    'bPaginate': true,
    //    "bProcessing": false,
    //    "bServerSide": true,
    //    "pageLength": 10,
    //    'responsive': true,
    //    language: { search: "" },
    //    "order": [[0, "asc"]],
    //    "sAjaxSource": "/Tasks/GetTasksEditableAsync",
    //    //"bServerSide": true,
    //    "bSort": true,
    //    //"bPaginate": true,
    //    "bFilter": true,
    //    "fnServerData": function (sSource, aoData, fnCallback, oSettings) {
    //        oSettings.jqXHR = $.ajax({
    //            "dataType": 'json',
    //            "type": "GET",
    //            "url": sSource,
    //            "cache": false,
    //            "data": aoData,
    //            "beforeSend": function () {
    //                Metronic.blockUI();
    //            },
    //            "success": function (response) {
    //                fnCallback(response);
    //                Metronic.unblockUI();
    //            },
    //            "error": function (response) {
    //                showMessageDialog(response.statusText);
    //                Metronic.unblockUI();
    //            }
    //        });
    //    },
    //    //ajax: "/Tasks/GetTasksEditableAsync",
    //    "aoColumns": [
    //        { "mData": "PriorityName" },
    //        { "mData": "Title" },
    //        { "mData": "Description" },
    //        { "mData": "ToDataTableToDateFormat" },

    //             {
    //                 "mData": function (data, type, full) {
    //                     debugger;

    //                     if (data.isExpired) {
    //                         isExpiredDate = "disabled";
    //                     }
    //                     else {
    //                         isExpiredDate = "";
    //                     }

    //                     return '<button type="button" class="btn btn-info btn-block finishTask"  onclick="FinishTask(' + data.Id + ')"' + isExpiredDate + '>Finish</button>';
    //                 }, "sClass": "center-alignment vertical-align-middle noBorderRight", "bSortable": false, 'sWidth': '10%'
    //             },
    //                {
    //                    "mData": function (data, type, full) {
    //                        return '<button type="button" class="btn btn-danger btn-block deleteTask"  onclick="DeleteTask(' + data.Id + ')"><span class="glyphicon glyphicon-trash"></span> Trash</button>';
    //                    }, "sClass": "center-alignment vertical-align-middle noBorderRight", "bSortable": false, 'sWidth': '10%'
    //                },
    //    ],
    //    order: [1, 'asc'],
    //    buttons: [
    //    ],
    //    drawCallback: function (settings) {
    //        debugger;
    //        $('#TasksTable_filter').addClass("form-group");
    //        $('input').addClass("form-control");
    //        $('input').attr('placeholder', 'Search a task ...');
    //        $('.dataTables_filter').css('margin-left', '0em;');

    //    }
    //});
}

function DeleteTask(taskId) {

    swal({
        title: "Are you sure?",
        text: "You will not be able to recover this task!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!",
        cancelButtonText: "No, cancel plx!",
        closeOnConfirm: false,
        closeOnCancel: false
    },
    function (isConfirm) {
        if (isConfirm) {

            Metronic.blockUI();

            var id_row = $('.deleteTask').closest('tr[id="' + taskId + '"').attr('id');
            var nRow = $('#TasksTable tbody tr[id=' + id_row + ']')[0];


            $.post("/Tasks/DeleteTask", { taskId: taskId }, function (data) {

                if (data.success) {

                    if ($('table#TasksTable').length) {
                        var table = $('table#TasksTable').dataTable({ bRetrieve: true });

                        table.fnDeleteRow(nRow, null, true);
                    }
                }

                swal("Deleted!", data.message, "success");

                Metronic.unblockUI();
            }).error(function (jqXHR) {
                showMessageDialog(jqXHR.responseText);
                Metronic.unblockUI();
            });
        } else {
            swal("Cancelled", "Your task is safe :)", "error");
        }
    });
}

function FinishTask(taskId) {
    Metronic.blockUI();
    
    var id_row = $('.finishTask').closest('tr[id="' + taskId + '"').attr('id');
    var nRow = $('#TasksTable tbody tr[id=' + id_row + ']')[0];

    $.post("/Tasks/FinishTask", { taskId: taskId }, function (data) {

        if (data.success) {

            if ($('table#TasksTable').length) {
                var table = $('table#TasksTable').dataTable({ bRetrieve: true });

                table.fnDeleteRow(nRow, null, true);
            }
            swal("Done!", data.message, "success");
        }

        Metronic.unblockUI();
    }).error(function (jqXHR) {
        showMessageDialog(jqXHR.responseText);
        Metronic.unblockUI();
    });
}



//function updateDate() {

//    $('#DTE_Field_ToDataTableToDateFormat').unbind().bind("keydown", function (e) { // Bind for enter key press

//        if (e.keyCode === 13) {
//            debugger;
//            if (this.value !== "") {

//                var id = $(this).closest('tr').attr('id');
//                var date = $(this).val();

//                Metronic.blockUI();

//                $.post('/Tasks/Update', { Id: id, ToDate: date }, function (data) {

//                    if (data.success) {

//                        swal("Done!", data.message, "success");
//                        Metronic.unblockUI();
//                    }
//                }).error(function (jqXHR) {
//                    showMessageDialog(jqXHR.responseText);
//                    Metronic.unblockUI();
//                });
//            }
//        }
//    });
//}

function RegisterSearchOverride() {
    $(".dataTables_filter input")
        .unbind() // Unbind previous default bindings
        .bind("input", function (e) { // Bind for field changes
            // Search if enough characters, or search cleared with backspace
            if (this.value === "") {
                var table = $('table#TasksTable').dataTable({ bRetrieve: true });
                // Call the API search function
                table.api().search(this.value).draw();
            }
        })
        .bind("keydown", function (e) { // Bind for enter key press
            // Search when user presses Enter
            if (e.keyCode === 13) {
                if (this.value.length >= 2 || this.value === "") {
                    var table = $('table#TasksTable').dataTable({ bRetrieve: true });
                    table.api().search(this.value).draw();
                }
            }
        });
    tableSearchInit = true;
}

function GenerateEditableFields() {

    var viewportWidth = $(window).width();
    if (viewportWidth > 600) {
        var fieldName = null;
        var newValue = null;
        $('#TasksTable .editable').each(function () {

            $(this).editable({
                mode: 'inline',
                type: 'text',
                //categoryID: $(this).attr('data-categoryID'),
                name: $(this).attr('data-name'),
                url: '/Tasks/Update/',
                title: 'Enter ' + $(this).attr('data-name'),
                validate: function (value) {
                    fieldName = $(this).attr('data-name');
                    if ($.trim(value) === '') {
                        return resourceRequiredField;
                    }
                    newValue = value;
                },
                success: function (data) {

                    if (!data.success) {
                        return data.message;
                    }

                    var table = $('table#TasksTable').dataTable({ bRetrieve: true });
                    table.fnDraw();

                    var tableRowData = table.api().row($(this).closest('tr')).data();
                    tableRowData[fieldName] = newValue;

                }
            });
        });
    }
}





