//Convert this to typescript
$(function () {

    function handleSubmitTableRowClick(e) {
        e.preventDefault();

        //get all the input form in the table row.
        var data = {};
        $(this).closest('tr').find('input').each((i, e) => {
            data[$(e).attr('name')] = $(e).val();
        });

        //submit the data to the controller
        var url = $(this).data('url');
        $.post(url, data).done(() => {
            alert('Record has been updated');
        });
    }

    function init() {
        $('body').on('click', ".js-ajaxSubmitRow", handleSubmitTableRowClick);
    }

    init();
});