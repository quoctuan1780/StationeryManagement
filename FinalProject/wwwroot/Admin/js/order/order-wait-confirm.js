function filter() {
    var customer = $('#customer').val();
    var paymentMethod = $('#paymentmethod').val();
    var orderDate = document.getElementsByName('orderDate')[0].value;

    var url = '/Admin/Order/OrderWaitComfirm?orderDate=' + orderDate +
        '&paymentMethod=' + paymentMethod +
        '&customer=' + customer;

    window.location.href = encodeURI(url);
}