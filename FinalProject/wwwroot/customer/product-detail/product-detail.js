var productIdOrder = -1;
var quantity = $('#Quantity').val();
var origin = null;
var color = null;
var price = null;

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

function setSizeQuantity(max, productId, color, origin, price) {
    $("#Quantity").attr({
        "max": max,
        "min": 1
    });

    $('#price-product-detail').val(price);

    $("#Quantity").val(1);

    productIdOrder = productId;

    $('#detail-quantity').text(max + " sản phẩm sẵn có");

    $('#type-choose').text(color);

    document.getElementById('product-detail-origin').textContent = origin;
    document.getElementById('product-detail-price').textContent = price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + " VNĐ";

    this.color = color;
    this.origin = origin;
    this.price = price;
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

function submit(image, productName, isLogin) {

    if (isLogin !== 'True') {
        showError('Bạn chưa đăng nhập vào hệ thống');
        return undefined;
    }

    var price = $('#price-product-detail').val();

    if (price === '' || price === '0') {
        showError('Bạn chưa chọn loại sản phẩm muốn đặt', '380px');
        return undefined;
    }
    
    var loading = verticalTextColor();

    loading;

    if (productIdOrder === -1) {
        loading.out();

        showError('Bạn chưa chọn loại sản phẩm muốn đặt', '380px');
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
                            loading.out();
                            break;
                        case "Updated":
                            $.ajax({
                                url: "/Cart/GetCartTotal",
                                method: "GET",
                                async: false,
                                data: {},
                                success: function (data) {
                                    $("#total-price").text(data.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + " VNĐ");
                                },
                                error: function (jqXHR, exception) {
                                    showError('Lỗi hệ thống không cập nhật được tổng tiền, vui lòng tải lại trang', '480px');
                                }
                            });

                            var productInCart = 'quantityProductDetail' + productIdOrder;

                            document.getElementById(productInCart).textContent = 'Số lượng: ' + quantity;

                            $("#total-item").text($("#cart-sidebar").children().length + " items");

                            loading.out();

                            showSuccess('Cập nhật sản phẩm thành công');

                            break;

                        default:
                            $.ajax({
                                url: "/Cart/GetCartTotal",
                                method: "GET",
                                async: false,
                                data: {},
                                success: function (data) {
                                    $("#total-price").text(data.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + " VNĐ");
                                },
                                error: function (code, err) {
                                    console.error(err);
                                    loading.out();
                                    showErrorSystem();
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
                                'Số lượng: ' + quantity + '</div><div class="d-block">Giá: ' + price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + ' VNĐ</div></p>' +
                                '</div></li>';

                            $("#cart-sidebar").append(node);

                            loading.out();

                            showSuccess('Thêm sản phẩm thành công');

                            $("#total-item").text($("#cart-sidebar").children().length + " items");
                            break;
                    }
                }
                , error: function (code, err) {
                    showError('Lỗi! vui lòng đăng xuất và đăng nhập lại hệ thống', '460px');
                    loading.out();
                    console.error(err);
                }
            });
        }
    }
}


function Rating(isLogin, productId) {
    if (isLogin === 'False') {
        showError('Bạn chưa đăng nhập vào hệ thống');
    }
    else {
        var rating = $("input[type='radio'][name='ratings']:checked").val();
        var ratingContent = $('#rating-content').val();

        if (rating === undefined) {
            showError('Bạn chưa chọn số sao đánh giá');
        }
        else if (ratingContent === '') {
            showError('Bạn chưa nhập nội dung đánh giá');
        }
        else {
            var loading = verticalTextColor();
            loading;
            $.ajax({
                url: '/Rating/AddRating',
                method: 'POST',
                async: true,
                data: { productId: productId, content: ratingContent, ratingId: rating },
                success: function (data) {
                    loading.out();
                    switch (data) {
                        case -4:
                            showError('Không thêm được đánh giá này');
                            break;
                        case -5:
                            showError('Bạn đã đánh giá sản phẩm này rồi');
                            break;
                        case 1:
                            showSuccess('Thêm đánh giá thành công');
                            break;
                        case -99:
                            showErrorSystem();
                            break;
                    }
                },
                error: function (code, err) {
                    loading.out();
                    showErrorSystem();
                }
            });
        }
    }
}

function getMoreRating(productId) {
    var skip = $('#skip-product').val();
    if (skip === '') {
        showError('Lỗi không tải được thêm, vui lòng thử lại sau');
    }
    else {
        $.ajax({
            url: '/Rating/GetMoreRating',
            method: 'GET',
            async: true,
            data: { skip: skip, productId: productId },
            success: function (data) {
                switch (data) {
                    case "-4":
                        showError('Lỗi không tải thêm được, vui lòng tải lại trang', '360px');
                        break;
                    case "-99":
                        showErrorSystem();
                        break;
                    default:
                        var json = JSON.parse(data);
                        var node = ``;
                        $.each(json, function (k, v) {
                            node +=
                                `<li class="clearfix">
                                            <div class="comment-block">`;
                            if (v.UserImage != null) {
                                node += `<img src="/images/user/` + v.Email + `/` + v.UserImage + `" class="avatar" alt="">`;
                            }
                            else {
                                node += `<img src="/images/user/avatar.png" class="avatar" alt="">`;
                            }
                            node += `
                                                <div class="post-comments">
                                                    <p class="meta"> ` +
                                v.RatingDate + ` <a href="javascript:void(0)">` + v.FullName + `</a>
                                                    </p>
                                                    <div class="meta" style="display: inline-block">
                                                        <span style="float: left">Đánh giá:</span>
                                                        <div class="rating-box"><span class="rating nobr" style="width:`+ v.Rating + `%;"></span></div>
                                                    </div>
                                                    <p>`+
                                v.Content + `
                                                    </p>
                                                </div>
                                            </div>
                                        </li>`;
                        });
                        $('#rating').append(node);
                        skip += 10;
                        $('#skip-product').val(skip);
                        break;
                }
            },
            error: function (core, err) {
                showErrorSystem();
            }
        });
    }
}