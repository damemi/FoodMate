using System;
using System.IO;

using Foundation;
using UIKit;

using Xamarin.Forms;
using Xamarin.Auth;
using Parse;
using Newtonsoft.Json.Linq;
using Shared;
namespace FoodMate_iOS
{
	partial class ItemViewController : UIViewController
	{
		public string foodName;
		public double price;
		public int amount;
		public string barcode;
		public string objectId;

		public ItemViewController (IntPtr handle) : base (handle)
		{
			foodName = "";
			price = 0;
			amount = 0;
		}
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			itemNameField.Text = foodName;
			itemPriceField.Text = System.Convert.ToString(price);
			itemAmountField.Text = System.Convert.ToString(amount);

			modifyButton.TouchUpInside += async (sender, e) => {
				DatabaseOperations db_op = new DatabaseOperations();
				ParseObject item = await db_op.getFood (objectId);
				item ["name"] = itemNameField.Text;
				item ["in_stock"] = int.Parse(itemAmountField.Text);
				item ["price"] = double.Parse(itemPriceField.Text);
				await item.SaveAsync ();

				MyHomeViewController homeView = (MyHomeViewController)this.Storyboard.InstantiateViewController("MyHomeViewController");
				NavigationController.PushViewController(homeView, true);
			};
		}
	}
}
