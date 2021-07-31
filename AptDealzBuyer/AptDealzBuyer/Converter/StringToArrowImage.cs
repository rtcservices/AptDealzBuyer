using AptDealzBuyer.Utility;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Converter
{
    public class StringToArrowImage : IValueConverter, IMarkupExtension
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var UpDownArrow = string.Empty;
                if (UpDownArrow == Constraints.Arrow_Right)
                    return Constraints.Arrow_Down;
                else
                    return Constraints.Arrow_Right;
            }
            else
            {
                return Constraints.Arrow_Right;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Only one way bindings are supported with this converter");
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
