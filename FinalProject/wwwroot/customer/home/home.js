function getProducts(e) {
    var $btn = $(e);
    var skip = parseInt($("#skip-product").val());

    $btn.button('loading');

    $.ajax({
        url: '/Product/GetProductSkip',
        method: 'GET',
        async: true,
        data: { skip: skip },
        success: function (data) {
            $btn.button('reset');
            if (data === "-4") {
                showError("Bạn đã xem đến sản phẩm cuối cùng");
            }
            else if (data === "-99") {
                showErrorSystem();
            }
            var json = JSON.parse(data);
            var node = ``;
            $.each(json, function (k, v) {
                node += `<div class="col-lg-3 col-md-6 col-sm-6 col-xs-9">
                            <div class="products-grid">
                                <div class="item">
                                    <div class="item-wrap">
                                        <div class="item-image">
                                            <a class="product-image no-touch" href="#" title="Ipad Air and iOS7">`;
                if (v.ProductImage !== '') {
                    node += `
                                                    <img class="first_image" src="/images/product/`+ v.ProductImage + `"
                                                            alt="Product demo" width="228" height="228" style="width: 100%;" />`;

                }
                else {
                    node += `
                                                    <img class="first_image" src="/images/product/img_1.jpg"
                                                         alt="Product demo" width="228" height="228" style="width: 100%;" />`;
                }
                node += `
                                            </a>
                                            <div class="item-btn">
                                                <div class="box-inner">
                                                    <a title="Add to wishlist" class="link-wishlist">&nbsp;</a>
                                                    <span class="qview">
                                                        <a class="vt_quickview_handler" data-original-title="Quick View"
                                                           data-placement="left" data-toggle="tooltip"
                                                           href="/Product/Detail?id=`+ v.ProductId + `">
                                                            <span>
                                                                Quick
                                                                View
                                                            </span>
                                                        </a>
                                                    </span>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="pro-info">
                                            <div class="pro-inner">
                                                <div class="pro-title product-name">
                                                    <a href="/Product/Detail/`+ v.ProductId + `">
                                                        `+ v.ProductName + `
                                                    </a>
                                                </div>
                                                <div class="pro-content">
                                                    <div class="wrap-price">
                                                        <div class="price-box" style="min-height: 60px">`;
                if (v.SalePrice > 0) {
                    node += `
                                                                <span class="regular-price">
                                                                    <span class="price">`+ v.SalePrice.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + ` VNĐ</span>
                                                                </span>
                                                                <p class="special-price">
                                                                    <span class="price">`+ v.Price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + ` VNĐ</span>
                                                                </p>`;
                }
                else {
                    node += `
                                                                <span class="regular-price">
                                                                    <span class="price">`+ v.Price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + ` VNĐ</span>
                                                                </span>`;
                }
                node += `
                                                        </div>
                                                    </div>
                                                    <div style="height: 110px">`;
                if (v.RatingRange > 0) {
                    node += `
                                                            <span>Đánh giá</span>
                                                            <div class="ratings" style="margin-bottom: .5rem;">
                                                                <div class="rating-box"> 
                                                                    <div class="rating" style="width:`+ v.RatingRange + `%">
                                                                    </div>
                                                                </div>
                                                            </div>`;
                }
                else {
                    node += `
                                                            <p>Chưa có đánh giá</p>`;
                }
                node += `
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <a class="add-to-cart" href="/Product/Detail/`+ v.ProductId + `">
                                            Chi tiết sản phẩm
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>`;
            });
            skip += 16;
            $("#skip-product").val(skip);
            $("#product-item").append(node);
        },
        error: function (code, err) {
            $btn.button('reset');
            console.error(err);
        }
    })
}