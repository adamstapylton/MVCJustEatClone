$('.dish-item-card').click(function () {
    var dishId = $(this).attr('data-id');
    $.ajax({
        type: 'GET',
        contentType: 'application/json',
        dataType: 'html',
        data: { dishId: dishId },
        url: '/Restaurant/GetDishModal',
        success: function (data) {
            $('#dishModalContent').html(data);
        },
        error: function (data) {
            console.log(data);
        }
    });
});



function GetOrderSummary(orderId) {
    console.log('working');
    $.ajax({
        type: 'GET',
        contentType: 'application/json',
        dataType: 'html',
        data: { orderId: orderId },
        url: '/Restaurant/OrderPartialView',
        success: function (data) {
            $('#orderSummaryContainer').html(data);
            closeModal('#dishItemModal');
        },
        error: function (data) {
            console.log(data);
        }
    });
}