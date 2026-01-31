using System;
using System.Collections.Generic;
using System.IO;
using PRG2-Assignment;

class Program
{
    static void Main()
    {
        List<Restaurant> restaurants = new List<Restaurant>();
        List<Customer> customers = new List<Customer>();
        List<Order> orders = new List<Order>();

        //Advanced Feature (b)
        const double DELIVERY_FEE = 5.00;
        const double GRUBEROO_RATE = 0.30;

        while (true)
        {
            Console.WriteLine("===== Gruberoo Food Delivery System ===== ");
            Console.WriteLine("1.List all restaurants and menu items"):
            Console.WriteLine("2.List all orders");
            Console.WriteLine("3.Create a new order");
            Console.WriteLine("4.Process an order");
            Console.WriteLine("5.Modify an existing order");
            Console.WriteLine("6.Delete an existing order");
            Console.WriteLine("7.Display total order amount");
            Console.WriteLine("0.Exit");

            Console.Write("Enter your choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            if (choice == 0)
            {
                break;
            }
            else if (choice == 1)
            {
                Feature3_ListAllRestaurantsAndMenuItems();
            }
            else if (choice == 2)
            {
                Feature4_ListAllOrders();
            }
            else if (choice == 3)
            {
                Feature5_CreateNewOrder();
            }
            else if (choice == 4)
            {
                Feature6_ProcessOrder();
            }
            else if (choice == 5)
            {
                ModifyExistingOrder();
            }
            else if (choice == 6)
            {
                Feature8_DeleteExistingOrder();
            }
            else if (choice == "B" || choice == "b")
            {
                DisplayTotalOrderAmount();
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }

        }

        // Calls for feature 1 (Loading of restaurants and food items files)
        LoadRestaurantsAndFoodItems();

        // Calls for feature 2 (Loading of customers and orders files)
        LoadCustomersAndOrders(customers, orders);


    }
    //========================================================== =
    // Student Number : S10273890E
    // Student Name : Shenise Lim Em Qing
    // Partner Name : Chloe Heng Chi Xuan
    //========================================================== =
    // Feature 1
    static void LoadRestaurantsAndFoodItems()
    {
        //restaurants
        string restaurantsFile = "restaurants.csv";

        using (StreamReader sr = new StreamReader(restaurantsFile))
        {
            string header = sr.ReadLine();

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split(',');

                if (parts.Length >= 3)
                {
                    string restaurantID = parts[0];
                    string restaurantName = parts[1];
                    string restaurantEmail = parts[2];

                    Restaurant restaurant = new Restaurant(restaurantID, restaurantName, restaurantEmail);
                    restaurants.Add(restaurant);
                }
            }
        }

        //food items
        string foodItemsFile = "fooditems.csv";

        using (StreamReader sr = new StreamReader(foodItemsFile))
        {
            string header = sr.ReadLine();

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split(',');
                if (parts.Length >= 4)
                {
                    string restaurantId = parts[0];
                    string itemName = parts[1];
                    string itemDesc = parts[2];
                    double itemPrice = Convert.ToDouble(parts[3]);

                    FoodItem food = new FoodItem(itemName, itemDesc, itemPrice, "");

                    Restaurant r = null;

                    foreach (Restaurant rest in restaurants)
                    {
                        if (rest.RestaurantId == restaurantId)
                        {
                            r = rest;
                            break;
                        }
                    }

                    if (r != null)
                    {
                        if (r.Menus.Count == 0)
                        {
                            r.Menus.Add(new Menu("M1", "Main Menu"));
                        }

                        r.Menus[0].FoodItems.Add(food);
                    }
                }
            }
        }
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
    // Student Number : S10273890E
    // Student Name : Shenise Lim Em Qing
    // Partner Name : Chloe Heng Chi Xuan
    //========================================================== =
    // Feature 4
    static void Feature4_ListAllOrders()
    {
        Console.WriteLine("All Orders ");
        Console.WriteLine("===========");
        Console.WriteLine("Order ID   Customer      Restaurant       Delivery Date/Time   Amount    Status  ");
        Console.WriteLine("--------   ----------    -------------    ------------------   ------    --------- ");

        foreach (Restaurant r in restaurants)
        {
            foreach (Order order in r.OrderQueue)
            {
                string customerName = "Unknown";

                foreach (Customer c in customers)
                {
                    if (c.Orders.Contains(order))
                    {
                        customerName = c.CustomerName;
                        break;
                    }
                }

                Console.WriteLine(
                    $"{order.OrderID,-10} " +
                    $"{r.RestaurantName,-15} " +
                    $"{order.DeliveryDateTime:dd/MM/yyyy HH:mm}   " +
                    $"{order.OrderTotal,8:C}   " +
                    $"{order.OrderStatus,-10}"
                );
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
    // Student Number : S10273890E
    // Student Name : Shenise Lim Em Qing
    // Partner Name : Chloe Heng Chi Xuan
    //========================================================== =
    // Feature 6

    static void Feature6_ProcessOrder()
    {
        Console.WriteLine("Process Order");
        Console.WriteLine("=============");

        Console.Write("Enter Restaurant ID: ");
        string restaurantID = Console.ReadLine();

        Restaurant selectedRestaurant = null;
        foreach (Restaurant r in restaurants)
        {
            if (r.RestaurantId == restaurantID)
            {
                selectedRestaurant = r;
                break;
            }
        }

        if (selectedRestaurant == null)
        {
            Console.WriteLine("Invalid Restaurant ID.");
            return;
        }

        if (selectedRestaurant.OrderQueue.Count == 0)
        {
            Console.WriteLine("No orders available for this restaurant.");
            return;
        }

        int queueCount = selectedRestaurant.OrderQueue.Count;

        for (int i = 0; i < queueCount; i++)
        {
            Order order = selectedRestaurant.OrderQueue.Dequeue();

            Console.WriteLine($"Order {order.OrderID}:");
            Console.WriteLine($"Customer: {customerName}");

            Console.WriteLine("Ordered Items: ");
            for (int j = 0; j < order.OrderedFoodItems.Count; j++)
            {
                OrderedFoodItem item = order.OrderedFoodItems[j];
                Console.WriteLine($"{j + 1}. {item.FoodItem.ItemName} - {item.QtyOrdered}");
            }

            Console.WriteLine($"Delivery date/time: {order.DeliveryDateTime:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"Total Amount: ${order.OrderTotal:C2}");
            Console.WriteLine($"Order Status: {order.OrderStatus}");



            Console.Write("[C]onfirm / [R]eject / [S]kip / [D]eliver: ");
            string action = Console.ReadLine().ToUpper();

            if (action == "C")
            {
                if (order.OrderStatus == "Pending")
                {
                    order.OrderStatus = "Preparing";
                    Console.WriteLine($"Order {order.OrderID} confirmed. Status: Preparing");
                }
                else
                {
                    Console.WriteLine("Cannot confirm. Order must be Pending.");
                }
            }
            else if (action == "R")
            {
                if (order.OrderStatus == "Pending")
                {
                    order.OrderStatus = "Rejected";
                    refundStack.Push(order);

                    Console.WriteLine($"Order {order.OrderID} rejected.");
                    Console.WriteLine($"Refund of ${order.OrderTotal:C2} processed.");
                }
                else
                {
                    Console.WriteLine("Cannot reject. Order must be Pending.");
                }
            }
            else if (action == "D")
            {
                if (order.OrderStatus == "Preparing")
                {
                    order.OrderStatus = "Delivered";
                    Console.WriteLine($"Order {order.OrderID} delivered successfully.");
                }
                else
                {
                    Console.WriteLine("Cannot deliver. Order must be Preparing.");
                }
            }
            else if (action == "S")
            {
                if (order.OrderStatus == "Cancelled")
                {
                    Console.WriteLine($"Order {order.OrderID} is cancelled. Skipping...");
                    selectedRestaurant.OrderQueue.Enqueue(order);
                    continue;
                }
                else
                {
                    Console.WriteLine("Cannot skip. Order must be Cancelled.");
                }
            }
            else
            {
                Console.WriteLine("Invalid action. Order skipped.");
                selectedRestaurant.OrderQueue.Enqueue(order);
            }

        }
    }



    //========================================================== =
    // Student Number : S10272203K
    // Student Name : Chloe Heng Chi Xuan
    // Partner Name : Shenise Lim Em Qing 
    //========================================================== =
    // Feature 7
    static void ModifyExistingOrder(List<Customer> customers, List<Restaurant> restaurants, List<Order> orders)
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

    //========================================================== =
    // Student Number : S10273890E
    // Student Name : Shenise Lim Em Qing
    // Partner Name : Chloe Heng Chi Xuan
    //========================================================== =
    // Feature 8

    static void Feature8_DeleteExistingOrder()
    {
        Console.WriteLine("Delete Order ");
        Console.WriteLine("=============");

        Console.Write("Enter Customer Email: ");
        string customerEmail = Console.ReadLine();

        Customer selectedCustomer = null;
        foreach (Customer c in customers)
        {
            if (c.EmailAddress == customerEmail)
            {
                selectedCustomer = c;
                break;
            }
        }

        if (selectedCustomer == null)
        {
            Console.WriteLine("Invalid customer email.");
            return;
        }


        List<Order> pendingOrders = new List<Order>();
        foreach (Order o in selectedCustomer.Orders)
        {
            if (o.OrderStatus == "Pending")
            {
                pendingOrders.Add(o);
            }
        }

        if (pendingOrders.Count == 0)
        {
            Console.WriteLine("No Pending orders found.");
            return;
        }

        Console.WriteLine("Pending Orders:");
        foreach (Order o in pendingOrders)
        {
            Console.WriteLine(o.OrderID);
        }

        Console.Write("Enter Order ID: ");
        int orderID = Convert.ToInt32(Console.ReadLine());

        Order targetOrder = null;
        foreach (Order o in pendingOrders)
        {
            if (o.OrderID == orderID)
            {
                targetOrder = o;
                break;
            }
        }

        if (targetOrder == null)
        {
            Console.WriteLine("Invalid Order ID.");
            return;
        }

        Console.WriteLine($"Customer: {selectedCustomer.CustomerName}");

        Console.WriteLine("Ordered Items:");
        for (int i = 0; i < targetOrder.OrderedFoodItems.Count; i++)
        {
            OrderedFoodItem item = targetOrder.OrderedFoodItems[i];
            Console.WriteLine($"{i + 1}. {item.FoodItem.ItemName} - {item.QtyOrdered}");
        }

        Console.WriteLine($"Delivery date/time: {targetOrder.DeliveryDateTime:dd/MM/yyyy HH:mm}");
        Console.WriteLine($"Total Amount: ${targetOrder.OrderTotal:C2}");
        Console.WriteLine($"Order Status: {targetOrder.OrderStatus}");

        Console.Write("Confirm deletion? [Y/N]: ");
        string confirm = Console.ReadLine().ToUpper();

        if (confirm != "Y")
        {
            Console.WriteLine("Deletion cancelled.");
            return;
        }

        targetOrder.OrderStatus = "Cancelled";
        refundStack.Push(targetOrder);

        Console.WriteLine($"Order {targetOrder.OrderID} cancelled. Refund of {targetOrder.OrderTotal:C} processed.");

    }

    //========================================================== =
    // Student Number : S10273890E
    // Student Name : Shenise Lim Em Qing
    // Partner Name : Chloe Heng Chi Xuan
    //========================================================== =
    // Advanced Feature B

    static void DisplayTotalOrderAmount()
    {
        Console.WriteLine("Display total order amount");
        Console.WriteLine("==========================");

        double grandTotalDeliveredLessDelivery = 0.0;
        double grandTotalRefunds = 0.0;

        foreach (Restaurant r in restaurants)
        {
            // Delivered totals (less delivery fee per order)
            double restaurantDeliveredLessDelivery = 0.0;

            foreach (Order o in r.OrderQueue)
            {
                if (o.OrderStatus == "Delivered")
                {
                    // less delivery fee per order
                    double amountLessDelivery = o.OrderTotal - DELIVERY_FEE;
                    if (amountLessDelivery < 0) amountLessDelivery = 0;

                    restaurantDeliveredLessDelivery += amountLessDelivery;
                }
            }

            // Refund totals
            double restaurantRefunds = 0.0;

            foreach (Order refunded in refundStack)
            {
                // Finds restaurant that refunded order belongs to
                bool belongsToThisRestaurant = false;

                foreach (Order o in r.OrderQueue)
                {
                    if (o.OrderID == refunded.OrderID)
                    {
                        belongsToThisRestaurant = true;
                        break;
                    }
                }

                if (belongsToThisRestaurant)
                {
                    restaurantRefundTotal += refunded.OrderTotal;
                }

                Console.WriteLine($"\nRestaurant: {r.RestaurantName} ({r.RestaurantId})");
                Console.WriteLine($"Total Delivered Order Amount (less delivery fee): {restaurantDeliveredLessDelivery:C}");
                Console.WriteLine($"Total Refunds: ${restaurantRefundTotal:C2}");

                // Add to grand total
                grandTotalDeliveredLessDelivery += restaurantDeliveredLessDelivery;
                grandTotalRefunds += restaurantRefundTotal;

                //final amount Greberoo earns
                double finalEarned = grandTotalDeliveredLessDelivery * GRUBEROO_RATE;

                Console.WriteLine($"Total order amount (less delivery fee): {grandTotalDeliveredLessDelivery:C}");
                Console.WriteLine($"Total refunds: ${grandTotalRefunds:C2}");
                Console.WriteLine($"Final amount Gruberoo earns (30%): ${finalEarned:C2}");

            }
        }
    }
}
