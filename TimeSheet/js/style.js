function updateStriping(jquerySelector) {
    var count = 0;
    $(jquerySelector).each(function (index, row) {
        $(row).removeClass('odd');
        if ($(row).is(":visible")) {
            if (count % 2 == 1) {
                $(row).addClass('odd');
            }
            count++;
        }
    });
}


function setCookie(cname, cvalue) {
    document.cookie = cname + "=" + cvalue;
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}

function checkCookie(cname) {
    var user = getCookie(cname);
    if (user != null) {
        return false;
    }
    return true;
}
