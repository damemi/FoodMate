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
	[Register ("SettingsViewController")]
	partial class SettingsViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton FacebookLoginButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView Settings { get; set; }

		[Action ("FacebookLoginButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void FacebookLoginButton_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (FacebookLoginButton != null) {
				FacebookLoginButton.Dispose ();
				FacebookLoginButton = null;
			}
			if (Settings != null) {
				Settings.Dispose ();
				Settings = null;
			}
		}
	}
}
