function deleteProduct(e, productId) {
    if (productId === '') {
        showError('Không tồn tại sản phẩm này');
    }
    else {
        Swal.fire({
            title: 'Xóa sản phẩm!',
            text: "Bạn có muốn xóa sản phẩm này ?",
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
                    url: '/Admin/Product/DeleteProduct',
                    method: 'DELETE',
                    async: true,
                    data: { productId: productId },
                    success: function (data) {
                        loadingOut(loading, 0);

                        switch (data) {
                            case -4:
                                showError('Sản phẩm không tồn tại');
                                break;

                            case 1:
                                showSuccess('Xóa sản phẩm thành công');
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