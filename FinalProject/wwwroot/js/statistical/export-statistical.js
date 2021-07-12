//region revenue by year
function getRevenueByYear() {
    var startDate = $("#start-date").val();
    var endDate = $("#end-date").val();
    if (startDate.trim() === '' || endDate.trim() === '') {
        showError("Bạn chưa chọn ngày bắt đầu hoặc ngày kết thúc", "380px");
        return undefined;
    }
    if (startDate > endDate) {
        showError("Năm bắt đầu không được lớn hơn năm kết thúc", "380px");
    }
    else {
        var loading = verticalTextColor();
        loading;
        $.ajax({
            url: "/Admin/Statistical/GetRevenueByYear",
            method: 'GET',
            async: true,
            data: { startDate: parseInt(startDate), endDate: parseInt(endDate) },
            success: function (data) {
                loading.out();
                if (data === '-4') {
                    showError("Năm bắt đầu hoặc năm kết thúc đang trống", "380px");
                }
                else if (data === 'NULL') {
                    showSuccess("Không có kết quả nào");
                    $("#show-table").addClass("hidden-table");
                }
                else {
                    var json = JSON.parse(data);
                    var node = '';
                    var quantityOrder = 0;
                    var totalOrder = 0;
                    var quantityOrderPaid = 0;
                    var totalOrderPaid = 0;
                    var quantityOrderRejected = 0;
                    var totalOrderRejected = 0;
                    var revenue = 0;
                    $.each(json, function (k, v) {
                        node +=
                            `<tr><td>` + v.FullName + `</td>
                                     <td>` + v.QuantityOfOrder + `</td>
                                        <td>` + v.TotalOfOrder.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + `</td>
                                        <td>` + v.QuantityOfOrderPaid + `</td>
                                        <td>` + v.TotalOfOrderPaid.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + `</td>
                                        <td>` + v.QuantityOfOrderRejected + `</td>
                                        <td>` + v.TotalOfOrderRejected.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + `</td>
                                        <td>` + v.Revenue.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + `</td>
                                    </tr>`;
                        quantityOrder += v.QuantityOfOrder;
                        totalOrder += v.TotalOfOrder;
                        quantityOrderPaid += v.QuantityOfOrderPaid;
                        totalOrderPaid += v.TotalOfOrderPaid;
                        quantityOrderRejected += v.QuantityOfOrder;
                        totalOrderRejected += v.TotalOfOrderRejected;
                        revenue += v.Revenue;
                    });

                    var nodeTotalDetail = `<h6><b>Tổng số đơn hàng:</b> <span>` + quantityOrder + `</span></h6>
                            <h6><b>Tổng tiền đơn hàng:</b> <span>`+ totalOrder.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + ` VNĐ</span></h6>
                            <h6><b>Tổng số đơn đã thanh toán:</b> <span>` + quantityOrderPaid + `</span></h6>
                            <h6><b>Tổng tiền đã thanh toán:</b> <span>`+ totalOrderPaid.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + ` VNĐ</span></h6>
                            <h6><b>Tổng số đơn bị hủy:</b> <span>` + quantityOrderRejected + `</span></h6>
                            <h6><b>Tổng tiền hủy hàng:</b> <span>`+ totalOrderRejected.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + ` VNĐ</span></h6>
                            <h6><b>Tổng tiền doanh thu:</b> <span>`+ revenue.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + ` VNĐ</span></h6>`;

                    $("#show-table").removeClass("hidden-table");
                    $("#table-body").children().remove();
                    $("#show-total-detail").children().remove();
                    $("#show-total-detail").append(nodeTotalDetail);
                    $("#table-body").append(node);
                }
            },
            error: function (code, err) {
                loading.out();
                showErrorSystem();
            }
        });
    }
}

function exportExcel() {
    var startDate = $("#start-date").val();
    var endDate = $("#end-date").val();
    if (startDate.trim() === '' || endDate.trim() === '') {
        showError("Bạn chưa chọn ngày bắt đầu hoặc ngày kết thúc", "380px");
        return undefined;
    }
    if (startDate > endDate) {
        showError("Năm bắt đầu không được lớn hơn năm kết thúc", "380px");
    }
    else {
        var loading = verticalTextColor();
        loading;
        $.ajax({
            url: "/Admin/Statistical/ExportExcel",
            method: 'GET',
            async: true,
            data: { startDate: parseInt(startDate), endDate: parseInt(endDate) },
            success: function (data) {
                loading.out();
                switch (data) {
                    case "-4":
                        showError('Ngày bắt đầu hoặc ngày kết thúc là trống', '400px');
                        break;
                    case "-99":
                        showErrorSystem();
                        break;
                    case "1":
                        showSuccess("Tệp đã được lưu vào thư mục downloads", "360px");
                        break;
                }
            },
            error: function (code, err) {
                loading.out();
                showErrorSystem();
            }
        });
    }
}
//end


//region revenue by month
function getRevenueByMonth() {
    var startDate = "01-" + $("#start-date").val();
    var endDate = "01-" + $("#end-date").val();
    var startDateJs = new Date(startDate);
    var endDateJs = new Date(endDate);
    if ($("#start-date").val().trim() === '' || $("#end-date").val().trim() === '') {
        showError("Bạn chưa chọn ngày bắt đầu hoặc ngày kết thúc", "380px");
        return undefined;
    }
    if (startDateJs > endDateJs) {
        showError("Thời gian bắt đầu không được lớn hơn thời gian kết thúc", "420px");
    }
    else {
        var loading = verticalTextColor();
        loading;
        $.ajax({
            url: "/Admin/Statistical/GetRevenueByMonth",
            method: 'GET',
            async: true,
            data: { startDate: startDate, endDate: endDate },
            success: function (data) {
                loading.out();
                if (data === '-4') {
                    showError("Thời gian bắt đầu hoặc thời gian kết thúc trống", "420px");
                }
                else if (data === 'NULL') {
                    showSuccess("Không có kết quả nào");
                    $("#show-table").addClass("hidden-table");
                }
                else {
                    var json = JSON.parse(data);
                    var node = '';
                    var quantityOrder = 0;
                    var totalOrder = 0;
                    var quantityOrderPaid = 0;
                    var totalOrderPaid = 0;
                    var quantityOrderRejected = 0;
                    var totalOrderRejected = 0;
                    var revenue = 0;
                    $.each(json, function (k, v) {
                        node +=
                            `<tr><td>` + v.FullName + `</td>
                                         <td>` + v.QuantityOfOrder + `</td>
                                            <td>` + v.TotalOfOrder.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + `</td>
                                            <td>` + v.QuantityOfOrderPaid + `</td>
                                            <td>` + v.TotalOfOrderPaid.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + `</td>
                                            <td>` + v.QuantityOfOrderRejected + `</td>
                                            <td>` + v.TotalOfOrderRejected.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + `</td>
                                            <td>` + v.Revenue.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + `</td>
                                        </tr>`;
                        quantityOrder += v.QuantityOfOrder;
                        totalOrder += v.TotalOfOrder;
                        quantityOrderPaid += v.QuantityOfOrderPaid;
                        totalOrderPaid += v.TotalOfOrderPaid;
                        quantityOrderRejected += v.QuantityOfOrder;
                        totalOrderRejected += v.TotalOfOrderRejected;
                        revenue += v.Revenue;
                    });

                    var nodeTotalDetail = `<h6><b>Tổng số đơn hàng:</b> <span>` + quantityOrder + `</span></h6>
                                <h6><b>Tổng tiền đơn hàng:</b> <span>`+ totalOrder.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + ` VNĐ</span></h6>
                                <h6><b>Tổng số đơn đã thanh toán:</b> <span>` + quantityOrderPaid + `</span></h6>
                                <h6><b>Tổng tiền đã thanh toán:</b> <span>`+ totalOrderPaid.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + ` VNĐ</span></h6>
                                <h6><b>Tổng số đơn bị hủy:</b> <span>` + quantityOrderRejected + `</span></h6>
                                <h6><b>Tổng tiền hủy hàng:</b> <span>`+ totalOrderRejected.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + ` VNĐ</span></h6>
                                <h6><b>Tổng tiền doanh thu:</b> <span>`+ revenue.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + ` VNĐ</span></h6>`;

                    $("#show-table").removeClass("hidden-table");
                    $("#table-body").children().remove();
                    $("#show-total-detail").children().remove();
                    $("#show-total-detail").append(nodeTotalDetail);
                    $("#table-body").append(node);
                }
            },
            error: function (code, err) {
                loading.out();
                showErrorSystem();
            }
        });
    }
}

function exportExcelByMonth() {
    var startDate = "01-" + $("#start-date").val();
    var endDate = "01-" + $("#end-date").val();
    var startDateJs = new Date(startDate);
    var endDateJs = new Date(endDate);
    if ($("#start-date").val().trim() === '' || $("#end-date").val().trim() === '') {
        showError("Bạn chưa chọn ngày bắt đầu hoặc ngày kết thúc", "380px");
        return undefined;
    }
    if (startDateJs > endDateJs) {
        showError("Thời gian bắt đầu không được lớn hơn thời gian kết thúc", "420px");
    }
    else {
        var loading = verticalTextColor();
        loading;
        $.ajax({
            url: "/Admin/Statistical/ExportExcelByMonth",
            method: 'GET',
            async: true,
            data: { startDate: startDate, endDate: endDate },
            success: function (data) {
                loading.out();
                switch (data) {
                    case "-4":
                        showError('Ngày bắt đầu hoặc ngày kết thúc là trống', '400px');
                        break;
                    case "-99":
                        showErrorSystem();
                        break;
                    case "1":
                        showSuccess("Tệp đã được lưu vào thư mục downloads", "360px");
                        break;
                }
            },
            error: function (code, err) {
                loading.out();
                showErrorSystem();
            }
        });
    }
}
//end

//region revenue by customer
function getRevenueByCustomer() {
    var startDate = $("#start-date").val();
    var endDate = $("#end-date").val();
    var customer = $("#customer").val();
    var startDateJs = new Date(startDate);
    var endDateJs = new Date(endDate);

    if (startDate.trim() === '' || endDate.trim() === '') {
        showError("Bạn chưa chọn ngày bắt đầu hoặc ngày kết thúc", "380px");
        return undefined;
    }

    if (startDateJs > endDateJs) {
        showError("Thời gian bắt đầu không được lớn hơn thời gian kết thúc", "420px");
    }
    else {
        var loading = verticalTextColor();
        loading;
        $.ajax({
            url: "/Admin/Statistical/GetRevenueByCustomer",
            method: 'GET',
            async: true,
            data: { startDate: startDate, endDate: endDate, customer: customer },
            success: function (data) {
                loading.out();
                if (data === '-4') {
                    showError("Thời gian bắt đầu hoặc thời gian kết thúc trống", "420px");
                }
                else if (data === 'NULL') {
                    showSuccess("Không có kết quả nào");
                    $("#show-table").addClass("hidden-table");
                }
                else {
                    var json = JSON.parse(data);
                    var node = '';
                    var quantityOrder = 0;
                    var totalOrder = 0;
                    var quantityOrderPaid = 0;
                    var totalOrderPaid = 0;
                    var quantityOrderRejected = 0;
                    var totalOrderRejected = 0;
                    var revenue = 0;
                    $.each(json, function (k, v) {
                        node +=
                            `<tr><td>` + v.FullName + `</td>
                                         <td>` + v.QuantityOfOrder + `</td>
                                            <td>` + v.TotalOfOrder.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + `</td>
                                            <td>` + v.QuantityOfOrderPaid + `</td>
                                            <td>` + v.TotalOfOrderPaid.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + `</td>
                                            <td>` + v.QuantityOfOrderRejected + `</td>
                                            <td>` + v.TotalOfOrderRejected.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + `</td>
                                            <td>` + v.Revenue.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + `</td>
                                        </tr>`;
                        quantityOrder += v.QuantityOfOrder;
                        totalOrder += v.TotalOfOrder;
                        quantityOrderPaid += v.QuantityOfOrderPaid;
                        totalOrderPaid += v.TotalOfOrderPaid;
                        quantityOrderRejected += v.QuantityOfOrder;
                        totalOrderRejected += v.TotalOfOrderRejected;
                        revenue += v.Revenue;
                    });

                    var nodeTotalDetail = `<h6><b>Tổng số đơn hàng:</b> <span>` + quantityOrder + `</span></h6>
                                <h6><b>Tổng tiền đơn hàng:</b> <span>`+ totalOrder.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + ` VNĐ</span></h6>
                                <h6><b>Tổng số đơn đã thanh toán:</b> <span>` + quantityOrderPaid + `</span></h6>
                                <h6><b>Tổng tiền đã thanh toán:</b> <span>`+ totalOrderPaid.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + ` VNĐ</span></h6>
                                <h6><b>Tổng số đơn bị hủy:</b> <span>` + quantityOrderRejected + `</span></h6>
                                <h6><b>Tổng tiền hủy hàng:</b> <span>`+ totalOrderRejected.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + ` VNĐ</span></h6>
                                <h6><b>Tổng tiền doanh thu:</b> <span>`+ revenue.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + ` VNĐ</span></h6>`;

                    $("#show-table").removeClass("hidden-table");
                    $("#table-body").children().remove();
                    $("#show-total-detail").children().remove();
                    $("#show-total-detail").append(nodeTotalDetail);
                    $("#table-body").append(node);
                }
            },
            error: function (code, err) {
                loading.out();
                showErrorSystem();
            }
        });
    }
}

function exportExcelByCustomer() {
    var startDate = $("#start-date").val();
    var endDate = $("#end-date").val();
    var startDateJs = new Date(startDate);
    var endDateJs = new Date(endDate);
    var customer = $("#customer").val();

    if (startDate.trim() === '' || endDate.trim() === '') {
        showError("Bạn chưa chọn ngày bắt đầu hoặc ngày kết thúc", "380px");
        return undefined;
    }

    if (startDateJs > endDateJs) {
        showError("Thời gian bắt đầu không được lớn hơn thời gian kết thúc", "420px");
    }
    else {
        var loading = verticalTextColor();
        loading;
        $.ajax({
            url: "/Admin/Statistical/ExportExcelByCustomer",
            method: 'GET',
            async: true,
            data: { startDate: startDate, endDate: endDate, customer: customer },
            success: function (data) {
                loading.out();
                switch (data) {
                    case "-4":
                        showError('Ngày bắt đầu hoặc ngày kết thúc là trống', '400px');
                        break;
                    case "-99":
                        showErrorSystem();
                        break;
                    case "1":
                        showSuccess("Tệp đã được lưu vào thư mục downloads", "360px");
                        break;
                }
            },
            error: function (code, err) {
                loading.out();
                showErrorSystem();
            }
        });
    }
}
//end

//region revenue by product

//end