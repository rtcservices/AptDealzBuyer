using Newtonsoft.Json;
using System;

namespace AptDealzBuyer.Model.Reponse
{
    public class Order
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("orderNo")]
        public string OrderNo { get; set; }

        [JsonProperty("quoteNo")]
        public string QuoteNo { get; set; }

        [JsonProperty("requirementNo")]
        public string RequirementNo { get; set; }

        [JsonProperty("buyer")]
        public string Buyer { get; set; }

        [JsonProperty("seller")]
        public string Seller { get; set; }

        [JsonProperty("shippingPincode")]
        public string ShippingPincode { get; set; }

        [JsonProperty("requestedQuantity")]
        public int RequestedQuantity { get; set; }

        [JsonProperty("unitPrice")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("netAmount")]
        public decimal NetAmount { get; set; }

        [JsonProperty("handlingCharges")]
        public decimal HandlingCharges { get; set; }

        [JsonProperty("shippingCharges")]
        public decimal ShippingCharges { get; set; }

        [JsonProperty("insuranceCharges")]
        public decimal InsuranceCharges { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("totalAmount")]
        public decimal TotalAmount { get; set; }

        [JsonProperty("expectedDelivery")]
        public DateTime ExpectedDelivery { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("paymentStatus")]
        public int PaymentStatus { get; set; }

        [JsonProperty("paidAmount")]
        public decimal PaidAmount { get; set; }

        [JsonProperty("trackingLink")]
        public object TrackingLink { get; set; }

        [JsonProperty("pickupProductDirectly")]
        public bool PickupProductDirectly { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("productDescription")]
        public string ProductDescription { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("quoteComments")]
        public object QuoteComments { get; set; }

        [JsonProperty("orderStatus")]
        public int OrderStatus { get; set; }

        [JsonProperty("orderStatusDescr")]
        public string OrderStatusDescr { get; set; }

        [JsonProperty("paymentStatusDescr")]
        public string PaymentStatusDescr { get; set; }

        [JsonProperty("isOrderCancelAllowed")]
        public bool IsOrderCancelAllowed { get; set; }


        [JsonProperty("sellerContact")]
        public SellerContact SellerContact { get; set; }

        [JsonProperty("buyerContact")]
        public BuyerContact BuyerContact { get; set; }


        #region [ Extra Properties ]
        [JsonIgnore]
        public string ExpectedDeliveryDate
        {
            get
            {
                if (ExpectedDelivery != null && ExpectedDelivery != DateTime.MinValue)
                {
                    return ExpectedDelivery.Date.ToString("dd/MM/yyyy");
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        [JsonIgnore]
        public string OrderStatusEnum
        {
            get
            {
                if (Status == (int)Utility.OrderStatus.Pending)
                {
                    return Utility.OrderStatus.Pending.ToString();
                }
                else if (Status == (int)Utility.OrderStatus.Accepted)
                {
                    return Utility.OrderStatus.Accepted.ToString();
                }
                else if (Status == (int)Utility.OrderStatus.ReadyForPickup)
                {
                    return Utility.OrderStatus.ReadyForPickup.ToString();
                }
                else if (Status == (int)Utility.OrderStatus.Shipped)
                {
                    return Utility.OrderStatus.Shipped.ToString();
                }
                else if (Status == (int)Utility.OrderStatus.Delivered)
                {
                    return Utility.OrderStatus.Delivered.ToString();
                }
                else if (Status == (int)Utility.OrderStatus.Completed)
                {
                    return Utility.OrderStatus.Completed.ToString();
                }
                else if (Status == (int)Utility.OrderStatus.CancelledFromBuyer)
                {
                    return Utility.OrderStatus.CancelledFromBuyer.ToString();
                }
                else if (Status == (int)Utility.OrderStatus.All)
                {
                    return Utility.OrderStatus.All.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        [JsonIgnore]
        public string OrderPaymentStatus
        {
            get
            {
                if (PaymentStatus == (int)Utility.OrderStatus.Pending)
                {
                    return Utility.OrderStatus.Pending.ToString();
                }
                else if (PaymentStatus == (int)Utility.OrderStatus.Accepted)
                {
                    return Utility.OrderStatus.Accepted.ToString();
                }
                else if (PaymentStatus == (int)Utility.OrderStatus.ReadyForPickup)
                {
                    return Utility.OrderStatus.ReadyForPickup.ToString();
                }
                else if (PaymentStatus == (int)Utility.OrderStatus.Shipped)
                {
                    return Utility.OrderStatus.Shipped.ToString();
                }
                else if (PaymentStatus == (int)Utility.OrderStatus.Delivered)
                {
                    return Utility.OrderStatus.Delivered.ToString();
                }
                else if (PaymentStatus == (int)Utility.OrderStatus.Completed)
                {
                    return Utility.OrderStatus.Completed.ToString();
                }
                else if (PaymentStatus == (int)Utility.OrderStatus.CancelledFromBuyer)
                {
                    return Utility.OrderStatus.CancelledFromBuyer.ToString();
                }
                else if (PaymentStatus == (int)Utility.OrderStatus.All)
                {
                    return Utility.OrderStatus.All.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion
    }
}
