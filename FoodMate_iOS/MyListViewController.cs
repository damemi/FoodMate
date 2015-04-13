
using System;

using Foundation;
using UIKit;

namespace FoodMate_iOS
{
	public partial class MyListViewController : UIViewController
	{
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public MyListViewController ()
			: base (UserInterfaceIdiomIsPhone ? "MyListViewController_iPhone" : "MyListViewController_iPad", null)
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

		partial void UIButton463_TouchUpInside (UIButton sender)
		{
			AddNewItemViewController newItemController = this.Storyboard.InstantiateViewController("AddNewItemViewController") as AddNewItemViewController;
			if (newItemController != null)
			{
			//	newItemController.CaseID = GetCurrentCaseID();
				this.NavigationController.PushViewController(newItemController, true);
			}  

			//throw new NotImplementedException ();
		}
	}
}

