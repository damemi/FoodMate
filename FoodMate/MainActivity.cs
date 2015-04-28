using System;
using System.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.App;

using Facebook;
using Android.Graphics;

using Parse;
using Xamarin.Auth;
using System.Linq;
using Shared;

// This is the main controller for the Android app
// The first function called is OnCreate(), below, which sets up the initial layout
// for the user and presents them with a login button

namespace FoodMate
{
	[Activity (Label = "FoodMate", MainLauncher = true, Icon = "@drawable/icon2")]
	public class MainActivity : Activity
	{
		private const string AppId = "716545131791857";
		private const string ExtendedPermissions = "";

		User currentUser;
		bool isLoggedIn;
		string accessToken;
		double expiresIn;
		DateTime expiryDate;

		// This function uses the Xamarin.Auth Facebook component to log in,
		// authorize the user, and set up the tabbed-page interface. It should probably
		// be broken into its own activity somewhere in here.
		async Task LoginToFacebook() {
			var auth = new OAuth2Authenticator (
				clientId: AppId,
				scope: "",
				authorizeUrl: new Uri ("https://m.facebook.com/dialog/oauth/"),
				redirectUrl: new Uri ("http://www.facebook.com/connect/login_success.html"));

			// When the user has finished logging in
			auth.Completed += async (sender, ee) => {
				if (!ee.IsAuthenticated) {
					var builder = new AlertDialog.Builder (this);
					builder.SetMessage ("Not Authenticated");
					builder.SetPositiveButton ("Ok", (o, e) => { });
					builder.Create().Show();
					return;
				}

				// Get info about Facebook account we're accessing
				accessToken = ee.Account.Properties["access_token"].ToString();
				expiresIn = Convert.ToDouble(ee.Account.Properties["expires_in"]);
				expiryDate = DateTime.Now + TimeSpan.FromSeconds( expiresIn );
					
				// Actually make Facebook request now for user info
				var request = new OAuth2Request ("GET", new Uri ("https://graph.facebook.com/me"), null, ee.Account);
				await request.GetResponseAsync().ContinueWith (t => {
					// Some error handling
					var builder = new AlertDialog.Builder (this);
					if (t.IsFaulted) {
						builder.SetTitle ("Error");
						builder.SetMessage (t.Exception.Flatten().InnerException.ToString());
					} else if (t.IsCanceled)
						builder.SetTitle ("Task Canceled");
					else {
						
						//Get user id, store in User object
						var obj = JsonValue.Parse (t.Result.GetResponseText());
						var id = obj["id"].ToString().Replace("\"",""); // Id has extraneous quotation marks
						var user = ParseFacebookUtils.LogInAsync(id, accessToken,expiryDate);
						currentUser = new User(ParseUser.CurrentUser);
						isLoggedIn = true;

						var myIntent = new Intent (this, typeof (HomeScreenActivity));
						myIntent.PutExtra ("userId", currentUser.objId());
						StartActivity (myIntent);
					}
						
				}, UIScheduler);

				//var myIntent = new Intent (this, typeof (HomeScreenActivity));
				//StartActivity (myIntent);
			};

			var intent = auth.GetUI (this);
			StartActivity (intent);

		}
			
		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			if (resultCode == Result.Ok) {
				var homeIntent = new Intent (this, typeof(HomeScreenActivity));
				homeIntent.PutExtra ("userId", data.GetStringExtra ("userId"));
				StartActivity (homeIntent);
			}
		}

		private static readonly TaskScheduler UIScheduler = TaskScheduler.FromCurrentSynchronizationContext();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Connect to Parse API
			ParseClient.Initialize ("zCD97bagtQLE7wZFtACpo6XzJm8OFznvF8ynUJoA", "C5jmH2AOT0T1GqF1tZpUT9bGthdfaqWjFveJbgGY");
			ParseFacebookUtils.Initialize ("716545131791857");

			// Choose which layout to present
			SetContentView (Resource.Layout.Main);

			// Link login button to login action
			var btnLogin = FindViewById<Button> (Resource.Id.btnLogin);
			btnLogin.Click += delegate {
				var task = Task.Run( async() => { await LoginToFacebook(); });
				task.Wait();
			};

		}
			
	}
}


