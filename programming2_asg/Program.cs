// See https://aka.ms/new-console-template for more information
using PRG2_Assignment;

using System;
using System.Collections.Generic;
using System.IO;


class Program
{
    static List<Restaurant> restaurants = new List<Restaurant>();
    static List<Customer> customers = new List<Customer>();
    static List<Order> orders = new List<Order>();
    static Stack<Order> refundStack = new Stack<Order>();

    //Advanced Feature (b) constants
    const double DELIVERY_FEE = 5.00;
    const double GRUBEROO_RATE = 0.30;

    // Favourite Orders Feature - Using Dictionary to track favourite orders per customer
    static Dictionary<string, List<int>> customerFavouriteOrders = new Dictionary<string, List<int>>();

    static void Main()
    {
        LoadRestaurantsAndFoodItems();
        LoadCustomersAndOrders();

        while (true)
        {
            Console.WriteLine("===== Gruberoo Food Delivery System ===== ");
            Console.WriteLine("1.List all restaurants and menu items");
            Console.WriteLine("2.List all orders");
            Console.WriteLine("3.Create a new order");
            Console.WriteLine("4.Process an order");
            Console.WriteLine("5.Modify an existing order");
            Console.WriteLine("6.Delete an existing order");
            Console.WriteLine("7.Display total order amount");
            Console.WriteLine("0.Exit");

            Console.Write("Enter your choice: ");
            string input = Console.ReadLine();

            //if input is not a int
            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            if (choice == 0)
            {
                break;
            }
            else if (choice == 1)
            {
                Feature3_ListAllRestaurantsAndMenuItems(List < Restaurant > restaurants);
            }
            else if (choice == 2)
            {
                Feature4_ListAllOrders();
            }
            else if (choice == 3)
            {
                Feature5_CreateNewOrder(List < Customer > customers, List < Restaurant > restaurants, List < Order > orders);
            }
            else if (choice == 4)
            {
                Feature6_ProcessOrder();
            }
            else if (choice == 5)
            {
                Feature7_ModifyExistingOrder(List < Customer > customers, List < Restaurant > restaurants, List < Order > orders);
            }
            else if (choice == 6)
            {
                Feature8_DeleteExistingOrder();
            }
            else if (choice == 7)
            {
                AdvancedA_BulkProcessPendingOrdersForToday();
            }
            else if (choice == 8)
            {
                DisplayTotalOrderAmount();
            }
            else if (choice == 9)
            {
                ManageFavouriteOrders();
            }
            else if (choice == 10)
            {
                DisplayFavouriteOrdersStatistics();
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }

        }

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

        if (!File.Exists(restaurantsFile))
        {
            Console.WriteLine($"Warning: {restaurantsFile} not found.");
            return;
        }

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

        if (!File.Exists(foodItemsFile))
        {
            Console.WriteLine($"Warning: {foodItemsFile} not found.");
            return;
        }

        using (StreamReader sr = new StreamReader(foodItemsFile))
        {
            string header = sr.ReadLine();

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split(',');
                if (parts.Length >= 4)
                {
                    string restaurantId = parts[0].Trim();
                    string itemName = parts[1].Trim();
                    string itemDesc = parts[2].Trim();

                    //parse price, but skip if invalid
                    if (!double.TryParse(parts[3].Trim(), out double itemPrice))
                    {
                        continue;
                    }

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

    static void LoadCustomersAndOrders(List<Customer> customers, List<Order> orders, List<Restaurant> restaurants)
    {
        // load customers
        string[] customerLines = File.ReadAllLines("customers.csv");

        for (int i = 1; i < customerLines.Length; i++)
        {
            string[] parts = customerLines[i].Split(',');
            if (parts.Length < 2) continue;

            string name = parts[0].Trim();
            string email = parts[1].Trim();

            customers.Add(new Customer(email, name));
        }

        // load orders
        string[] orderLines = File.ReadAllLines("orders.csv");

        for (int i = 1; i < orderLines.Length; i++)
        {
            string[] parts = orderLines[i].Split(',');
            if (parts.Length < 10) continue;

            int orderId = int.Parse(parts[0].Trim());
            string customerEmail = parts[1].Trim();
            string restaurantId = parts[2].Trim();
            string deliveryDate = parts[3].Trim();
            string deliveryTime = parts[4].Trim();
            string deliveryAddress = parts[5].Trim();
            string createdDateTime = parts[6].Trim();
            double totalAmount = double.Parse(parts[7].Trim());
            string status = parts[8].Trim();
            string itemsField = parts[9].Trim().Replace("\"", "");

            DateTime orderDate = DateTime.Parse(createdDateTime);
            DateTime deliveryDateTime = DateTime.Parse(deliveryDate + " " + deliveryTime);

            Order order = new Order(orderId, orderDate, totalAmount, status, deliveryDateTime, deliveryAddress, "", false);

            // to find customers using email
            Customer foundCustomer = null;
            for (int c = 0; c < customers.Count; c++)
            {
                if (customers[c].EmailAddress == customerEmail)
                {
                    foundCustomer = customers[c];
                    break;
                }
            }

            // to find restaurants using restaurant id
            Restaurant foundRestaurant = null;
            for (int r = 0; r < restaurants.Count; r++)
            {
                if (restaurants[r].RestaurantId == restaurantId)
                {
                    foundRestaurant = restaurants[r];
                    break;
                }
            }

            if (foundCustomer == null)
                Console.WriteLine($"Warning: Customer not found for order {orderId}: {customerEmail}");

            if (foundRestaurant == null)
                Console.WriteLine($"Warning: Restaurant not found for order {orderId}: {restaurantId}");

            // parsing of ordered items if the restaurant exits through search
            if (foundRestaurant != null && itemsField.Length > 0)
            {
                string[] itemParts = itemsField.Split('|');

                for (int p = 0; p < itemParts.Length; p++)
                {
                    string[] pieces = itemParts[p].Split(',');
                    if (pieces.Length < 2) continue;

                    string itemName = pieces[0].Trim();
                    int qty = int.Parse(pieces[1].Trim());

                    // customisation - only if customers have any
                    string customise = "";
                    if (pieces.Length >= 3)
                    {
                        customise = pieces[2].Trim();
                        for (int k = 3; k < pieces.Length; k++)
                        {
                            customise += "," + pieces[k];
                        }
                        customise = customise.Trim();
                    }

                    // looking up food item from restaurant menus
                    FoodItem foundFood = null;
                    foreach (Menu menu in foundRestaurant.Menus)
                    {
                        foreach (FoodItem fi in menu.FoodItems)
                        {
                            if (fi.ItemName == itemName)
                            {
                                foundFood = fi;
                                break;
                            }
                        }
                        if (foundFood != null) break;
                    }

                    if (foundFood != null)
                    {

                        FoodItem copy = new FoodItem(foundFood.ItemName, foundFood.ItemDesc, foundFood.ItemPrice, foundFood.Customise);
                        copy.Customise = customise;
                        // Adding ordered item to order
                        order.OrderedFoodItems.Add(new OrderedFoodItem(qty, copy));
                    }
                }
                // and this is to calculate the final order total.
                order.OrderTotal = order.CalculateOrderTotal();
            }

            // followed by storing order in the system
            orders.Add(order);
            // and then linking it to customer and restaurant queue
            if (foundCustomer != null)
                foundCustomer.AddOrder(order);
            // enqueue to restaurant order queue
            if (foundRestaurant != null)
                foundRestaurant.OrderQueue.Enqueue(order);
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

            // this is to check if the restaurant has any food items at all
            bool hasItems = false;

            for (int m = 0; m < r.Menus.Count; m++)
            {
                Menu menu = r.Menus[m];

                for (int f = 0; f < menu.FoodItems.Count; f++)
                {
                    FoodItem item = menu.FoodItems[f];

                    // displaying food item details
                    Console.WriteLine($"- {item.ItemName}: {item.ItemDesc} - ${item.ItemPrice:F2}");
                    hasItems = true;
                }
            }

            // this will only be shown if no food items were found
            if (!hasItems)
            {
                Console.WriteLine("- No food items available");
            }

            Console.WriteLine();
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
        Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-20} {4,-10} {5,-10}",
            "Order ID", "Customer", "Restaurant", "Delivery Date/Time", "Amount", "Status");
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

                Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-20} {4,-10:C} {5,-10}",
                    order.OrderID,
                    customerName,
                    r.RestaurantName,
                    order.DeliveryDateTime.ToString("dd/MM/yyyy HH:mm"),
                    order.OrderTotal,
                    order.OrderStatus);
            }
        }
    }





    //========================================================== =
    // Student Number : S10272203K
    // Student Name : Chloe Heng Chi Xuan
    // Partner Name : Shenise Lim Em Qing 
    //========================================================== =
    //Feature 5 and bonus feature - creating an order with a special offer

    static void Feature5_CreateNewOrder(List<Customer> customers, List<Restaurant> restaurants, List<Order> orders)
    {
        Console.WriteLine("\nCreate New Order");
        Console.WriteLine("================");

        // this is to get customer for validation to check if they exist.
        Console.Write("Enter Customer Email: ");
        string custEmail = Console.ReadLine().Trim();

        // to find customer using email
        Customer customer = null;
        for (int i = 0; i < customers.Count; i++)
        {
            if (customers[i].EmailAddress.ToLower() == custEmail.ToLower())
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

        Console.Write("Enter Restaurant ID: ");
        string restaurantId = Console.ReadLine().Trim();

        // get restaurant and validate that it exists using restaurant id
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

        // to collect delivery details for the order 
        Console.Write("Enter Delivery Date (dd/mm/yyyy): ");
        string deliveryDate = Console.ReadLine().Trim();

        Console.Write("Enter Delivery Time (hh:mm): ");
        string deliveryTime = Console.ReadLine().Trim();

        Console.Write("Enter Delivery Address: ");
        string deliveryAddress = Console.ReadLine().Trim();

        // build available food list
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

        // user selects item until they enter 0 to finish
        List<OrderedFoodItem> selectedItems = new List<OrderedFoodItem>();

        while (true)
        {
            Console.Write("Enter item number (0 to finish): ");
            int itemNo = Convert.ToInt32(Console.ReadLine());

            if (itemNo == 0) break;

            // validate item selection range
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

            // this si to copy menu item so that we dont modify the restaurants original menu data
            FoodItem copy = new FoodItem(chosen.ItemName, chosen.ItemDesc, chosen.ItemPrice, chosen.Customise);

            selectedItems.Add(new OrderedFoodItem(qty, copy));
        }

        if (selectedItems.Count == 0)
        {
            Console.WriteLine("No items selected. Order cancelled.");
            return;
        }

        // this is a single special request for the whole order that applies to all selected items.
        Console.Write("Add special request? [Y/N]: ");
        string specialAns = Console.ReadLine().Trim().ToUpper();

        string specialRequest = "";
        if (specialAns == "Y")
        {
            Console.Write("Enter special request: ");
            specialRequest = SafeCustomise(Console.ReadLine());
        }

        if (specialRequest != "")
        {
            for (int i = 0; i < selectedItems.Count; i++)
            {
                selectedItems[i].FoodItem.Customise = specialRequest;
            }
        }

        // generate new order id
        int newOrderId = 1001;
        if (orders.Count > 0)
        {
            int max = orders[0].OrderID;
            for (int i = 1; i < orders.Count; i++)
            {
                if (orders[i].OrderID > max) max = orders[i].OrderID;
            }
            newOrderId = max + 1;
        }

        // creatinng the order object (items added next)
        DateTime createdDateTime = DateTime.Now;
        DateTime deliveryDateTime = DateTime.Parse(deliveryDate + " " + deliveryTime);

        Order newOrder = new Order(newOrderId, createdDateTime, 0, "Pending", deliveryDateTime, deliveryAddress, "", false);

        // adding of selected items to order 
        for (int i = 0; i < selectedItems.Count; i++)
        {
            newOrder.OrderedFoodItems.Add(selectedItems[i]);
        }

        newOrder.OrderTotal = newOrder.CalculateOrderTotal();
        Console.WriteLine($"Order Total: ${(newOrder.OrderTotal - 5.00):F2} + $5.00 (delivery) = ${newOrder.OrderTotal:F2}");


        // ================= Bonus feature: Creating an order with a special order  =================
        if (restaurant.SpecialOffers.Count > 0)
        {
            Console.Write("Apply special offer? [Y/N]: ");
            string offerAns = Console.ReadLine().Trim().ToUpper();

            if (offerAns == "Y")
            {
                Console.Write("Enter offer code: ");
                string offerCode = Console.ReadLine().Trim();

                SpecialOffer matchedOffer = null;

                for (int i = 0; i < restaurant.SpecialOffers.Count; i++)
                {
                    if (restaurant.SpecialOffers[i].OfferCode.ToLower() == offerCode.ToLower())
                    {
                        matchedOffer = restaurant.SpecialOffers[i];
                        break;
                    }
                }

                if (matchedOffer != null)
                {
                    // apply discount to food total, then add delivery back
                    double foodTotal = newOrder.OrderTotal - 5.00;
                    double discountAmount = foodTotal * (matchedOffer.Discount / 100);
                    double newTotal = foodTotal - discountAmount + 5.00;

                    Console.WriteLine($"Special Offer Applied: {matchedOffer.OfferDesc}");
                    Console.WriteLine($"Discount: {matchedOffer.Discount}% (-${discountAmount:F2})");

                    newOrder.OrderTotal = newTotal;

                    Console.WriteLine($"New Order Total: ${newOrder.OrderTotal:F2}");
                }
                else
                {
                    Console.WriteLine("Invalid offer code. No discount applied.");
                }
            }
        }

        // Payment part of the feature 
        Console.Write("Proceed to payment? [Y/N]: ");
        string proceedPay = Console.ReadLine().Trim().ToUpper();

        if (proceedPay != "Y")
        {
            Console.WriteLine("Payment not done. Exit feature.");
            return;
        }

        // to get payment method and validate allowed options
        Console.WriteLine("Payment method:");
        Console.Write("[CC] Credit Card / [PP] PayPal / [CD] Cash on Delivery: ");
        string paymentMethod = Console.ReadLine().Trim().ToUpper();

        if (paymentMethod != "CC" && paymentMethod != "PP" && paymentMethod != "CD")
        {
            Console.WriteLine("Invalid payment method. Exit feature.");
            return;
        }

        // to mark that the payment is a success
        newOrder.OrderPaymentMethod = paymentMethod;
        newOrder.OrderPaid = true;

        // save in memory structures 
        orders.Add(newOrder);
        customer.AddOrder(newOrder);
        restaurant.OrderQueue.Enqueue(newOrder);

        // append to csv file
        AppendOrderToCsv(newOrder, custEmail, restaurant.RestaurantId);

        Console.WriteLine($"Order {newOrderId} created successfully! Status: Pending");
    }


    static void AppendOrderToCsv(Order order, string customerEmail, string restaurantId)
    {
        string deliveryDate = order.DeliveryDateTime.ToString("dd/MM/yyyy");
        string deliveryTime = order.DeliveryDateTime.ToString("HH:mm");
        string createdDateTime = order.OrderDateTime.ToString("dd/MM/yyyy HH:mm");

        string itemsText = "";
        for (int i = 0; i < order.OrderedFoodItems.Count; i++)
        {
            if (i > 0) itemsText += "|";

            string itemName = order.OrderedFoodItems[i].FoodItem.ItemName;
            int qty = order.OrderedFoodItems[i].QtyOrdered;
            string customise = SafeCustomise(order.OrderedFoodItems[i].FoodItem.Customise);

            itemsText += itemName + ", " + qty + ", " + customise;
        }

        string line =
            $"{order.OrderID},{customerEmail},{restaurantId},{deliveryDate},{deliveryTime}," +
            $"{order.DeliveryAddress},{createdDateTime},{order.OrderTotal},{order.OrderStatus}," +
            $"\"{itemsText}\"";

        // Append new line into orders.csv so data is kept even after program closes
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

            //find customer name
            string customerName = "Unknown";
            foreach (Customer c in customers)
            {
                if (c.Orders.Contains(order))
                {
                    customerName = c.CustomerName;
                    break;
                }
            }

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
                selectedRestaurant.OrderQueue.Enqueue(order);
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
                    selectedRestaurant.OrderQueue.Enqueue(order);
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
                    selectedRestaurant.OrderQueue.Enqueue(order);
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

    static void Feature7_ModifyExistingOrder(List<Customer> customers, List<Restaurant> restaurants, List<Order> orders)
    {
        Console.WriteLine("\nModify Order");
        Console.WriteLine("============");

        Console.Write("Enter Customer Email: ");
        string custEmail = Console.ReadLine().Trim();

        Customer customer = null;
        for (int i = 0; i < customers.Count; i++)
        {
            if (customers[i].EmailAddress.ToLower() == custEmail.ToLower())
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

        // this shows only oending orders and only pending orders can be modified 
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

        // select the order to modify
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

        // Show current order details
        Console.WriteLine("\nOrder Items:");
        for (int i = 0; i < target.OrderedFoodItems.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {target.OrderedFoodItems[i].FoodItem.ItemName} - {target.OrderedFoodItems[i].QtyOrdered} (Customise: {target.OrderedFoodItems[i].FoodItem.Customise})");
        }

        Console.WriteLine("\nAddress:");
        Console.WriteLine(target.DeliveryAddress);

        Console.WriteLine("\nDelivery Date/Time:");
        Console.WriteLine(target.DeliveryDateTime.ToString("dd/MM/yyyy HH:mm"));

        Console.Write("Modify: [1] Items [2] Address [3] Delivery Time: ");
        string choice = Console.ReadLine().Trim();

        // Store old total to compare later
        double oldTotal = target.OrderTotal;

        // Modify based on choice - option 1 is to modify items 
        if (choice == "1")
        {
            // ask restaurant ID so we can load menu items
            Console.Write("Enter Restaurant ID (to load menu items): ");
            string restId = Console.ReadLine().Trim();

            Restaurant restaurant = null;
            for (int i = 0; i < restaurants.Count; i++)
            {
                if (restaurants[i].RestaurantId.ToLower() == restId.ToLower())
                {
                    restaurant = restaurants[i];
                    break;
                }
            }

            if (restaurant == null)
            {
                Console.WriteLine("Restaurant not found. Cannot modify items.");
                return;
            }

            // build list of food items from restaurant menus
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

            Console.WriteLine("\nAvailable Food Items:");
            for (int i = 0; i < availableFood.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {availableFood[i].ItemName} - ${availableFood[i].ItemPrice:F2}");
            }

            // re-enter items
            target.OrderedFoodItems.Clear();

            while (true)
            {
                Console.Write("Enter item number (0 to finish): ");
                int itemNo = Convert.ToInt32(Console.ReadLine());

                if (itemNo == 0) break;

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
                FoodItem copy = new FoodItem(chosen.ItemName, chosen.ItemDesc, chosen.ItemPrice, chosen.Customise);

                target.OrderedFoodItems.Add(new OrderedFoodItem(qty, copy));
            }

            if (target.OrderedFoodItems.Count == 0)
            {
                Console.WriteLine("No items selected. Update cancelled.");
                return;
            }

            // one special request for the whole order
            Console.Write("Add special request for this order? [Y/N]: ");
            string specialAns = Console.ReadLine().Trim().ToUpper();

            string specialRequest = "";
            if (specialAns == "Y")
            {
                Console.Write("Enter special request: ");
                specialRequest = SafeCustomise(Console.ReadLine());
            }

            if (specialRequest != "")
            {
                for (int i = 0; i < target.OrderedFoodItems.Count; i++)
                {
                    target.OrderedFoodItems[i].FoodItem.Customise = specialRequest;
                }
            }
        }

        // modify address - option 2
        else if (choice == "2")
        {
            Console.Write("Enter new Address: ");
            target.DeliveryAddress = Console.ReadLine().Trim();
        }

        // modify delivery - option 3
        else if (choice == "3")
        {
            Console.Write("Enter new Delivery Time (hh:mm): ");
            string newTime = Console.ReadLine().Trim();

            string datePart = target.DeliveryDateTime.ToString("dd/MM/yyyy");
            target.DeliveryDateTime = DateTime.Parse(datePart + " " + newTime);
        }
        else
        {
            Console.WriteLine("Invalid choice.");
            return;
        }

        // recalculate order total after modification
        target.OrderTotal = target.CalculateOrderTotal();

        // if total increases, user must confirm and pay again
        if (target.OrderTotal > oldTotal)
        {
            Console.WriteLine($"\nOld Total: ${oldTotal:F2}");
            Console.WriteLine($"New Total: ${target.OrderTotal:F2}");

            Console.Write("Proceed to payment? [Y/N]: ");
            string proceedPay = Console.ReadLine().Trim().ToUpper();

            if (proceedPay != "Y")
            {
                Console.WriteLine("Payment not done. Modification cancelled.");
                return;
            }

            Console.Write("[CC] Credit Card / [PP] PayPal / [CD] Cash on Delivery: ");
            string paymentMethod = Console.ReadLine().Trim().ToUpper();

            if (paymentMethod != "CC" && paymentMethod != "PP" && paymentMethod != "CD")
            {
                Console.WriteLine("Invalid payment method. Modification cancelled.");
                return;
            }

            target.OrderPaymentMethod = paymentMethod;
            target.OrderPaid = true;
        }

        Console.WriteLine($"\nOrder {target.OrderID} updated.");
        Console.WriteLine($"Updated Total: ${target.OrderTotal:F2}");
        Console.WriteLine($"Updated Delivery Date/Time: {target.DeliveryDateTime:dd/MM/yyyy HH:mm}");
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
            if (c.EmailAddress.ToLower() == customerEmail.ToLower())
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

        Console.WriteLine($"\nCustomer: {selectedCustomer.CustomerName}");

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
    // Student Number : S10272203K
    // Student Name : Chloe Heng Chi Xuan
    // Partner Name : Shenise Lim Em Qing 
    //========================================================== =
    // Advanced feature A 
    static void AdvancedA_BulkProcessPendingOrdersForToday(List<Restaurant> restaurants)
    {
        Console.WriteLine("\nBulk Processing of Pending Orders (Today)");
        Console.WriteLine("=========================================");

        DateTime today = DateTime.Today;
        DateTime now = DateTime.Now;

        int totalPendingToday = 0;

        // count total pending orders for today
        for (int r = 0; r < restaurants.Count; r++)
        {
            foreach (Order o in restaurants[r].OrderQueue)
            {
                if (o.OrderStatus.ToLower() == "pending" &&
                    o.DeliveryDateTime.Date == today)
                {
                    totalPendingToday++;
                }
            }
        }

        Console.WriteLine($"Total Pending orders in queues for today: {totalPendingToday}");

        if (totalPendingToday == 0)
        {
            Console.WriteLine("No pending orders to process for today.");
            return;
        }

        int processed = 0;
        int preparingCount = 0;
        int rejectedCount = 0;

        // process orders
        for (int r = 0; r < restaurants.Count; r++)
        {
            Queue<Order> queue = restaurants[r].OrderQueue;
            int queueSize = queue.Count;

            for (int i = 0; i < queueSize; i++)
            {
                Order o = queue.Dequeue();

                if (o.OrderStatus.ToLower() == "pending" &&
                    o.DeliveryDateTime.Date == today)
                {
                    if (o.DeliveryDateTime < now.AddHours(1))
                    {
                        o.OrderStatus = "Rejected";
                        rejectedCount++;
                    }
                    else
                    {
                        o.OrderStatus = "Preparing";
                        preparingCount++;
                    }

                    processed++;
                }

                queue.Enqueue(o);
            }
        }

        // Summary
        Console.WriteLine("\nSummary");
        Console.WriteLine("-------");
        Console.WriteLine($"Number of orders processed: {processed}");
        Console.WriteLine($"Preparing: {preparingCount}");
        Console.WriteLine($"Rejected: {rejectedCount}");

        double percentage = (processed * 100.0) / totalPendingToday;
        Console.WriteLine($"Percentage auto-processed: {percentage:F2}%");
    }

    // helper to sanitise customise input
    static string SafeCustomise(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return "";

        // Prevent breaking CSV format
        return input.Replace("|", " ").Replace(",", " ").Trim();
    }



    //========================================================== =
    // Student Number : S10273890E
    // Student Name : Shenise Lim Em Qing
    // Partner Name : Chloe Heng Chi Xuan
    //========================================================== =
    // Advanced Feature B

    static void DisplayTotalOrderAmount()
    {
        Console.WriteLine("\nDisplay total order amount");
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
            double restaurantRefunds = 0.0; // FIXED: Changed from restaurantRefundTotal to restaurantRefunds

            foreach (Order refunded in refundStack)
            {
                // Find if refunded order belongs to this restaurant
                bool belongsToThisRestaurant = r.Orders.Contains(refunded);

                if (belongsToThisRestaurant)
                {
                    restaurantRefunds += refunded.OrderTotal; // FIXED: Changed variable name
                }
            }

            Console.WriteLine($"\nRestaurant: {r.RestaurantName} ({r.RestaurantId})");
            Console.WriteLine($"  Total Delivered Order Amount (less delivery fee): ${restaurantDeliveredLessDelivery:F2}");
            Console.WriteLine($"  Total Refunds: ${restaurantRefunds:F2}");

            // Add to grand total
            grandTotalDeliveredLessDelivery += restaurantDeliveredLessDelivery;
            grandTotalRefunds += restaurantRefunds;
        }

        //final amount Gruberoo earns - FIXED: Moved outside the loop
        double finalEarned = grandTotalDeliveredLessDelivery * GRUBEROO_RATE;

        Console.WriteLine("\n--- Grand Total ---");
        Console.WriteLine($"Total order amount (less delivery fee): ${grandTotalDeliveredLessDelivery:F2}");
        Console.WriteLine($"Total refunds: ${grandTotalRefunds:F2}");
        Console.WriteLine($"Final amount Gruberoo earns (30%): ${finalEarned:F2}");
    }

    //==========================================================
    // BONUS ADVANCED FEATURE C: FAVOURITE ORDERS
    // Student Number : S10273890E
    // Student Name : Shenise Lim Em Qing
    // Partner Name : Chloe Heng Chi Xuan
    //==========================================================

    static void ManageFavouriteOrders()
    {
        Console.WriteLine("\nManage Favourite Orders");
        Console.WriteLine("========================");

        Console.Write("Enter Customer Email: ");
        string customerEmail = Console.ReadLine();

        Customer selectedCustomer = null;
        foreach (Customer c in customers)
        {
            if (c.EmailAddress.ToLower() == customerEmail.ToLower())
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

        // Initialize favourite list if not exists
        if (!customerFavouriteOrders.ContainsKey(selectedCustomer.EmailAddress))
        {
            customerFavouriteOrders[selectedCustomer.EmailAddress] = new List<int>();
        }

        Console.WriteLine("\n[1] Mark order as favourite");
        Console.WriteLine("[2] View my favourite orders");
        Console.WriteLine("[3] Remove order from favourites");
        Console.Write("Choose option: ");

        string choice = Console.ReadLine();

        if (choice == "1")
        {
            // Mark order as favourite
            if (selectedCustomer.Orders.Count == 0)
            {
                Console.WriteLine("No orders found for this customer.");
                return;
            }

            Console.WriteLine("\nYour Orders:");
            foreach (Order o in selectedCustomer.Orders)
            {
                string isFavourite = customerFavouriteOrders[selectedCustomer.EmailAddress].Contains(o.OrderID) ? " [FAVOURITE]" : "";
                Console.WriteLine($"Order ID: {o.OrderID} - Status: {o.OrderStatus} - Total: ${o.OrderTotal:F2}{isFavourite}");
            }

            Console.Write("\nEnter Order ID to mark as favourite: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                Console.WriteLine("Invalid Order ID.");
                return;
            }

            Order targetOrder = null;
            foreach (Order o in selectedCustomer.Orders)
            {
                if (o.OrderID == orderId)
                {
                    targetOrder = o;
                    break;
                }
            }

            if (targetOrder == null)
            {
                Console.WriteLine("Order not found.");
                return;
            }

            if (customerFavouriteOrders[selectedCustomer.EmailAddress].Contains(orderId))
            {
                Console.WriteLine("This order is already marked as favourite.");
            }
            else
            {
                customerFavouriteOrders[selectedCustomer.EmailAddress].Add(orderId);
                Console.WriteLine($"Order {orderId} marked as favourite!");
            }
        }
        else if (choice == "2")
        {
            // View favourite orders
            List<int> favourites = customerFavouriteOrders[selectedCustomer.EmailAddress];

            if (favourites.Count == 0)
            {
                Console.WriteLine("You have no favourite orders.");
                return;
            }

            Console.WriteLine($"\nYou have {favourites.Count} favourite order(s):");
            foreach (int favOrderId in favourites)
            {
                Order favOrder = null;
                foreach (Order o in selectedCustomer.Orders)
                {
                    if (o.OrderID == favOrderId)
                    {
                        favOrder = o;
                        break;
                    }
                }

                if (favOrder != null)
                {
                    // Find restaurant
                    string restaurantName = "Unknown";
                    foreach (Restaurant r in restaurants)
                    {
                        if (r.Orders.Contains(favOrder))
                        {
                            restaurantName = r.RestaurantName;
                            break;
                        }
                    }

                    Console.WriteLine($"\nOrder ID: {favOrder.OrderID}");
                    Console.WriteLine($"Restaurant: {restaurantName}");
                    Console.WriteLine($"Status: {favOrder.OrderStatus}");
                    Console.WriteLine($"Total: ${favOrder.OrderTotal:F2}");
                    Console.WriteLine("Items:");
                    foreach (OrderedFoodItem item in favOrder.OrderedFoodItems)
                    {
                        Console.WriteLine($"  - {item.FoodItem.ItemName} x{item.QtyOrdered}");
                    }
                }
            }
        }
        else if (choice == "3")
        {
            // Remove from favourites
            List<int> favourites = customerFavouriteOrders[selectedCustomer.EmailAddress];

            if (favourites.Count == 0)
            {
                Console.WriteLine("You have no favourite orders.");
                return;
            }

            Console.WriteLine("\nYour Favourite Orders:");
            foreach (int favOrderId in favourites)
            {
                Console.WriteLine($"Order ID: {favOrderId}");
            }

            Console.Write("\nEnter Order ID to remove from favourites: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                Console.WriteLine("Invalid Order ID.");
                return;
            }

            if (favourites.Remove(orderId))
            {
                Console.WriteLine($"Order {orderId} removed from favourites.");
            }
            else
            {
                Console.WriteLine("Order not found in favourites.");
            }
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }
    }

    static void DisplayFavouriteOrdersStatistics()
    {
        Console.WriteLine("\n========================================");
        Console.WriteLine("Favourite Orders Statistics");
        Console.WriteLine("========================================");

        int totalFavouriteOrders = 0;
        double totalAmountFromFavourites = 0.0;
        Dictionary<string, int> restaurantFavouriteCount = new Dictionary<string, int>();

        // Process each customer
        foreach (var customerEntry in customerFavouriteOrders)
        {
            string customerEmail = customerEntry.Key;
            List<int> favouriteOrderIds = customerEntry.Value;

            if (favouriteOrderIds.Count == 0)
                continue;

            // Find customer
            Customer customer = null;
            foreach (Customer c in customers)
            {
                if (c.EmailAddress == customerEmail)
                {
                    customer = c;
                    break;
                }
            }

            if (customer == null)
                continue;

            Console.WriteLine($"\n--- Customer: {customer.CustomerName} ({customerEmail}) ---");
            Console.WriteLine($"Total Favourite Orders: {favouriteOrderIds.Count}");

            double customerFavouriteTotal = 0.0;
            Dictionary<string, int> customerRestaurantCount = new Dictionary<string, int>();

            foreach (int favouriteOrderId in favouriteOrderIds)
            {
                // Find the order
                Order favouriteOrder = null;
                foreach (Order o in customer.Orders)
                {
                    if (o.OrderID == favouriteOrderId)
                    {
                        favouriteOrder = o;
                        break;
                    }
                }

                if (favouriteOrder == null)
                    continue;

                totalFavouriteOrders++;
                customerFavouriteTotal += favouriteOrder.OrderTotal;

                // Find restaurant for this order
                string restaurantName = "Unknown";
                foreach (Restaurant r in restaurants)
                {
                    if (r.Orders.Contains(favouriteOrder))
                    {
                        restaurantName = r.RestaurantName;

                        // Count for global statistics
                        if (!restaurantFavouriteCount.ContainsKey(restaurantName))
                        {
                            restaurantFavouriteCount[restaurantName] = 0;
                        }
                        restaurantFavouriteCount[restaurantName]++;

                        // Count for customer statistics
                        if (!customerRestaurantCount.ContainsKey(restaurantName))
                        {
                            customerRestaurantCount[restaurantName] = 0;
                        }
                        customerRestaurantCount[restaurantName]++;

                        break;
                    }
                }
            }

            Console.WriteLine($"Total Amount Spent on Favourites: ${customerFavouriteTotal:F2}");

            // Display customer's most frequently ordered restaurant
            if (customerRestaurantCount.Count > 0)
            {
                string mostFrequentRestaurant = "";
                int maxCount = 0;

                foreach (var entry in customerRestaurantCount)
                {
                    if (entry.Value > maxCount)
                    {
                        maxCount = entry.Value;
                        mostFrequentRestaurant = entry.Key;
                    }
                }

                Console.WriteLine($"Most Frequently Ordered Restaurant: {mostFrequentRestaurant} ({maxCount} favourite order(s))");
            }

            totalAmountFromFavourites += customerFavouriteTotal;
        }

        // Display global statistics
        Console.WriteLine("\n========================================");
        Console.WriteLine("GLOBAL STATISTICS");
        Console.WriteLine("========================================");
        Console.WriteLine($"Total Favourite Orders in System: {totalFavouriteOrders}");
        Console.WriteLine($"Total Amount from Favourite Orders: ${totalAmountFromFavourites:F2}");

        if (restaurantFavouriteCount.Count > 0)
        {
            string mostPopularRestaurant = "";
            int maxGlobalCount = 0;

            foreach (var entry in restaurantFavouriteCount)
            {
                if (entry.Value > maxGlobalCount)
                {
                    maxGlobalCount = entry.Value;
                    mostPopularRestaurant = entry.Key;
                }
            }

            Console.WriteLine($"Most Popular Restaurant (Across All Favourites): {mostPopularRestaurant} ({maxGlobalCount} favourite order(s))");

            Console.WriteLine("\nFavourite Orders by Restaurant:");
            foreach (var entry in restaurantFavouriteCount)
            {
                Console.WriteLine($"  - {entry.Key}: {entry.Value} favourite order(s)");
            }
        }
        else
        {
            Console.WriteLine("No favourite orders in the system yet.");
        }
    }
}
