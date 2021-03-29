﻿function addProductDetail() {
    var node =
        '<div class="row" style="padding-top: .5rem;"><div class= "col-lg-2" >' +
        '<input type="text" name="Origins" class="form-control" /></div ><div class="col-lg-2">' +
        '<input type="number" placeholder="KG" name="Weights" class="form-control"/></div>' +
        '<div class="col-lg-3">' +
        '<div style="display: flex; align-items:center;">' +
        '<input type="number" style="width: 32.33%" min="1" name="Widths" class="form-control" placeholder="Dài(mm)" />' +
        'x<input type="number" style="width: 32.33%" min="1" name="Lengths" class="form-control" placeholder="Rộng(mm)" />' +
        'x<input type="number" style="width: 32.33%" min="1" name="Heights" class="form-control" placeholder="Cao(mm)" />' +
        '</div></div><div class="col-lg-2">' +
        '<input type="text" name="Colors" class="form-control"/>' +
        '</div><div class="col-lg-1 text-center align-items-center">' +
        '<button type="button" class="btn btn-danger" onclick="removeProductDetail(this)">' +
        '<i class="fa fa-minus"></i></button></div></div> ';

    $('#product-detail').append(node);
}

function validateProductDetail() {
    var Origins = document.getElementsByName('Origins');
    var Weights = document.getElementsByName('Weights');
    var Widths = document.getElementsByName('Widths');
    var Lengths = document.getElementsByName('Lengths');
    var Heights = document.getElementsByName('Heights');
    var Colors = document.getElementsByName('Colors');
    var countErrorValidate = 0;

    $('.validate-product-detail').remove();

    for (let i = 0; i < Origins.length; i++) {
        if (Origins[i].value === '') {
            var node = Origins[i];
            var text = '<span class="field-validation-valid text-danger validate-product-detail">' +
                'Tên nhà sản xuất không được để trống</span>';
            countErrorValidate++;
            $(node).parent().append(text);
        }

        if (Weights[i].value === '') {
            var node = Weights[i];
            var text = '<span class="field-validation-valid text-danger validate-product-detail">' +
                'Trọng lượng không được để trống</span>';
            countErrorValidate++;
            $(node).parent().append(text);
        }

        if (Widths[i].value === '' || Lengths[i].value === '' || Heights[i].value === '') {
            var node = Widths[i];
            var text = '<span class="field-validation-valid text-danger validate-product-detail">' +
                'Kích thước không được để trống</span>';
            countErrorValidate++;
            $(node).parent().parent().append(text);
        }

        if (Colors[i].value === '') {
            var node = Colors[i];
            var text = '<span class="field-validation-valid text-danger validate-product-detail">' +
                'Màu sắc không được để trống</span>';
            countErrorValidate++;
            $(node).parent().append(text);
        }

        return countErrorValidate;
    }
}