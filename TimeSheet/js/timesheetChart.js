$(document).ready(function () {
    showDropDownList();
    $("#lbYear").hide();
    $("input[name=rdbEmployee]:radio").change(function () {
        showDropDownList();
    });

    $("#ChartSubmit").click(function () {
        if ($('#ddlYear :selected').val() == "") {
            $("#lbYear").text("Year is required.").show();
            $("form").bind("submit", preventDefault);
        }
        else {
            $("form").unbind("submit", preventDefault);
            $("#lbYear").text("").hide();
        }
    })

    $("#ChartReset").click(function () {
        $('#ddlYear').val("");
        $('#ddlMonth').val("");
        $("#lbYear").text("");
    })
})

function showDropDownList() {
    $("#ddlAllEmployees").hide();
    $("#ddlActiveEmployees").hide();
    $("#ddlInactiveEmployees").hide();
    var type = $('input:radio[name=rdbEmployee]:checked').val();
    switch(type){
        case "All":
            $("#ddlAllEmployees").show();
            break;
        case "Active":
            $("#ddlActiveEmployees").show();
            break;
        case "Inactive":
            $("#ddlInactiveEmployees").show();
            break;
    }
}

function preventDefault(e) {
    e.preventDefault();
}

function getChartTitle() {
    var d = new Date();
    if ($('#ddlYear :selected').val() == "") {
        var y = d.getFullYear().toString();
        var m = (d.getMonth() + 1).toString();
    }
    else {
        var y = $('#ddlYear :selected').val();
        var m = $('#ddlMonth :selected').val();
    }
    var type = $('input:radio[name=rdbEmployee]:checked').val();
    var emp
    switch (type) {
        case "All":
            emp = $("#ddlAllEmployees :selected").text();
            break;
        case "Active":
            emp = $("#ddlActiveEmployees :selected").text();
            break;
        case "Inactive":
            emp = $("#ddlInactiveEmployees :selected").text();
            break;
    }

    var chartTitle;
    if (m == "")
        chartTitle = "Timesheet Chart - " + y + " - (" + emp + ")";
    else
        chartTitle = "Timesheet Chart - " + m + "/" + y + " - (" + emp + ")";
    return chartTitle;
}
