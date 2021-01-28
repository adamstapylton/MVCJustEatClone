$(document).delegate('#addToOrderButton', 'click', function () {
    var dishItem = {
        quantity: parseInt($('input[name = "Quantity"]').val()),
        restaurantId: parseInt($('input[name = "RestaurantId"]').val()),
        dish: {
            dishId: parseInt($('input[name = "Dish.DishId"]').val()),
            dishName: $('input[name = "Dish.DishName"]').val(),
            price: parseFloat($('input[name = "Dish.Price"]').val())
        }
    }

    console.log(dishItem);

    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: 'json',
        data: JSON.stringify(dishItem),
        url: '/Api/order/',
        success: function (data) {
            console.log(data);
            GetOrderSummary(data);
        },
        error: function (data) {
            console.log(data);
        }
    });
});

$(document).delegate('.remove-item-button', 'click', function () {
    var orderId = $(this).attr('data-orderId');
    var orderItemId = $(this).attr('data-orderItemId');
    $.ajax({
        type: 'DELETE',
        contentType: 'application/json; charset=UTF-8',
        dataType: 'json',
        url: `/api/order/${orderId}/${orderItemId}`,
        success: function (data) {
            GetOrderSummary(orderId);
        },
        error: function (data) {
            console.log(data);
        }
    });
});