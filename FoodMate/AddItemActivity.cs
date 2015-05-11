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
using ZXing;

using Parse;
using Xamarin.Auth;
using System.Linq;
using Shared;
namespace FoodMate
{
	[Activity (Label = "Add Item")]			
	public class AddItemActivity : Activity
	{

		async void addItem(String itemName, int itemQuantity, double itemPrice, string barcode = "0", string objectId = "0") {
			DatabaseOperations db_op = new DatabaseOperations();

			if (objectId == "0") {
				await db_op.addNewFood (itemName, 0, itemQuantity, barcode);
			} else {
				ParseObject item = await db_op.getFood (objectId);
				item ["name"] = itemName;
				item ["in_stock"] = itemQuantity;
				item ["price"] = itemPrice;
				await item.SaveAsync ();
			}

		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.addItem);
			var itemName = FindViewById<EditText>(Resource.Id.itemName);
			var itemQuantity = FindViewById<EditText>(Resource.Id.itemQuantity);
			var itemPrice = FindViewById<EditText>(Resource.Id.Price);
			var AddItemButton = FindViewById<Button>(Resource.Id.addItem);
			var buttonScan = FindViewById<Button>(Resource.Id.Barcode);

			string barcode = null;
			string userId = Intent.GetStringExtra ("userId");
			string objectId = "0";
		
			AddItemButton.Click += delegate { 
				int qty = Convert.ToInt32 (itemQuantity.Text);
				double price = Convert.ToDouble (itemPrice.Text);
				addItem(itemName.Text, qty, price, barcode, objectId); 
				Finish();
			};

			buttonScan.Click += async delegate {
				//NOTE: On Android you MUST pass a Context into the Constructor!
				var scanner = new ZXing.Mobile.MobileBarcodeScanner(this);
				var result = await scanner.Scan();

				if (result != null) {
					barcode = result.Text;
					var query = ParseObject.GetQuery ("Food").WhereEqualTo ("barcode", barcode);
					var results = await query.FindAsync ();
					if(results.Count() > 0) {

						foreach(var r in results) {
							itemName.Text = r.Get<string>("name");
							itemQuantity.Text = Convert.ToString(r.Get<int>("in_stock"));
							objectId = r.ObjectId;
						}

						/*
						DatabaseOperations db_op = new DatabaseOperations();
						string objectId = null;
						foreach(var r in results) {
							objectId = r.ObjectId;
							ParseObject item2 = await db_op.getFood (objectId);
							List<object> wanted_by = item2.Get<List<object>> ("wanted_by");
							if (!(wanted_by.Contains(userId))) {
								wanted_by.Add (userId);
								item2 ["wanted_by"] = wanted_by;
								await item2.SaveAsync ();
							}
						}*/
					}
					Console.WriteLine("Scanned Barcode: " + result.Text);
				}
			};
		}
	}
}
