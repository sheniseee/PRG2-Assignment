//========================================================== 
// Student Number : S10273890
// Student Name : Shenise Lim Em Qing 
// Partner Name : Chloe Heng Chi Xuan
//========================================================== 

namespace PRG2_Assignment
{
    public class Order
    {
        private int orderID;
        public int OrderID
        {
            get { return orderID; }
            set { orderID = value; }
        }

        private DateTime orderDateTime;
        public DateTime OrderDateTime
        {
            get { return orderDateTime; }
            set { orderDateTime = value; }
        }

        private double orderTotal;
        public double OrderTotal
        {
            get { return orderTotal; }
            set { orderTotal = value; }
        }

        private string orderStatus;
        public string OrderStatus
        {
            get { return orderStatus; }
            set { orderStatus = value; }
        }

        private DateTime deliveryDateTime;
        public DateTime DeliveryDateTime
        {
            get { return deliveryDateTime; }
            set { deliveryDateTime = value; }
        }

        private string deliveryAddress;
        public string DeliveryAddress
        {
            get { return deliveryAddress; }
            set { deliveryAddress = value; }
        }

        private string orderPaymentMethod;
        public string OrderPaymentMethod
        {
            get { return orderPaymentMethod; }
            set { orderPaymentMethod = value; }
        }

        private bool orderPaid;
        public bool OrderPaid
        {
            get { return orderPaid; }
            set { orderPaid = value; }
        }

        private List<OrderedFoodItem> orderedFoodItems;
        public List<OrderedFoodItem> OrderedFoodItems
        {
            get { return orderedFoodItems; }
            set { orderedFoodItems = value; }
        }

        public Order(int orderID, DateTime orderDateTime, double orderTotal, string orderStatus, DateTime deliveryDateTime, string deliveryAddress, string orderPaymentMethod, bool orderPaid)
        {
            OrderID = orderID;
            OrderDateTime = orderDateTime;
            OrderTotal = orderTotal;
            OrderStatus = orderStatus;
            DeliveryDateTime = deliveryDateTime;
            DeliveryAddress = deliveryAddress;
            OrderPaymentMethod = orderPaymentMethod;
            OrderPaid = orderPaid;
            OrderedFoodItems = new List<OrderedFoodItem>();
        }

        public double CalculateOrderTotal()
        {
            double total = 0;

            foreach (OrderedFoodItem item in OrderedFoodItems)
            {
                total += item.CalculateSubtotal();

            }

            total += 5.00; // Delivery fee
            return total;
        }

        public void AddOrderedFoodItem(OrderedFoodItem foodItem)
        {
            if (foodItem != null)
            {
                orderedFoodItems.Add(foodItem);
                OrderTotal = CalculateOrderTotal(); // Recalculate total after adding item
                Console.WriteLine($"Food item '{foodItem.FoodItemName}' added to the order.");
            }
            else
            {
                Console.WriteLine("Cannot add a null food item.");
                return;
            }
        }

        public bool RemoveOrderedFoodItem(OrderedFoodItem foodItem)
        {
            if (foodItem == null)
            {
                Console.WriteLine("Cannot remove a null food item.");
                return false;
            }

            bool result = orderedFoodItems.Remove(foodItem);
            if (result)
            {
                OrderTotal = CalculateOrderTotal(); 
                Console.WriteLine($"Food item '{foodItem.FoodItemName}' removed from the order.");
            }
            else
            {
                Console.WriteLine("Food item not found in the order.");
            }
            return result;
        }

        public void DisplayOrderedFoodItems()
        {
            if (orderedFoodItems.Count == 0)
            {
                Console.WriteLine("No food items in the order.");
                return;
            }
            else
            {
                Console.WriteLine("Ordered Food Items:");
                foreach (var foodItem in OrderedFoodItems)
                {
                    Console.WriteLine($"ID: {foodItem.FoodItemId}, Name: {foodItem.FoodItemName}, Quantity: {foodItem.Quantity}, Total Price: {foodItem.TotalPrice:C}");
                }
            }
        }

        public override string ToString()
        {
            return $"Order ID: {OrderID}, DateTime: {OrderDateTime}, Total: {OrderTotal}, Status: {OrderStatus}, Delivery DateTime: {DeliveryDateTime}, Address: {DeliveryAddress}, Payment Method: {OrderPaymentMethod}, Paid: {OrderPaid}";

        }
    }
}
        