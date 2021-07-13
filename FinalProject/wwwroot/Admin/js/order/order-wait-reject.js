function filter() {
    var customer = $('#customer').val();
    var paymentMethod = $('#paymentmethod').val();
    var url = '/Admin/Order/OrderWaitReject?paymentMethod=' + paymentMethod +
        '&customer=' + customer;

    window.location.href = encodeURI(url);
}