using System;
using System.IO;
using System.Drawing;

using Foundation;
using UIKit;

using Xamarin.Forms;
using Xamarin.Auth;
using Parse;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Shared;

namespace FoodMate_iOS
{
	public partial class MyHomeViewController : UIViewController
	{
		UITableView table;
	
		public MyHomeViewController(IntPtr handle) : base (handle)
		{
			Title = NSBundle.MainBundle.LocalizedString ("MyHome", "MyHome");
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
			scanButton.TouchUpInside += async (sender, e) => {

				//Make an instance of scanner
				var scanner = new MWBarcodeScanner.Scanner();

				//Call the scaner and wait for result
				var result =  await scanner.Scan();

				//If canceled, result is null
				if (result != null){
					new UIAlertView(result.type, result.code, null, "Close", null).Show();
				}

			};
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{

			base.ViewDidAppear (animated);
			table = new UITableView(View.Bounds); // defaults to Plain style
			DatabaseOperations db_op = new DatabaseOperations ();
			//List<Food> allFoods;
			var task = Task.Run(async () => { await db_op.getFoods(); });
			task.Wait();

			List<Food> allFoods = db_op.AllFoods;
			string[] tableItems = new string[allFoods.Count];
			for (int i = 0; i < allFoods.Count; i++) 
			{
				tableItems [i] = allFoods[i].name;
			}
			var source = new HomeTableSource (tableItems);
			table.Source = source;
			Add (table);

			//var detail = Storyboard.InstantiateViewController("ItemView") as  ItemViewController;
			ItemViewController itemView = (ItemViewController)this.Storyboard.InstantiateViewController("ItemViewController");

		//	NavigationController.PushViewController(detail, true);
			source.RowTouched += (sender, e) => {
				this.NavigationController.PushViewController(itemView, true); 

			//	NavigationController.PushViewController (detail, true);
			};
			/*source.RowTouched += (sender, e) => {
				Console.WriteLine("Row touched");
				ItemViewController itemView = new ItemViewController ("ItemViewController");
				this.NavigationController.PushViewController(itemView, true); 
			};
	*/
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}

		#endregion

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
	}
}

