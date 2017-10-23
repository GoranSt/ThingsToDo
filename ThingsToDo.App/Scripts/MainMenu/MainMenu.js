function preselectMenuItem() {

    var url = location.pathname.toLowerCase();

    if (url.lastIndexOf('/') == url.length - 1) {
        url = url.substr(0, url.length - 1);
    }
    var menu = $('.page-sidebar-menu');
    var el;

    menu.find('a').each(function () {
        var path = $(this).attr("href").toLowerCase();
        var altPath = $(this).attr("althref") ? $(this).attr("althref").toLowerCase() : "";
        // url match condition         
        //if (path.length > 1 && url.substr(1, path.length - 1) == path.substr(1)) {
        if (path == url || altPath == url) {
            el = $(this);
            return;
        }
    });

    expandInterviewQuestionsAndBehavioralDesc();

    if (!el || el.size() == 0) {
        return;
    }

    if (el.attr('href').toLowerCase() === 'javascript:;' || el.attr('href').toLowerCase() === '#') {
        return;
    }

    var slideSpeed = parseInt(menu.data("slide-speed"));
    var keepExpand = menu.data("keep-expanded");

    // disable active states
    menu.find('li.active').removeClass('active');
    menu.find('li > a > .selected').remove();

    if (menu.hasClass('page-sidebar-menu-hover-submenu') === false) {
        menu.find('li.open').each(function () {
            if ($(this).children('.sub-menu').size() === 0) {
                $(this).removeClass('open');
                $(this).find('> a > .arrow.open').removeClass('open');
            }
        });
    } else {
        menu.find('li.open').removeClass('open');
    }

    el.parents('li').each(function () {
        $(this).addClass('active');
        $(this).find('> a > span.arrow').addClass('open');

        if ($(this).parent('ul.page-sidebar-menu').size() === 1) {
            $(this).find('> a').append('<span class="selected"></span>');
        }

        if ($(this).children('ul.sub-menu').size() === 1) {
            $(this).addClass('open');
        }
    });
};

function expandInterviewQuestionsAndBehavioralDesc() {
    debugger;
    var url = document.URL;
    var url_array = url.split('/');
    var category = url_array[url_array.length - 2];
    if (category === 'Show') {

        $('#categoryItem').addClass('open active');
    }
}


jQuery(document).ready(function () {
    preselectMenuItem(); //preselect menu item   
  
});
