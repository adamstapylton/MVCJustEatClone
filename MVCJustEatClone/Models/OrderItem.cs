namespace MVCJustEatClone.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public Dish Dish { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
