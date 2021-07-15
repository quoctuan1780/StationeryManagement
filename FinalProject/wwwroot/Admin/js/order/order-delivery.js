function filter() {
    var shipper = $('#shipper').val();
    var warehouse = $('#warehouse').val();
    var exportWarehouseDate = document.getElementsByName('exportWarehouseDate')[0].value;
    var receivedDeliveryDate = document.getElementsByName('receivedDeliveryDate')[0].value;
    var url = '/Admin/Order/OrderDelivery?exportWarehouseDate=' + exportWarehouseDate +
        '&receivedDeliveryDate=' + receivedDeliveryDate +
        '&warehouse=' + warehouse +
        '&shipper=' + shipper;

    window.location.href = encodeURI(url);
}