function filter() {
    var customer = $('#customer').val();
    var warehouse = $('#warehouse').val();
    var exportWarehouseDate = document.getElementsByName('exportWarehouseDate')[0].value;

    var url = '/Admin/Order/OrderWaitToPick?exportWarehouseDate=' + exportWarehouseDate +
        '&warehouse=' + warehouse +
        '&customer=' + customer;

    window.location.href = encodeURI(url);
}