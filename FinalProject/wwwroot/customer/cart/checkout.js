$(document).ready(function () {
    $('#left-nav').remove();
    $('#banner').remove();
    $('#cart').remove();

    $(".table-wrap").each(function () {
        var nmtTable = $(this);
        var nmtHeadRow = nmtTable.find("thead tr");
        nmtTable.find("tbody tr").each(function () {
            var curRow = $(this);
            for (var i = 0; i < curRow.find("td").length; i++) {
                var rowSelector = "td:eq(" + i + ")";
                var headSelector = "th:eq(" + i + ")";
                curRow.find(rowSelector).attr('data-title', nmtHeadRow.find(headSelector).text());
            }
        });
    });
});

function validateValue(e) {

    setInputFilter(e, function (value) {
        return /^\d*$/.test(value) && ((value === "" || parseInt(value) > 0) && parseInt(value) <= parseInt($(e).attr('max')));
    });
}

function decrease(e, productDetailId, maxQuantity, price) {
    var node = $(e).parent().children('.input-quantity').get(0);

    var value = parseInt($(node).val());

    if (value > 1) {
        value -= 1;

        $(node).val(value.toString());

        setPrice(e, productDetailId, maxQuantity, price);
    }
}

function ascending(e, productDetailId, maxQuantity, price) {
    var node = $(e).parent().children('.input-quantity').get(0);

    var quantity = $(node).val();

    var value = parseInt(quantity);

    var max = parseInt($(node).attr('max'))

    if (value < max)
        value += 1;
    $(node).val(value)

    setPrice(e, productDetailId, maxQuantity, price);
}

function setPrice(e, productDetailId, maxQuantity, price) {
    var loading = verticalTextColor();

    loading

    var node = $(e).parent().children('.input-quantity').get(0);

    var quantity = $(node).val();

    if (quantity < 0 || quantity > maxQuantity) {
        loading.out();

        showError("Số lượng bạn nhập nhỏ hơn 0 hoặc lớn hơn tổng sản phẩm sẵn có!", "500px");
    }
    else
        $.ajax({
            url: "/Cart/ChangeQuantityCartItem",
            method: "PUT",
            async: true,
            data: { productDetailId: productDetailId, quantity: quantity },
            success: function (data) {
                loading.out();

                switch (parseFloat(data)) {
                    case -99:
                        showError('Lỗi hệ thống vui lòng tải lại trang!');
                        break;
                    case -4:
                        showError('Sản phẩm hoặc số lượng bạn nhập không hợp lệ!', '450px');
                        break;
                    default:
                        $("#total").text(data);
                        var total = price * quantity;
                        $($(e).parent().parent().children('.total-item').get(0)).text(total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + " VNĐ");
                        break;
                }
            },
            error: function (code, err) {
                loading.out();

                showErrorSystem();
            }
        })

}

function removeCartItem(e, productDetailId, price, quantity) {
    Swal.fire({
        title: 'Bạn có muốn xóa sản phẩm này không?',
        showDenyButton: true,
        confirmButtonText: `Xóa`,
        denyButtonText: `Hủy`,
    }).then((result) => {
        if (result.isConfirmed) {
            var loading = verticalTextColor();

            loading;

            $.ajax({
                url: '/Cart/RemoveCartItem',
                method: 'DELETE',
                async: true,
                data: { productDetailId: productDetailId },
                success: function (data) {
                    loading.out();

                    switch (data) {
                        case "Success":
                            showSuccess('Xóa sản phẩm thành công');

                            var total = parseFloat($("#total").text()) - parseFloat(price) * quantity

                            $(e).parent().parent().remove();

                            $("#total").text(total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + " VNĐ");

                            break;
                        case "Fail":
                            showErrorSystem();
                            break;
                        case "Miss":
                            showError('Bạn chưa đăng nhập vào hệ thống');
                            break;
                    }
                },
                error: function (code, err) {
                    loading.out();
                    showErrorSystem()
                }
            });
        }
    });
}