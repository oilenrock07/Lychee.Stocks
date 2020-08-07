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
                $.get(url).done((partialView) => {
                    $(e).html(partialView);
                    $(e).removeAttr('data-url');
                    $(e).removeAttr('data-verb');
                });
            }

        });
    }

    init();
});