var files = [];

var table = $('#tableSale').DataTable();

var productDetailIds = [];

function checkCheckbox() {
    table.search('').draw();

    productDetailIds = [];

    var rows = table.rows({ 'search': 'applied' }).nodes();

    for (let i = 0; i < rows.length; i++) {
        if ($('input[type="checkbox"]', rows[i]).is(":checked")) {
            productDetailIds.push($('input[type="checkbox"]', rows[i]).val());
        }
    }
}

function ShowProduct() {
    $("#table-product").removeClass("hidden");
    $("#from-order-price").addClass("hidden");
}

function ShowOrder() {
    $("#from-order-price").removeClass("hidden");
    $("#table-product").addClass("hidden");
}

function previewImage(e) {
    var node = '';

    var imageFiles = e.target.files;

    if (imageFiles.length > 0) {
        files.push(imageFiles[0].name);
        node += `<div class="show-image">
                                        <img src="`+ URL.createObjectURL(imageFiles[0]) + `" class="image-product" width="200" height="200" />
                                        <i class="delete fa fa-times" onclick="confirmDeleteImage(this)">
                                        </i>
                                    </div>`;

        $('#show-image-preview').children().remove();
        $('#show-image-preview').append(node);
        $('#image-preview-deleted').val('');
    }
}

function confirmDeleteImage(e) {
    Swal.fire({
        title: 'Xóa hình ảnh!',
        text: "Bạn có muốn xóa hình ảnh này ?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: `Xóa`,
        cancelButtonText: `Hủy`,
    }).then((result) => {
        if (result.isConfirmed) {
            $(e).closest('.show-image').remove();

            $('#image-preview-deleted').val(files);

            files = [];
        }
    })
}

function getDateTimeNow() {
    var dateObj = new Date();
    var month = dateObj.getUTCMonth() + 1;
    var day = dateObj.getUTCDate();
    var year = dateObj.getUTCFullYear
    newdate = day + "/" + month + "/" + year;

    return newdate;
}

function submit() {
    var startDate = new Date($("#start-date").val());
    var endDate = new Date($("#end-date").val());
    var today = new Date(getDateTimeNow());

    if (today.get > startDate.getTime()) {
        Swal.fire({
            title: 'Thông báo',
            text: "Ngày bắt đầu phải lớn hơn hoặc bằng ngày hiện tại",
            icon: 'info',
            confirmButtonColor: '#3085d6',
            confirmButtonText: `Ok`,
        });

        return undefined;
    }
    if (startDate.getTime() > endDate.getTime()) {
        Swal.fire({
            title: 'Thông báo',
            text: "Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc",
            icon: 'info',
            confirmButtonColor: '#3085d6',
            confirmButtonText: `Ok`,
        });

        return undefined;
    }
    checkCheckbox();
    if (productDetailIds.length === undefined) {
        Swal.fire({
            title: 'Thông báo',
            text: "Bạn chưa chọn sản phẩm nào",
            icon: 'info',
            confirmButtonColor: '#3085d6',
            confirmButtonText: `Ok`,
        });

        return undefined;
    }

    $('#product-id-input').val(productDetailIds);

    $('#form').submit();
}