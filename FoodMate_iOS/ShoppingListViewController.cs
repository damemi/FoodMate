﻿using System;
using System.Drawing;
using Foundation;
using UIKit;

using Shared;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FoodMate_iOS
{
	public partial class ShoppingListViewController : UIViewController
	{
		UITableView table;
		public ShoppingListViewController (IntPtr handle) : base (handle)
		{
			Title = NSBundle.MainBundle.LocalizedString ("ShoppingList", "ShoppingList");
			TabBarItem.Image = UIImage.FromBundle ("second");
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			// After the initial is constructed, create a tableView
			table = new UITableView (View.Bounds);
			// Create db object to query data from database
			DatabaseOperations db_op = new DatabaseOperations ();

			// Wait for database query
			var task = Task.Run(async () => { await db_op.getOutOfStockFoods(); });
			task.Wait();

			// Get data of food
			List<Food> OutOfStockFoods = db_op.OutOfStockFoods;
			string[] tableItems = new string[OutOfStockFoods.Count];

			// Add data to the table
			for (int i = 0; i < OutOfStockFoods.Count; i++) 
			{
				tableItems [i] = OutOfStockFoods[i].name;
			}
			table.Source = new TableSource(tableItems);
			Add (table);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}

		partial void CheckoutButton_TouchUpInside (UIButton sender)
		{
			throw new NotImplementedException ();
		}
		#endregion
	

/*		partial void UIButton21_TouchUpInside (UIButton sender)
		{
			//test adding new food
			string name = "asdasfas";
			int price = 14;

			Console.WriteLine("Adding new food, button pressed");
			DatabaseOperations db_op = new DatabaseOperations();
			db_op.addNewFood(name, price);		
		}*/
	}
}

