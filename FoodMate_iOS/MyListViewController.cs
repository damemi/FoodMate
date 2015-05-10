using System;
using System.Drawing;
using Foundation;
using UIKit;
using Parse;

using Shared;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FoodMate_iOS
{
	public partial class MyListViewController : UIViewController
	{
		UITableView table;
		List<Food> myRequestFood;
		public MyListViewController (IntPtr handle) : base (handle)
		{
			Title = NSBundle.MainBundle.LocalizedString ("My List", "My List");
			TabBarItem.Image = UIImage.FromBundle ("first");
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			scanButton.TouchUpInside += async (sender, e) => {

				//Make an instance of scanner
				var scanner = new MWBarcodeScanner.Scanner();

				//Call the scaner and wait for result
				var result =  await scanner.Scan();

				//If canceled, result is null
				if (result != null){
					new UIAlertView(result.type, result.code, null, "Close", null).Show();
					AddNewItemViewController newItemView = (AddNewItemViewController)this.Storyboard.InstantiateViewController("AddNewItemViewController");
					newItemView.barCode = result.code;

					var query = ParseObject.GetQuery ("Food").WhereEqualTo ("barcode", result.code);
					var results = await query.FindAsync ();
					foreach(var r in results) {
						newItemView.itemName = r.Get<string>("name");
						newItemView.itemAmount = Convert.ToString(r.Get<int>("in_stock"));
						newItemView.objectId = r.ObjectId;
						newItemView.itemPrice = r.Get<double>("price");
					}

					this.NavigationController.PushViewController(newItemView, true);
				}

			};
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			// After the initial is constructed, create a tableView
			table = new UITableView(new RectangleF(0,60,320,455));
			// Create db object to query data from database
			DatabaseOperations db_op = new DatabaseOperations ();

			var userSettings = NSUserDefaults.StandardUserDefaults;
			Console.WriteLine("User ID is: " + userSettings.StringForKey("objId"));
			// Wait for database query
			var task = Task.Run(async () => { await db_op.getRequestedFoods(userSettings.StringForKey("objId")); });
			task.Wait();

			// Get data of food
			myRequestFood = db_op.RequestedFoods;
			string[] tableItems = new string[myRequestFood.Count];

			// Add data to the table
			for (int i = 0; i < myRequestFood.Count; i++) 
			{
				tableItems [i] = myRequestFood[i].name;
			}

			var source = new HomeTableSource (tableItems);
			table.Source = source;
			Add (table);

			ItemViewController itemView = (ItemViewController)this.Storyboard.InstantiateViewController("ItemViewController");

			source.RowTouched += async (sender, e) => {
				itemView.foodName = myRequestFood[source.currentIndex].getName();
				itemView.price = myRequestFood[source.currentIndex].price;
				itemView.amount = myRequestFood[source.currentIndex].getStock();

				var query = ParseObject.GetQuery ("Food").WhereEqualTo ("name", itemView.foodName);
				var results = await query.FindAsync ();
				foreach(var r in results) {
					itemView.objectId = r.ObjectId;
				}
				this.NavigationController.PushViewController(itemView, true);
			};
		}
	}
}

