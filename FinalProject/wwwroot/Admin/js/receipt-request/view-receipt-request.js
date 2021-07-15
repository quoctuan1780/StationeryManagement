function DeleteReceipt(e, requestId) {
    if (requestId === '') {
        showError('Không tồn tại phiếu yêu cầu nhập kho này');
    }
    else {
        Swal.fire({
            title: 'Xóa phiếu yêu cầu nhập kho!',
            text: "Bạn có muốn xóa phiếu yêu cầu nhập kho này ?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: `Xóa`,
            cancelButtonText: `Hủy`,
        }).then((result) => {
            if (result.isConfirmed) {
                var loading = verticalTextColor();
                loading;
                $.ajax({
                    url: '/Warehouse/Home/DeleteRequest',
                    method: 'DELETE',
                    async: true,
                    data: { requestId: requestId },
                    success: function (data) {
                        loadingOut(loading, 0);

                        switch (data) {
                            case -4:
                                showError('Yêu cầu nhập hàng không tồn tại');
                                break;

                            case 1:
                                showSuccess('Xóa thành công');
                                $(e).closest('tr').remove();
                                break;

                            case -99:
                                showErrorSystem();
                                break;
                        }
                    },
                    error: function (code, err) {
                        loadingOut(loading, 0);
                        showErrorSystem();
                    }
                });
            }
        });
    }
}