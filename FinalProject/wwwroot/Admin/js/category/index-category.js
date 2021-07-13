function deleteCategory(e, categoryId) {
    if (categoryId === '') {
        showError('Không tồn tại loại sản phẩm này');
    }
    else {
        Swal.fire({
            title: 'Xóa loại sản phẩm!',
            text: "Bạn có muốn xóa loại sản phẩm này ?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: `Xóa`,
            cancelButtonText: `Hủy`,
        }).then((result) => {
            if (result.isConfirmed) {
                var loading = verticalTextColor();
                loading;
                $.ajax({
                    url: '/Admin/Category/DeleteCategory',
                    method: 'DELETE',
                    async: true,
                    data: { categoryId: categoryId },
                    success: function (data) {
                        loadingOut(loading, 0);

                        switch (data) {
                            case -4:
                                showError('Loại sản phẩm không tồn tại');
                                break;

                            case 1:
                                showSuccess('Xóa loại sản phẩm thành công');
                                $(e).closest('tr').remove();
                                break;

                            case -99:
                                showErrorSystem();
                                break;
                        }
                    },
                    error: function (code, err) {
                        loadingOut(loading, 0);
                        showErrorSystem();
                    }
                });
            }
        });
    }
}

function repairCategory(e, categoryId) {
    if (categoryId === '') {
        showError('Không tồn tại loại sản phẩm này');
    }
    Swal.fire({
        title: 'Nhập nội dung',
        html: '<textarea class="form-control" rows="5" id="category-input"></textarea>',
        showCancelButton: true,
        confirmButtonText: 'Cập nhật',
        cancelButtonText: 'Hủy'
    }).then((result) => {
        if (result.isConfirmed) {
            var content = $("#category-input").val();
            if (content === '' || content.trim() === '') {
                showError('Bạn chưa nhập tên sản phẩm')
            }
            else
                $.ajax({
                    url: '/Admin/Category/RepairCategory',
                    async: true,
                    method: 'PUT',
                    data: { categoryId: categoryId, content: content },
                    success: function (data) {
                        switch (data) {
                            case -4:
                                showError("Lỗi không tìm thấy loại sản phẩm");
                                break;
                            case 1:
                                $($($(e).closest('tr').get(0)).children('td.category-name').get(0)).text(content)
                                showSuccess('Cập nhật loại sản phẩm thành công');
                                break;
                            case -99:
                                showErrorSystem();
                                break;
                            case 3:
                                showError("Lỗi! Loại sản phẩm đã tồn tại");
                                break;
                        }
                    },
                    error: function (code, err) {
                        showErrorSystem();
                    }
                });
        }
    })
}