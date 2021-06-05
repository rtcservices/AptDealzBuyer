using System.ComponentModel;
using Xamarin.Forms;
using AptDealzBuyer.Utility;

namespace AptDealzBuyer.Model
{
    public class RequirementM : INotifyPropertyChanged
    {
        public string RequirementNo { get; set; }
        public string RequirementDes { get; set; }
        public string CatDescription { get; set; }
        public string ReqDate { get; set; }
        public string ReqStatus { get; set; }
        public int QuoteCount { get; set; }
        public Color StatusColor
        {
            get
            {
                if (ReqStatus == "Completed")
                {
                    return (Color)App.Current.Resources["Green"];
                }
                else if (ReqStatus == "Rejected")
                {
                    return (Color)App.Current.Resources["Red"];
                }
                else if (ReqStatus == "Inactive")
                {
                    return (Color)App.Current.Resources["Orange"];
                }
                else
                {
                    return (Color)App.Current.Resources["Black"];
                }
            }
        }

        private string _ArrowImage { get; set; } = Constraints.Arrow_Right;
        public string ArrowImage
        {
            get { return _ArrowImage; }
            set { _ArrowImage = value; PropertyChangedEventArgs("ArrowImage"); }
        }
        private bool _ShowCategory { get; set; } = false;
        public bool ShowCategory
        {
            get { return _ShowCategory; }
            set { _ShowCategory = value; PropertyChangedEventArgs("ShowCategory"); }
        }

        private bool _ShowDelete { get; set; } = true;
        public bool ShowDelete
        {
            get { return _ShowDelete; }
            set { _ShowDelete = value; PropertyChangedEventArgs("ShowDelete"); }
        }

        private LayoutOptions _Layout { get; set; } = LayoutOptions.CenterAndExpand;
        public LayoutOptions Layout
        {
            get { return _Layout; }
            set { _Layout = value; PropertyChangedEventArgs("Layout"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void PropertyChangedEventArgs(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
