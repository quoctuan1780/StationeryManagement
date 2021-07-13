function filter() {
    var customer = $('#customer').val();
    var exportWarehouseDate = document.getElementsByName('exportWarehouseDate')[0].value;
    var receivedDeliveryDate = document.getElementsByName('receivedDeliveryDate')[0].value;
    var url = '/Shipper/Order/OrderDelivery?exportWarehouseDate=' + exportWarehouseDate +
        '&receivedDeliveryDate=' + receivedDeliveryDate +
        '&customer=' + customer;

    window.location.href = encodeURI(url);
}