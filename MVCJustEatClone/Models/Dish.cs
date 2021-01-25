namespace MVCJustEatClone.Models
{
    public class Dish
    {
        public int DishId { get; set; }
        public string DishName { get; set; }
        public MenuCategory Category { get; set; }
        public double Price { get; set; }
    }
}