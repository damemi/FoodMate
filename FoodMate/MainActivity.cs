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
		private const string ExtendedPermissions = "";

		//GenericFragmentPagerAdaptor adaptor = new GenericFragmentPagerAdaptor (SupportFragmentManager);
		User currentUser;
		bool isLoggedIn;

		void updateUser() {
		}

		void updateInventory(ref ViewPager pager) {
			pager.SetCurrentItem (0, true);

			var foodView = FindViewById<ListView>(Resource.Id.ListView);
			//foodView.Adapter = new CustomListAdapter(this, inventory);

			pager.Adapter.NotifyDataSetChanged ();
		}

		void editItemActivity(Object item) {
			Console.WriteLine ("here");
			var myIntent = new Intent (this, typeof (EditItemActivity));
			StartActivityForResult (myIntent, 0);
		}

		async void addItemActivity() {
			var myIntent = new Intent (this, typeof(AddItemActivity));
			//myIntent.PutExtra ("pager", pager);
			StartActivityForResult (myIntent, 0);

			DatabaseOperations db_op = new DatabaseOperations();
			var foodList = await db_op.getFoods ();

			List<Food> inventory = new List<Food>();
			foreach (ParseObject food in foodList) {
				Food newFood = new Food(food);
				inventory.Add(newFood);
			}
			var foodView = FindViewById<ListView>(Resource.Id.ListView);
			foodView.Adapter = new CustomListAdapter(this, inventory);
		}

		async void LoginToFacebook() {
			var auth = new OAuth2Authenticator (
				clientId: AppId,
				scope: "",
				authorizeUrl: new Uri ("https://m.facebook.com/dialog/oauth/"),
				redirectUrl: new Uri ("http://www.facebook.com/connect/login_success.html"));

			auth.Completed += async (sender, ee) => {
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

				DatabaseOperations db_op = new DatabaseOperations();
				var foodList = await db_op.getFoods ();
				List<Food> inventory = new List<Food>();
				foreach (ParseObject food in foodList) {
					Food newFood = new Food(food);
					inventory.Add(newFood);
				}
					
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
						currentUser = new User(ParseUser.CurrentUser);
						var testUser = currentUser;
						//currentUser.SaveAsync();
						isLoggedIn = true;

						if(isLoggedIn) {
							SetContentView(Resource.Layout.Home);
							//SetContentView(Resource.Layout.settings);

							ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
							var pager = FindViewById<ViewPager> (Resource.Id.pager);
							var adaptor = new GenericFragmentPagerAdaptor (SupportFragmentManager);
							adaptor.AddFragmentView( (i, v, b) =>
								{
									var view = i.Inflate(Resource.Layout.inventory, v, false);

									var foodView = view.FindViewById<ListView>(Resource.Id.ListView);
									foodView.Adapter = new CustomListAdapter(this, inventory);	
									//foodView.OnItemClickListener

									foodView.ItemClick += (object sender2, AdapterView.ItemClickEventArgs e) => {
										var item = foodView.GetItemAtPosition(e.Position);
										editItemActivity(item);
									};



									var AddItemButton = view.FindViewById<Button>(Resource.Id.addItemButton);
									AddItemButton.Click += delegate { 
										addItemActivity(); 
										//updateInventory(pager);
									};
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
									var textInput = view.FindViewById<EditText>(Resource.Id.GroupInfo);
									textInput.Text = "Test";//currentUser.getGroup();
									//var submitButton = view.FindViewById<Button>(Resource.Id.submitSettings);

									//Need to get form information to update user settings
									//submitButton += delegate { updateUser(); };
									return view;
								}
							);

							pager.Adapter = adaptor;
							pager.SetOnPageChangeListener(new ViewPageListenerForActionBar(ActionBar));

							ActionBar.Tab tab = ActionBar.NewTab();
							ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "", Resource.Drawable.ic_home));
							ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "", Resource.Drawable.ic_shopping_list));
							ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "", Resource.Drawable.ic_my_list));
							ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "", Resource.Drawable.ic_action_settings));

							var settingsPage = adaptor.GetItem(3);
							var editText = FindViewById<TextView>(Resource.Id.textView1);
							editText.Text = "Tested";
						}
					}
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
			ParseFacebookUtils.Initialize ("716545131791857");

			SetContentView (Resource.Layout.Main);

			var btnLogin = FindViewById<Button> (Resource.Id.btnLogin);
				

			btnLogin.Click += delegate {
				LoginToFacebook ();
			};

			//btnLogin.Visibility = ViewStates.Gone;
			//var inventory = new Intent (this, typeof(InventoryActivity));
			//StartActivity (inventory);

		}
			
	}
}


