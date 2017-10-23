$(function () {
    $(document).on('click', 'a#ChangePassword', function (e) {
        ChangePasswordDialogOpen();
    });
});

$(function () {
    $(document).on('click', 'a#ReciveEmails', function (e) {
        ReciveEmails();
    });
});

$(function () {
    $(document).on('click', 'a#userPersonalProfile', function (e) {
        e.preventDefault();
        if (typeof (personId) != 'undefined' && personId != -1) {
            var href = $(this).attr('href');
            window.location.replace(href + "?id=" + personId);
        }
        else {
            showMessageDialog(noProfileForThisUser);
        }
    });
});

function clearlocalstorage() {
    if (typeof (Storage) !== "undefined") {
        if (localStorage.getItem("EUID") != null)
            localStorage.removeItem("EUID");
        if (localStorage.getItem("EUTITLE") != null)
            localStorage.removeItem("EUTITLE");
    }
}

function showMessageDialog(message) {
    BootstrapDialog.ok({
        message: message,
        closable: true,
        draggable: true,
        callback: function (result) {
            if (result) {

            } else {
            }
        }
    });
}

function HideAndClearGlobalModal() {
    $('#globalModalContent').html('');
    $('#globalModal').modal('hide');
}

function HideAndClearSecondGlobalModal() {
    $('#globalSecondModalContent').html('');
    $('#globalSecondModal').modal('hide');
}

function ChangePasswordDialogOpen() {
    $.get('/Account/ChangePassword/', function (data) {
        if (typeof (data.message) == 'undefined') {
            $('#globalModalContent').html(data);
            $('#globalModal').modal('show');
            $.validator.unobtrusive.parse($("form#frmChangePassword"));
        } else {
            showMessageDialog(data.message);
        }
    }).error(function (jqXHR) {
        showMessageDialog(jqXHR.responseText);
    });
}

function Select2ValidationDefaults() {
    $.validator.setDefaults({
        errorElement: "span",
        errorClass: "input-validation-error",
        //	validClass: 'stay',
        highlight: function (element, errorClass, validClass) {
            $(element).addClass('input-validation-error'); //.removeClass(errorClass);
            $(element).parent().find('.select2-selection--single').addClass('input-validation-error');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass('.input-validation-error'); //.addClass(validClass);
            $(element).parent().find('.select2-selection--single').removeClass('.input-validation-error');
        },
        errorPlacement: function (error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else if (element.hasClass('select2')) {
                error.insertAfter(element.next('span'));
            } else {
                error.insertAfter(element);
            }
        }
    });
}

function ReciveEmails() {
    Metronic.blockUI();
    $.get('/UserSetings/ProfileSetings/', function (data) {
        $('#globalModalContent').html(data);
        $('#globalModal').modal('show');
        Metronic.unblockUI();
    });
}