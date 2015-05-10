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
	[Register ("MyHomeViewController")]
	partial class MyHomeViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView Home { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton scanButton { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (Home != null) {
				Home.Dispose ();
				Home = null;
			}
			if (scanButton != null) {
				scanButton.Dispose ();
				scanButton = null;
			}
		}
	}
}
