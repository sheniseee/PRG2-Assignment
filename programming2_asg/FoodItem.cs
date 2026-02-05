//========================================================== 
// Student Number : S10272203K
// Student Name : Chloe Heng Chi Xuan
// Partner Name : Shenise Lim Em Qing 
//========================================================== 

using System;

namespace PRG2_Assignment
{
    public class FoodItem
    {
        private string itemName;
        public string ItemName
		{
            get { return itemName; }
            set { itemName = value; }
        }

        private string itemDesc;
        public string ItemDesc
        {
            get { return itemDesc; }
            set { itemDesc = value; }
        }

        private double itemPrice;
        public double ItemPrice
        {
            get { return itemPrice; }
            set { itemPrice = value; }
        }

        private string customise;
        public string Customise
        {
            get { return customise; }
            set { customise = value; }
        }

        public FoodItem()
        {

        }

        public FoodItem(string itemName, string itemDesc, double itemPrice, string customise)
        {
            ItemName = itemName;
            ItemDesc = itemDesc;
            ItemPrice = itemPrice;
            Customise = customise;
        }

        public override string ToString()
        {
            return $"{ItemName} - {ItemDesc} (${ItemPrice:F2}) [{Customise}]";
        }

    }
}
