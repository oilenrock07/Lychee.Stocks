//Convert this to typescript
$(function () {

    function init() {
        $.each($('partial-loader'), (i, e) => {
            var verb = $(e).data('verb');
            var url = $(e).data('url');

            if (verb.toLowerCase() === 'post') {
                $.post('/Settings/UpdateSetting', data).done(() => {
                    alert('Setting has been successfully updated');
                });
            } else {
                $.post(url).done((partialView) => {
                    $(e).html(partialView);
                    $(e).removeAttr('data-url');
                });
            }

        });
    }

    init();
});