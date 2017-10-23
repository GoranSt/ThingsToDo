$(function () {
    debugger;

    $('.CreateTask').on('click', function () {

        createTaskForCategory();
       
    });
      
});


function createTaskForCategory() {
    debugger;
    $.get('/Tasks/Create/?categoryId=' + GetCategoryId(), function (data) {
        debugger;
        $('#globalModalContent').html(data);
        $('#globalModal').modal('show');

    }).error(function (jqXHR) {

    });
}

function GetCategoryId() {
    debugger;
    var full_url = document.URL; // Get current url
    var url_array = full_url.split('/') // Split the string into an array with / as separator
    var categoryId = url_array[url_array.length - 1];  // Get the last part of the array (-1)

    return categoryId;
}


