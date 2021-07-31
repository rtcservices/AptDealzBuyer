using AptDealzBuyer.Utility;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderStatusPopup : PopupPage
    {
        #region Objects
        public event EventHandler isRefresh;
        #endregion

        #region Constructor
        public OrderStatusPopup(int? StatusBy)
        {
            InitializeComponent();
            BindSource(StatusBy);
        }
        #endregion

        protected override bool OnBackgroundClicked()
        {
            base.OnBackgroundClicked();
            return false;
        }

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindLabel();
        }

        private void BindLabel()
        {
            try
            {
                lblFirstType.Text = OrderStatus.All.ToString();
                lblSecondType.Text = OrderStatus.Accepted.ToString();
                lblThirdType.Text = OrderStatus.Pending.ToString();
                lblFourType.Text = Common.ToCamelCase(OrderStatus.ReadyForPickup.ToString());
                lblFiveType.Text = OrderStatus.Shipped.ToString();
                lblSixType.Text = OrderStatus.Delivered.ToString();
                lblSevenType.Text = OrderStatus.Completed.ToString();
                lblEightType.Text = "Cancelled";
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderStatusPopup/BindLabel: " + ex.Message);
            }
        }

        private void BindSource(int? viewSource)
        {
            try
            {
                if (viewSource > 0)
                {
                    if (viewSource == (int)OrderStatus.All)
                    {
                        ClearSource();
                        imgFirstType.Source = Constraints.Radio_Selected;
                    }
                    else if (viewSource == (int)OrderStatus.Accepted)
                    {
                        ClearSource();
                        imgSecondType.Source = Constraints.Radio_Selected;
                    }
                    else if (viewSource == (int)OrderStatus.Pending)
                    {
                        ClearSource();
                        imgThirdType.Source = Constraints.Radio_Selected;
                    }
                    else if (viewSource == (int)OrderStatus.ReadyForPickup)
                    {
                        ClearSource();
                        imgFourType.Source = Constraints.Radio_Selected;
                    }
                    else if (viewSource == (int)OrderStatus.Shipped)
                    {
                        ClearSource();
                        imgFiveType.Source = Constraints.Radio_Selected;
                    }
                    else if (viewSource == (int)OrderStatus.Delivered)
                    {
                        ClearSource();
                        imgSixType.Source = Constraints.Radio_Selected;
                    }
                    else if (viewSource == (int)OrderStatus.Completed)
                    {
                        ClearSource();
                        imgSevenType.Source = Constraints.Radio_Selected;
                    }
                    else if (viewSource == (int)OrderStatus.CancelledFromBuyer)
                    {
                        ClearSource();
                        imgEightType.Source = Constraints.Radio_Selected;
                    }
                    else
                    {
                        ClearSource();
                        imgFirstType.Source = Constraints.Radio_Selected;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderStatusPopup/BindSource: " + ex.Message);
            }
        }

        private void ClearSource()
        {
            imgFirstType.Source = Constraints.Redio_UnSelected;
            imgSecondType.Source = Constraints.Redio_UnSelected;
            imgThirdType.Source = Constraints.Redio_UnSelected;
            imgFourType.Source = Constraints.Redio_UnSelected;
            imgFiveType.Source = Constraints.Redio_UnSelected;
            imgSixType.Source = Constraints.Redio_UnSelected;
            imgSevenType.Source = Constraints.Redio_UnSelected;
            imgEightType.Source = Constraints.Redio_UnSelected;
        }
        #endregion

        #region Events
        private void StkFirstType_Tapped(object sender, EventArgs e)
        {
            try
            {
                BindSource((int)OrderStatus.All);
                isRefresh?.Invoke(OrderStatus.All.ToString(), null);
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderStatusPopup/StkFirstType_Tapped: " + ex.Message);
            }
        }

        private void StkSecondType_Tapped(object sender, EventArgs e)
        {
            try
            {
                BindSource((int)OrderStatus.Accepted);
                isRefresh?.Invoke(OrderStatus.Accepted.ToString(), null);
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderStatusPopup/StkSecondType_Tapped: " + ex.Message);
            }
        }

        private void StkThirdType_Tapped(object sender, EventArgs e)
        {
            try
            {
                BindSource((int)OrderStatus.Pending);
                isRefresh?.Invoke(OrderStatus.Pending.ToString(), null);
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderStatusPopup/StkThirdType_Tapped: " + ex.Message);
            }
        }

        private void StkFourType_Tapped(object sender, EventArgs e)
        {
            try
            {
                BindSource((int)OrderStatus.ReadyForPickup);
                isRefresh?.Invoke(OrderStatus.ReadyForPickup.ToString(), null);
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderStatusPopup/StkFourType_Tapped: " + ex.Message);
            }
        }

        private void StkFiveType_Tapped(object sender, EventArgs e)
        {
            try
            {
                BindSource((int)OrderStatus.Shipped);
                isRefresh?.Invoke(OrderStatus.Shipped.ToString(), null);
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderStatusPopup/StkFiveType_Tapped: " + ex.Message);
            }
        }

        private void StkSixType_Tapped(object sender, EventArgs e)
        {
            try
            {
                BindSource((int)OrderStatus.Delivered);
                isRefresh?.Invoke(OrderStatus.Delivered.ToString(), null);
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderStatusPopup/StkSixType_Tapped: " + ex.Message);
            }
        }

        private void StkSevenType_Tapped(object sender, EventArgs e)
        {
            try
            {
                BindSource((int)OrderStatus.Completed);
                isRefresh?.Invoke(OrderStatus.Completed.ToString(), null);
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderStatusPopup/StkSevenType_Tapped: " + ex.Message);
            }
        }

        private void StkEightType_Tapped(object sender, EventArgs e)
        {
            try
            {
                BindSource((int)OrderStatus.CancelledFromBuyer);
                isRefresh?.Invoke("Cancelled", null);
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderStatusPopup/StkEightType_Tapped: " + ex.Message);
            }
        }
        #endregion
    }
}