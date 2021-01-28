$('.item-quantity-input').change(function CheckQuantity() {
    var quantity = $(this).val();
    var button = $(this).closest('form').find('.add-to-order-button');
    if (quantity == 0 || quantity == null) {
        $(button).prop('disabled', 'true');
    } else {
        $(button).removeAttr('disabled');
    }
});

$('.item-quantity-input').keyup(function CheckQuantity() {
    var quantity = $(this).val();
    var button = $(this).closest('form').find('.add-to-order-button');
    if (quantity == 0 || quantity == null) {
        $(button).prop('disabled', 'true');
    } else {
        $(button).removeAttr('disabled');
    }
});