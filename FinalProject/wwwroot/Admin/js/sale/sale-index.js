function deleteSale(e, saleId) {
    if (saleId === '') {
        showError('Không tồn tại loại quảng cáo này');
    }
    else {
        Swal.fire({
            title: 'Xóa khuyến mại!',
            text: "Bạn có muốn xóa khuyến mại này ?",
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
                    url: '/Admin/Sale/DeleteSale',
                    method: 'DELETE',
                    async: true,
                    data: { saleId: saleId },
                    success: function (data) {
                        loadingOut(loading, 0);

                        switch (data) {
                            case -4:
                                showError('Khuyến mại không tồn tại');
                                break;

                            case 1:
                                showSuccess('Xóa khuyến mại thành công');
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