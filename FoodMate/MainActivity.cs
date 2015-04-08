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

namespace FoodMate
{
	[Activity (Label = "FoodMate", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		private const string AppId = "716545131791857";

		//private const string ExtendedPermissions = "user_about_me,read_stream,publish_stream";
		private const string ExtendedPermissions = "";

		FacebookClient fb;
		string accessToken;
		bool isLoggedIn;

		void LoginToFacebook() {
			var auth = new OAuth2Authenticator (
				clientId: AppId,
				scope: "",
				authorizeUrl: new Uri ("https://m.facebook.com/dialog/oauth/"),
				redirectUrl: new Uri ("http://www.facebook.com/connect/login_success.html"));

			auth.Completed += (sender, ee) => {
				if (!ee.IsAuthenticated) {
					var builder = new AlertDialog.Builder (this);
					builder.SetMessage ("Not Authenticated");
					builder.SetPositiveButton ("Ok", (o, e) => { });
					builder.Create().Show();
					return;
				}


				//var accessToken = ee.Account.Properties["access_token"].ToString();
				//var expiresIn = Convert.ToDouble(ee.Account.Properties["expires_in"]);
				//var expiryDate = DateTime.Now + TimeSpan.FromSeconds( expiresIn );


				var request = new OAuth2Request ("GET", new Uri ("https://graph.facebook.com/me"), null, ee.Account);
				/*var response = request.GetResponseAsync();
				var obj = JsonValue.Parse (response.Result.GetResponseText());

				var id = obj["id"].ToString().Replace("\"",""); // Id has extraneous quotation marks

				var user = ParseFacebookUtils.LogInAsync(id, accessToken,expiryDate);

				var inventory = new Intent (this, typeof(InventoryActivity));
				StartActivity (inventory);*/

				request.GetResponseAsync().ContinueWith (t => {
					var builder = new AlertDialog.Builder (this);
					if (t.IsFaulted) {
						builder.SetTitle ("Error");
						builder.SetMessage (t.Exception.Flatten().InnerException.ToString());
					} else if (t.IsCanceled)
						builder.SetTitle ("Task Canceled");
					else {
						var obj = JsonValue.Parse (t.Result.GetResponseText());

						builder.SetTitle ("Logged in");
						builder.SetMessage ("Name: " + obj["name"]);
					}

					builder.SetPositiveButton ("Ok", (o, e) => { });
					builder.Create().Show();
				}, UIScheduler);

			};

			var intent = auth.GetUI (this);
			StartActivity (intent);

		}


		private static readonly TaskScheduler UIScheduler = TaskScheduler.FromCurrentSynchronizationContext();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			ParseClient.Initialize ("zCD97bagtQLE7wZFtACpo6XzJm8OFznvF8ynUJoA", "C5jmH2AOT0T1GqF1tZpUT9bGthdfaqWjFveJbgGY");

			SetContentView (Resource.Layout.Main);

			var btnLogin = FindViewById<Button> (Resource.Id.btnLogin);

			/*btnLogin.Click += (sender, e) => {
				var webAuth = new Intent (this, typeof(FBWebViewAuthActivity));
				webAuth.PutExtra ("AppId", AppId);
				webAuth.PutExtra ("ExtendedPermissions", ExtendedPermissions);
				StartActivityForResult (webAuth, 0);
			};*/
			btnLogin.Click += delegate {
				LoginToFacebook ();
			};

			/*
			if (isLoggedIn) {
				btnLogin.Visibility = ViewStates.Gone;
				var inventory = new Intent (this, typeof(InventoryActivity));
				StartActivity (inventory);
			}*/

		}
			
		public async void LoginComplete( object sender, AuthenticatorCompletedEventArgs e )
		{
			// We presented the UI, so it's up to us to dismiss it.
			//DismissViewController (true, null);

			if (!e.IsAuthenticated) {
				Console.WriteLine ("Not Authorised");
				return;
			}

			var accessToken = e.Account.Properties["access_token"].ToString();
			var expiresIn = Convert.ToDouble(e.Account.Properties["expires_in"]);
			var expiryDate = DateTime.Now + TimeSpan.FromSeconds( expiresIn );

			// Now that we're logged in, make a OAuth2 request to get the user's id.
			var request = new OAuth2Request ("GET", new Uri ("https://graph.facebook.com/me"), null, e.Account);
			var response = await request.GetResponseAsync();
			//var obj = JObject.Parse (response.GetResponseText());

			//var id = obj["id"].ToString().Replace("\"",""); // Id has extraneous quotation marks
			Console.WriteLine (response.GetResponseText ());

			var user = await ParseFacebookUtils.LogInAsync("123", accessToken,expiryDate);

		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);

			switch (resultCode) {
			case Result.Ok:

				accessToken = data.GetStringExtra ("AccessToken");
				string userId = data.GetStringExtra ("UserId");
				string error = data.GetStringExtra ("Exception");

				fb = new FacebookClient (accessToken);

				ImageView imgUser = FindViewById<ImageView> (Resource.Id.imgUser);
				TextView txtvUserName = FindViewById<TextView> (Resource.Id.txtvUserName);

				fb.GetTaskAsync ("me").ContinueWith( t => {
					if (!t.IsFaulted) {

						var result = (IDictionary<string, object>)t.Result;
						
						// available picture types: square (50x50), small (50xvariable height), large (about 200x variable height) (all size in pixels)
						// for more info visit http://developers.facebook.com/docs/reference/api
						string profilePictureUrl = string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}", userId, "square", accessToken);
						var bm = BitmapFactory.DecodeStream (new Java.Net.URL(profilePictureUrl).OpenStream());
						string profileName = (string)result["name"];
						//int fbid = (int)result["id"];
						
						RunOnUiThread (()=> {
							imgUser.SetImageBitmap (bm);
							txtvUserName.Text = profileName;
						});
						Console.WriteLine(userId);
						//var user = ParseFacebookUtils.LogInAsync(userId, accessToken);
						isLoggedIn = true;
					} else {
						Alert ("Failed to Log In", "Reason: " + error, false, (res) => {} );
					}
				});

				break;
			case Result.Canceled:
				Alert ("Failed to Log In", "User Cancelled", false, (res) => {} );
				break;
			default:
				break;
			}
		}

		public void Alert (string title, string message, bool CancelButton , Action<Result> callback)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder(this);
			builder.SetTitle(title);
			builder.SetIcon(Resource.Drawable.Icon);
			builder.SetMessage(message);

			builder.SetPositiveButton("Ok", (sender, e) => {
				callback(Result.Ok);
			});

			if (CancelButton) {
				builder.SetNegativeButton("Cancel", (sender, e) => {
					callback(Result.Canceled);
				});
			}
			
			builder.Show();
		}

	}
}


