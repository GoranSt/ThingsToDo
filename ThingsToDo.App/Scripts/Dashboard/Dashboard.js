$(function () {
      
    showCategories();

    $('#CreateTask').on('click', function () {
        createTask();
    });

    $('#CreateCategory').on('click', function () {

        swal({
            title: "Add category!",
            text: "Write something to group tasks:",
            type: "input",
            showCancelButton: true,
            closeOnConfirm: false,
            animation: "slide-from-top",
            inputPlaceholder: "Write something"
        },
function (inputValue) {
    if (inputValue === false) return false;

    if (inputValue === "") {
        swal.showInputError("You need to write something!");
        return false
    }

    $.post('/Categories/Create', { Title: inputValue }, function (data) {

        if (data.success) {

            showCategories();
           
            swal("Done!", inputValue + " " + data.message, "success");
        }
        else {
            swal("Sorry!", data.message, "error");

        }
    });
});
    });


});

function printItemList(id, title) {

    var html = '';

    html += '<li class="nav-item">';
    html += '<a href="/Categories/Show/' + id + '" class="nav-link ">';
    html += '<span class="title">' + title + '</span></a></li>';

    return html;
};


function showCategories() {

    $.get('/Categories/GetAllCategories', function (data) {

        $('#ulCategoryList').empty();
        $.each(data.categories, function (index, element) {

            /// do stuff
            $('#ulCategoryList').append(printItemList(element.Id, element.Title));

        });

    }).error(function (jqXHR) {

    });

};

function createTask() {
    debugger;
    Metronic.blockUI();
    $.get('/Tasks/Create', function (data) {
        $('#globalModalContent').html(data);
        $('#globalModal').modal('show');
        Metronic.unblockUI();
    }).error(function (jqXHR) {
        Metronic.unblockUI();
    });
};

function createCategory() {

    $.get('/Categories/Create', function (data) {

        $('#globalModalContent').html(data);
        $('#globalModal').modal('show');

    }).error(function (jqXHR) {

    });
};
