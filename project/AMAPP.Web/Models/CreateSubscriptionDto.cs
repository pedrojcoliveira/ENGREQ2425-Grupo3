﻿namespace AMAPP.Web.Models
{
    public class CreateSubscriptionDto
    {
        public int SubscriptionPeriodId { get; set; }
        public required List<SubscriptionProductOffers> SubscriptionProductOffers { get; set; }
    }

    public class SubscriptionProductOffers
    {
        public int ProductOfferId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentMode PaymentMode { get; set; }
        public List<SubscriptionProductOfferDates> SubscriptionProductOfferDates { get; set; } = new();
    }

    public class SubscriptionProductOfferDates
    {
        public DateTime DeliveryDate { get; set; }
        public double Amount { get; set; }
    }
}
