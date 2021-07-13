function filter() {
    var customer = $('#customer').val();
    var address = $('#address').val();
    var pickedOrderDate = $('#date-input').val();

    var url = '/Shipper/Order/OrderWaitDelivery?customer=' + customer +
        '&pickedOrderDate=' + pickedOrderDate + '&address=' + address;

    window.location.href = encodeURI(url);
}