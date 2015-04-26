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
	[Activity (Label = "FoodMate")]			
	public class HomeScreenActivity : FragmentActivity
	{

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
			//var foodList = await db_op.getFoods ();

			// Build generic list to be past to ListView page element
			var task = Task.Run(async() => { await db_op.getFoods (); });
			task.Wait();
			List<Food> inventory = db_op.AllFoods;

			var foodView = FindViewById<ListView>(Resource.Id.ListView);
			foodView.Adapter = new CustomListAdapter(this, inventory);
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			DatabaseOperations db_op = new DatabaseOperations();
			var task = Task.Run(async() => { await db_op.getFoods (); });
			task.Wait();
			List<Food> inventory = db_op.AllFoods;

			var task2 = Task.Run(async() => { await db_op.getOutOfStockFoods(); });
			task2.Wait();
			List<Food> outOfStock = db_op.OutOfStockFoods;

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
				// Shopping list
				adaptor.AddFragmentView((i, v, b) =>
					{
						var view = i.Inflate(Resource.Layout.inventory, v, false);

						//@TODO populate list with foods that have in_stock==0

						var foodView = view.FindViewById<ListView>(Resource.Id.ListView);
						foodView.Adapter = new CustomListAdapter(this, outOfStock);	

						// Delegate item to launch "Edit" activity and button to launch "Add" activity
						foodView.ItemClick += (object sender2, AdapterView.ItemClickEventArgs e) => {
							Food item = outOfStock[e.Position];
							editItemActivity(item);

						};

						var AddItemButton = view.FindViewById<Button>(Resource.Id.addItemButton);
						AddItemButton.Click += delegate { 
							addItemActivity(); 
						};

						return view;
					}
				);

				// My List
				adaptor.AddFragmentView((i, v, b) =>
					{
						var view = i.Inflate(Resource.Layout.inventory, v, false);

						//@TODO populate list with foods that have user_id in the list of
						//		users that have requested it

						return view;
					}
				);

				// Basic user settings page with inputs. In-progress
				adaptor.AddFragmentView((i, v, b) =>
					{
						var view = i.Inflate(Resource.Layout.settings, v, false);
						//var sampleTextView = view.FindViewById<TextView>(Resource.Id.textView2);
						//var textInput = view.FindViewById<EditText>(Resource.Id.GroupInfo);
						//textInput.Text = "Test";//currentUser.getGroup();
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

				//var settingsPage = adaptor.GetItem(3);
				//var editText = FindViewById<TextView>(Resource.Id.textView1);
				//editText.Text = "Tested";


		}

	}
}

