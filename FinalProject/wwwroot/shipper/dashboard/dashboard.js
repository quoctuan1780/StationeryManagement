"use strict";
connection.on("orderWaitForPick", function () {
    loadOrderWaitForPickData();
});

connection.on("orderWaitToDelivery", function () {
    loadOrderWaitToDeliveryData();
});

connection.on("orderDelivering", function () {
    loadOrderDeliveringData();
});

connection.on("orderDelivered", function () {
    loadOrderDeliveredData();
});

connection.on("orderDeliveringOrDelivered", function () {
    loadOrderDeliveringOrDeliveredData();
});

loadOrderWaitForPickData();
loadOrderDeliveringData();
loadOrderDeliveredData();
loadOrderWaitToDeliveryData();
loadOrderDeliveringOrDeliveredData();



function loadOrderWaitForPickData() {
    $.ajax({
        url: '/Shipper/Home/GetOrderWaitForPick',
        method: 'GET',
        async: true,
        success: function (data) {
            $("#order-wait-for-pick").text(data);
        },
        error: function (err) {
            console.log(err);
        }
    })
}

function loadOrderDeliveringData() {
    $.ajax({
        url: '/Shipper/Home/GetOrderDelivering',
        method: 'GET',
        async: true,
        success: function (data) {
            $("#order-delivering-pick").text(data);
        },
        error: function (err) {
            console.log(err);
        }
    })
}

function loadOrderDeliveredData() {
    $.ajax({
        url: '/Shipper/Home/GetOrderDelivered',
        method: 'GET',
        async: true,
        success: function (data) {
            $("#order-delivered-pick").text(data);
        },
        error: function (err) {
            console.log(err);
        }
    })
}

function loadOrderWaitToDeliveryData() {
    $.ajax({
        url: '/Shipper/Home/GetOrderWaitToConfirmDelivery',
        method: 'GET',
        async: true,
        success: function (data) {
            $("#order-waiting-delivery-pick").text(data);
        },
        error: function (err) {
            console.log(err);
        }
    })
}

function loadOrderDeliveringOrDeliveredData() {
    $.ajax({
        url: '/Shipper/Home/GetOrderDeliveringOrDelivered',
        method: 'GET',
        async: true,
        success: function (data) {
            var json = JSON.parse(data);
            var chart = new CanvasJS.Chart("pie-chart", {
                animationEnabled: true,
                title: {
                    text: "Đơn hàng đang giao / đã giao"
                },
                data: [{
                    type: "pie",
                    startAngle: 240,
                    yValueFormatString: "##0.00\"%\"",
                    indexLabel: "{label} {y}",
                    dataPoints: json
                }]
            });
            chart.render();
        },
        error: function (err) {
            console.log(err);
        }
    })
}