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

        public Menu(string menuId, string menuName)
        {
            MenuId = menuId;
            MenuName = menuName;
            foodItems = new List<FoodItem>();
        }

        public void AddFoodItem(FoodItem foodItem)
        {
            if (foodItem != null)
            {
                foodItems.Add(foodItem);
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
                    return true;
                }
            }
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

        public string ToString()
        {
            return $"Menu ID: {MenuId}, Menu Name: {MenuName}, Number of Food Items: {foodItems.Count}";
        }
    }
}