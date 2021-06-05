using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using AptDealzBuyer.Droid.CustomRenderers;
using AptDealzBuyer.Extention;
using dotMorten.Xamarin.Forms;
using dotMorten.Xamarin.Forms.Platform.Android;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Label), typeof(LabelCustomRenderer))]
[assembly: ExportRenderer(typeof(ExtEntry), typeof(EntryCustomRenderer))]
[assembly: ExportRenderer(typeof(AutoSuggestBox), typeof(AutoSuggestBoxCustomRenderer))]
[assembly: ExportRenderer(typeof(ExtEntryCenter), typeof(EntryCenterCustomRenderer))]
[assembly: ExportRenderer(typeof(Picker), typeof(PickerCustomRenderer))]
[assembly: ExportRenderer(typeof(Editor), typeof(EditorCustomRenderer))]

namespace AptDealzBuyer.Droid.CustomRenderers
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class LabelCustomRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            string fontFamily = e.NewElement?.FontFamily;
            if (!string.IsNullOrEmpty(fontFamily))
            {
                var label = (TextView)Control; // for example
                Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, fontFamily + ".otf");
                label.Typeface = font;
            }
        }
    }

    public class EntryCustomRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            string fontFamily = e.NewElement?.FontFamily;
            if (!string.IsNullOrEmpty(fontFamily))
            {
                var textbox = (TextView)Control; // for example
                Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, fontFamily + ".otf");
                textbox.Typeface = font;
            }

            var editText = (Android.Widget.EditText)this.Control;
            GradientDrawable gd = new GradientDrawable();
            gd.SetCornerRadius(0);
            gd.SetColor(Android.Graphics.Color.Transparent);
            editText.Background = gd;

            var maxLenght = e.NewElement?.MaxLength;
            if (maxLenght == 1)
            {
                Control.Gravity = Android.Views.GravityFlags.Center;
            }
            else
            {
                Control.Gravity = Android.Views.GravityFlags.CenterVertical;
            }
        }
    }

    public class AutoSuggestBoxCustomRenderer : AutoSuggestBoxRenderer
    {
        public AutoSuggestBoxCustomRenderer(Android.Content.Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AutoSuggestBox> e)
        {
            base.OnElementChanged(e);
            var editText = (Android.Widget.EditText)this.Control;
            GradientDrawable gd = new GradientDrawable();
            gd.SetCornerRadius(0);
            gd.SetColor(Android.Graphics.Color.Transparent);
            editText.Background = gd;
        }
    }

    public class EntryCenterCustomRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            string fontFamily = e.NewElement?.FontFamily;
            if (!string.IsNullOrEmpty(fontFamily))
            {
                var textbox = (TextView)Control; // for example
                Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, fontFamily + ".otf");
                textbox.Typeface = font;
            }

            var editText = (Android.Widget.EditText)this.Control;
            GradientDrawable gd = new GradientDrawable();
            gd.SetCornerRadius(0);
            gd.SetColor(Android.Graphics.Color.Transparent);
            editText.Background = gd;

            Control.SetPadding(0, 0, 0, 0);
            Control.Gravity = Android.Views.GravityFlags.Center;
        }
    }

    public class PickerCustomRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            string fontFamily = e.NewElement?.FontFamily;
            if (!string.IsNullOrEmpty(fontFamily))
            {
                var label = (TextView)Control; // for example
                Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, fontFamily + ".otf");
                label.Typeface = font;
            }

            var nativeedittextfield = (Android.Widget.EditText)this.Control;
            GradientDrawable gd = new GradientDrawable();
            gd.SetCornerRadius(0);
            gd.SetColor(Android.Graphics.Color.Transparent);
            nativeedittextfield.Background = gd;

            Control.SetPadding(0, 0, 0, 0);
            Control.Gravity = Android.Views.GravityFlags.CenterVertical;
        }
    }

    public class EditorCustomRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            string fontFamily = e.NewElement?.FontFamily;
            if (!string.IsNullOrEmpty(fontFamily))
            {
                var label = (TextView)Control; // for example
                Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, fontFamily + ".otf");
                label.Typeface = font;
            }

            var nativeedittextfield = (Android.Widget.EditText)Control;
            GradientDrawable gd = new GradientDrawable();
            gd.SetCornerRadius(0);
            gd.SetColor(Android.Graphics.Color.Transparent);
            nativeedittextfield.Background = gd;

            Control.SetPadding(0, 0, 0, 0);
            Control.Gravity = Android.Views.GravityFlags.Start;
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete
}