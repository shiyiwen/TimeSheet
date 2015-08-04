$(document).ready(function () {
    $("#tblTimesheet").tablesorter({
        headers: {
            4: {
                sorter: false
            },
            5: {
                sorter: false
            }
        },
        widgets: ['zebra']
    })

    var today = new Date();
    var dd = ("0" + (today.getDate())).slice(-2);
    var mm = ("0" + (today.getMonth() + 1)).slice(-2);
    var yyyy = today.getFullYear();
    if (mm != "01") {
        var lm = ("0" + (today.getMonth())).slice(-2);
        var ly = yyyy;
    }
    else {
        var lm = "12";
        var ly = yyyy - 1;
    }
    var fd = "01";
    var lastdate = new Date(yyyy, lm, 0);
    var ld = ("0" + (lastdate.getDate())).slice(-2);
    today = yyyy + '-' + mm + '-' + dd;
    firstDateofMonth = yyyy + '-' + mm + '-' + fd;
    firstDateofLastMonth = ly + '-' + lm + '-' + fd;
    lastDateofLastMonth = ly + '-' + lm + '-' + ld;
    $("#TimesheetThisMonth").attr("value", mm + '/' + yyyy);
    $("#TimesheetLastMonth").attr("value", lm + '/' + ly);

    var tb = ["searchEmployees", "searchInTimeFrom", "searchInTimeTo", "searchOutTimeFrom", "searchOutTimeTo"];

    if (checkCookie(searchInTimeFrom) && checkCookie(searchInTimeTo) && checkCookie(searchOutTimeFrom) && checkCookie(searchOutTimeTo)) {
        $("#searchEmployees").val("");
        $("#searchInTimeFrom, #searchOutTimeFrom").val(firstDateofMonth);
        $("#searchInTimeTo, #searchOutTimeTo").val(today);
    }
    else {
        $.each(tb, function (i, val) {
            $("#" + val).val(getCookie(val));
        });
    }

    $("#TimesheetSubmit").click(function () {
        $.each(tb, function (i, val) {
            setCookie(val, $("#" + val).val());
        });
    })

    $("#TimesheetReset").click(function () {
        $("#searchEmployees").val("");
        $("#searchInTimeFrom, #searchOutTimeFrom").val("");
        $("#searchInTimeTo, #searchOutTimeTo").val("");
    })

    $("#TimesheetThisMonth").click(function () {
        $("#searchInTimeFrom, #searchOutTimeFrom").val(firstDateofMonth);
        $("#searchInTimeTo, #searchOutTimeTo").val(today);
    })

    $("#TimesheetLastMonth").click(function () {
        $("#searchInTimeFrom, #searchOutTimeFrom").val(firstDateofLastMonth);
        $("#searchInTimeTo, #searchOutTimeTo").val(lastDateofLastMonth);
    })

    $("#btnExport").click(function () {
        $('#tblTimesheet').tableExport({
            type: 'excel',
            escape: 'false'
        });
    })

    showDropDownList();
    $("input[name=rdbEmployee]:radio").change(function () {
        showDropDownList();
    });

});

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

function showDropDownList() {
    $("#searchEmployees").hide();
    $("#searchActiveEmployees").hide();
    $("#searchInactiveEmployees").hide();
    var type = $('input:radio[name=rdbEmployee]:checked').val();
    //alert("from showDropDownList"+type);
    switch(type){
        case "All":
            $("#searchEmployees").show();
            break;
        case "Active":
            $("#searchActiveEmployees").show();
            break;
        case "Inactive":
            $("#searchInactiveEmployees").show();
            break;
    }
}