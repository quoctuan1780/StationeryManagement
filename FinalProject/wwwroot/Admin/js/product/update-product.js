CKEDITOR.replace("description", { htmlEncodeOutput: true, enterMode: CKEDITOR.ENTER_DIV });

var imageRemove = [];
var productDetail = [];
var files = [];
var fileDeleted = [];

function previewImage(e) {
    var node = '';

    var imageFiles = e.target.files;

    if (imageFiles.length > 0) {
        for (let i = 0; i < imageFiles.length; i++) {
            files.push(imageFiles[i].name);
            node += `<div class="show-image">
                                <img src="`+ URL.createObjectURL(imageFiles[i]) + `" class="image-product" width="150" height="150" />
                                <i class="delete fa fa-times" onclick="confirmDeletePreviewImage(this, `+ i + `)">
                                </i>
                            </div>`;
        }

        $('#show-image-preview').children().remove();
        $('#show-image-preview').append(node);
        $('#image-preview-deleted').val('');
    }
}

function confirmDeletePreviewImage(e, pos) {
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

            fileDeleted.push(files[pos]);

            $(e).closest('.show-image').remove();

            $('#image-preview-deleted').val(fileDeleted);
        }
    })
}

function confirmDeleteImage(image, id) {
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
            if (imageRemove.indexOf(image) === -1) {

                imageRemove.push(image);

                document.getElementById(id).remove();
            }

            document.getElementById("imageRemove").value = imageRemove;
        }
    })
}

function removeProductDetail(e) {
    var node = $(e).parent();

    productDetail.push($(node.children("input[name=productsDetailId]")).val());

    $(e).parent().parent().remove();

    $('#productsDetailsId').val(productDetail);
}

function submit() {
    Swal.fire({
        title: 'Cập nhật thông tin ?',
        text: "Bạn có muốn cập nhật thông tin sản phẩm không ?",
        icon: 'info',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: `Cập nhật`,
        cancelButtonText: `Hủy`,
    }).then((result) => {
        if (result.isConfirmed) {


            var countErrorValidate = validateProductDetail();

            var Origins = $('input[name=Origins]');

            if (Origins.length === 0) {
                alert('Bạn phải có chi tiết sản phẩm');
            }
            else
                if (countErrorValidate === 0) {
                    $('#form-product').submit();
                }
        }
    })
}