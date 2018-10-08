﻿using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Microsoft.WindowsAzure.MobileServices;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.Threading.Tasks;

namespace ToDooList.Droid
{
	[Activity (Label = "ToDooList.Droid",
		Icon = "@drawable/icon",
		MainLauncher = true,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
		Theme = "@android:style/Theme.Holo.Light")]
    //public class MainActivity : FormsApplicationActivity
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity, IAuthenticate
    {

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            // Initialize the authenticator before loading the app.
            App.Init((IAuthenticate)this);

            // Initialize Azure Mobile Apps
            CurrentPlatform.Init();

			// Initialize Xamarin Forms
			Forms.Init (this, bundle);

            // Load the main application
            LoadApplication (new App ());
        }

        // Define a authenticated user.
        private MobileServiceUser user;

        public async Task<bool> Authenticate()
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Google login using a server-managed flow.
                user = await TodoItemManager.DefaultManager.CurrentClient.LoginAsync(this,
                    MobileServiceAuthenticationProvider.Google, "todoolist");
                if (user != null)
                {
                    message = string.Format("Kirjautuminen onnistui, SID: {0}.",
                        user.UserId);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            // Display the success or failure message.
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle("Kirjautumisen tila");
            builder.Create().Show();

            return success;
        }
    }
}

