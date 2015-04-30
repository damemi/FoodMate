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
	[Activity (Label = "Edit Item")]			
	public class EditItemActivity : Activity
	{

		string _objectId;
		string _userId;

		async void editItem(string name, int qty) {
			//Get actual Parse object
			DatabaseOperations db_op = new DatabaseOperations();
			ParseObject item2 = await db_op.getFood (_objectId);

			// Save to database
			item2 ["name"] = name;
			item2 ["in_stock"] = qty;
			await item2.SaveAsync ();
		}

		async void removeItem() {
			//Get actual Parse object
			DatabaseOperations db_op = new DatabaseOperations();
			ParseObject item2 = await db_op.getFood (_objectId);
			await item2.DeleteAsync ();
		}

		async void voteItem() {
			//Get actual Parse object
			DatabaseOperations db_op = new DatabaseOperations();
			ParseObject item2 = await db_op.getFood (_objectId);
			List<object> wanted_by = item2.Get<List<object>> ("wanted_by");
			if (!(wanted_by.Contains(_userId))) {
				wanted_by.Add (_userId);
				Console.WriteLine (_userId);
				item2 ["wanted_by"] = wanted_by;
				await item2.SaveAsync ();
			}
		}

		protected async override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			_objectId = Intent.GetStringExtra ("objectId");
			_userId = Intent.GetStringExtra ("userId");

			SetContentView (Resource.Layout.editItem);

			// Populate with current item data
			var itemNameBox = FindViewById<EditText>(Resource.Id.itemName);
			var itemName = Intent.GetStringExtra ("itemName");
			itemNameBox.Text = itemName;

			DatabaseOperations db_op = new DatabaseOperations();
			ParseObject obj = await db_op.getFood (_objectId);
			List<object> wanted_by = obj.Get<List<object>> ("wanted_by");
			List<User> requestedUsers = new List<User> ();
			foreach (string userId in wanted_by) {
				if (userId == "0" || userId == "1") {
					continue;
				}
				ParseUser user = await db_op.getUser (userId);
				requestedUsers.Add (new User(user));
			}

			ListView requestedList = FindViewById<ListView>(Resource.Id.requestedListView);
			requestedList.Adapter = new RequestedListAdapter(this, requestedUsers);

			FindViewById<EditText> (Resource.Id.itemQuantity).Text = Convert.ToString (Intent.GetIntExtra ("itemStock", 0));

			var EditItemButton = FindViewById<Button>(Resource.Id.editItem);
			EditItemButton.Click += delegate { 
				// Get updated item data
				int newItemQty = Convert.ToInt32(FindViewById<EditText>(Resource.Id.itemQuantity).Text);
				string newItemName = FindViewById<EditText>(Resource.Id.itemName).Text;
				editItem(newItemName, newItemQty); 
				Finish();
			};

			var RemoveItemButton = FindViewById<Button>(Resource.Id.removeItem);
			RemoveItemButton.Click += delegate { 
				// Delete item from inventory
				removeItem(); 
				Finish();
			};

			var VoteItemButton = FindViewById<Button>(Resource.Id.voteForItem);
			VoteItemButton.Click += delegate { 
				// Add user id to list of requested users
				voteItem(); 
				Finish();
			};
		}
	}
}
