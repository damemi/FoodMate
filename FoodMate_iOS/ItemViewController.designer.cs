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
		UIView ItemView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (ItemView != null) {
				ItemView.Dispose ();
				ItemView = null;
			}
		}
	}
}
