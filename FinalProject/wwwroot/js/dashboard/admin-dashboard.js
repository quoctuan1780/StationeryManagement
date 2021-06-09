"use strict";

$(() => {
    let connection = new signalR.HubConnectionBuilder().withUrl("/signalServer").build();

    connection.start();

    connection.on("newOrder", function () {
        loadData();
    });

    loadData();

    function loadData() {
        $.ajax({
            url: '/Admin/Home/GetOrderQuantity',
            method: 'GET',
            async: true,
            success: function (data) {
                $("#new-order-quantity").text(data);
            },
            error: function (err) {
                console.log(err);
            }
        })
    }
});