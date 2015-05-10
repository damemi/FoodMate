// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace FoodMate_iOS
{
	[Register ("ItemViewController")]
	partial class ItemViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField itemAmountField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField itemNameField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField itemPriceField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView ItemView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton modifyButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton requestButton { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (itemAmountField != null) {
				itemAmountField.Dispose ();
				itemAmountField = null;
			}
			if (itemNameField != null) {
				itemNameField.Dispose ();
				itemNameField = null;
			}
			if (itemPriceField != null) {
				itemPriceField.Dispose ();
				itemPriceField = null;
			}
			if (ItemView != null) {
				ItemView.Dispose ();
				ItemView = null;
			}
			if (modifyButton != null) {
				modifyButton.Dispose ();
				modifyButton = null;
			}
			if (requestButton != null) {
				requestButton.Dispose ();
				requestButton = null;
			}
		}
	}
}
