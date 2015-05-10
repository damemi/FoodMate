
using System;

using Foundation;
using UIKit;
using Shared;
using Parse;

namespace FoodMate_iOS
{
	public partial class AddNewItemViewController : UIViewController
	{
		public string barCode;
		public string objectId;
		public string itemName;
		public string itemAmount;
		public double itemPrice;

		public AddNewItemViewController(IntPtr ptr) : base (ptr)
		{
			barCode = "0";
			objectId = "";
			itemName = "";
			itemAmount = "";
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
			var g = new UITapGestureRecognizer(() => View.EndEditing(true));
			g.CancelsTouchesInView = false; //for iOS5
			View.AddGestureRecognizer(g);
			if (barCode != "0") 
			{
				barCodeField.Text = barCode;
			}
			if (objectId != "" && itemName != "" && itemAmount != "") 
			{
				NameField.Text = itemName;
				amountField.Text = itemAmount;
				PriceField.Text = System.Convert.ToString (itemPrice);
			}

			addNewItemButton.TouchUpInside += async (sender, e) => {
				DatabaseOperations db_op = new DatabaseOperations();

				if (objectId == "") {
					double price = Double.Parse(PriceField.Text);
					string name = NameField.Text;
					int amount = int.Parse(amountField.Text);
					await db_op.addNewFood (name, price, amount, barCode);
				} else {
					ParseObject item = await db_op.getFood (objectId);
					item ["name"] = NameField.Text;
					item ["in_stock"] = int.Parse(amountField.Text);
					item ["price"] = double.Parse(PriceField.Text);
					await item.SaveAsync ();
				}
			};
			// Perform any additional setup after loading the view, typically from a nib.
		}
	}
}

