function deleteBanner(e, bannerId) {
    if (bannerId === '') {
        showError('Không tồn tại loại quảng cáo này');
    }
    else {
        Swal.fire({
            title: 'Xóa quảng cáo!',
            text: "Bạn có muốn xóa quảng cáo này ?",
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
                    url: '/Admin/Banner/DeleteBanner',
                    method: 'DELETE',
                    async: true,
                    data: { bannerId: bannerId },
                    success: function (data) {
                        loadingOut(loading, 0);

                        switch (data) {
                            case -4:
                                showError('Quảng cáo không tồn tại');
                                break;

                            case 1:
                                showSuccess('Xóa quảng cáo thành công');
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