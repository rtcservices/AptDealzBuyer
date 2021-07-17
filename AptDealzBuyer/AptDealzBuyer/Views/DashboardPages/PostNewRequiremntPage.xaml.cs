using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Extention;
using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Model.Request;
using AptDealzBuyer.Repository;
using AptDealzBuyer.Utility;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.DashboardPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostNewRequiremntPage : ContentPage
    {
        #region Objects     
        private List<Category> mCategories;
        private List<SubCategory> mSubCategories;
        private List<string> selectedSubCategory;
        private bool isIndiaProducts = false;
        private bool isPickupProduct = false;
        private bool isInsurance = false;
        private string relativePath = string.Empty;
        private BuyerDetails mBuyerDetail;
        #endregion

        #region Constructor
        public PostNewRequiremntPage()
        {
            InitializeComponent();
            mCategories = new List<Category>();
            mSubCategories = new List<SubCategory>();
            selectedSubCategory = new List<string>();
            BindBillingAddress();
            BindCategories();
            CapitalizeWord();
            dpExpectedDeliveryDate.NullableDate = null;
            dpExpectedDeliveryDate.MinimumDate = DateTime.Today;
        }
        #endregion

        #region Methods    
        void CapitalizeWord()
        {
            txtTitle.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
            txtDescription.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
            txtSourceSupply.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);

            txtName.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
            txtBuilding.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
            txtStreet.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
            txtCity.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);

            txtSAName.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
            txtSABuilding.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
            txtSAStreet.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
            txtSACity.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
            txtSALandmark.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        async void BindBillingAddress()
        {
            try
            {
                ProfileAPI profileAPI = new ProfileAPI();
                UserDialogs.Instance.ShowLoading(Constraints.Loading);

                var mResponse = await profileAPI.GetMyProfileData();
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (Newtonsoft.Json.Linq.JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        mBuyerDetail = jObject.ToObject<Model.Request.BuyerDetails>();
                        if (mBuyerDetail != null)
                        {
                            txtName.Text = mBuyerDetail.FullName;
                            txtBuilding.Text = mBuyerDetail.Building;
                            txtStreet.Text = mBuyerDetail.Street;
                            txtCity.Text = mBuyerDetail.City;
                            txtPinCode.Text = mBuyerDetail.PinCode;
                        }
                    }
                }
                else
                {
                    if (mResponse != null)
                        Common.DisplayErrorMessage(mResponse.Message);
                    else
                        Common.DisplayErrorMessage(Constraints.Something_Wrong);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PostNewRequiremntPage/GetProfile: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        async void BindCategories()
        {
            try
            {
                mCategories = await DependencyService.Get<ICategoryRepository>().GetCategory();
                if (mCategories != null || mCategories.Count > 0)
                {
                    pkCategory.ItemsSource = mCategories.Select(x => x.Name).ToList();
                }

                if (pkCategory.SelectedItem != null)
                {
                    var categoryId = mCategories.Where(x => x.Name == pkCategory.SelectedItem.ToString()).FirstOrDefault()?.CategoryId;
                    GetSubCategoryByCategoryId(categoryId);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PostNewRequiremntPage/BindList: " + ex.Message);
            }
        }

        async void GetSubCategoryByCategoryId(string categoryId)
        {
            try
            {
                mSubCategories = await DependencyService.Get<ICategoryRepository>().GetSubCategory(categoryId);
                pkSubCategory.ItemsSource = mSubCategories.Select(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PostNewRequiremntPage/GetSubCategoryByCategoryId: " + ex.Message);
            }
        }

        public bool Validations()
        {
            bool isValid = false;
            try
            {
                if (Common.EmptyFiels(txtTitle.Text) || Common.EmptyFiels(txtDescription.Text)
                    || pkCategory.SelectedIndex == -1 || pkSubCategory.SelectedIndex == -1
                    || Common.EmptyFiels(txtQuantity.Text)
                    || Common.EmptyFiels(txtName.Text) || Common.EmptyFiels(txtBuilding.Text)
                    || Common.EmptyFiels(txtStreet.Text) || Common.EmptyFiels(txtCity.Text)
                    || Common.EmptyFiels(txtPinCode.Text) || pkQuantityUnits.SelectedIndex == -1
                    || Common.EmptyFiels(txtEstimation.Text))
                {
                    RequiredFields();
                    isValid = false;
                }

                if (Common.EmptyFiels(txtTitle.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Title);
                }
                else if (pkCategory.SelectedIndex == -1)
                {
                    Common.DisplayErrorMessage(Constraints.Required_Category);
                }
                else if (pkSubCategory.SelectedIndex == -1)
                {
                    Common.DisplayErrorMessage(Constraints.Required_SubCategory);
                }
                else if (pkQuantityUnits.SelectedIndex == -1)
                {
                    Common.DisplayErrorMessage(Constraints.Required_QuantityUnits);
                }
                else if (Common.EmptyFiels(txtDescription.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Description);
                }
                else if (Common.EmptyFiels(txtQuantity.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Quantity);
                }
                else if (Common.EmptyFiels(txtEstimation.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_PriceEstimation);
                }
                else if (Common.EmptyFiels(txtName.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Billing_Name);
                }
                else if (Common.EmptyFiels(txtBuilding.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Billing_Building);
                }
                else if (Common.EmptyFiels(txtStreet.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Billing_Street);
                }
                else if (Common.EmptyFiels(txtCity.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Billing_City);
                }
                else if (Common.EmptyFiels(txtPinCode.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Billing_PinCode);
                }
                else
                {
                    isValid = true;
                }

                if (isValid && !isPickupProduct)
                {
                    if (Common.EmptyFiels(txtSAName.Text) || Common.EmptyFiels(txtSABuilding.Text)
                        || Common.EmptyFiels(txtSAStreet.Text) || Common.EmptyFiels(txtSACity.Text)
                        || Common.EmptyFiels(txtSAPinCode.Text) || Common.EmptyFiels(txtSALandmark.Text))
                    {
                        return RequiredShippingAddressFields();
                    }
                }
                else
                {
                    if (!isPickupProduct)
                        EmptyRedShippingAddress();
                }

                //else if (!Common.EmptyFiels(txtLocationPinCode.Text))
                //{
                //    if (!isPickupProduct)
                //    {
                //        BoxLocationPinCode.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                //        Common.DisplayErrorMessage(Constraints.Required_Delivery_PinCode);
                //    }
                //    else
                //    {
                //        isValid = true;
                //    }
                //}
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PostNewRequiremntPage/Validations: " + ex.Message);
            }
            return isValid;
        }

        void RequiredFields()
        {
            try
            {
                //Common.DisplayErrorMessage(Constraints.Required_All);

                if (Common.EmptyFiels(txtTitle.Text))
                {
                    BoxTitle.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (pkCategory.SelectedIndex == -1)
                {
                    BoxCategory.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (pkSubCategory.SelectedIndex == -1)
                {
                    BoxSubCategory.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (pkQuantityUnits.SelectedIndex == -1)
                {
                    BoxQuantityUnits.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtDescription.Text))
                {
                    BoxDescription.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtQuantity.Text))
                {
                    BoxQuantity.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                //if (Common.EmptyFiels(txtLocationPinCode.Text))
                //{
                //    BoxLocationPinCode.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                //}

                //if (Common.EmptyFiels(txtSourceSupply.Text))
                //{
                //    BoxSourceSupply.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                //}

                if (Common.EmptyFiels(txtEstimation.Text))
                {
                    BoxPriceEstimation.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtName.Text))
                {
                    BoxName.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtBuilding.Text))
                {
                    BoxBuilding.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtStreet.Text))
                {
                    BoxStreet.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtCity.Text))
                {
                    BoxCity.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtPinCode.Text))
                {
                    BoxPinCode.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PostNewRequiremntPage/RequiredFields: " + ex.Message);
            }
        }

        void EmptyRedShippingAddress()
        {
            if (Common.EmptyFiels(txtSAName.Text))
            {
                BoxSAName.BackgroundColor = (Color)App.Current.Resources["LightRed"];
            }

            if (Common.EmptyFiels(txtSABuilding.Text))
            {
                BoxSABuilding.BackgroundColor = (Color)App.Current.Resources["LightRed"];
            }

            if (Common.EmptyFiels(txtSAStreet.Text))
            {
                BoxSAStreet.BackgroundColor = (Color)App.Current.Resources["LightRed"];
            }

            if (Common.EmptyFiels(txtSACity.Text))
            {
                BoxSACity.BackgroundColor = (Color)App.Current.Resources["LightRed"];
            }

            if (Common.EmptyFiels(txtSAPinCode.Text))
            {
                BoxSAPinCode.BackgroundColor = (Color)App.Current.Resources["LightRed"];
            }

            if (Common.EmptyFiels(txtSALandmark.Text))
            {
                BoxSALandmark.BackgroundColor = (Color)App.Current.Resources["LightRed"];
            }
        }

        void EmptyGreyShippingAddress()
        {
            BoxSAName.BackgroundColor = (Color)App.Current.Resources["LightGray"];
            BoxSABuilding.BackgroundColor = (Color)App.Current.Resources["LightGray"];
            BoxSAStreet.BackgroundColor = (Color)App.Current.Resources["LightGray"];
            BoxSACity.BackgroundColor = (Color)App.Current.Resources["LightGray"];
            BoxSAPinCode.BackgroundColor = (Color)App.Current.Resources["LightGray"];
            BoxSALandmark.BackgroundColor = (Color)App.Current.Resources["LightGray"];
        }

        public bool RequiredShippingAddressFields()
        {
            bool isValid = false;

            EmptyRedShippingAddress();

            if (Common.EmptyFiels(txtSAName.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_Shipping_Name);
            }
            else if (Common.EmptyFiels(txtSABuilding.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_Shipping_Building);
            }
            else if (Common.EmptyFiels(txtSAStreet.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_Shipping_Street);
            }
            else if (Common.EmptyFiels(txtSACity.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_Shipping_City);
            }
            else if (Common.EmptyFiels(txtSAPinCode.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_Shipping_PinCode);
            }
            else if (Common.EmptyFiels(txtSALandmark.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_Shipping_Landmark);
            }
            else
            {
                isValid = true;
            }

            return isValid;
        }

        void FieldsTrim()
        {
            try
            {
                txtTitle.Text = txtTitle.Text.Trim();
                txtDescription.Text = txtDescription.Text.Trim();
                txtQuantity.Text = txtQuantity.Text.Trim();
                txtEstimation.Text = txtEstimation.Text.Trim();

                txtName.Text = txtName.Text.Trim();
                txtBuilding.Text = txtBuilding.Text.Trim();
                txtStreet.Text = txtStreet.Text.Trim();
                txtCity.Text = txtCity.Text.Trim();
                txtPinCode.Text = txtPinCode.Text.Trim();

                if (!Common.EmptyFiels(txtSAName.Text))
                    txtSAName.Text = txtSAName.Text.Trim();
                if (!Common.EmptyFiels(txtSABuilding.Text))
                    txtSABuilding.Text = txtSABuilding.Text.Trim();
                if (!Common.EmptyFiels(txtSAStreet.Text))
                    txtSAStreet.Text = txtSAStreet.Text.Trim();
                if (!Common.EmptyFiels(txtSACity.Text))
                    txtSACity.Text = txtSACity.Text.Trim();
                if (!Common.EmptyFiels(txtSAPinCode.Text))
                    txtSAPinCode.Text = txtSAPinCode.Text.Trim();
                if (!Common.EmptyFiels(txtSALandmark.Text))
                    txtSALandmark.Text = txtSALandmark.Text.Trim();

                if (!Common.EmptyFiels(txtSourceSupply.Text))
                    txtSourceSupply.Text = txtSourceSupply.Text.Trim();
                if (!Common.EmptyFiels(txtLocationPinCode.Text))
                    txtLocationPinCode.Text = txtLocationPinCode.Text.Trim();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PostNewRequiremntPage/FieldsTrim: " + ex.Message);
            }
        }

        Model.Request.Requirement FillRequirement()
        {
            Requirement mRequirement = new Requirement();
            try
            {
                mRequirement.UserId = Settings.UserId;
                mRequirement.Title = txtTitle.Text;
                mRequirement.Category = mCategories.Where(x => x.Name == pkCategory.SelectedItem.ToString()).FirstOrDefault()?.Name;
                mRequirement.SubCategories = selectedSubCategory;
                mRequirement.ProductDescription = txtDescription.Text;
                mRequirement.Quantity = Convert.ToInt32(txtQuantity.Text);
                mRequirement.Unit = pkQuantityUnits.SelectedItem.ToString();
                mRequirement.TotalPriceEstimation = Convert.ToDecimal(txtEstimation.Text);

                mRequirement.BillingAddressName = txtName.Text;
                mRequirement.BillingAddressBuilding = txtBuilding.Text;
                mRequirement.BillingAddressStreet = txtStreet.Text;
                mRequirement.BillingAddressCity = txtCity.Text;
                mRequirement.BillingAddressPinCode = txtPinCode.Text;

                mRequirement.ShippingAddressName = txtSAName.Text;
                mRequirement.ShippingAddressBuilding = txtSABuilding.Text;
                mRequirement.ShippingAddressStreet = txtSAStreet.Text;
                mRequirement.ShippingAddressCity = txtSACity.Text;
                mRequirement.ShippingAddressPinCode = txtSAPinCode.Text;
                mRequirement.ShippingAddressLandmark = txtSALandmark.Text;

                mRequirement.PreferInIndiaProducts = isIndiaProducts;
                mRequirement.PickupProductDirectly = isPickupProduct;
                mRequirement.NeedInsuranceCoverage = isInsurance;

                if (!Common.EmptyFiels(relativePath))
                {
                    string baseURL = (string)App.Current.Resources["BaseURL"];
                    mRequirement.ProductImage = relativePath.Replace(baseURL, "");
                }
                if (!Common.EmptyFiels(txtSourceSupply.Text))
                {
                    mRequirement.PreferredSourceOfSupply = txtSourceSupply.Text;
                }
                if (dpExpectedDeliveryDate.NullableDate != null && dpExpectedDeliveryDate.NullableDate != DateTime.MinValue)
                {
                    mRequirement.ExpectedDeliveryDate = dpExpectedDeliveryDate.NullableDate.Value;
                }
                if (!Common.EmptyFiels(txtLocationPinCode.Text))
                {
                    mRequirement.DeliveryLocationPinCode = txtLocationPinCode.Text;
                }
            }
            catch (Exception)
            {
                return null;
            }

            return mRequirement;
        }

        async void SubmitRequirement()
        {
            try
            {
                //grdShippingAddress.IsVisible = true;
                //imgShippingDown.Source = Constraints.Arrow_Up;
                //await ScrPrimary.ScrollToAsync(StkShipping, ScrollToPosition.Start, true);

                if (Validations())
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    if (!await PinCodeValidation(txtPinCode.Text, BoxPinCode))
                        return;

                    if (!Common.EmptyFiels(txtLocationPinCode.Text))
                    {
                        if (!await PinCodeValidation(txtLocationPinCode.Text, BoxLocationPinCode))
                            return;
                    }

                    if (!isPickupProduct)
                    {
                        if (!await PinCodeValidation(txtSAPinCode.Text, BoxSAPinCode))
                            return;
                    }
                    else
                    {
                        if (!Common.EmptyFiels(txtSAPinCode.Text))
                        {
                            if (!await PinCodeValidation(txtSAPinCode.Text, BoxSAPinCode))
                                return;
                        }
                    }

                    FieldsTrim();
                    RequirementAPI requirementAPI = new RequirementAPI();
                    var mRequirement = FillRequirement();

                    var mResponse = await requirementAPI.CreateRequirement(mRequirement);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        SuccessfullRequirement(mResponse.Message);
                        ClearPropeties();
                    }
                    else
                    {
                        if (mResponse != null)
                            Common.DisplayErrorMessage(mResponse.Message);
                        else
                            Common.DisplayErrorMessage(Constraints.Something_Wrong);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PostNewRequiremntPage/SubmitRequirement: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        void SuccessfullRequirement(string MessageString)
        {
            try
            {
                var successPopup = new PopupPages.SuccessPopup(MessageString);
                successPopup.isRefresh += (s1, e1) =>
                {
                    bool res = (bool)s1;
                    if (res)
                    {
                        Navigation.PopAsync();
                    }
                };

                PopupNavigation.Instance.PushAsync(successPopup);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PostNewRequiremntPage/SuccessfullRequirement: " + ex.Message);
            }
        }

        void ClearPropeties()
        {
            try
            {
                txtTitle.Text = string.Empty;
                pkCategory.SelectedIndex = -1;
                pkSubCategory.SelectedIndex = -1;
                txtDescription.Text = string.Empty;
                txtQuantity.Text = string.Empty;
                txtLocationPinCode.Text = string.Empty;
                txtEstimation.Text = string.Empty;
                txtSourceSupply.Text = string.Empty;

                txtName.Text = string.Empty;
                txtBuilding.Text = string.Empty;
                txtStreet.Text = string.Empty;
                txtCity.Text = string.Empty;
                txtPinCode.Text = string.Empty;

                txtSAName.Text = string.Empty;
                txtSABuilding.Text = string.Empty;
                txtSAStreet.Text = string.Empty;
                txtSACity.Text = string.Empty;
                txtSAPinCode.Text = string.Empty;
                txtSALandmark.Text = string.Empty;

                ImgProductImage.Source = "imgUploadImage.png";
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PostNewRequiremntPage/ClearPropeties: " + ex.Message);
            }
        }

        void UnfocussedFields(Entry entry = null, Editor editor = null, Picker picker = null)
        {
            try
            {
                if (entry != null)
                {
                    if (entry.ClassId == "Title")
                    {
                        BoxTitle.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "Quantity")
                    {
                        BoxQuantity.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "LocationPinCode")
                    {
                        BoxLocationPinCode.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "PriceEstimation")
                    {
                        BoxPriceEstimation.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "Name")
                    {
                        BoxName.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "Building")
                    {
                        BoxBuilding.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "Street")
                    {
                        BoxStreet.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "City")
                    {
                        BoxCity.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "PinCode")
                    {
                        BoxPinCode.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "SAName")
                    {
                        BoxSAName.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "SABuilding")
                    {
                        BoxSABuilding.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "SAStreet")
                    {
                        BoxSAStreet.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "SACity")
                    {
                        BoxSACity.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "SAPinCode")
                    {
                        BoxSAPinCode.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "SALandmark")
                    {
                        BoxSALandmark.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                }

                if (editor != null)
                {
                    if (editor.ClassId == "Description")
                    {
                        BoxDescription.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                }

                if (picker != null)
                {
                    if (picker.ClassId == "Category")
                    {
                        BoxCategory.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (picker.ClassId == "SubCategory")
                    {
                        BoxSubCategory.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (picker.ClassId == "QuantityUnits")
                    {
                        BoxQuantityUnits.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PostNewRequiremntPage/UnfocussedFields: " + ex.Message);
            }
        }

        void PickupDirectlyFromSeller()
        {
            try
            {
                if (imgPickup.Source.ToString().Replace("File: ", "") == Constraints.CheckBox_Checked)
                {
                    imgPickup.Source = Constraints.CheckBox_UnChecked;
                    lblLocationPINCode.Text = "Delivery Location PIN Code";
                    lblDeliveryDate.Text = "Expected Delivery Date";
                    StkDeliveryLocationPINCode.IsVisible = true;
                    isPickupProduct = false;
                    lblSAName.IsVisible = true;
                    lblSABuilding.IsVisible = true;
                    lblSAStreet.IsVisible = true;
                    lblSACity.IsVisible = true;
                    lblSAPINCode.IsVisible = true;
                    lblSALandmark.IsVisible = true;
                }
                else
                {
                    imgPickup.Source = Constraints.CheckBox_Checked;
                    lblLocationPINCode.Text = "Pickup Location PIN Code";
                    lblDeliveryDate.Text = "Expected Pickup Date";
                    StkDeliveryLocationPINCode.IsVisible = false;
                    EmptyGreyShippingAddress();
                    isPickupProduct = true;
                    lblSAName.IsVisible = false;
                    lblSABuilding.IsVisible = false;
                    lblSAStreet.IsVisible = false;
                    lblSACity.IsVisible = false;
                    lblSAPINCode.IsVisible = false;
                    lblSALandmark.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PostNewRequiremntPage/PickupDirectlyFromSeller: " + ex.Message);
            }
        }

        void SameAsBillingAddress()
        {
            try
            {
                if (imgSameAddress.Source.ToString().Replace("File: ", "") == Constraints.CheckBox_Checked)
                {
                    imgSameAddress.Source = Constraints.CheckBox_UnChecked;
                    txtSAName.Text = string.Empty;
                    txtSABuilding.Text = string.Empty;
                    txtSAStreet.Text = string.Empty;
                    txtSACity.Text = string.Empty;
                    txtSAPinCode.Text = string.Empty;
                    txtSALandmark.Text = string.Empty;
                }
                else
                {
                    imgSameAddress.Source = Constraints.CheckBox_Checked;
                    UnfocussedFields(entry: txtSAName);
                    UnfocussedFields(entry: txtSABuilding);
                    UnfocussedFields(entry: txtSAStreet);
                    UnfocussedFields(entry: txtSACity);
                    UnfocussedFields(entry: txtSAPinCode);
                    UnfocussedFields(entry: txtSALandmark);
                    txtSAName.Text = txtName.Text;
                    txtSABuilding.Text = txtBuilding.Text;
                    txtSAStreet.Text = txtStreet.Text;
                    txtSACity.Text = txtCity.Text;
                    txtSAPinCode.Text = txtPinCode.Text;
                    txtSALandmark.Text = mBuyerDetail.Landmark;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PostNewRequiremntPage/StkSameAddress_Tapped: " + ex.Message);
            }
        }

        async Task<bool> PinCodeValidation(string PinCode, BoxView boxView)
        {
            bool isValid = false;
            try
            {
                if (!Common.EmptyFiels(PinCode))
                {
                    PinCode = PinCode.Trim();
                    isValid = await DependencyService.Get<IProfileRepository>().ValidPincode(PinCode);
                    if (isValid)
                    {
                        boxView.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else
                    {
                        boxView.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                    }
                }
                else
                {
                    Common.DisplayErrorMessage(Constraints.Required_PinCode);
                    boxView.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PostNewRequiremntPage/PinCodeValidation: " + ex.Message);
            }
            return isValid;
        }
        #endregion

        #region Events    
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(image: ImgMenu);
            //Common.OpenMenu();
        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            Navigation.PopAsync();
        }

        private void StkPrefer_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (imgPrefer.Source.ToString().Replace("File: ", "") == Constraints.CheckBox_Checked)
                {
                    isIndiaProducts = false;
                    imgPrefer.Source = Constraints.CheckBox_UnChecked;
                }
                else
                {
                    isIndiaProducts = true;
                    imgPrefer.Source = Constraints.CheckBox_Checked;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PostNewRequiremntPage/StkPrefer_Tapped: " + ex.Message);
            }
        }

        private void StkPickup_Tapped(object sender, EventArgs e)
        {
            PickupDirectlyFromSeller();
        }

        private void StkShipping_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (imgShippingDown.Source.ToString().Replace("File: ", "") == Constraints.Arrow_Down)
                {
                    imgShippingDown.Source = Constraints.Arrow_Up;
                    grdShippingAddress.IsVisible = true;
                    ScrPrimary.ScrollToAsync(StkShipping, ScrollToPosition.Start, true);
                }
                else
                {
                    imgShippingDown.Source = Constraints.Arrow_Down;
                    grdShippingAddress.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PostNewRequiremntPage/StkShipping_Tapped: " + ex.Message);
            }
        }

        private void StkInsCoverage_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (imgInsCoverage.Source.ToString().Replace("File: ", "") == Constraints.CheckBox_Checked)
                {
                    isInsurance = false;
                    imgInsCoverage.Source = Constraints.CheckBox_UnChecked;
                }
                else
                {
                    isInsurance = true;
                    imgInsCoverage.Source = Constraints.CheckBox_Checked;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PostNewRequiremntPage/StkInsCoverage_Tapped: " + ex.Message);
            }
        }

        private void BtnSubmitRequirement_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnSubmitRequirement);
            SubmitRequirement();
        }

        private void pkCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pkCategory.SelectedIndex > -1 && pkCategory.SelectedItem != null)
                {
                    var catId = mCategories.Where(x => x.Name == pkCategory.SelectedItem.ToString()).Select(x => x.CategoryId).FirstOrDefault();
                    if (catId != null)
                    {
                        GetSubCategoryByCategoryId(catId);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PostNewRequiremntPage/Category_SelectedIndex: " + ex.Message);
            }
        }

        private void BtnCategory_Clicked(object sender, EventArgs e)
        {
            pkCategory.Focus();
        }

        private void BtnSubCategory_Clicked(object sender, EventArgs e)
        {
            pkSubCategory.Focus();
        }

        private void pkSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pkSubCategory.SelectedIndex > -1 && pkSubCategory.SelectedItem != null)
                {
                    if (selectedSubCategory.Where(x => x == pkSubCategory.SelectedItem.ToString()).Count() == 0)
                    {
                        selectedSubCategory.Add(pkSubCategory.SelectedItem.ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PostNewRequiremntPage/pkSubCategory_SelectedIndexChanged: " + ex.Message);
            }
        }

        private void ExpectedDeliveryDate_Tapped(object sender, EventArgs e)
        {
            dpExpectedDeliveryDate.Focus();
        }

        private async void UploadProductImage_Tapped(object sender, EventArgs e)
        {
            try
            {
                Common.BindAnimation(image: ImgUplode);
                ImageConvertion.SelectedImagePath = ImgProductImage;
                ImageConvertion.SetNullSource((int)FileUploadCategory.RequirementImages);
                await ImageConvertion.SelectImage();
                relativePath = await DependencyService.Get<IFileUploadRepository>().UploadFile((int)FileUploadCategory.ProfileDocuments);

            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PostNewRequiremntPage/UploadProductImage: " + ex.Message);
            }
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            var entry = (ExtEntry)sender;
            if (!Common.EmptyFiels(entry.Text))
            {
                UnfocussedFields(entry: entry);
            }
        }

        private void Editor_Unfocused(object sender, FocusEventArgs e)
        {
            var editor = (Editor)sender;
            if (!Common.EmptyFiels(editor.Text))
            {
                UnfocussedFields(editor: editor);
            }
        }

        private void Picker_Unfocused(object sender, FocusEventArgs e)
        {
            var picker = (Picker)sender;
            if (picker.SelectedIndex != -1)
            {
                UnfocussedFields(picker: picker);
            }
        }

        private void StkSameAddress_Tapped(object sender, EventArgs e)
        {
            SameAsBillingAddress();
        }

        private void BtnUnits_Clicked(object sender, EventArgs e)
        {
            pkQuantityUnits.Focus();
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }
        #endregion
    }
}