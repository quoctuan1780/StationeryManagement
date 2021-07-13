function decrease() {
    var value = parseInt($('#Quantity').val());

    if (value > 1) {
        value -= 1;
        document.getElementById("Quantity").value = value;
    }
}

function ascending() {
    var quantity = $('#Quantity');
    var value = parseInt(quantity.val());
    var max = parseInt(quantity.attr('max'))
    if (value < max)
        value += 1;
    document.getElementById("Quantity").value = value;
}

function setQuantity() {
    quantity = parseInt($('#Quantity').val());
}

function checkValue() {
    if ($("#Quantity").val() <= 0)
        $("#Quantity").val(1);
}