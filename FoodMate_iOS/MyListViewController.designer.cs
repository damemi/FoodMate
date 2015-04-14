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
	[Register ("MyListViewController")]
	partial class MyListViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView MyList { get; set; }

		[Action ("UIButton463_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void UIButton463_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (MyList != null) {
				MyList.Dispose ();
				MyList = null;
			}
		}
	}
}
