function adminConfirm(orderId) {
    if (orderId === '') {
        showError("Không tồn tại đơn hàng này");
    }
    else
        Swal.fire({
            title: 'Xác nhận đơn hàng!',
            text: "Bạn đồng ý giao đơn hàng này ?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: `Đồng ý`,
            cancelButtonText: `Hủy`,
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/Admin/Order/AdminComfirmOrder',
                    method: 'PUT',
                    async: true,
                    data: { orderId: orderId },
                    success: function (data) {
                        switch (data) {
                            case 1:
                                window.location.reload();
                                break;
                            case -4:
                                showError('Lỗi không tìm thấy đơn hàng này');
                                break;
                            case -99:
                                showErrorSystem();
                                break;
                        }
                    },
                    error: function (code, err) {
                        showErrorSystem();
                    }
                });
            }
        })
}

function RejectOrder(e, orderId) {
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
                    url: '/Admin/Order/RejectOrder',
                    method: 'PUT',
                    async: true,
                    data: { orderId: orderId, content: content },
                    success: function (data) {
                        loading.out();
                        switch (data) {
                            case 1:
                                $(e).remove();
                                $("#admin-confirm").remove();
                                showSuccess('Hủy đơn hàng thành công');
                                break;
                            case 3:
                                $(e).remove();
                                $("#admin-confirm").remove();
                                showError('Đơn hàng này đã được hoàn lại tiền');
                                break;
                            case -4:
                                showError('Lỗi không tìm thấy đơn hàng này');
                                break;
                            case -99:
                                showErrorSystem();
                                break;
                        }
                    },
                    error: function (code, err) {
                        showErrorSystem();
                        loading.out();
                        console.error(err);
                    }
                });
            }
        })
    }
}

function ConfirmRejectOrder(e, orderId) {
    if (orderId === '') {
        showError("Không tồn tại đơn hàng này");
    }
    else
        Swal.fire({
            title: 'Xác nhận đơn hàng!',
            text: "Bạn đồng ý hủy đơn hàng này và hoàn tiền lại cho khách hàng ?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: `Đồng ý`,
            cancelButtonText: `Hủy`,
        }).then((result) => {
            if (result.isConfirmed) {
                var loading = verticalTextColor();
                loading;
                $.ajax({
                    url: '/Admin/Order/RejectOrder',
                    method: 'PUT',
                    async: true,
                    data: { orderId: orderId },
                    success: function (data) {
                        loading.out();
                        switch (data) {
                            case 1:
                                $(e).remove();
                                showSuccess('Hủy đơn hàng thành công');
                                break;
                            case 3:
                                $(e).remove();
                                showError('Đơn hàng này đã được hoàn lại tiền');
                                break;
                            case -4:
                                showError('Lỗi không tìm thấy đơn hàng này');
                                break;
                            case -99:
                                showErrorSystem();
                                break;
                        }
                    },
                    error: function (code, err) {
                        showErrorSystem();
                        loading.out();
                        console.error(err);
                    }
                });
            }
        })
}