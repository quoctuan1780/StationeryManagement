$(document).ready(function () {
    $('.selectpicker').selectpicker({
        liveSearch: true,
        showSubtext: true,
        Size: 10
    });
});

function filter() {
    var customer = $('#customer').val();
    var receivedDate = $('#date-input').val();
    var url = '/Shipper/Order/OrderDelivered?receivedDate=' + receivedDate + '&customer=' + customer;

    window.location.href = encodeURI(url);
}