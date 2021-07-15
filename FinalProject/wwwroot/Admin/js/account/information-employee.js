function confirmLockOrUnlockAccount(id, isLocked) {
    if (id === '' || isLocked === '') {
        showErrorSystem();
        return undefined;
    }

    Swal.fire({
        title: 'Khóa tài khoản!',
        text: "Bạn có muốn khóa tài khoản này không ?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: `Khóa`,
        cancelButtonText: `Hủy`,
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Admin/Account/LockAccount',
                method: 'PUT',
                async: true,
                data: { id: id, isLocked, isLocked },
                success: function (data) {
                    $('#show-button').children().remove();
                    if (isLocked) {
                        showSuccess('Khóa tài khoản thành công');
                        var node = `<button class="btn btn-success" onclick="confirmLockOrUnlockAccount('` + id + `', false)">Mở khóa tài khoản</button>`
                        $('#show-button').append(node);
                    } else {
                        showSuccess('Mở khóa tài khoản thành công');
                        var node = `<button class="btn btn-primary" onclick="confirmLockOrUnlockAccount('` + id + `', true)">Khóa tài khoản</button>`
                        $('#show-button').append(node);
                    }
                },
                error: function (code, err) {
                    showErrorSystem();
                }
            })
        }
    })
}