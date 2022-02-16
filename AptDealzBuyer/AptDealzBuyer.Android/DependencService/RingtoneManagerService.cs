using Android.App;
using Android.Media;
using AptDealzBuyer.Droid.DependencService;
using AptDealzBuyer.Interfaces;
using AptDealzBuyer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;

[assembly: Xamarin.Forms.Dependency(typeof(RingtoneManagerService))]
namespace AptDealzBuyer.Droid.DependencService
{
    public class RingtoneManagerService : IRingtoneManager
    {
#pragma warning disable CS0618 // Type or member is obsolete
        private Android.Media.Ringtone SelectedRingtone { get; set; }
        NotificationManager notificationManager;

        public Dictionary<int, string> GetRingtones()
        {
            Dictionary<int, string> mRingtones = new Dictionary<int, string>();

            List<Android.Net.Uri> alarms = new List<Android.Net.Uri>();
            var activity = (MainActivity)Forms.Context;

            try
            {
                RingtoneManager ringtoneMgr = new RingtoneManager(activity);
                ringtoneMgr.SetType(RingtoneType.Notification);

                var alarmsCursor = ringtoneMgr.Cursor;
                int alarmsCount = alarmsCursor.Count;
                if (alarmsCount == 0 && !alarmsCursor.MoveToFirst())
                {
                    alarmsCursor.Close();
                    return null;
                }

                while (!alarmsCursor.IsAfterLast && alarmsCursor.MoveToNext())
                {
                    int id = 0;
                    string title = "";

                    int currentPosition = alarmsCursor.Position;
                    var mRingToneuri = ringtoneMgr.GetRingtoneUri(currentPosition);


                    alarms.Add(mRingToneuri);
                    if (!Common.EmptyFiels(mRingToneuri.Query))
                    {
                        title = mRingToneuri.Query.Split('&')[0].Replace("title=", "");//title=Argo Navis&canonical=1
                    }
                    int.TryParse(mRingToneuri.LastPathSegment, out id);

                    if (id > 0 && !Common.EmptyFiels(title))
                        mRingtones.Add(id, title);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RingtoneManagerService/GetRingtones: " + ex.Message);
            }

            return mRingtones;
        }

        public void PlayRingTone(int Id)
        {
            try
            {
                List<Android.Net.Uri> alarms = new List<Android.Net.Uri>();
                var activity = (MainActivity)Forms.Context;

                RingtoneManager ringtoneMgr = new RingtoneManager(activity);
                ringtoneMgr.SetType(RingtoneType.Notification);

                var alarmsCursor = ringtoneMgr.Cursor;
                int alarmsCount = alarmsCursor.Count;
                if (alarmsCount == 0 && !alarmsCursor.MoveToFirst())
                {
                    alarmsCursor.Close();
                    return;
                }

                while (!alarmsCursor.IsAfterLast && alarmsCursor.MoveToNext())
                {
                    int currentPosition = alarmsCursor.Position;
                    alarms.Add(ringtoneMgr.GetRingtoneUri(currentPosition));
                }

                if (alarms != null && alarms.Count > 0)
                {
                    var mRingToneuri = alarms.Where(x => x.LastPathSegment == Id.ToString()).FirstOrDefault();

                    InstanceStopPlaying();

                    SelectedRingtone = RingtoneManager.GetRingtone(activity, mRingToneuri);
                    SelectedRingtone.Play();
                    SelectedRingtone.Volume = 100;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RingtoneManagerService/PlayRingTone: " + ex.Message);
            }
        }

        public void SaveRingTone(int Id)
        {
            try
            {
                List<Android.Net.Uri> alarms = new List<Android.Net.Uri>();
                var activity = (MainActivity)Forms.Context;

                RingtoneManager ringtoneMgr = new RingtoneManager(activity);
                ringtoneMgr.SetType(RingtoneType.Notification);

                var alarmsCursor = ringtoneMgr.Cursor;
                int alarmsCount = alarmsCursor.Count;
                if (alarmsCount == 0 && !alarmsCursor.MoveToFirst())
                {
                    alarmsCursor.Close();
                    return;
                }

                while (!alarmsCursor.IsAfterLast && alarmsCursor.MoveToNext())
                {
                    int currentPosition = alarmsCursor.Position;
                    var mRingToneuri = ringtoneMgr.GetRingtoneUri(currentPosition);
                    alarms.Add(mRingToneuri);
                }

                if (alarms != null && alarms.Count > 0)
                {
                    var mRingToneuri = alarms.Where(x => x.LastPathSegment == Id.ToString()).FirstOrDefault();

                    if (mRingToneuri != null)
                    {
                        MainActivity.DefaultNotificationSoundURI = mRingToneuri;
                        RingtoneManager.SetActualDefaultRingtoneUri(activity, RingtoneType.Notification, mRingToneuri);

                        if (!Common.EmptyFiels(mRingToneuri.Query))
                        {
                            Settings.NotificationToneName = mRingToneuri.Query.Split('&')[0].Replace("title=", "");
                        }
                    }

                    notificationManager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);
                    var mChannels = notificationManager.NotificationChannels;
                    if (mChannels != null)
                    {
                        foreach (var mChannel in mChannels)
                        {
                            notificationManager.DeleteNotificationChannel(mChannel.Id);
                        }
                    }

                    string channelId = "fcm_fallback_notification_channel";
                    string channelName = "Miscellaneous";
                    SetDefaultNotificationSound(mRingToneuri, channelId, channelName);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RingtoneManagerService/SaveRingTone: " + ex.Message);
            }
        }

        public void SetDefaultNotificationSound(Android.Net.Uri mRingToneuri, string channelId, string channelName)
        {
            try
            {
                var importance = NotificationImportance.High;
                NotificationChannel chan = new NotificationChannel(channelId, channelName, importance);
                chan.EnableVibration(true);
                chan.LockscreenVisibility = NotificationVisibility.Public;
                if (mRingToneuri != null)
                {
                    var alarmAttributes = new AudioAttributes.Builder()
                                        .SetContentType(AudioContentType.Sonification)
                                        .SetUsage(AudioUsageKind.NotificationRingtone).Build();
                    chan.SetSound(mRingToneuri, alarmAttributes);
                }

                DependencyService.Get<NotificationHelper>().CreateNotificationChannel(chan);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RingtoneManagerService/SetDefaultNotificationSound: " + ex.Message);
            }
        }

        bool InstanceStopPlaying()
        {
            bool isPlay = false;
            try
            {
                if (SelectedRingtone != null)
                {
                    SelectedRingtone.Stop();
                    SelectedRingtone = null;
                    isPlay = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RingtoneManagerService/InstanceStopPlaying: " + ex.Message);
            }
            return isPlay;
        }
#pragma warning restore CS0618 // Type or member is obsolete
    }
}