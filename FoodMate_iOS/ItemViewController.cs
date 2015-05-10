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

		public ItemViewController (IntPtr handle) : base (handle)
		{
			foodName = "";
		}
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			itemNameField.Text = foodName;
		}
	}
}
