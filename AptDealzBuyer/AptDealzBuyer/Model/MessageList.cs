using Xamarin.Forms;

namespace AptDealzBuyer.Model
{
    public class MessageList
    {
        public string Message { get; set; }
        public LayoutOptions MessagePosition { get; set; }
        public Thickness MessageMargin { get; set; }
        public Color MessageBackgroundColor { get; set; } = (Color)App.Current.Resources["LightGray"];
        public Color MessageTextColor { get; set; } = (Color)App.Current.Resources["Black"];
        public string UserName { get; set; }
        public string Time { get; set; }
        public string ContactImage { get; set; } = "imgContact.jpg";
        public string BuyerImage { get; set; } = "imgBuyer.png";
        public bool IsContact { get; set; } = false;
        public bool IsBuyer { get; set; } = false;
    }
}
