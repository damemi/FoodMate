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

		async void addItem(String itemName, int itemQuantity, string barcode = "0") {
			DatabaseOperations db_op = new DatabaseOperations();

			await db_op.addNewFood (itemName, 0, itemQuantity, barcode);

		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.addItem);
			var itemName = FindViewById<EditText>(Resource.Id.itemName);
			var itemQuantity = FindViewById<EditText>(Resource.Id.itemQuantity);
			var AddItemButton = FindViewById<Button>(Resource.Id.addItem);
			var buttonScan = FindViewById<Button>(Resource.Id.Barcode);

			string barcode = null;
		
			AddItemButton.Click += delegate { 
				int qty = Convert.ToInt32 (itemQuantity.Text);


				addItem(itemName.Text, qty, barcode); 
				Finish();
			};

			buttonScan.Click += async delegate {
				//NOTE: On Android you MUST pass a Context into the Constructor!
				var scanner = new ZXing.Mobile.MobileBarcodeScanner(this);
				var result = await scanner.Scan();

				if (result != null)
					barcode = result.Text;
					Console.WriteLine("Scanned Barcode: " + result.Text);
			};
		}
	}
}
