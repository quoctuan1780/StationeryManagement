function getRevenueByProduct() {
    var startDate = $("#start-date").val();
    var endDate = $("#end-date").val();
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
            url: "/Admin/Statistical/GetRevenueByProduct",
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
                    var revenue = 0;
                    $.each(json, function (k, v) {
                        node +=
                            `<tr><td>` + v.ProductName + `</td>
                                         <td>` + v.Color + `</td>
                                        <td>` + v.QuantityRemaining + `</td>
                                        <td>` + v.QuantitySold + `</td>
                                        <td>` + v.TotalOfPrice.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + ` VNĐ</td>
                                        </tr>`;
                        revenue += v.TotalOfPrice;
                    });

                    var nodeTotalDetail = `
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

function exportExcelByProduct() {
    var startDate = $("#start-date").val();
    var endDate = $("#end-date").val();
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
            url: "/Admin/Statistical/ExportExcelByProduct",
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