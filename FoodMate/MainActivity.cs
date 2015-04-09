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

namespace FoodMate
{
	[Activity (Label = "FoodMate", MainLauncher = true, Icon = "@drawable/icon2")]
	public class MainActivity : FragmentActivity
	{
		private const string AppId = "716545131791857";

		//private const string ExtendedPermissions = "user_about_me,read_stream,publish_stream";
		private const string ExtendedPermissions = "";

		FacebookClient fb;
		string accessToken;
		//GenericFragmentPagerAdaptor adaptor = new GenericFragmentPagerAdaptor (SupportFragmentManager);
		bool isLoggedIn;
		User currentUser;

		async void addItem(String itemName) {
			Console.WriteLine (itemName);
			DatabaseOperations db_op = new DatabaseOperations();
			db_op.addNewFood(itemName, (int)1);
			var foodList = await db_op.getFoods ();
			var sampleTextView = FindViewById<TextView>(Resource.Id.textView1);
			foreach (ParseObject foodObj in foodList) {
				sampleTextView.Text = foodObj.Get<string>("name");
			}
		}

		void LoginToFacebook(/*GenericFragmentPagerAdaptor adaptor*/) {
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


				var accessToken = ee.Account.Properties["access_token"].ToString();
				var expiresIn = Convert.ToDouble(ee.Account.Properties["expires_in"]);
				var expiryDate = DateTime.Now + TimeSpan.FromSeconds( expiresIn );

				//var request = new OAuth2Request ("GET", new Uri ("https://graph.facebook.com/me"), null, ee.Account);
				var request = new OAuth2Request ("GET", new Uri ("https://graph.facebook.com/me"), null, ee.Account);
				request.GetResponseAsync().ContinueWith (t => {
					var builder = new AlertDialog.Builder (this);
					if (t.IsFaulted) {
						builder.SetTitle ("Error");
						builder.SetMessage (t.Exception.Flatten().InnerException.ToString());
					} else if (t.IsCanceled)
						builder.SetTitle ("Task Canceled");
					else {
						var obj = JsonValue.Parse (t.Result.GetResponseText());
						var id = obj["id"].ToString().Replace("\"",""); // Id has extraneous quotation marks

						var user = ParseFacebookUtils.LogInAsync(id, accessToken,expiryDate);
						//currentUser = new User(ParseUser.CurrentUser);
						//currentUser.SaveAsync();

						isLoggedIn = true;
						var btnLogin = FindViewById<Button> (Resource.Id.btnLogin);
						btnLogin.Visibility = ViewStates.Gone;
						SetContentView(Resource.Layout.Home);
						//SetContentView(Resource.Layout.settings);

						ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
						var pager = FindViewById<ViewPager> (Resource.Id.pager);
						var adaptor = new GenericFragmentPagerAdaptor (SupportFragmentManager);
						adaptor.AddFragmentView((i, v, b) =>
							{
								var view = i.Inflate(Resource.Layout.addItem, v, false);
								var sampleTextView = view.FindViewById<TextView>(Resource.Id.textView1);
								sampleTextView.Text = obj["name"];
								var itemName = view.FindViewById<EditText>(Resource.Id.itemName);
								var AddItemButton = view.FindViewById<Button>(Resource.Id.addItem);
								AddItemButton.Click += delegate { addItem(itemName.Text); };
								return view;
							}
						);

						adaptor.AddFragmentView((i, v, b) =>
							{
								var view = i.Inflate(Resource.Layout.tab, v, false);
								var sampleTextView = view.FindViewById<TextView>(Resource.Id.textView1);
								sampleTextView.Text = "Shopping List";
								return view;
							}
						);
						adaptor.AddFragmentView((i, v, b) =>
							{
								var view = i.Inflate(Resource.Layout.tab, v, false);
								var sampleTextView = view.FindViewById<TextView>(Resource.Id.textView1);
								sampleTextView.Text = "My List";
								return view;
							}
						);

						adaptor.AddFragmentView((i, v, b) =>
							{
								var view = i.Inflate(Resource.Layout.settings, v, false);
								var sampleTextView = view.FindViewById<TextView>(Resource.Id.textView2);
								var textInput = view.FindViewById<EditText>(Resource.Id.editText1);
								textInput.Text = "Group name";
								var submitButton = view.FindViewById<Button>(Resource.Id.submitButton);
								sampleTextView.Text = "Settings";
								return view;
							}
						);

						pager.Adapter = adaptor;
						pager.SetOnPageChangeListener(new ViewPageListenerForActionBar(ActionBar));

						ActionBar.Tab tab = ActionBar.NewTab();
						ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "Home"));
						ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "Shopping List"));
						ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "My List"));
						ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "Settings"));

						var settingsPage = adaptor.GetItem(3);
						var editText = FindViewById<TextView>(Resource.Id.textView1);
						editText.Text = "Tested";

						//var view = (LinearLayout) View.Inflate(this, Resource.Layout.settings, null);
						//var settingsPageView = settingsPage.View.FindViewById<EditText>(Resource.Id.editText1);
						//View view = LayoutInflater.From(this).Inflate(Resource.Layout.settings, null);
						//View view = LayoutInflater.Inflate(Resource.Layout.settings, null);

						//var settingsPageView = view.FindViewById<EditText>(Resource.Id.editText1);
						//settingsPageView.Text = "Test";
						//var pager = FindViewById<ViewPager> (Resource.Id.pager);
						//adaptor.NotifyDataSetChanged();
					}

				}, UIScheduler);
			};
			/*
				var response = request.GetResponseAsync();
				var obj = JsonValue.Parse (response.Result.GetResponseText());

				var id = obj["id"].ToString().Replace("\"",""); // Id has extraneous quotation marks

				var user = ParseFacebookUtils.LogInAsync(id, accessToken,expiryDate);
				isLoggedIn = true;
				var btnLogin = FindViewById<Button> (Resource.Id.btnLogin);
				btnLogin.Visibility = ViewStates.Gone;
				Console.WriteLine(id);
				var settingsPage = adaptor.GetItem(3);
				var settingsPageView = settingsPage.View.FindViewById<EditText>(Resource.Id.editText1);
				//settingsPageView.Text = obj["name"].ToString();
				settingsPageView.Text = "H4CKED!!!!";
				*/
			//};

			var intent = auth.GetUI (this);
			StartActivity (intent);

		}


		private static readonly TaskScheduler UIScheduler = TaskScheduler.FromCurrentSynchronizationContext();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			ParseClient.Initialize ("zCD97bagtQLE7wZFtACpo6XzJm8OFznvF8ynUJoA", "C5jmH2AOT0T1GqF1tZpUT9bGthdfaqWjFveJbgGY");
			ParseFacebookUtils.Initialize ("716545131791857");

			SetContentView (Resource.Layout.Main);

			var btnLogin = FindViewById<Button> (Resource.Id.btnLogin);
				

			btnLogin.Click += delegate {
				LoginToFacebook (/*adaptor*/);
			};

			//btnLogin.Visibility = ViewStates.Gone;
			//var inventory = new Intent (this, typeof(InventoryActivity));
			//StartActivity (inventory);

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

				//ImageView imgUser = FindViewById<ImageView> (Resource.Id.imgUser);
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
							//imgUser.SetImageBitmap (bm);
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


