//========================================================== 
// Student Number : S10273890
// Student Name : Shenise Lim Em Qing 
// Partner Name : Chloe Heng Chi Xuan
//========================================================== 


namespace PRG2_Assignment
{
    public class Menu
    {
        private string menuId;
        public string MenuId
        {
            get { return menuId; }
            set { menuId = value; }
        }

        private string menuName;
        public string MenuName
        {
            get { return menuName; }
            set { menuName = value; }
        }

        private List<FoodItem> foodItems;
        public List<FoodItem> FoodItems
        {
            get { return foodItems; }
            set { foodItems = value; }
        }

        private Restaurant restaurant;
        public Restaurant Restaurant
        {
            get { return restaurant; }
            set { restaurant = value; }
        }

        public Menu(string menuId, string menuName, Restaurant restaurant)
        {
            MenuId = menuId;
            MenuName = menuName;
            Restaurant = restaurant;
            foodItems = new List<FoodItem>();
        }

        public void AddFoodItem(FoodItem foodItem)
        {
            if (foodItem != null)
            {
                foodItems.Add(foodItem);
                Console.WriteLine($"Food item '{foodItem.FoodItemName}' added to the menu '{MenuName}' successfully.");
            }
            else
            {
                Console.WriteLine("Food item cannot be null.");
            }
        }

        public bool RemoveFoodItem(int foodItemId)
        {
            if (int i = 0; i < foodItems.Count; i++)
            {
                if (foodItems[i].FoodItemId == foodItemId)
                {
                    foodItems.RemoveAt(i);
                    Console.WriteLine($"Food item with ID {foodItemId} removed from the menu '{MenuName}' successfully.");
                    return true;
                }
            }
            Console.WriteLine("Food item not found.");
            return false;
        }

        public void DisplayFoodItems()
        {
            if (foodItems.Count == 0)
            {
                Console.WriteLine("No food items available in the menu.");
            }
            else
            {
                Console.WriteLine("Food items in the menu:");
                foreach (var item in foodItems)
                {
                    Console.WriteLine($"ID: {item.FoodItemId}, Name: {item.FoodItemName}, Price: {item.Price:C}");
                }
            }
        }

        public override string ToString()
        {
            return $"Menu ID: {MenuId}, Menu Name: {MenuName}, Number of Food Items: {foodItems.Count}";
        }
    }
}