function shipperConfirm(orderId) {
    if (orderId === '') {
        showError("Không tồn tại đơn hàng này");
    }
    else
        Swal.fire({
            title: 'Xác nhận giao đơn hàng!',
            text: "Bạn xác nhận giao đơn hàng này ?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: `Xác nhận`,
            cancelButtonText: `Hủy`,
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/Shipper/Order/ShipperComfirmOrder',
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

function shipperConfirmDelivered(orderId) {
    if (orderId === '') {
        showError("Không tồn tại đơn hàng này");
    }
    else
        Swal.fire({
            title: 'Xác nhận đã giao đơn hàng!',
            text: "Bạn xác nhận đã giao đơn hàng này ?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: `Xác nhận`,
            cancelButtonText: `Hủy`,
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/Shipper/Order/ShipperComfirmDelivery',
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