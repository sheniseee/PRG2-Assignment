using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        List<Restaurant> restaurants = new List<Restaurant>();
        List<Customer> customers = new List<Customer>();
        List<Order> orders = new List<Order>();

        // Calls for feature 2 (Loading of customers and orders files)
        LoadCustomersAndOrders(customers, orders);

        // Calls for feature 3

        // Calls for feature 5

        // Calls for feature 7 

    }

    //========================================================== =
    // Student Number : S10272203K
    // Student Name : Chloe Heng Chi Xuan
    // Partner Name : Shenise Lim Em Qing 
    //========================================================== =
    // Feature 2
    static void LoadCustomersAndOrders(List<Customer> customers, List<Order> orders)
    {
        // Customers
        string[] customerLines = File.ReadAllLines("customers.csv");
        for (int i = 1; i < customerLines.Length; i++)
        {
            string[] parts = customerLines[i].Split(',');
            if (parts.Length < 2) continue;

            string name = parts[0].Trim();
            string email = parts[1].Trim();

            Customer c = new Customer(name, email);
            customers.Add(c);

        }

        // Orders
        string[] orderLines = File.ReadAllLines("orders.csv");
        for (int i = 1; i < orderLines.Length; i++)
        {
            string[] parts = orderLines[i].Split(',');
            if (parts.Length < 10) continue;

            int orderId = Convert.ToInt32(parts[0]);
            string custEmail = parts[1];
            string restId = parts[2];
            string deliveryDate = parts[3];
            string deliveryTime = parts[4];
            string deliveryAddress = parts[5];
            string createdDateTime = parts[6];
            double totalAmount = double.Parse(parts[7]);
            string status = parts[8];
            string items = parts[9];

            DateTime orderDateTime = Convert.ToDateTime(createdDateTime);
            DateTime deliveryDateTime = Convert.ToDateTime(deliveryDate + " " + deliveryTime);

            Order o = new Order(orderId, orderDateTime, totalAmount, status, deliveryDateTime, deliveryAddress, "", false);
            orders.Add(o);

        }
    }

    //========================================================== =
    // Student Number : S10272203K
    // Student Name : Chloe Heng Chi Xuan
    // Partner Name : Shenise Lim Em Qing 
    //========================================================== =
    // Feature 3
    static void Feature3_ListAllRestaurantsAndMenuItems(List<Restaurant> restaurants)
    {
        Console.WriteLine("\nAll Restaurants and Menu Items");
        Console.WriteLine("==============================");

        for (int i = 0; i < restaurants.Count; i++)
        {
            Restaurant r = restaurants[i];
            Console.WriteLine($"Restaurant: {r.RestaurantName} ({r.RestaurantId})");

            for (int m = 0; m < r.Menus.Count; m++)
            {
                Menu menu = r.Menus[m];

                for (int f = 0; f < menu.FoodItems.Count; f++)
                {
                    FoodItem item = menu.FoodItems[f];

                    Console.WriteLine($"- {item.ItemName}: {item.ItemDesc} - ${item.ItemPrice:F2}");
                }
            }
        }
    }

    //========================================================== =
    // Student Number : S10272203K
    // Student Name : Chloe Heng Chi Xuan
    // Partner Name : Shenise Lim Em Qing 
    //========================================================== =
    //Feature 5
    static void Feature5_CreateNewOrder(List<Customer> customers, List<Restaurant> restaurants, List<Order> orders)
    {
        Console.WriteLine("\nCreate New Order");
        Console.WriteLine("=======================");

        Console.Write("Enter customer email: ");
        string custEmail = Console.ReadLine().Trim();

        Console.Write("Enter restaurant ID: ");
        string restaurantId = Console.ReadLine().Trim();

        Console.Write("Enter Delivery Date (dd/mm/yyyy): ");
        string deliveryDate = Console.ReadLine().Trim();

        Console.Write("Enter Delivery Time (hh:mm): ");
        string deliveryTime = Console.ReadLine().Trim();

        Console.Write("Enter delivery address: ");
        string deliveryAddress = Console.ReadLine().Trim();

        // This is to verify if customer email exists ^^
        Customer customer = null;
        for (int i = 0; i < customers.Count; i++)
        {
            if (customers[i].Email.ToLower() == custEmail.ToLower())
            {
                customer = customers[i];
                break;
            }
        }
        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }

        // This is to verify if restaurant ID exists ^^
        Restaurant restaurant = null;
        for (int i = 0; i < restaurants.Count; i++)
        {
            if (restaurants[i].RestaurantId.ToLower() == restaurantId.ToLower())
            {
                restaurant = restaurants[i];
                break;
            }
        }
        if (restaurant == null)
        {
            Console.WriteLine("Restaurant not found.");
            return;
        }

        // This is the display of food items ^^
        List<FoodItem> availableFood = new List<FoodItem>();

        for (int m = 0; m < restaurant.Menus.Count; m++)
        {
            for (int f = 0; f < restaurant.Menus[m].FoodItems.Count; f++)
            {
                availableFood.Add(restaurant.Menus[m].FoodItems[f]);
            }
        }

        if (availableFood.Count == 0)
        {
            Console.WriteLine("No food items available.");
            return;
        }

        Console.WriteLine("Available Food Items:");
        for (int i = 0; i < availableFood.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {availableFood[i].ItemName} - ${availableFood[i].ItemPrice:F2}");
        }

        // This is the selection of multiple items + quantity ^^
        List<OrderedFoodItem> selectedItems = new List<OrderedFoodItem>();

        while (true)
        {
            Console.Write("Enter item number (0 to finish): ");
            int itemNo = Convert.ToInt32(Console.ReadLine());

            if (itemNo == 0)
                break;

            if (itemNo < 1 || itemNo > availableFood.Count)
            {
                Console.WriteLine("Invalid item number.");
                continue;
            }

            Console.Write("Enter quantity: ");
            int qty = Convert.ToInt32(Console.ReadLine());

            if (qty <= 0)
            {
                Console.WriteLine("Quantity must be more than 0.");
                continue;
            }

            FoodItem chosen = availableFood[itemNo - 1];

            OrderedFoodItem ordered = new OrderedFoodItem(chosen.ItemName, qty);
            selectedItems.Add(ordered);
        }

        if (selectedItems.Count == 0)
        {
            Console.WriteLine("No items selected. Order cancelled.");
            return;
        }

        // This is to req for special req 
        Console.Write("Add special request? [Y/N]: ");
        string specialAns = Console.ReadLine().Trim().ToUpper();

        string specialRequest = "";
        if (specialAns == "Y")
        {
            Console.Write("Enter special request: ");
            specialRequest = Console.ReadLine().Trim();
        }

        // This is to create the order total ^^
        double itemsTotal = 0;

        for (int i = 0; i < selectedItems.Count; i++)
        {
            string orderedName = selectedItems[i].FoodItemName;
            int qty = selectedItems[i].Quantity;

            double price = 0;
            for (int j = 0; j < availableFood.Count; j++)
            {
                if (availableFood[j].ItemName == orderedName)
                {
                    price = availableFood[j].ItemPrice;
                    break;
                }
            }

            itemsTotal += price * qty;
        }

        double deliveryFee = 5.00;
        double finalTotal = itemsTotal + deliveryFee;

        Console.WriteLine($"Order Total: ${itemsTotal:F2} + ${deliveryFee:F2} (delivery) = ${finalTotal:F2}");

        // This is to Proceed with payment ^^
        Console.Write("Proceed to payment? [Y/N]: ");
        string proceedPay = Console.ReadLine().Trim().ToUpper();

        if (proceedPay != "Y")
        {
            Console.WriteLine("Payment not done. Exit feature.");
            return;
        }

        // This is the payment method ^^
        Console.WriteLine("Payment method:");
        Console.Write("[CC] Credit Card / [PP] PayPal / [CD] Cash on Delivery: ");
        string paymentMethod = Console.ReadLine().Trim().ToUpper();

        if (paymentMethod != "CC" && paymentMethod != "PP" && paymentMethod != "CD")
        {
            Console.WriteLine("Invalid payment method. Exit feature.");
            return;
        }

        // This is to assign a new order Id ^^
        int newOrderId = 1001;

        if (orders.Count > 0)
        {
            int max = orders[0].OrderID;
            for (int i = 1; i < orders.Count; i++)
            {
                if (orders[i].OrderID > max)
                    max = orders[i].OrderID;
            }
            newOrderId = max + 1;
        }

        // This is to create order object ^^
        DateTime createdDateTime = DateTime.Now;
        DateTime deliveryDateTime = Convert.ToDateTime(deliveryDate + " " + deliveryTime);

        Order newOrder = new Order(newOrderId, createdDateTime, finalTotal, "Pending", deliveryDateTime, deliveryAddress, paymentMethod, true);

        for (int i = 0; i < selectedItems.Count; i++) // adding selected items to order
        {
            newOrder.OrderedFoodItems.Add(selectedItems[i]);
        }

        // This is to add order to customer and restaurant ^^
        orders.Add(newOrder);
        customer.Orders.Add(newOrder);
        restaurant.Orders.Add(newOrder);

        // This is to append order to csv ^^
        AppendOrderToCsv(newOrder, custEmail, restaurant.RestaurantId);

        // This is the order confirmmation ^^
        Console.WriteLine($"Order {newOrderId} created successfully! Status: Pending");

    }

    static void AppendOrderToCsv(Order order, string customerEmail, string restaurantId)
    {
        string deliveryDate = order.DeliveryDateTime.ToString("dd/MM/yyyy");
        string deliveryTime = order.DeliveryDateTime.ToString("HH:mm");
        string created = order.OrderDateTime.ToString("dd/MM/yyyy HH:mm");

        // This is to build item column
        string itemsText = "";
        for (int i = 0; i < order.OrderedFoodItems.Count; i++)
        {
            if (i > 0) itemsText += "|";
            itemsText += order.OrderedFoodItems[i].FoodItemName + "," + order.OrderedFoodItems[i].Quantity;

        }

        string line =
            $"{order.OrderID},{customerEmail},{restaurantId},{deliveryDate},{deliveryTime}," +
            $"{order.DeliveryAddress},{created},{order.OrderTotal},{order.OrderStatus},\"{itemsText}\"";

        File.AppendAllText("orders.csv", "\n" + line);
    }

    //========================================================== =
    // Student Number : S10272203K
    // Student Name : Chloe Heng Chi Xuan
    // Partner Name : Shenise Lim Em Qing 
    //========================================================== =
    // Feature 7
    static void ModifyExistingOrder(List<Customer> customers,List<Restaurant> restaurants,List<Order> orders)
    {
        Console.WriteLine("\nModify Existing Order");
        Console.WriteLine("=========================");

        Console.Write("Enter customer email: ");
        string custEmail = Console.ReadLine().Trim();

        // This is to verify and see if the customer exists ^^
        Customer customer = null;

        for (int i = 0; i < customers.Count; i++)
        {
            if (customers[i].Email.ToLower() == custEmail.ToLower())
            {
                customer = customers[i];
                break;
            }
        }

        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }

        // This is to display pending orders^^
        Console.WriteLine("Pending Orders:");
        List<Order> pending = new List<Order>();

        for (int i = 0; i < customer.Orders.Count; i++)
        {
            if (customer.Orders[i].OrderStatus.ToLower() == "pending")
            {
                pending.Add(customer.Orders[i]);
                Console.WriteLine(customer.Orders[i].OrderID);
            }
        }

        if (pending.Count == 0)
        {
            Console.WriteLine("No pending orders.");
            return;
        }

        Console.Write("Enter Order ID: ");
        int orderId = Convert.ToInt32(Console.ReadLine());

        Order target = null;
        for (int i = 0; i < pending.Count; i++)
        {
            if (pending[i].OrderID == orderId)
            {
                target = pending[i];
                break;
            }
        }

        if (target == null)
        {
            Console.WriteLine("Order not found.");
            return;
        }

        // Display current order details ^^

        Console.WriteLine("Order Items:");
        for (int i = 0; i < target.OrderedFoodItems.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {target.OrderedFoodItems[i].FoodItem.ItemName} - {target.OrderedFoodItems[i].QtyOrdered}");
        }

        Console.WriteLine("Address:");
        Console.WriteLine(target.DeliveryAddress);

        Console.WriteLine("Delivery Date/Time:");
        Console.WriteLine(target.DeliveryDateTime.ToString("d/M/yyyy, HH:mm"));

        Console.Write("Modify: [1] Items [2] Address [3] Delivery Time: ");
        string choice = Console.ReadLine().Trim();

        if (choice == "1")
        {
            target.OrderedFoodItems.Clear();

            Console.WriteLine("Re-enter items (0 to finish):");
            while (true)
            {
                Console.Write("Food name (0 to finish): ");
                string name = Console.ReadLine().Trim();
                if (name == "0") break;

                Console.Write("Quantity: ");
                int qty = Convert.ToInt32(Console.ReadLine());

                target.OrderedFoodItems.Add(new OrderedFoodItem(name, qty));
            }
        }
        else if (choice == "2")
        {
            Console.Write("Enter new Address: ");
            target.DeliveryAddress = Console.ReadLine().Trim();
        }
        else if (choice == "3")
        {
            Console.Write("Enter new Delivery Time (hh:mm): ");
            string newTime = Console.ReadLine().Trim();

            DateTime oldDT = target.DeliveryDateTime;
            string datePart = oldDT.ToString("dd/MM/yyyy");
            target.DeliveryDateTime = Convert.ToDateTime(datePart + " " + newTime);
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }

        Console.WriteLine($"Order {target.OrderID} updated.");
        Console.WriteLine($"New Delivery Date/Time: {target.DeliveryDateTime:dd/MM/yyyy HH:mm}");
    
    }
         