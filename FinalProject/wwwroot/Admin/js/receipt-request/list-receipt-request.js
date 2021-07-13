function rejectReceipt(el, id) {
    if (id === '') {
        showErrorSystem();
        return undefined;
    }
    $.ajax({
        url: '/Admin/Warehouse/RejectReceipt',
        method: 'DELETE',
        async: true,
        data: { id: id },
        success: function (data) {
            switch (data) {
                case 1:
                    showSuccess('Từ chối phiếu yêu cầu nhập kho thành công', '420px');
                    $(el).closest('tr').remove();
                    break;
                default:
                    showErrorSystem();
                    break;
            }
        },
        error: function (code, err) {
            showErrorSystem();
        }
    })
}

function approvedReceipt(id) {
    if (id === '') {
        showErrorSystem();
        return undefined;
    }
    $.ajax({
        url: '/Admin/Warehouse/ApproveReceipt',
        method: 'PUT',
        async: true,
        data: { id: id },
        success: function (data) {
            switch (data) {
                case 1:
                    window.location.reload();
                    break;
                default:
                    showErrorSystem();
                    break;
            }
        },
        error: function (code, err) {
            showErrorSystem();
        }
    })
}