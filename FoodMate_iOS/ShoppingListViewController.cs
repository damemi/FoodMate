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
	public partial class ShoppingListViewController : UIViewController
	{
		UITableView table;
		List<Food> OutOfStockFoods;

		public ShoppingListViewController (IntPtr handle) : base (handle)
		{
			Title = NSBundle.MainBundle.LocalizedString ("Shopping List", "Shopping List");
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

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			// After the initial is constructed, create a tableView
			table = new UITableView(new RectangleF(0,60,320,390));
			// Create db object to query data from database
			DatabaseOperations db_op = new DatabaseOperations ();

			// Wait for database query
			var task = Task.Run(async () => { await db_op.getOutOfStockFoods(); });
			task.Wait();

			// Get data of food
			OutOfStockFoods = db_op.OutOfStockFoods;
			string[] tableItems = new string[OutOfStockFoods.Count];

			// Add data to the table
			for (int i = 0; i < OutOfStockFoods.Count; i++) 
			{
				tableItems [i] = OutOfStockFoods[i].name;
			}

			var source = new HomeTableSource (tableItems);
			table.Source = source;
			Add (table);

			ItemViewController itemView = (ItemViewController)this.Storyboard.InstantiateViewController("ItemViewController");

			source.RowTouched += async (sender, e) => {
				itemView.foodName = OutOfStockFoods[source.currentIndex].getName();
				itemView.price = OutOfStockFoods[source.currentIndex].price;
				itemView.amount = OutOfStockFoods[source.currentIndex].getStock();

				var query = ParseObject.GetQuery ("Food").WhereEqualTo ("name", itemView.foodName);
				var results = await query.FindAsync ();
				foreach(var r in results) {
					itemView.objectId = r.ObjectId;
				}
				this.NavigationController.PushViewController(itemView, true);
			};
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

