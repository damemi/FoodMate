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
	[Register ("ShoppingListViewController")]
	partial class ShoppingListViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton CheckoutButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton scanButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView ShoppingList { get; set; }

		[Action ("CheckoutButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void CheckoutButton_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (CheckoutButton != null) {
				CheckoutButton.Dispose ();
				CheckoutButton = null;
			}
			if (scanButton != null) {
				scanButton.Dispose ();
				scanButton = null;
			}
			if (ShoppingList != null) {
				ShoppingList.Dispose ();
				ShoppingList = null;
			}
		}
	}
}
