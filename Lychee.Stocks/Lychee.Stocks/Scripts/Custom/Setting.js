//Convert this to typescript
$(function () {

    function handleUpdateClick(e) {
        e.preventDefault();
        var value = $(this).closest('tr').find('input').val();
        var id = $(this).data('id');

        var data = { settingId: id, value: value };
        $.post('/Settings/UpdateSetting', data);
    }

    function init() {
        $('body').on('click', ".js-update", handleUpdateClick);
    }

    init();
});