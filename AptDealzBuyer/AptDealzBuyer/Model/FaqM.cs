using AptDealzBuyer.Utility;
using System.ComponentModel;

namespace AptDealzBuyer.Model
{
    public class FaqM : INotifyPropertyChanged
    {
        public string FaqTitle { get; set; }
        public string FaqDesc { get; set; }

        private string _ArrowImage { get; set; } = Constraints.GreenArrow_Down;
        public string ArrowImage
        {
            get { return _ArrowImage; }
            set { _ArrowImage = value; PropertyChangedEventArgs("ArrowImage"); }
        }
        private bool _ShowFaqDesc { get; set; } = false;
        public bool ShowFaqDesc
        {
            get { return _ShowFaqDesc; }
            set { _ShowFaqDesc = value; PropertyChangedEventArgs("ShowFaqDesc"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void PropertyChangedEventArgs(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
