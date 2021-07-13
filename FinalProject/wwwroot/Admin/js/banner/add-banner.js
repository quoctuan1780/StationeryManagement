var files = [];

function previewImage(e) {
    var node = '';

    var imageFiles = e.target.files;

    if (imageFiles.length > 0) {
        files.push(imageFiles[0].name);
        node += `<div class="show-image">
                                <img src="`+ URL.createObjectURL(imageFiles[0]) + `" class="image-product" style="width: 100%; height: 60%;" />
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

            files = [];

            $(e).closest('.show-image').remove();

            $('#image-preview-deleted').val(files);
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
    }
    else if (startDate.getTime() > endDate.getTime()) {
        Swal.fire({
            title: 'Thông báo',
            text: "Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc",
            icon: 'info',
            confirmButtonColor: '#3085d6',
            confirmButtonText: `Ok`,
        });

    }
    else {
        $('#form').submit();
    }
}