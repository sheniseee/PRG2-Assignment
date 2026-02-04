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

        public List<Order> Orders
        {
            get; set;
        }

        public List<SpecialOffer> SpecialOffers
        {
            get; set;
        }

        public Restaurant(string restaurantId, string restaurantName, string restaurantEmail)
        {
            RestaurantId = restaurantId;
            RestaurantName = restaurantName;
            RestaurantEmail = restaurantEmail;

            Menus = new List<Menu>();
            OrderQueue = new Queue<Order>();
            Orders = new List<Order>();
            SpecialOffers = new List<SpecialOffer>();
        }

        public void DisplayOrders()
        {
            if (OrderQueue.Count == 0)
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
            if (SpecialOffers.Count == 0)
            {
                Console.WriteLine("No special offers available.");
                return;
            }
            else
            {
                Console.WriteLine("Special Offers:");
                foreach (var offer in SpecialOffers)
                {
                    Console.WriteLine($"Offer Code: {offer.OfferCode}");
                    Console.WriteLine($"Description: {offer.OfferDesc}");
                    Console.WriteLine($"Discount: {offer.Discount}%");
                }
            }
        }

        public void DisplayMenu()
        {
            if (Menus.Count == 0)
            {
                Console.WriteLine("No menu available.");
                return;
            }

            Console.WriteLine("Menu Items:");
            foreach (var menu in Menus)
            {
                Console.WriteLine($"Menu ID: {menu.MenuId}");
                Console.WriteLine($"Menu Name: {menu.MenuName}");
                Console.WriteLine("Food Items:");
                foreach (var item in menu.FoodItems)
                {
                    Console.WriteLine($"- {item.ItemName}, {item.ItemDesc}, Price: {item.ItemPrice:C2}");
                }
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
                Menus.Add(menu);
                Console.WriteLine($"Menu '{menu.MenuName}' added successfully.");
            }

        }

        public bool RemoveMenu(Menu menu)
        {
            if (menu == null)
            {
                Console.WriteLine("Menu cannot be null.");
                return false;
            }

            bool result = Menus.Remove(menu);
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