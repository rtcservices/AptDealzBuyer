using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Model.Request;
using AptDealzBuyer.Repository;
using AptDealzBuyer.Utility;
using System;
using System.Threading.Tasks;

namespace AptDealzBuyer.Services
{
    public class FileUploadRepository : IFileUploadRepository
    {
        public async Task<string> UploadFile(int fileUploadCategory)
        {
            string relativePath = "";
            try
            {
                if (ImageConvertion.SelectedImageByte != null)
                {
                    var base64String = Convert.ToBase64String(ImageConvertion.SelectedImageByte);
                    var fileName = Guid.NewGuid().ToString() + ".png";

                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    ProfileAPI profileAPI = new ProfileAPI();
                    FileUpload mFileUpload = new FileUpload();

                    mFileUpload.Base64String = base64String;
                    mFileUpload.FileName = fileName;
                    mFileUpload.FileUploadCategory = fileUploadCategory;

                    var mResponse = await profileAPI.FileUpload(mFileUpload);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        var jObject = (Newtonsoft.Json.Linq.JObject)mResponse.Data;
                        if (jObject != null)
                        {
                            var mBuyerFile = jObject.ToObject<Model.Reponse.BuyerFileDocument>();
                            if (mBuyerFile != null)
                            {
                                relativePath = mBuyerFile.relativePath;
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
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("FileUplodeRepository/UplodeFile: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            return relativePath;
        }
    }
}
