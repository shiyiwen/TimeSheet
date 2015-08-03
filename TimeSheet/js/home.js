$(document).ready(function () {
    $("#tblHome").tablesorter({
        headers: {
            2: { sorter: false }
        },
        widgets: ['zebra']
    });

    $("#SearchFirstName").keyup(function () {
        var rows = $("#tblHomeBody").find("tr").hide();
        if (this.value.length) {
            var data = $.trim(this.value).split(" ");
            $.each(data, function (i, v) {
                $("#tblHomeBody td.col1").filter(function () {
                    return $(this).text().toLowerCase().indexOf(v.toLowerCase()) > -1;
                }).parent('tr').show();
            });
            $("#SearchLastName").attr('disabled', true);
        }
        else {
            rows.show();
            $("#SearchLastName").attr('disabled', false);
        }
        updateStriping("#tblHomeBody tr");
    })

    $("#SearchLastName").keyup(function () {
        var rows = $("#tblHomeBody").find("tr").hide();
        if (this.value.length) {
            var data = $.trim(this.value).split(" ");
            $.each(data, function (i, v) {
                $("#tblHomeBody td.col2").filter(function () {
                    return $(this).text().toLowerCase().indexOf(v.toLowerCase()) > -1;
                }).parent('tr').show();
            });
            $("#SearchFirstName").attr('disabled', true);
        }
        else {
            rows.show();
            $("#SearchFirstName").attr('disabled', false);
        }
        updateStriping("#tblHomeBody tr");
    })

    $("#ResetHomeSearch").click(function () {
        $('#SearchFirstName, #SearchLastName').val('');
        $("#SearchFirstName, #SearchLastName").attr('disabled', false);
        var rows = $("#tblHomeBody").find("tr").show();
        updateStriping("#tblHomeBody tr");
    })
});