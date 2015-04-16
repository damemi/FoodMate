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
	public class MainActivity : FragmentActivity
	{
		private const string AppId = "716545131791857";
		private const string ExtendedPermissions = "";

		User currentUser;
		bool isLoggedIn;

		// @TODO: Present new user information
		void updateUser() {
		}

		// In progress: update display of inventory dynamically
		void updateInventory(ref ViewPager pager) {
			pager.SetCurrentItem (0, true);
			var foodView = FindViewById<ListView>(Resource.Id.ListView);
			pager.Adapter.NotifyDataSetChanged ();
		}

		// Helper function to launch the "Edit Item" window
		void editItemActivity(Food item) {
			var myIntent = new Intent (this, typeof (EditItemActivity));

			// Pass data about the item to the next window
			myIntent.PutExtra ("objectId", item.objId ());
			myIntent.PutExtra ("itemName", item.getName ());
			myIntent.PutExtra ("itemStock", item.getStock ());

			// Start window
			StartActivityForResult (myIntent, 0);
		}

		// Function to add item and update item display screen
		// Currently, the part of the code that updates is firing as soon
		//   as the user chooses the add item button. Need it to fire after,
		//   so that the new item is selected in the DB query and updated to display.
		async void addItemActivity() {
			var myIntent = new Intent (this, typeof(AddItemActivity));
			StartActivityForResult (myIntent, 0);

			// Connect to database and  get food list
			DatabaseOperations db_op = new DatabaseOperations();
			var foodList = await db_op.getFoods ();

			// Build generic list to be past to ListView page element
			List<Food> inventory = new List<Food>();
			foreach (ParseObject food in foodList) {
				Food newFood = new Food(food);
				inventory.Add(newFood);
			}
			var foodView = FindViewById<ListView>(Resource.Id.ListView);
			foodView.Adapter = new CustomListAdapter(this, inventory);
		}

		// This function uses the Xamarin.Auth Facebook component to log in,
		// authorize the user, and set up the tabbed-page interface. It should probably
		// be broken into its own activity somewhere in here.
		async void LoginToFacebook() {
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
				var accessToken = ee.Account.Properties["access_token"].ToString();
				var expiresIn = Convert.ToDouble(ee.Account.Properties["expires_in"]);
				var expiryDate = DateTime.Now + TimeSpan.FromSeconds( expiresIn );

				// Kind of a side step, but we'll need the inventory to display further down
				// so grab it now, build into a generic list of Food objects
				DatabaseOperations db_op = new DatabaseOperations();
				var foodList = await db_op.getFoods ();
				List<Food> inventory = new List<Food>();
				foreach (ParseObject food in foodList) {
					Food newFood = new Food(food);
					inventory.Add(newFood);
				}
					
				// Actually make Facebook request now for user info
				var request = new OAuth2Request ("GET", new Uri ("https://graph.facebook.com/me"), null, ee.Account);
				request.GetResponseAsync().ContinueWith (t => {
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

						if(isLoggedIn) {
							// Start doing tabbed display stuff. This works like this:
							// "Home" layout page has a "ViewPager" element inside of it.
							// The viewpager works like an i-frame, showing the various other layouts
							// for the other tabs (that we will provide). Then, the built-in "ActionBar" in 
							// Android is told to turn all of these viewpages into tabs.
							SetContentView(Resource.Layout.Home);

							// Set up the actionbar
							ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

							// pager is the main ViewPager element, adaptor holds all of the pages
							var pager = FindViewById<ViewPager> (Resource.Id.pager);
							var adaptor = new GenericFragmentPagerAdaptor (SupportFragmentManager);

							// Add pages to the adapter. Similar to just setting up any view
							adaptor.AddFragmentView( (i, v, b) =>
								{
									var view = i.Inflate(Resource.Layout.inventory, v, false);

									// The list of inventory items is even more tiered.
									// Main "ListView" element in the "inventory" layout page, which is given an
									// adapter which will hold each element in the list. The elements in the list
									// also have their own layout, and the adapter is a custom type we've defined.
									var foodView = view.FindViewById<ListView>(Resource.Id.ListView);
									foodView.Adapter = new CustomListAdapter(this, inventory);	

									// Delegate item to launch "Edit" activity and button to launch "Add" activity
									foodView.ItemClick += (object sender2, AdapterView.ItemClickEventArgs e) => {
										Food item = inventory[e.Position];
										editItemActivity(item);

									};

									var AddItemButton = view.FindViewById<Button>(Resource.Id.addItemButton);
									AddItemButton.Click += delegate { 
										addItemActivity(); 
									};
									return view;
								}
							);

							// Next two are not yet implemented
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

							// Basic user settings page with inputs. In-progress
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

							// Give the pager the adapter full of pages, give the action bar the tabs and
							// icons to display for each tab, and finish.
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

			// Connect to Parse API
			ParseClient.Initialize ("zCD97bagtQLE7wZFtACpo6XzJm8OFznvF8ynUJoA", "C5jmH2AOT0T1GqF1tZpUT9bGthdfaqWjFveJbgGY");
			ParseFacebookUtils.Initialize ("716545131791857");

			// Choose which layout to present
			SetContentView (Resource.Layout.Main);

			// Link login button to login action
			var btnLogin = FindViewById<Button> (Resource.Id.btnLogin);
			btnLogin.Click += delegate {
				LoginToFacebook ();
			};

		}
			
	}
}


