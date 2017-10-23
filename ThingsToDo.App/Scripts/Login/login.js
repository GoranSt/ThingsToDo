function register() {
    $.ajax({
        // Get Student PartialView
        url: "/Account/Register",
        type: 'Post',
        data: AddAntiForgeryToken({ TenantId: $('#regtenantid').val(), Email: $('#regemail').val(), Password: $('#regpassword').val(), ConfirmPassword: $('#regconfirmpassword').val() }),
        success: function (data) {
            if (data.status == 'error') {
                $('#regerror').html('');
                $('#regerror').html($('<div/>').html(data.value).text());
            }

            if (data.status == 'success') {
                location.href = data.value;
            }
        },
        error: function (data) {
            if (data.status == 'error') {
                $('#regerror').html('');
                $('#regerror').html($('<div/>').html(data.value).text());
            }

            if (data.status == 'success') {
                location.href = data.value;
            }
        }
    });
}

function reset() {
    $.ajax({
        // Get Student PartialView
        url: "/Account/Reset",
        type: 'Post',
        data: AddAntiForgeryToken({ Email: $('#reset-email').val() }),
        success: function (data) {
            if (data.status == 'error') {
                $('#reseterror').html('');
                $('#reseterror').html($('<div/>').html(data.value).text());
            }

            if (data.status == 'success') {
                $('#resetmsg').html(data.value);
            }
        },
        error: function (data) {
            if (data.status == 'error') {
                $('#reseterror').html('');
                $('#reseterror').html($('<div/>').html(data.value).text());
            }

            if (data.status == 'success') {
                $('#resetmsg').html(data.value);
            }
        }
    });
}

AddAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]').val();
    return data;
};

$(document).ready(function () {
    Metronic.init(); // init metronic core components
    Layout.init(); // init current layout

    Login.init();

    // init background slide images
    $.backstretch([
     "/Content/Images/background/1.jpg",
     "/Content/Images/background/2.jpg",
     "/Content/Images/background/3.jpg",
     "/Content/Images/background/4.jpg"
    ], {
        fade: 1000,
        duration: 8000
    }
 );

    $('#register-submit-btn').click(function (e) {
        e.preventDefault();
        register();
    });

    $('#forgot-submit-btn').click(function (e) {
        e.preventDefault();
        reset();
    });
});