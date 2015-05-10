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
	[Register ("AddNewItemViewController")]
	partial class AddNewItemViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton addNewItemButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField amountField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField barCodeField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField NameField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField PriceField { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (addNewItemButton != null) {
				addNewItemButton.Dispose ();
				addNewItemButton = null;
			}
			if (amountField != null) {
				amountField.Dispose ();
				amountField = null;
			}
			if (barCodeField != null) {
				barCodeField.Dispose ();
				barCodeField = null;
			}
			if (NameField != null) {
				NameField.Dispose ();
				NameField = null;
			}
			if (PriceField != null) {
				PriceField.Dispose ();
				PriceField = null;
			}
		}
	}
}
