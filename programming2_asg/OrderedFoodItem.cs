//========================================================== 
// Student Number : S10272203K
// Student Name : Chloe Heng Chi Xuan
// Partner Name : Shenise Lim Em Qing 
//========================================================== 

using PRG2_Assignment;
using System;

namespace PRG2_Assignment
{
    public class OrderedFoodItem
    {
        private int qtyOrdered;
        public int QtyOrdered
        {
            get { return qtyOrdered; }
            set { qtyOrdered = value; }
        }

        private double subtotal;
        public double Subtotal
        {
            get { return subtotal; }
            private set { subtotal = value; }
        }

        private FoodItem foodItem;
        public FoodItem FoodItem
        {
            get { return foodItem; }
            set { foodItem = value; }
        }

        public OrderedFoodItem()
        {
            QtyOrdered = 0;
            FoodItem = new FoodItem();
            CalculateSubtotal();
        }

        public OrderedFoodItem(int qtyOrdered, FoodItem fooditem)
        {
            QtyOrdered = qtyOrdered;
            FoodItem = fooditem;
            CalculateSubtotal();
        }

        public double CalculateSubtotal()
        {
            if (FoodItem == null)
            {
                Subtotal = 0;
            }
            else
            {
                Subtotal = QtyOrdered * FoodItem.ItemPrice;
            }

            return Subtotal;
        }

        public override string ToString()
        {
            CalculateSubtotal();
            if (foodItem != null)
            {
                return $"Ordered Item: {foodItem.ItemName} x {qtyOrdered} = ${Subtotal:F2}";
            }
            else
            {
                return $"Ordered Item: Unknown x {qtyOrdered} = ${Subtotal:F2}";
            }

        }
    }
}