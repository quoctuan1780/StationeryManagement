function filter() {
    var customer = $('#customer').val();
    var url = '/Admin/Order/OrderWaitExportWarehouse?customer=' + customer;

    window.location.href = encodeURI(url);
}