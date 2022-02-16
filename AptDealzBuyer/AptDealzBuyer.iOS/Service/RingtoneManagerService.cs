using AptDealzBuyer.Interfaces;
using AptDealzBuyer.iOS.Service;
using AptDealzBuyer.Utility;
using AudioToolbox;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(RingtoneManagerService))]
namespace AptDealzBuyer.iOS.Service
{
    public class RingtoneManagerService : IRingtoneManager
    {
        Dictionary<int, string> mRingtones { get; set; }

        public Dictionary<int, string> GetRingtones()
        {
            mRingtones = new Dictionary<int, string>();
            try
            {
                mRingtones.Add(1000, "New Mail");
                mRingtones.Add(1001, "Mail Sent");
                mRingtones.Add(1002, "Voicemail");
                mRingtones.Add(1003, "Received Message");
                mRingtones.Add(1004, "Sent Message");
                mRingtones.Add(1005, "Alarm");
                mRingtones.Add(1006, "Low Power");
                mRingtones.Add(1007, "SMS Received1");
                mRingtones.Add(1008, "SMS Received2");
                mRingtones.Add(1009, "SMS Received3");
                mRingtones.Add(1010, "SMS Received4");
                mRingtones.Add(1012, "SMS Received7");
                mRingtones.Add(1013, "SMS Received5");
                mRingtones.Add(1014, "SMS Received6");
                mRingtones.Add(1016, "Tweet Sent");
                mRingtones.Add(1020, "Anticipate");
                mRingtones.Add(1021, "Bloom");
                mRingtones.Add(1022, "Calypso");
                mRingtones.Add(1023, "Choo Choo");
                mRingtones.Add(1024, "Descent");
                mRingtones.Add(1025, "Fanfare");
                mRingtones.Add(1026, "Ladder");
                mRingtones.Add(1027, "Minuet");
                mRingtones.Add(1028, "News Flash");
                mRingtones.Add(1029, "Noir");
                mRingtones.Add(1030, "Sherwood Forest");
                mRingtones.Add(1031, "Spell");
                mRingtones.Add(1032, "Suspense");
                mRingtones.Add(1033, "Telegraph");
                mRingtones.Add(1034, "Tiptoes");
                mRingtones.Add(1035, "Typewriters");
                mRingtones.Add(1036, "Update");
                mRingtones.Add(1050, "ussd");
                mRingtones.Add(1051, "SIMToolkitCallDropped");
                mRingtones.Add(1052, "SIMToolkitGeneralBeep");
                mRingtones.Add(1053, "SIMToolkitNegativeACK");
                mRingtones.Add(1054, "SIMToolkitPositiveACK");
                mRingtones.Add(1055, "SIMToolkitSMS");
                mRingtones.Add(1057, "Tink");
                mRingtones.Add(1070, "CT Busy");
                mRingtones.Add(1071, "CT Congestion");
                mRingtones.Add(1072, "CT Path Ack");
                mRingtones.Add(1073, "CT Error");
                mRingtones.Add(1074, "CT Call Waiting");
                mRingtones.Add(1075, "CT Keytone2");
                mRingtones.Add(1100, "Lock");
                mRingtones.Add(1101, "Unlock");
                mRingtones.Add(1104, "Tock");
                mRingtones.Add(1106, "Beep Beep");
                mRingtones.Add(1107, "Ringer Changed");
                mRingtones.Add(1108, "Photo Shutter");
                mRingtones.Add(1109, "Shake");
                mRingtones.Add(1110, "JBL Begin");
                mRingtones.Add(1111, "JBL Confirm");
                mRingtones.Add(1112, "JBL Cancel");
                mRingtones.Add(1113, "Begin Record");
                mRingtones.Add(1114, "End Record");
                mRingtones.Add(1115, "JBL Ambiguous");
                mRingtones.Add(1116, "JBL No Match");
                mRingtones.Add(1117, "Begin Video Record");
                mRingtones.Add(1118, "End Video Record");
                mRingtones.Add(1150, "VC Invitation Accepted");
                mRingtones.Add(1151, "VC Ringing");
                mRingtones.Add(1152, "VC Ended");
                mRingtones.Add(1200, "DTMF 0");
                mRingtones.Add(1201, "DTMF 1");
                mRingtones.Add(1202, "DTMF 2");
                mRingtones.Add(1203, "DTMF 3");
                mRingtones.Add(1204, "DTMF 4");
                mRingtones.Add(1205, "DTMF 5");
                mRingtones.Add(1206, "DTMF 6");
                mRingtones.Add(1207, "DTMF 7");
                mRingtones.Add(1208, "DTMF 8");
                mRingtones.Add(1209, "DTMF 9");
                mRingtones.Add(1210, "DTMF star");
                mRingtones.Add(1211, "DTMF pound");
                mRingtones.Add(1254, "Long Low Short High");
                mRingtones.Add(1255, "Short Double High");
                mRingtones.Add(1256, "Short Low High");
                mRingtones.Add(1257, "Short Double Low");
                mRingtones.Add(1259, "Middle 9 Short Double Low");
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("iOSRingtoneManagerService/GetRingtones: " + ex.Message);
            }

            return mRingtones;
        }

        //public string OpenNotificationSettings()
        //{
        //    try
        //    {
        //        var settingsString = UIKit.UIApplication.OpenSettingsUrlString;
        //        var url = new NSUrl(settingsString);
        //        UIApplication.SharedApplication.OpenUrl(url);
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.DisplayErrorMessage("iOSRingtoneManagerService/OpenNotificationSettings: " + ex.Message);
        //    }
        //    return "";
        //}

        public void PlayRingTone(int Id)
        {
            var sound = new SystemSound(Convert.ToUInt32(Id));
            sound.PlaySystemSound();
        }

        public void SaveRingTone(int Id)
        {
            try
            {
                var sound = new SystemSound(Convert.ToUInt32(Id));
                sound.PlaySystemSound();
                string SoundFileName = mRingtones.Where(x => x.Key == Id).FirstOrDefault().Value;
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("SaveRingTone-Exception", ex.Message, "Ok");
            }
        }

        //public void StopRingTone(int Id)
        //{
        //    var sound = new SystemSound(Convert.ToUInt32(Id));
        //    sound.Close();
        //}
    }
}