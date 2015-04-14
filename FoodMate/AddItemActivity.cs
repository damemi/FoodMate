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
	[Activity (Label = "Add Item")]			
	public class AddItemActivity : Activity
	{

		async void addItem(String itemName, int itemQuantity) {
			DatabaseOperations db_op = new DatabaseOperations();

			await db_op.addNewFood(itemName, itemQuantity, 0, 0);
			var foodList = await db_op.getFoods ();

			List<Food> inventory = new List<Food>();
			foreach (ParseObject food in foodList) {
				Food newFood = new Food(food);
				inventory.Add(newFood);
			}
			var foodView = FindViewById<ListView>(Resource.Id.ListView);
			foodView.Adapter = new CustomListAdapter(this, inventory);
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.addItem);
			var itemName = FindViewById<EditText>(Resource.Id.itemName);
			var itemQuantity = FindViewById<EditText>(Resource.Id.itemQuantity);
			var AddItemButton = FindViewById<Button>(Resource.Id.addItem);
		
			AddItemButton.Click += delegate { 
				int qty = Convert.ToInt32 (itemQuantity.Text);
				addItem(itemName.Text, qty); 
				Finish();};
		}
	}
}
