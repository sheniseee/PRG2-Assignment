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

            foreach (var item in OrderedFoodItems)
            {
                total += item.TotalPrice * OrderedFoodItems.Quantity;
            }

            total += 5.00; // Delivery fee
            return total;
        }

        public void AddOrderedFoodItem(OrderedFoodItem foodItem)
        {
            orderedFoodItems.Add(foodItem)
            OrderTotal = CalculateOrderTotal(); //recalculate total after adding item

        }

        public bool RemoveOrderedFoodItem(OrderedFoodItem foodItem)
        {
            orderedFoodItems.Remove(foodItem)
            OrderTotal = CalculateOrderTotal(); 
            return true;
        }

        public void DisplayOrderedFoodItems()
        {
            Console.WriteLine("Ordered Food Items:");
            foreach (var foodItem in OrderedFoodItems)
            {
                Console.WriteLine(item.ToString());
            }
        }

        public override string ToString()
        {
            return $"Order ID: {OrderID}, DateTime: {OrderDateTime}, Total: {OrderTotal}, Status: {OrderStatus}, Delivery DateTime: {DeliveryDateTime}, Address: {DeliveryAddress}, Payment Method: {OrderPaymentMethod}, Paid: {OrderPaid}";

        }
}
        