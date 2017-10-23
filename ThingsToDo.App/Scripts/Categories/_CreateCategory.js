$(function () {
    //$('#btnCreateGC').on('click', function (e) {
    $('#btnCreateCategory').on('click', function (e) {
        debugger;
        e.preventDefault();
        PostUpdate();
    });
})

function PostUpdate() {
    debugger;
    var updateTransForm = $('form#CategoryCreate')
    Metronic.blockUI();
    var validator = updateTransForm.validate();
    if (updateTransForm.valid()) {
        $.post('/Categories/Create', updateTransForm.serialize(), function (data) {
            debugger;
            if (data.success) {
                HideAndClearGlobalModal();
                showMessageDialog(data.message);
                debugger;
                showCategories();



                Metronic.unblockUI();
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