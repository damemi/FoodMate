
using System;

using Foundation;
using UIKit;
using Shared;

namespace FoodMate_iOS
{
	public partial class AddNewItemViewController : UIViewController
	{
		public AddNewItemViewController (IntPtr ptr) : base (ptr)
		{
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
		}

		partial void UIButton158_TouchUpInside (UIButton sender)
		{
			Console.WriteLine("Adding new food, button pressed");

			DatabaseOperations db_op = new DatabaseOperations();
			double price = Double.Parse(PriceField.Text);
			string name = NameField.Text;
			//double price = [PriceField doubleValue];
			//double price = Convert.ToDouble(PriceField.ToString());
			db_op.addNewFood(name, price);
		}
	}
}

