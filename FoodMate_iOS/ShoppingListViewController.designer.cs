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
		UILabel ShoppingListLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (CheckoutButton != null) {
				CheckoutButton.Dispose ();
				CheckoutButton = null;
			}
			if (ShoppingListLabel != null) {
				ShoppingListLabel.Dispose ();
				ShoppingListLabel = null;
			}
		}
	}
}
