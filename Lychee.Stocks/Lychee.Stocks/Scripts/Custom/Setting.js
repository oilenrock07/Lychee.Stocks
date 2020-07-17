//Convert this to typescript
$(function () {

    function handleUpdateClick(e) {
        e.preventDefault();
        var value = $(this).closest('tr').find('input').val();
        var id = $(this).data('id');
        var name = $(this).data('name');

        var data = {  name: name, settingId: id, value: value };
        $.post('/Settings/UpdateSetting', data );
    }

    function init() {
        $('body').on('click', ".js-update", handleUpdateClick);
    }

    init();
});