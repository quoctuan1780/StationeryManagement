function rejectOrder(e, orderId) {
    if (orderId === '') {
        showError('Không tồn tại đơn hàng này');
    }
    else {
        Swal.fire({
            title: 'Nhập nội dung',
            html: '<textarea class="form-control" rows="5" id="category-input"></textarea>',
            showCancelButton: true,
            confirmButtonText: 'Hủy đơn hàng',
            cancelButtonText: 'Hủy'
        }).then((result) => {
            if (result.isConfirmed) {
                var content = $("#category-input").val();
                if (content === '' || content.trim() === '') {
                    showError('Bạn chưa nhập lý do hủy hàng')
                }
                else
                    var loading = verticalTextColor();
                $.ajax({
                    url: '/Order/RejectOrder',
                    async: true,
                    method: 'PUT',
                    data: { orderId: orderId, content: content },
                    success: function (data) {
                        loading.out();
                        switch (data) {
                            case -4:
                                showError("Lỗi không tìm thấy đơn hàng này");
                                break;
                            case 1:
                                $(e).remove();
                                showSuccess('Hủy đơn hàng thành công');
                                break;
                            case -99:
                                showErrorSystem();
                                break;
                        }
                    },
                    error: function (code, err) {
                        loading.out();
                        showErrorSystem();
                    }
                });
            }
        })
    }
}