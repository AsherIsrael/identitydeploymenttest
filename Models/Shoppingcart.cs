namespace TheWall.Models
{
    public class Shoppingcart
    {
        public int ShoppingcartId { get; set; }

        public int TestUserId { get; set; }

        public TestUser TestUser { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}