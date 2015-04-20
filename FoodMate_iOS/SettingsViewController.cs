using System;
using System.IO;

using Foundation;
using UIKit;

using Xamarin.Forms;
using Xamarin.Auth;
using Parse;
using Newtonsoft.Json.Linq;

namespace FoodMate_iOS
{
	public partial class SettingsViewController : UIViewController
	{
		public SettingsViewController (IntPtr handle) : base (handle)
		{
			Title = NSBundle.MainBundle.LocalizedString ("Settings", "Settings");
			TabBarItem.Image = UIImage.FromBundle ("first");
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();

			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}
		void LoginToFacebook ()
		{

			String appID = File.ReadAllText("appID.txt");
			var auth = new OAuth2Authenticator (
				clientId: appID,
				scope: "",
				authorizeUrl: new Uri ("https://m.facebook.com/dialog/oauth/"),
				redirectUrl: new Uri ("http://www.facebook.com/connect/login_success.html"));

			// If authorization succeeds or is canceled, .Completed will be fired.
			auth.Completed += LoginComplete;

			UIViewController vc = auth.GetUI ();
			PresentViewController (vc, true, null);
		}

		public async void LoginComplete( object sender, AuthenticatorCompletedEventArgs e )
		{
			// We presented the UI, so it's up to us to dismiss it.
			DismissViewController (true, null);

			if (!e.IsAuthenticated) {
				Console.WriteLine ("Not Authorised");
				return;
			}

			var accessToken = e.Account.Properties["access_token"].ToString();
			var expiresIn = Convert.ToDouble(e.Account.Properties["expires_in"]);
			var expiryDate = DateTime.Now + TimeSpan.FromSeconds( expiresIn );

			// Now that we're logged in, make a OAuth2 request to get the user's id.
			var request = new OAuth2Request ("GET", new Uri ("https://graph.facebook.com/me"), null, e.Account);
			var response = await request.GetResponseAsync();
			var obj = JObject.Parse (response.GetResponseText());

			var id = obj["id"].ToString().Replace("\"",""); // Id has extraneous quotation marks

			var user = await ParseFacebookUtils.LogInAsync(id, accessToken,expiryDate);

		}

		partial void FacebookLoginButton_TouchUpInside (UIButton sender)
		{
			LoginToFacebook();
		}
		#endregion
	}

}

