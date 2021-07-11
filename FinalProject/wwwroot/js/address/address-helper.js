function getWards() {
    var loading = verticalTextColor();

    loading;

    var districtId = $("#district-id").val();

    var node = '<select class="selectpicker show-tick form-control" name="WardCode" id="ward-id"'
        + 'data-live-search="true"><option value="null">Phường/Xã</option>';

    $($('#street-name').children()).val('');

    if (districtId === 'null') {
        node += '</select>';
        $('#ward').children().remove();
        $('#ward').append(node);

        $('#ward').children().selectpicker({
            liveSearch: true,
            showSubtext: true,
            Size: 10
        });
    }
    else

        $.ajax({
            url: '/Shipper/Account/Ward',
            method: 'GET',
            async: true,
            data: { districtId: districtId },
            success: function (data) {
                for (let i of JSON.parse(data)) {
                    node += '<option value="' + i.WardCode + '">' + i.WardName + '</option>'
                }
                node += '</select>';

                $('#ward').children().remove();
                $('#ward').append(node);

                $('#ward').children().selectpicker({
                    liveSearch: true,
                    showSubtext: true,
                    Size: 10
                });
            }
        })
    loading.out();
}

function getDistricts() {

    var loading = verticalTextColor();

    loading;

    var provinceId = $('#province').val();

    var node = '<select class="selectpicker show-tick form-control" name="DistrictId" id="district-id"' + 'onchange="getWards()"'
        + 'data-live-search="true"><option value="null">Quận/Huyện</option>';

    var nodeWard = '<select name="WardCode" id="ward-id"' +
        'class="selectpicker show-tick form-control" data - live - search="true" >' +
        '<option value="null">Phường/Xã</option</select>';
    '<option value="null">Phường/Xã</option</select>';

    $('#ward').children().remove();
    $('#ward').append(nodeWard);

    $('#ward').children().selectpicker({
        liveSearch: true,
        showSubtext: true,
        Size: 10
    });

    $($('#street-name').children()).val('');

    if (provinceId === 'null') {
        node += '</select>';

        $('#district').children().remove();
        $('#district').append(node);


        $('#district').children().selectpicker({
            liveSearch: true,
            showSubtext: true,
            Size: 10
        });
    }
    else
        $.ajax({
            url: '/Shipper/Account/District',
            method: 'GET',
            async: true,
            data: { provinceId: provinceId },
            success: function (data) {
                for (let i of JSON.parse(data)) {
                    node += '<option value="' + i.DistrictId + '">' + i.DistrictName + '</option>'
                }

                node += '</select>';

                $('#district').children().remove();
                $('#district').append(node);

                $('#district').children().selectpicker({
                    liveSearch: true,
                    showSubtext: true,
                    Size: 10
                });
            }
        });
    loading.out();
}

function clearStreetName() {
    $($('#street-name').children()).val('');
}