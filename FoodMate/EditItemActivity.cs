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
		async void editItem(String itemName) {
			Console.WriteLine (itemName);
			DatabaseOperations db_op = new DatabaseOperations();
			db_op.addNewFood(itemName, (int)1);
			var foodList = await db_op.getFoods ();

			/*
			var sampleTextView = FindViewById<TextView>(Resource.Id.textView1);

			foreach (ParseObject foodObj in foodList) {
				sampleTextView.Text = foodObj.Get<string>("name");
			}
			*/


			List<Food> inventory = new List<Food>();
			foreach (ParseObject food in foodList) {
				Food newFood = new Food(food);
				inventory.Add(newFood);
			}
			//			var foodView = FindViewById<ListView>(Resource.Id.ListView);
			//			foodView.Adapter = new CustomListAdapter(this, inventory);
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.editItem);
			var itemName = FindViewById<EditText>(Resource.Id.itemName);
			var AddItemButton = FindViewById<Button>(Resource.Id.addItem);
			AddItemButton.Click += delegate { editItem(itemName.Text); Finish();};
		}
	}
}
