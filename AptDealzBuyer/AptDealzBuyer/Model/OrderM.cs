using System;
using System.Collections.Generic;
using System.Text;

namespace AptDealzBuyer.Model
{
    public class OrderM
    {
        public string OrdNo { get; set; }
        public string OrdPrice { get; set; }
        public string OrdStatus { get; set; }
        public string OrdDate { get; set; }
        public string StatusActions
        {
            get
            {
                if (OrdStatus == "Shipped")
                {
                    return "Track";
                }
                else if (OrdStatus == "Completed")
                {
                    return "Reorder";
                }
                else if (OrdStatus == "Ready for pickup")
                {
                    return "Show QR Code";
                }
                else
                {
                    return null;
                }
            }
        }

        public bool ShowAction
        {
            get
            {
                if (OrdStatus == "Accepted")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
