//Convert this to typescript
$(function () {

    function handleDeleteConfirmation(e) {
        return confirm('Are you sure you want to delete this record');
    }

    function init() {
        $('body').on('click', ".js-delete", handleDeleteConfirmation);
    }

    init();
});