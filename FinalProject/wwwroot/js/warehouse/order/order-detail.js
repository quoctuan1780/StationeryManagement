function warehouseManagementConfirm(orderId) {
    if (orderId === '') {
        showError("Không tồn tại đơn hàng này");
    }
    else
        Swal.fire({
            title: 'Xác nhận chuẩn bị hàng xong!',
            text: "Bạn đã chuẩn bị hàng xong cho đơn hàng này ?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: `Đồng ý`,
            cancelButtonText: `Hủy`,
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/Warehouse/Order/WarehouseManagementComfirmOrder',
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

function filter() {
    var customer = $('#customer').val();
    var orderDate = $('#date-input').val();

    var url = '/Warehouse/Order/OrderWaitExportWarehouse?customer=' + customer +
        '&orderDate=' + orderDate;

    window.location.href = encodeURI(url);
}
        //function warehouseManagementPrepareOrder(orderId, productId,e) {
        //    if (orderId === '') {
        //        showError("Không tồn tại đơn hàng này");
        //    }
        //    else
        //        Swal.fire({
        //            title: 'Xác nhận chuẩn bị hàng xong!',
        //            text: "Bạn đã chuẩn bị hàng xong cho đơn hàng này ?",
        //            icon: 'warning',
        //            showCancelButton: true,
        //            confirmButtonColor: '#3085d6',
        //            cancelButtonColor: '#d33',
        //            confirmButtonText: `Đồng ý`,
        //            cancelButtonText: `Hủy`,
        //        }).then((result) => {
        //            if (result.isConfirmed) {
        //                $.ajax({
        //                    url: '/Warehouse/Order/PrepareOrder',
        //                    method: 'PUT',
        //                    async: true,
        //                    data: { OrderId: orderId, ProductDetailId:productId },
        //                    success: function (data) {
        //                        switch (data) {
        //                            case 1:
        //                                $(e).closest('.tr').children('.Status').val("Chờ lấy hàng");
        //                                break;
        //                            case -4:
        //                                showError('Lỗi không tìm thấy đơn hàng này');
        //                                break;
        //                            case -99:
        //                                showErrorSystem();
        //                                break;
        //                        }
        //                    },
        //                    error: function (code, err) {

        //                    }
        //                });
        //            }
        //        })
        //}

        //function warehouseManagementReject(orderId, e) {
        //    if (orderId === '') {
        //        showError("Không tồn tại đơn hàng này");
        //    }
        //    else
        //        Swal.fire({
        //            title: 'Xác nhận từ chối đơn hàng!',
        //            text: "Bạn xác nhận từ chối đơn hàng này ?",
        //            icon: 'warning',
        //            showCancelButton: true,
        //            confirmButtonColor: '#3085d6',
        //            cancelButtonColor: '#d33',
        //            confirmButtonText: `Đồng ý`,
        //            cancelButtonText: `Hủy`,
        //        }).then((result) => {
        //            if (result.isConfirmed) {
        //                $.ajax({
        //                    url: '/Warehouse/Order/RejectOrder',
        //                    method: 'POST',
        //                    async: true,
        //                    data: { id: orderId },
        //                    success: function (data) {
        //                        switch (data) {
        //                            case 1:
        //                                $(e).closest('.tr').children('.Status').val("Đã hủy");
        //                                break;
        //                            case -4:
        //                                showError('Lỗi không tìm thấy đơn hàng này');
        //                                break;
        //                            case -99:
        //                                showErrorSystem();
        //                                break;
        //                        }
        //                    },
        //                    error: function (code, err) {

        //                    }
        //                });
        //            }
        //        })
        //}