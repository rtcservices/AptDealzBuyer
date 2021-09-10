using AptDealzBuyer.Utility;
using Newtonsoft.Json;
using System.IO;

namespace AptDealzBuyer.Model.Reponse
{
    public class BuyerFileDocument
    {
        [JsonProperty("fileUri")]
        public string FileUri { get; set; }

        [JsonProperty("relativePath")]
        public string RelativePath { get; set; }

        [JsonIgnore]
        public string DocumentPath
        {
            get
            {
                string extensionDoc = string.Empty;
                string baseURL = (string)App.Current.Resources["BaseURL"];

                if (!Utility.Common.EmptyFiels(FileUri))
                {
                    string extension = Path.GetExtension(FileUri).ToLower();

                    if (extension == ".mp3" || extension == ".wma" || extension == ".acc")
                        extensionDoc = Constraints.Img_Music;
                    else if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                        extensionDoc = baseURL + RelativePath;
                    else if (extension == ".mp4" || extension == ".mov" || extension == ".wmv" || extension == ".qt" || extension == ".gif")
                        extensionDoc = Constraints.Img_Video;
                    else
                        extensionDoc = Constraints.Img_File;
                }
                return extensionDoc;
            }
        }
    }
}
