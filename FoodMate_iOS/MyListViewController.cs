
using System;

using Foundation;
using UIKit;

namespace FoodMate_iOS
{
	public partial class MyListViewController : UIViewController
	{
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
				}

			};
		}

	/*	partial void UIButton463_TouchUpInside (UIButton sender)
		{
			AddNewItemViewController newItemController = this.Storyboard.InstantiateViewController("AddNewItemViewController") as AddNewItemViewController;
			if (newItemController != null)
			{
			//	newItemController.CaseID = GetCurrentCaseID();
				this.NavigationController.PushViewController(newItemController, true);
			}  

			//throw new NotImplementedException ();
		}*/
	}
}

