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
	public class RequestItemActivity : Activity
	{

		string _userId;

		async void requestItem(string name) {
			DatabaseOperations db_op = new DatabaseOperations();

			await db_op.requestNewFood (name, 0, 0, _userId);
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			_userId = Intent.GetStringExtra ("userId");

			SetContentView (Resource.Layout.addItem);

			FindViewById<EditText> (Resource.Id.itemQuantity).Text = "0";

			var AddItemButton = FindViewById<Button>(Resource.Id.addItem);
			AddItemButton.Click += delegate { 
				string itemName = FindViewById<EditText>(Resource.Id.itemName).Text;
				requestItem(itemName); 
				Finish();
			};
		}
	}
}
