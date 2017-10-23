$(function () {
    debugger;

   

    
    $("#fromDate").datepicker({ dateFormat: 'dd MM, yy' });
    $("#toDate").datepicker({ dateFormat: 'dd MM, yy' });
    
    $('#btnCreateTask').on('click', function (e) {
        debugger;
        e.preventDefault();
        PostUpdate();
    });
})



function PostUpdate() {
    debugger;
    var updateTransForm = $('#frmTaskCreate');
    console.log(updateTransForm); 
    Metronic.blockUI();
  var validator = updateTransForm.validate();
    if (updateTransForm.valid()) {
        $.post('/Tasks/Create', updateTransForm.serialize(), function (data) {
            debugger;
            if (data.success) {

                HideAndClearGlobalModal();
                debugger;
                if ($('table#TasksTable').length) {
                    //var table = $('table#TasksTable').dataTable({ bRetrieve: true });
                   var table = $('table#TasksTable').dataTable();
                   table.fnReloadAjax();

                   swal("Done!", data.message, "success");

                    //showMessageDialog(data.message);
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