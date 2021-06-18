using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Model.Request;
using AptDealzBuyer.Repository;
using AptDealzBuyer.Utility;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
        #endregion

        #region Constructor
        public PostNewRequiremntPage()
        {
            InitializeComponent();
            mCategories = new List<Category>();
            mSubCategories = new List<SubCategory>();
            selectedSubCategory = new List<string>();
            BindCategories();
            CapitalizeWord();
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
                else if (Common.EmptyFiels(txtDescription.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Description);
                }
                else if (Common.EmptyFiels(txtQuantity.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Quantity);
                }
                else if (Common.EmptyFiels(txtLocationPinCode.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Delivery_PinCode);
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
                else if (Common.EmptyFiels(txtSAName.Text))
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
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PostNewRequiremntPage/Validations: " + ex.Message);
            }
            return isValid;
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
                mRequirement.DeliveryLocationPinCode = txtLocationPinCode.Text;

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
                if (!Common.EmptyFiels(txtEstimation.Text))
                {
                    mRequirement.TotalPriceEstimation = Convert.ToInt32(txtEstimation.Text);
                }
                if (!Common.EmptyFiels(txtSourceSupply.Text))
                {
                    mRequirement.PreferredSourceOfSupply = txtSourceSupply.Text;
                }
                if (dpExpectedDeliveryDate.Date != null)
                {
                    mRequirement.ExpectedDeliveryDate = dpExpectedDeliveryDate.Date;
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
                if (Validations())
                {
                    RequirementAPI requirementAPI = new RequirementAPI();
                    var mRequirement = FillRequirement();

                    UserDialogs.Instance.ShowLoading(Constraints.Loading);

                    var mResponse = await requirementAPI.CreateRequirement(mRequirement);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        SuccessfullRequirement();
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

        void SuccessfullRequirement()
        {
            try
            {
                var successPopup = new PopupPages.SuccessPopup();
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
                txtEstimation.Text = string.Empty;
                txtSourceSupply.Text = string.Empty;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PostNewRequiremntPage/ClearPropeties: " + ex.Message);
            }
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
            Common.BindAnimation(image: ImgBack);
            Navigation.PopAsync();
        }

        private void StkPrefer_Tapped(object sender, EventArgs e)
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

        private void StkPickup_Tapped(object sender, EventArgs e)
        {
            if (imgPickup.Source.ToString().Replace("File: ", "") == Constraints.CheckBox_Checked)
            {
                isPickupProduct = false;
                imgPickup.Source = Constraints.CheckBox_UnChecked;
            }
            else
            {
                isPickupProduct = true;
                imgPickup.Source = Constraints.CheckBox_Checked;
            }
        }

        private void StkShipping_Tapped(object sender, EventArgs e)
        {
            if (imgShippingDown.Source.ToString().Replace("File: ", "") == Constraints.Arrow_Down)
            {
                imgShippingDown.Source = Constraints.Arrow_Up;
                grdShippingAddress.IsVisible = true;
            }
            else
            {
                imgShippingDown.Source = Constraints.Arrow_Down;
                grdShippingAddress.IsVisible = false;
            }
        }

        private void StkInsCoverage_Tapped(object sender, EventArgs e)
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

        private void FrmSubmitRequirement_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(frame: FrmSubmitRequirement);
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
            Common.BindAnimation(image: ImgUplode);
            ImageConvertion.SelectedImagePath = ImgProductImage;
            ImageConvertion.SetNullSource((int)FileUploadCategory.ProfileDocuments);
            await ImageConvertion.SelectImage();
            relativePath = await DependencyService.Get<IFileUploadRepository>().UploadFile((int)FileUploadCategory.ProfileDocuments);
        }
        #endregion
    }
}