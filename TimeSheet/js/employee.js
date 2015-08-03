$(document).ready(function () {
    updateStriping("#tblEmployeeBody tr");
    $("table").tablesorter({
        headers: {
            2: {
                sorter: false
            },
            3: {
                sorter: false
            }
        },
        widgets: ['zebra']
    });

    $("#SearchFirstName").keyup(function () {
        var rows = $("#tblEmployeeBody").find("tr").hide();
        if (this.value.length) {
            var data = $.trim(this.value).split(" ");
            $.each(data, function (i, v) {
                $("#tblEmployeeBody td.col1").filter(function () {
                    return $(this).text().toLowerCase().indexOf(v.toLowerCase()) > -1;
                }).parent('tr').show();
            });
            $("#SearchLastName, #SearchEmail").attr('disabled', true);
        }
        else {
            rows.show();
            $("#SearchLastName, #SearchEmail").attr('disabled', false);
        }
        updateStriping("#tblEmployeeBody tr");
    })

    $("#SearchLastName").keyup(function () {
        var rows = $("#tblEmployeeBody").find("tr").hide();
        if (this.value.length) {
            var data = $.trim(this.value).split(" ");
            $.each(data, function (i, v) {
                $("#tblEmployeeBody td.col2").filter(function () {
                    return $(this).text().toLowerCase().indexOf(v.toLowerCase()) > -1;
                }).parent('tr').show();
            });
            $("#SearchFirstName, #SearchEmail").attr('disabled', true);
        }
        else {
            rows.show();
            $("#SearchFirstName, #SearchEmail").attr('disabled', false);
        }
        updateStriping("#tblEmployeeBody tr");
    })

    $("#SearchEmail").keyup(function () {
        var rows = $("#tblEmployeeBody").find("tr").hide();
        if (this.value.length) {
            var data = $.trim(this.value).split(" ");
            $.each(data, function (i, v) {
                $("#tblEmployeeBody td.col3").filter(function () {
                    return $(this).text().toLowerCase().indexOf(v.toLowerCase()) > -1;
                }).parent('tr').show();
            });
            $("#SearchFirstName, #SearchLastName").attr('disabled', true);
        }
        else {
            rows.show();
            $("#SearchFirstName, #SearchLastName").attr('disabled', false);
        }
        updateStriping("#tblEmployeeBody tr");
    })

    $("#ResetEmployeeSearch").click(function () {
        $('#SearchFirstName, #SearchLastName, #SearchEmail').val('');
        $("#SearchFirstName, #SearchLastName, #SearchEmail").attr('disabled', false);
        var rows = $("#tblEmployee").find("tr").show();
        updateStriping("#tblEmployeeBody tr");
    })
});