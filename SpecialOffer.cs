//========================================================== =
// Student Number : S10272203K
// Student Name : Chloe Heng Chi Xuan
// Partner Name : Shenise Lim Em Qing 
//========================================================== =

using System;

namespace PRG2_Assignment
{
    public class SpecialOffer
    {
        private string offerCode;
        public string OfferCode
        {
            get { return offerCode; }
            set { offerCode = value; }
        }

        private string offerDesc;
        public string OfferDesc
        {
            get { return OfferDesc; }
            set { offerDesc = value; }
        }

        private double discount;
        public double Discount
        {
            get { return discount; }
            set { discount = value; }
        }

        public SpecialOffer()
        {

        }

        public SpecialOffer(string offerCode, string offerDesc, double discount)
        {
            OfferCode = offerCode;
            OfferDesc = offerDesc;
            Discount = discount;
        }

        public override string Tostring()
        {
            return $"Offer Code: {offerCode}, Description: {offerDesc}, Discount: {discount}%";
        }
    }
}

