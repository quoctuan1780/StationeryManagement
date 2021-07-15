
$(document).ready(function () {
    $('.selectpicker').selectpicker({
        liveSearch: true,
        showSubtext: true,
        Size: 10
    });
    });

    function getRevenueByProvince() {
        var startDate = $("#start-date").val();
        var endDate = $("#end-date").val();
        var startDateJs = new Date(startDate);
        var endDateJs = new Date(endDate);
        var province = $("#province").val();
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
    url: "/Admin/Statistical/GetRevenueByProvince",
                method: 'GET',
                async: true,
                data: {startDate: startDate, endDate: endDate, province: province },
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
    `<tr><td>` + v.Province + `</td>
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

    function exportExcelByProvince() {
        var startDate = $("#start-date").val();
        var endDate = $("#end-date").val();
        var startDateJs = new Date(startDate);
        var endDateJs = new Date(endDate);
        var province = $("#province").val();
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
    url: "/Admin/Statistical/ExportExcelByProvince",
                method: 'GET',
                async: true,
                data: {startDate: startDate, endDate: endDate, province: province },
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
