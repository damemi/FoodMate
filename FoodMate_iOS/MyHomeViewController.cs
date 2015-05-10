using System;
using System.IO;
using System.Drawing;

using Foundation;
using UIKit;

using Xamarin.Forms;
using Xamarin.Auth;
using Parse;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Shared;

namespace FoodMate_iOS
{
	public partial class MyHomeViewController : UIViewController
	{
		UITableView table;
		string[] tableItems;
		List<Food> allFoods;
	
		public MyHomeViewController(IntPtr handle) : base (handle)
		{
			Title = NSBundle.MainBundle.LocalizedString ("MyHome", "MyHome");
			TabBarItem.Image = UIImage.FromBundle ("first");
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
			table = new UITableView(View.Bounds); // defaults to Plain style
			DatabaseOperations db_op = new DatabaseOperations ();
			//List<Food> allFoods;
			var task = Task.Run(async () => { await db_op.getFoods(); });
			task.Wait();

			allFoods = db_op.AllFoods;
			tableItems = new string[allFoods.Count];
			for (int i = 0; i < allFoods.Count; i++) 
			{
				tableItems [i] = allFoods[i].name;
			}
			var source = new HomeTableSource (tableItems);
			table.Source = source;
			Add (table);

			ItemViewController itemView = (ItemViewController)this.Storyboard.InstantiateViewController("ItemViewController");

			source.RowTouched += async (sender, e) => {
				itemView.foodName = allFoods[source.currentIndex].getName();
				itemView.price = allFoods[source.currentIndex].price;
				itemView.amount = allFoods[source.currentIndex].getStock();

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

		#endregion
	}
}

