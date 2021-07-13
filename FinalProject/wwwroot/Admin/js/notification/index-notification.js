$(() => {
    $('#notification-show').remove();
})
function deleteNotify(el, id) {
    if (id === '') {
        showErrorSystem();
        return undefined;
    }

    $.ajax({
        url: '/Admin/Notification/DeleteNofity',
        method: 'DELETE',
        async: true,
        data: { notificationId: id },
        success: function (data) {
            switch (data) {
                case 1:
                    showSuccess("Xóa thông báo thành công");
                    $(el).closest('.row').remove();
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