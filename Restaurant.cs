namespace PRG2_Assignment
{
    public class Restaurant
    {
        private string restaurantId;
        public string RestaurantId
        {
            get { return restaurantId; }
            set { restaurantId = value; }
        }

        private string restaurantName;
        public string RestaurantName
        {
            get { return restaurantName; }
            set { restaurantName = value; }
        }

        private string restaurantEmail;
        public string RestaurantEmail
        {
            get { return restaurantEmail; }
            set { restaurantEmail = value; }
        }

        private List<Menu> menus;
        public List<Menu> Menus
        {
            get { return menus; }
            set { menus = value; }
        }

        public Restaurant(string restaurantId, string restaurantName, string restaurantEmail)
        {
            RestaurantId = restaurantId;
            RestaurantName = restaurantName;
            RestaurantEmail = restaurantEmail;
            menus = new List<Menu>();
        }

        public void DisplayOrders()
        {
            foreach (var order in orders)
            {
                Console.WriteLine(order.ToString());
            }
        }

        public void DisplaySpecialOffers()
        {
            foreach (var offer in specialOffers)
            {
                Console.WriteLine(offer.ToString());
            }
        }

        public void DisplayMenu()
        {
            foreach (var menu in menus)
            {
                Console.WriteLine(menu.ToString());
            }
        }

        public void AddMenu(Menu)
        {
            menus.Add(Menu);

        }

        public bool RemoveMenu(Menu menu)
        {
            menus.Remove(menu);
            return true;
        }

        public override string ToString()
        {
            return $"Restaurant ID: {RestaurantId}, Name: {RestaurantName}, Email: {RestaurantEmail}";
        }
    }
}