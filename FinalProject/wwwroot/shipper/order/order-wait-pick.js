var chooseDelivery = [];
        var rowVersions = [];

        function chooseOrder(e, rowVersion) {
            var dom = $(e);

            if (dom[0].checked) {
                chooseDelivery.push(dom.val());
                rowVersions.push(rowVersion);
            }
            else {
                chooseDelivery.splice(chooseDelivery.indexOf(dom.val()), 1);
                rowVersions.splice(rowVersion.indexOf(rowVersion), 1);
            }

            console.log(chooseDelivery)
        }

        function selectAllOrder() {
            $('input[name=delivery-choose]').prop('checked', true);

            var checkbox = $('input[name=delivery-choose]');

            var rowVersion = $('input[name=row-version]');

            chooseDelivery = [];

            for (let item of checkbox) {
                chooseDelivery.push(item.value);
            }

            for (let item of rowVersion) {
                rowVersions.push(item.value);
            }
        }

        function unselectAllOrder() {
            chooseDelivery = [];
            rowVersions = [];
            $('input[name=delivery-choose]').prop('checked', false);
            console.log(chooseDelivery)
        }

        function confirmOrder() {
            if (chooseDelivery.length < 1) {
                showError("Bạn chưa chọn đơn hàng nào");
            }
            else {
                var loading = verticalTextColor();
                loading;
                $.ajax({
                    url: '/Shipper/Order/ConfirmOrder',
                    method: 'PUT',
                    async: true,
                    data: { ordersPicked: chooseDelivery, rowVersion: rowVersions },
                    success: function (data) {
                        loadingOut(loading, 0);
                        if (data === "-4")
                            showError("Bạn chưa chọn đơn hàng nào");
                        else if (data === "-99")
                            showErrorSystem();
                        else {
                            var json = JSON.parse(data);
                            var icon = 'success';
                            var html = "";

                            if (json.Success !== "") {
                                html += "<p class='alert alert-success'>Các đơn hàng đã nhận thành công có mã là: " + json.Success + "</p>";
                            }

                            if (json.Fail !== "") {
                                icon = 'warning';
                                html += "<p class='alert alert-danger'>Các đơn hàng đã được người khác nhận có mã là: "
                                    + json.Fail + "</p>";
                            }

                            Swal.fire({
                                title: 'Thông báo',
                                html: html,
                                icon: icon,
                                confirmButtonColor: '#3085d6',
                                confirmButtonText: 'Xác nhận',
                                allowOutsideClick: false
                            }).then((result) => {
                                if (result.isConfirmed) {
                                    window.location.href = "/Shipper/Order/OrderWaitDelivery";
                                }
                            })
                        }
                    },
                    error: function (code, err) {
                        console.error(err);
                        loadingOut(loading, 0);
                    }
                });
            }
        }

        function filter() {
            var customer = $('#customer').val();
            var address = $('#address').val();
            var orderDate = $('#date-input').val();

            var url = '/Shipper/Order/OrderWaitPick?customer=' + customer +
                '&orderDate=' + orderDate + '&address=' + address;

            window.location.href = encodeURI(url);
        }