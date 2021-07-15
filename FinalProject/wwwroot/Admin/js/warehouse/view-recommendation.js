
$(document).ready(function () {
    $('#best-seller').DataTable();
        $('#buy-together').DataTable();
    });

    function ShowBestSeller() {
        var quantity = $('#quantity').val();
        var fromDate = $('#start-date').val();
        var toDate = $('#end-date').val();
        var listProduct = [];

        if (fromDate === '' || toDate === '') {
    showError("Ngày bắt đầu và ngày kết thúc khồng được để trống", "400px");
            return undefined;
        }

        var fromDateJs = new Date(fromDate);
        var toDateJs = new Date(toDate);

        if (fromDateJs > toDateJs) {
            showError("Ngày bắt đầu không được lớn hơn ngày kết thúc", "400px");
            return undefined;
        }

        if (quantity === '' || parseInt(quantity) <= 0) {
    showError("Số lượng phải lớn hơn 0");
            return undefined;
        }

        $.ajax({
    url: '/Admin/Warehouse/GetBestSeller',
            method: 'GET',
            async: true,
            data: {fromDate: fromDate, toDate: toDate, quantity: quantity },
            success: function (data) {
                if (data === 'null') {
    showSuccess("Không có kết quả nào");
                    return undefined;
                }
                listProduct = JSON.parse(data);
                var str = '';
                for (let item of listProduct) {
                    str += '<tr><td>' + item.productDetailId + '</td>' +
                    '<td>' + item.productName + '</td>' +
                    '<td>' + item.color + '</td>' +
                    '<td>' + item.totalQuantity + '</td>' +
                    '<td>' + item.quantityOrdered + '</td>' +
                    '<td>' + item.RemainingQuantity + '</td></tr>';
                                }
                                $('#BestSeller').html(str);
                                if (listProduct.length > 0) {
                    $.ajax({
                        url: '/Admin/Warehouse/GetRecommandation',
                        method: 'GET',
                        async: true,
                        success: function (data) {
                            if (data === "null") {
                                showSuccess("Không có sản phẩm mua kèm theo", "310px");
                                return undefined;
                            }
                            var listProduct = JSON.parse(data);
                            var str = '';
                            for (let item of listProduct) {
                                str += '<tr><td>' + item.productDetailId + '</td>' +
                                    '<td>' + item.productName + '</td>' +
                                    '<td>' + item.color + '</td>' +
                                    '<td>' + item.totalQuantity + '</td>' +
                                    '<td>' + item.quantityOrdered + '</td>' +
                                    '<td>' + item.RemainingQuantity + '</td></tr>';
                            }
                            $('#Rule').html(str);
                        }
                    })
                }
            }
        });
    }
