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

        public List<FoodItem> FoodItems { get; set; }

        public Menu(string menuId, string menuName)
        {
            MenuId = menuId;
            MenuName = menuName;
            FoodItems = new List<FoodItem>();
        }

        public void AddFoodItem(FoodItem foodItem)
        {
            if (foodItem == null)
            {
                Console.WriteLine("Invalid Input");
                return;
            }

            if (FoodItems.Contains(foodItem))
            {
                Console.WriteLine("Food item already exists in the menu.");
                return;
            }
            else
            {
                FoodItems.Add(foodItem);
                Console.WriteLine("Food item added successfully.");
            }
        }

        public bool RemoveFoodItem(int foodItemId)
        {
            for (int i = 0; i < FoodItems.Count; i++) // Use 'FoodItems' and correct the loop logic
            {
                if (FoodItems[i].FoodItemId == foodItemId)
                {
                    FoodItems.RemoveAt(i);
                    Console.WriteLine($"Food item with ID {foodItemId} removed from the menu '{MenuName}' successfully.");
                    return true;
                }
            }
            Console.WriteLine("Food item not found.");
            return false;
        }

        public void DisplayFoodItems()
        {
            if (FoodItems.Count == 0)
            {
                Console.WriteLine("No food items available in the menu.");
                return;
            }
            else
            {
                Console.WriteLine("Food items in the menu:");
                foreach (var item in FoodItems)
                {
                    Console.WriteLine($"ID: {item.FoodItemId}, Name: {item.FoodItemName}, Price: {item.Price:C}");
                }
            }
        }

        public override string ToString()
        {
            return $"Menu ID: {MenuId}, Menu Name: {MenuName}, Number of Food Items: {FoodItems.Count}.";
        }
    }
}
