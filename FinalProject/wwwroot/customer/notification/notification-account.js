function seenNotify(id) {
    $.ajax({
        url: '/Home/SeenNotify',
        method: 'PUT',
        async: true,
        data: { notificationId: id },
        success: function (data) {

        },
        error: function (code, err) {

        }
    });
}

function deleteNotify(el, id) {
    if (id === '') {
        showErrorSystem();
        return undefined;
    }

    $.ajax({
        url: '/Home/DeleteNofity',
        method: 'DELETE',
        async: true,
        data: { notificationId: id },
        success: function (data) {
            switch (data) {
                case 1:
                    showSuccess("Xóa thông báo thành công");
                    $(el).closest('tr').remove();
                    break;
                default:
                    showError("Hệ thống xảy ra lỗi vui lòng thử lại sau", '360px')
                    break;
            }
        },
        error: function (code, err) {
            showErrorSystem();
        }
    })
}