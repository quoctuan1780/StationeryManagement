"use strict";

connection.on("newOrder", function () {
    loadData();
});

connection.on("newWarehouse", function () {
    loadWarehouseWaitData();
});

connection.on("newAccount", function () {
    loadAccountData();
    console.log("new account is called");
});

connection.on("newRevenue", function () {
    loadRevenueData();
});

connection.on("topProduct", function () {
    loadTopProductData();
});

connection.on("topRevenueCurrentMonth", function () {
    loadRevenueCurrentMonthData();
});

connection.on("newOrderWaitReject", function () {
    loadOrderWatiReject();
});

loadData();
loadWarehouseWaitData();
loadAccountData();
loadRevenueData();
loadTopProductData();
loadRevenueCurrentMonthData();
loadOrderWatiReject();

function loadData() {
    $.ajax({
        url: '/Admin/Home/GetOrderQuantity',
        method: 'GET',
        async: true,
        success: function (data) {
            $("#new-order-quantity").text(data);
        },
        error: function (code, err) {
            console.error(err);
        }
    })
}

function loadWarehouseWaitData() {
    $.ajax({
        url: '/Admin/Home/GetWarehouseWait',
        method: 'GET',
        async: true,
        success: function (data) {
            $("#new-warehouse-quantity").text(data);
        },
        error: function (code, err) {
            console.error(err);
        }
    })
}

function loadAccountData() {
    $.ajax({
        url: '/Admin/Home/GetAccount',
        method: 'GET',
        async: true,
        success: function (data) {
            var json = JSON.parse(data);
            $("#new-account-quantity").text(json.month2);
            var curr = json.month2;
            var pre = json.month1;
            if (curr === parseFloat(0)) {
                $("#account-percent").removeClass('text-success');
                $("#account-percent").addClass('text-success');
                $("#account-percent").text(0 + "%")
            } else {
                if (curr === parseFloat(0)) {
                    curr = parseFloat(1);
                }
                if (pre === parseFloat(0)) {
                    pre = parseFloat(1);
                }
                var result = curr / pre * 100;
                if (result - 100 < 0) {
                    result -= 100;
                    $("#account-percent").removeClass('text-danger');
                    $("#account-percent").addClass('text-danger');
                    $("#account-percent").text(result.toFixed(2) + "%")
                }
                else {
                    $("#account-percent").removeClass('text-success');
                    $("#account-percent").addClass('text-success');
                    $("#account-percent").text(" + " + result.toFixed(2) + "%")
                }
            }
        },
        error: function (code, err) {
            console.error(err);
        }
    })
}

function loadRevenueData() {
    $.ajax({
        url: '/Admin/Home/GetRevenue',
        method: 'GET',
        async: true,
        success: function (data) {
            var json = JSON.parse(data);
            $("#revenue-value").text(json.CurrentRevenue + ' VNĐ');
            if (json.CurrentRevenue !== 0) {
                var curr = parseFloat(json.CurrentRevenue);
                var pre = parseFloat(json.PreRevenue);
                if (curr === parseFloat(0)) {
                    curr = parseFloat(1);
                }
                if (pre === parseFloat(0)) {
                    pre = parseFloat(1);
                }
                var result = curr / pre * 100;
                if (result - 100 < 0) {
                    result -= 100;
                    $("#revenue-percent").removeClass('text-danger');
                    $("#revenue-percent").addClass('text-danger');
                    $("#revenue-percent").text(result.toFixed(2) + "%")
                }
                else {
                    $("#revenue-percent").removeClass('text-success');
                    $("#revenue-percent").addClass('text-success');
                    $("#revenue-percent").text(" + " + result.toFixed(2) + "%")
                }
            }
            else {
                $("#revenue-percent").removeClass('text-success');
                $("#revenue-percent").addClass('text-success');
                $("#revenue-percent").text(0 + "%");
            }
        },
        error: function (code, err) {
            console.error(err);
        }
    })
}

function loadTopProductData() {
    $.ajax({
        url: '/Admin/Home/GetTopProduct',
        method: 'GET',
        async: true,
        success: function (data) {
            var json = JSON.parse(data);
            var chart = new CanvasJS.Chart("top-product-chart", {
                theme: "light2", // "light1", "light2", "dark1", "dark2"
                exportEnabled: true,
                animationEnabled: true,
                title: {
                    text: "Các sản phẩm có doanh thu cao"
                },
                data: [{
                    type: "pie",
                    startAngle: 25,
                    toolTipContent: "<b>{label}</b>: {y}%",
                    showInLegend: "true",
                    legendText: "{label}",
                    indexLabelFontSize: 16,
                    indexLabel: "{label}  {y}%",
                    dataPoints: json
                }]
            });
            chart.render();
        },
        error: function (code, err) {
            console.error(err);
        }
    })
}

function loadRevenueCurrentMonthData() {
    $.ajax({
        url: '/Admin/Home/GetRevenueCurrentMonth',
        method: 'GET',
        async: true,
        success: function (data) {
            var json = JSON.parse(data);
            $.each(json, function (k, v) {
                v.x = new Date(v.x);
            });
            var chart = new CanvasJS.Chart("chart-revenue-current-month", {
                animationEnabled: true,
                exportEnabled: true,
                title: {
                    text: "Doanh thu tháng"
                },
                axisX: {
                    valueFormatString: "DD MMM",
                    crosshair: {
                        enabled: true,
                        snapToDataPoint: true
                    }
                },
                axisY: {
                    title: "Doanh thu (VNĐ)",
                    valueFormatString: "##0",
                    crosshair: {
                        enabled: true,
                        snapToDataPoint: true,
                        labelFormatter: function (e) {
                            return "$" + CanvasJS.formatNumber(e.value, "##0.00");
                        }
                    }
                },
                data: [{
                    type: "area",
                    xValueFormatString: "DD MMM",
                    yValueFormatString: "$##0.00",
                    dataPoints: json
                }]
            });
            chart.render();
        },
        error: function (code, err) {
            console.error(err);
        }
    })
}

function loadOrderWatiReject() {
    $.ajax({
        url: '/Admin/Home/GetOrderWatiReject',
        method: 'GET',
        async: true,
        success: function (data) {
            $('#order-wait-reject').text(data);
        },
        error: function (code, err) {
            console.error(err);
        }
    })
}