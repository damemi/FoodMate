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
	[Register ("HomeViewController")]
	partial class HomeViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel HomeLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (HomeLabel != null) {
				HomeLabel.Dispose ();
				HomeLabel = null;
			}
		}
	}
}
