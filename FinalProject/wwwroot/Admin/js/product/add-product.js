var files = [];
var fileDeleted = [];

CKEDITOR.replace("description", { htmlEncodeOutput: true, enterMode: CKEDITOR.ENTER_DIV });

function removeProductDetail(e) {
    $(e).parent().parent().remove();
}

function previewImage(e) {
    var node = '';

    var imageFiles = e.target.files;

    if (imageFiles.length > 0) {
        for (let i = 0; i < imageFiles.length; i++) {
            files.push(imageFiles[i].name);
            node += `<div class="show-image">
                                <img src="`+ URL.createObjectURL(imageFiles[i]) + `" class="image-product" width="150" height="150" />
                                <i class="delete fa fa-times" onclick="confirmDeleteImage(this, `+ i + `)">
                                </i>
                            </div>`;
        }

        $('#show-image-preview').children().remove();
        $('#show-image-preview').append(node);
        $('#image-preview-deleted').val('');
    }
}

function confirmDeleteImage(e, pos) {
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

function submit() {

    var countErrorValidate = validateProductDetail();

    if (Origins.length === 0) {
        Swal.fire({
            title: 'Thông báo',
            text: "Bạn phải có chi tiết sản phẩm",
            icon: 'info',
            confirmButtonColor: '#3085d6',
            confirmButtonText: `Ok`,
        });
    }
    else
        if (countErrorValidate === 0) {
            $('#form-product').submit();
        }
}