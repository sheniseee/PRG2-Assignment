//========================================================== 
// Student Number : S10273890
// Student Name : Shenise Lim Em Qing 
// Partner Name : Chloe Heng Chi Xuan
//========================================================== 

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

        public List<Menu> Menus
        {
            get; set;
        }

        public Queue<Order> OrderQueue
        {
            get; set;
        }

        public List<SpecialOffer> SpecialOffers
        {
            get; set;
        }

        public Restaurant(string restaurantId, string restaurantName, string restaurantEmail)
        {
            RestaurantId = restaurantID;
            RestaurantName = restaurantName;
            RestaurantEmail = restauarantEmail;

            Menus = new List<Menu>();
            OrderQueue = new Queue<Order>();
            SpecialOffers = new List<SpecialOffer>();
        }

        public void DisplayOrders()
        {
            if (orders.Count == 0)
            {
                Console.WriteLine("No orders available.");
                return;
            }

            Console.WriteLine("Orders:");
            foreach (var order in OrderQueue)
            {
                Console.WriteLine($"Order ID: {order.OrderID}");
                Console.WriteLine($"Customer: {order.OrderDateTime}");
                Console.WriteLine($"Total: {order.OrderTotal:C}");
                Console.WriteLine($"Status: {order.OrderStatus}");
                Console.WriteLine($"Delivery Date/Time: {order.DeliveryDateTime}");
                Console.WriteLine($"Address: {order.DeliveryAddress}");
                Console.WriteLine($"Payment Method: {order.OrderPaymentMethod}");
                Console.WriteLine($"Paid: {order.OrderPaid}");
            }
        }

        public void DisplaySpecialOffers()
        {
            if (specialOffers.Count == 0)
            {
                Console.WriteLine("No special offers available.");
                return;
            }
            else
            {
                Console.WriteLine("Special Offers:");
                foreach (var offer in specialOffers)
                {
                    Console.WriteLine($"Offer Code: {offer.OfferCode}");
                    Console.WriteLine($"Description: {offer.OfferDesc}");
                    Console.WriteLine($"Discount: {offer.DiscountAmount}%");
                }
            }
        }

        public void DisplayMenu()
        {
            if (menus.Count == 0)
            {
                Console.WriteLine("No menu available.");
                return;
            }

            Console.WriteLine("Menu Items:");
            foreach (var menu in menus)
            {
                Console.WriteLine($"Menu ID: {menu.MenuId}");
                Console.WriteLine($"Menu Name: {menu.MenuName}");
                Console.WriteLine("Food Items:");
                foreach (var item in menu.FoodItems)
                {
                    Console.WriteLine($"- {item.FoodItemName}, {item.Description}, Price: {item.Price:C2}");
                }
            }

        public void AddMenu(Menu menu)
        {
            if (menu == null)
            {
                Console.WriteLine("Menu cannot be null.");
                return;
            }
            else
            {
                menus.Add(menu);
                Console.WriteLine($"Menu '{menu.MenuName} added successfully.");
            }

        }

        public bool RemoveMenu(Menu menu)
        {
            if (menu == null)
            {
                Console.WriteLine("Menu cannot be null.");
                return false;
            }

            bool result = menus.Remove(menu);
            if (result)
            {
                Console.WriteLine($"Menu '{menu.MenuName}' removed successfully.");
                return true;
            }
            else
            {
                Console.WriteLine($"Menu '{menu.MenuName}' not found.");
                return false;
            }
        }

        public override string ToString()
        {
            return $"Restaurant ID: {RestaurantId}, Name: {RestaurantName}, Email: {RestaurantEmail}";
        }
    }
}