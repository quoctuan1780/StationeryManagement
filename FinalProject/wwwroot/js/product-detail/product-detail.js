var productIdOrder = -1;
var quantity = $('#Quantity').val();
var origin = null;
var color = null;

$(document).ready(function () {
    $('#left-nav').remove();
    $('#banner').remove();

    $('.image-load').imagesLoaded(function () {
        $("#exzoom").exzoom({
            autoPlay: false,
        });
        $("#exzoom").removeClass('hidden-exzoom')
    });

    setInputFilter(document.getElementById("Quantity"), function (value) {
        return /^\d*$/.test(value) && ((value === "" || parseInt(value) > 0) && parseInt(value) <= parseInt($('#Quantity').attr('max')));
    });
});

function setSizeQuantity(max, productId, color, origin) {
    $("#Quantity").attr({
        "max": max,
        "min": 1
    });

    productIdOrder = productId;

    $('#detail-quantity').text(max + " sản phẩm sẵn có");

    $('#type-choose').text(color);

    document.getElementById('product-detail-origin').textContent = origin;

    this.color = color;
    this.origin = origin;
}

function loadingOut(loading) {
    setTimeout(() => loading.out(), 3000);
}

function verticalTextColor() {
    var loading = new Loading({
        title: 'Xin hãy đợi',
        titleColor: 'rgb(217, 83, 79)',
        discription: 'Loading...',
        discriptionColor: 'rgb(77, 150, 223)',
        animationOriginColor: 'rgb(33, 179, 132)',
        mask: true,
        loadingPadding: '20px 50px',
        defaultApply: true,
    });

    return loading;
}

function submit(price, image, productName, isLogin) {

    if (isLogin !== 'True') {
        $.notiny({
            position: 'right-top',
            theme: 'danger',
            template: '<div class="notiny-base"><div class="MuiPaper-root MuiAlert-root MuiAlert-filledError MuiPaper-elevation6"><div class="MuiAlert-icon"><svg class="MuiSvgIcon-root MuiSvgIcon-fontSizeInherit" focusable="false" viewBox="0 0 24 24" aria-hidden="true"><path d="M11 15h2v2h-2zm0-8h2v6h-2zm.99-5C6.47 2 2 6.48 2 12s4.47 10 9.99 10C17.52 22 22 17.52 22 12S17.52 2 11.99 2zM12 20c-4.42 0-8-3.58-8-8s3.58-8 8-8 8 3.58 8 8-3.58 8-8 8z"></path></svg></div><div class="notiny-text MuiAlert-message"></div></div></div>',
            text: 'Bạn chưa đăng nhập vào hệ thống',
        });
    }
    else {
        var loading = verticalTextColor();

        loading;

        if (productIdOrder === -1) {
            loading.out();

            $.notiny({
                position: 'right-top',
                width: '380px',
                theme: 'danger',
                template: '<div class="notiny-base"><div class="MuiPaper-root MuiAlert-root MuiAlert-filledError MuiPaper-elevation6"><div class="MuiAlert-icon"><svg class="MuiSvgIcon-root MuiSvgIcon-fontSizeInherit" focusable="false" viewBox="0 0 24 24" aria-hidden="true"><path d="M11 15h2v2h-2zm0-8h2v6h-2zm.99-5C6.47 2 2 6.48 2 12s4.47 10 9.99 10C17.52 22 22 17.52 22 12S17.52 2 11.99 2zM12 20c-4.42 0-8-3.58-8-8s3.58-8 8-8 8 3.58 8 8-3.58 8-8 8z"></path></svg></div><div class="notiny-text MuiAlert-message"></div></div></div>',
                text: 'Bạn chưa chọn loại sản phẩm muốn đặt',
            });
        }

        else {
            var quantity = $("#Quantity").val();

            if (quantity > 0) {
                $.ajax({
                    url: '/Cart/AddToCart',
                    method: 'POST',
                    async: true,
                    data: { productDetailId: productIdOrder, quantity: quantity, price: price },
                    success: function (data) {
                        switch (data) {
                            case "Miss":
                                alert("Hệ thống xảy ra lỗi, vui lòng thử lại sau");
                                break;
                            case "Updated":
                                $.ajax({
                                    url: "/Cart/GetCartTotal",
                                    method: "GET",
                                    async: false,
                                    data: {},
                                    success: function (data) {
                                        $("#total-price").text(data + " VNĐ");
                                    },
                                    error: function (jqXHR, exception) {
                                        $.notiny({
                                            position: 'right-top',
                                            width: '480px',
                                            theme: 'danger',
                                            template: '<div class="notiny-base"><div class="MuiPaper-root MuiAlert-root MuiAlert-filledError MuiPaper-elevation6"><div class="MuiAlert-icon"><svg class="MuiSvgIcon-root MuiSvgIcon-fontSizeInherit" focusable="false" viewBox="0 0 24 24" aria-hidden="true"><path d="M11 15h2v2h-2zm0-8h2v6h-2zm.99-5C6.47 2 2 6.48 2 12s4.47 10 9.99 10C17.52 22 22 17.52 22 12S17.52 2 11.99 2zM12 20c-4.42 0-8-3.58-8-8s3.58-8 8-8 8 3.58 8 8-3.58 8-8 8z"></path></svg></div><div class="notiny-text MuiAlert-message"></div></div></div>',
                                            text: 'Lỗi hệ thống không cập nhật được tổng tiền, vui lòng tải lại trang',
                                        });
                                    }
                                });

                                var productInCart = 'quantityProductDetail' + productIdOrder;

                                document.getElementById(productInCart).textContent = 'Số lượng: ' + quantity;

                                $("#total-item").text($("#cart-sidebar").children().length + " items");

                                loading.out();

                                $.notiny({
                                    position: 'right-top',
                                    theme: 'success',
                                    template: '<div class="notiny-base"><div class="MuiPaper-root MuiAlert-root MuiAlert-filledSuccess MuiPaper-elevation6"><div class="MuiAlert-icon"><svg class="MuiSvgIcon-root MuiSvgIcon-fontSizeInherit" focusable="false" viewBox="0 0 24 24" aria-hidden="true"><path d="M20,12A8,8 0 0,1 12,20A8,8 0 0,1 4,12A8,8 0 0,1 12,4C12.76,4 13.5,4.11 14.2, 4.31L15.77,2.74C14.61,2.26 13.34,2 12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0, 0 22,12M7.91,10.08L6.5,11.5L11,16L21,6L19.59,4.58L11,13.17L7.91,10.08Z"></path></svg></div><div class="notiny-text MuiAlert-message"></div></div></div>',
                                    text: 'Cập nhật sản phẩm thành công',
                                });

                                break;

                            default:
                                $.ajax({
                                    url: "/Cart/GetCartTotal",
                                    method: "GET",
                                    async: false,
                                    data: {},
                                    success: function (data) {
                                        $("#total-price").text(data + " VNĐ");
                                    }
                                });
                                var cartItem = JSON.parse(data);

                                $("#nullItem").remove();

                                var cartItemPostive = 'quantityProductDetail' + cartItem.ProductDetailId;

                                var node = '<li class="item"><input type="hidden" class="cart" value="' + cartItem.ProductDetailId + '"/>' +
                                    '<a href="/Product/Detail/' + cartItem.ProductDetailId + '" class="product-image">' +
                                    '<img src="/images/product/' + image + '" width="70" height = "70"/></a>' +
                                    '<a href="javascript:void(0)" onclick="removeItem(this)" class="btn-remove">Remove This Item</a>' +
                                    '<div class="product-details">' +
                                    '<p class="product-name"><a href="/Product/Detail/' + cartItem.ProductDetailId + '">' + productName +
                                    '</a><div class="d-block">Phân loại hàng: ' + color + '</div>' +
                                    '<div class="d-block">Xuất xứ: ' + origin + '</div>' +
                                    '<div id="' + cartItemPostive + '" class="d-block">' +
                                    'Số lượng: ' + quantity + '</div><div class="d-block">Giá: ' + price + ' VNĐ</div></p>' +
                                    '</div></li>';

                                $("#cart-sidebar").append(node);

                                loading.out();

                                $.notiny({
                                    position: 'right-top',
                                    theme: 'success',
                                    template: '<div class="notiny-base"><div class="MuiPaper-root MuiAlert-root MuiAlert-filledSuccess MuiPaper-elevation6"><div class="MuiAlert-icon"><svg class="MuiSvgIcon-root MuiSvgIcon-fontSizeInherit" focusable="false" viewBox="0 0 24 24" aria-hidden="true"><path d="M20,12A8,8 0 0,1 12,20A8,8 0 0,1 4,12A8,8 0 0,1 12,4C12.76,4 13.5,4.11 14.2, 4.31L15.77,2.74C14.61,2.26 13.34,2 12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0, 0 22,12M7.91,10.08L6.5,11.5L11,16L21,6L19.59,4.58L11,13.17L7.91,10.08Z"></path></svg></div><div class="notiny-text MuiAlert-message"></div></div></div>',
                                    text: 'Thêm sản phẩm thành công',
                                });

                                $("#total-item").text($("#cart-sidebar").children().length + " items");
                                break;
                        }
                    }
                });
            }
        }
    }
}