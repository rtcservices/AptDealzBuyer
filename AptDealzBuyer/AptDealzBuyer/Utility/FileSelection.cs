using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AptDealzBuyer.Utility
{
    public class FileSelection
    {
        public static string fileName = string.Empty;
        public static byte[] fileByte = null;
        public static string fileExtension = string.Empty;
        public static string relativePath;

        public static async Task FilePickup()
        {
            try
            {
                FileData fileData = await CrossFilePicker.Current.PickFile();
                if (fileData == null)
                {
                    fileName = string.Empty;
                    fileByte = null;
                    fileExtension = string.Empty;
                    return;
                }

                fileName = fileData.FileName;
                fileByte = fileData.DataArray;
                fileExtension = Path.GetExtension(fileName);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("FileSelection/FilePickup: " + ex.Message);
            }
        }
    }
}
