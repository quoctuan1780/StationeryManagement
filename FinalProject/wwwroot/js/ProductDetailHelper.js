var Origins = document.getElementsByName('Origins');
var Weights = document.getElementsByName('Weights');
var Colors = document.getElementsByName('Colors');

function addProductDetail() {
    var node =
        '<div class="row" style="padding-top: .5rem;"><div class= "col-lg-3" >' +
        '<input type="text" name="Origins" class="form-control" /></div ><div class="col-lg-3">' +
        '<input type="number" placeholder="KG" name="Weights" class="form-control"/></div>' +
        '<div class="col-lg-3">' +
        '<input type="text" name="Colors" class="form-control"/>' +
        '</div><div class="col-lg-3 text-center align-items-center">' +
        '<input type="hidden" name="Quantities" value="0" />' +
        '<button type="button" class="btn btn-danger" onclick="removeProductDetail(this)">' +
        '<i class="fa fa-minus"></i></button></div></div> ';

    $('#product-detail').append(node);
}

function validateProductDetail() {
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

        if (Colors[i].value === '') {
            var node = Colors[i];
            var text = '<span class="field-validation-valid text-danger validate-product-detail">' +
                'Màu sắc không được để trống</span>';
            countErrorValidate++;
            $(node).parent().append(text);
        }

    }
    return countErrorValidate;
}

function setInputFilter(textbox, inputFilter) {
    ["input", "keydown", "keyup", "mousedown", "mouseup", "select", "contextmenu", "drop"].forEach(function (event) {
        textbox.addEventListener(event, function () {
            if (inputFilter(this.value)) {
                this.oldValue = this.value;
                this.oldSelectionStart = this.selectionStart;
                this.oldSelectionEnd = this.selectionEnd;
            } else if (this.hasOwnProperty("oldValue")) {
                this.value = this.oldValue;
                this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
            } else {
                this.value = "";
            }
        });
    });
}