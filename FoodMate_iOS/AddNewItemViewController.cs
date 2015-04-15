
using System;

using Foundation;
using UIKit;

namespace FoodMate_iOS
{
	public partial class AddNewItemViewController : UIViewController
	{
		public AddNewItemViewController (IntPtr handle) : base (handle)
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
			throw new NotImplementedException ();
		}
	}
}

