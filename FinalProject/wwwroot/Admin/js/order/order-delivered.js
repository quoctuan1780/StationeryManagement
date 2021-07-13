$(document).ready(function () {
    $('.selectpicker').selectpicker({
        liveSearch: true,
        showSubtext: true,
        Size: 10
    });
});

function filter() {
    var shipper = $('#shipper').val();
    var customer = $('#customer').val();
    var receivedDate = $('#received-date').val();
    var url = '/Admin/Order/OrderDelivered?receivedDate=' + receivedDate + '&customer=' + customer +
        '&shipper=' + shipper;

    window.location.href = encodeURI(url);
}