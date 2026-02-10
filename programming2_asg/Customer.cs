//==========================================================
// Student Number : S10272203K
// Student Name : Chloe Heng Chi Xuan
// Partner Name : Shenise Lim Em Qing 
//========================================================== 

using System;

namespace PRG2_Assignment
{
    public class Customer
    {

        private string emailAddress;
        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; }
        }

        private string customerName;
        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; }
        }

        private List<Order> orders;
        public List<Order> Orders
        {
            get { return orders; }
            private set { orders = value; }
        }

        public Customer()
        {
            EmailAddress = "";
            CustomerName = "";
            Orders = new List<Order>();
        }

        public Customer(string emailAddress, string customerName)
        {
            EmailAddress = emailAddress;
            CustomerName = customerName;
            Orders = new List<Order>();
        }

        public void AddOrder(Order order)
        {
            if (order != null)
            {
                Orders.Add(order);
            }
        }

        public bool RemoveOrder(Order order)
        {
            if (order != null)
            {
                return Orders.Remove(order);
            }
            return false;
        }


        public void DisplayOrders()
        {
            Console.WriteLine($"Orders for {CustomerName}:");
            if (Orders.Count == 0)
            {
                Console.WriteLine("No orders found.");
            }
            else
            {
                foreach (Order order in Orders)
                {
                    Console.WriteLine($"Order ID: {order.OrderId}, Total: ${order.OrderTotal:F2}");
                }
            }
        }


        public override string ToString()
        {
            return $"Customer: {CustomerName}\nEmail: {EmailAddress}\nNumber of Orders: {Orders.Count}";
        }
    }
}
