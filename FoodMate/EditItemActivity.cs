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

		async void editItem(string name, int qty) {
			//Get actual Parse object
			DatabaseOperations db_op = new DatabaseOperations();
			ParseObject item2 = await db_op.getFood (_objectId);

			// Save to database
			item2 ["name"] = name;
			item2 ["in_stock"] = qty;
			await item2.SaveAsync ();
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			_objectId = Intent.GetStringExtra ("objectId");

			SetContentView (Resource.Layout.editItem);

			// Populate with current item data
			var itemNameBox = FindViewById<EditText>(Resource.Id.itemName);
			var itemName = Intent.GetStringExtra ("itemName");
			itemNameBox.Text = itemName;

			FindViewById<EditText> (Resource.Id.itemQuantity).Text = Convert.ToString (Intent.GetIntExtra ("itemStock", 0));

			var EditItemButton = FindViewById<Button>(Resource.Id.editItem);
			EditItemButton.Click += delegate { 
				// Get updated item data
				int newItemQty = Convert.ToInt32(FindViewById<EditText>(Resource.Id.itemQuantity).Text);
				string newItemName = FindViewById<EditText>(Resource.Id.itemName).Text;
				editItem(newItemName, newItemQty); 
				Finish();
			};
		}
	}
}
