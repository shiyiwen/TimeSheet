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
