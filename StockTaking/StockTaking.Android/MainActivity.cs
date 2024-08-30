using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Android;
using Xamarin.Essentials;
using Xamarin.Forms.Platform.Android;
using Platform = Xamarin.Forms.Platform.Android.Platform;

namespace StockTaking.Droid
{
    [Activity(Label = "Stock Taking", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnResume()
        {
            base.OnResume();
           
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == (int)Android.Content.PM.Permission.Granted)
                {
                }
                else
                {
                    ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.WriteExternalStorage }, 533);
                }
           
                /*if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadMediaAudio) == (int)Android.Content.PM.Permission.Granted)
                {
                }
                else
                {
                    ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadMediaAudio }, 533);
                }

                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadMediaImages) == (int)Android.Content.PM.Permission.Granted)
                {
                }
                else
                {
                    ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadMediaImages }, 534);
                }

                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadMediaVideo) == (int)Android.Content.PM.Permission.Granted)
                {
                }
                else
                {
                    ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadMediaVideo }, 535);
                }*/
            

        }
    }
}