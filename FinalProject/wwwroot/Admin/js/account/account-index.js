function Delete(id) {
    Swal.fire({
        title: 'Xóa tài khoản!',
        text: "Bạn có muốn xóa tài khoản này không ?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: `Xóa`,
        cancelButtonText: `Hủy`,
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Admin/Account/Delete',
                method: 'DELETE',
                async: true,
                data: { id: id },
                success: function (data) {
                    if (data === 1) {
                        showSuccess('Xóa tài khoản thành công');
                        return undefined;
                    }

                    if (data === 3) {
                        showError('Xóa tài khoản không thành công');
                        return undefined;
                    }
                },
                error: function (code, err) {
                    showErrorSystem();
                }
            })
        }
    })
}