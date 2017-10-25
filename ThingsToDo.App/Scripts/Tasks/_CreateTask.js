$(function () {
   
    $("#fromDate").datepicker({ dateFormat: 'dd MM, yy' });
    $("#toDate").datepicker({ dateFormat: 'dd MM, yy' });
    
    $('#btnCreateTask').on('click', function (e) {
        e.preventDefault();
        PostUpdate();
    });
})

function PostUpdate() {
   Metronic.blockUI();

    var updateTransForm = $('#frmTaskCreate');
    var validator = updateTransForm.validate();

    if (updateTransForm.valid()) {
        $.post('/Tasks/Create', updateTransForm.serialize(), function (data) {
          
            if (data.success) {

                HideAndClearGlobalModal();
             
                if ($('table#TasksTable').length)
                {
                   var table = $('table#TasksTable').dataTable();
                   table.fnReloadAjax();
                   swal("Done!", data.message, "success");

                    Metronic.unblockUI();
                }
            }
        }).error(function (jqXHR) {
            showMessageDialog(jqXHR.responseText);
            Metronic.unblockUI();
        });
    } else {
        validator.showErrors();
        Metronic.unblockUI();
    }
}