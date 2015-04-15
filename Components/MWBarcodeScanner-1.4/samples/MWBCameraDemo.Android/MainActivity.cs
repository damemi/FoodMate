using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using MWBarcodeScanner;
using System.Drawing;

namespace MWBCameraDemo
{
	[Activity (Label = "MWBCameraDemo", MainLauncher = true)]
	public class MainActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			
			button.Click += async delegate {

				var scanner = new MWBarcodeScanner.Scanner(this);
				customDecoderInit();

				/* Toggle visibility of Zoom button
				* Available options are: true, false; Default: true
				*/
				//scanner.useZoom = false;

				/* Choose desired zoom levels
				* zoomLevel1, zoomLevel2 - zoom level in % ; Default: 150,300
				* initialZoomLevel       - the initial zoom level index; Available options: 0 (no zoom), 1, 2; Default: 0
				*/
				//scanner.setZoomLevels(200, 400, 1);


				/* Set the number of CPU cores to be used
				 * Available options are: 1,2,...,n (will be clipped to maxumum available on device); Default: max number of cpu cores available
				 */
				//scanner.setMaxThreads(2);



				var result =  await scanner.Scan();

				//If canceled, result is null
				if (result != null){
					new Handler().PostDelayed(()=>RunOnUiThread(()=>{
						var dialog = new AlertDialog.Builder(this);
						dialog.SetTitle(result.type + (result.isGS1?" (GS1)":""));
						dialog.SetMessage(result.code);
						dialog.Show();
					}),500);
				}

			};
		}

		public static RectangleF RECT_LANDSCAPE_1D = new RectangleF(6, 20, 88, 60);
		public static RectangleF RECT_LANDSCAPE_2D = new RectangleF(20, 6, 60, 88);
		public static RectangleF RECT_PORTRAIT_1D = new RectangleF(20, 6, 60, 88);
		public static RectangleF RECT_PORTRAIT_2D = new RectangleF(20, 6, 60, 88);
		public static RectangleF RECT_FULL_1D = new RectangleF(6, 6, 88, 88);
		public static RectangleF RECT_FULL_2D = new RectangleF(20, 6, 60, 88);
		public static RectangleF RECT_DOTCODE = new RectangleF(30, 20, 40, 60);

		public static void customDecoderInit() {

			Console.WriteLine("Decoder initialization");
			//register your copy of library with givern user/password
			BarcodeConfig.MWB_registerCode(BarcodeConfig.MWB_CODE_MASK_39,      "username", "key");
			BarcodeConfig.MWB_registerCode(BarcodeConfig.MWB_CODE_MASK_93,      "username", "key");
			BarcodeConfig.MWB_registerCode(BarcodeConfig.MWB_CODE_MASK_25,      "username", "key");
			BarcodeConfig.MWB_registerCode(BarcodeConfig.MWB_CODE_MASK_128,     "username", "key");
			BarcodeConfig.MWB_registerCode(BarcodeConfig.MWB_CODE_MASK_AZTEC,   "username", "key");
			BarcodeConfig.MWB_registerCode(BarcodeConfig.MWB_CODE_MASK_DM,      "username", "key");
			BarcodeConfig.MWB_registerCode(BarcodeConfig.MWB_CODE_MASK_EANUPC,  "username", "key");
			BarcodeConfig.MWB_registerCode(BarcodeConfig.MWB_CODE_MASK_QR,      "username", "key");
			BarcodeConfig.MWB_registerCode(BarcodeConfig.MWB_CODE_MASK_PDF,     "username", "key");
			BarcodeConfig.MWB_registerCode(BarcodeConfig.MWB_CODE_MASK_RSS,     "username", "key");
			BarcodeConfig.MWB_registerCode(BarcodeConfig.MWB_CODE_MASK_CODABAR, "username", "key");
			BarcodeConfig.MWB_registerCode(BarcodeConfig.MWB_CODE_MASK_CODABAR, "username", "key");
			BarcodeConfig.MWB_registerCode(BarcodeConfig.MWB_CODE_MASK_DOTCODE, "username", "key");
			BarcodeConfig.MWB_registerCode(BarcodeConfig.MWB_CODE_MASK_11,      "username", "key");
			BarcodeConfig.MWB_registerCode(BarcodeConfig.MWB_CODE_MASK_MSI,     "username", "key");


			// choose code type or types you want to search for

			// Our sample app is configured by default to search all supported barcodes...
			BarcodeConfig.MWB_setActiveCodes(BarcodeConfig.MWB_CODE_MASK_25    |
				BarcodeConfig.MWB_CODE_MASK_39     |
				BarcodeConfig.MWB_CODE_MASK_93     |
				BarcodeConfig.MWB_CODE_MASK_128    |
				BarcodeConfig.MWB_CODE_MASK_AZTEC  |
				BarcodeConfig.MWB_CODE_MASK_DM     |
				BarcodeConfig.MWB_CODE_MASK_EANUPC |
				BarcodeConfig.MWB_CODE_MASK_PDF    |
				BarcodeConfig.MWB_CODE_MASK_QR     |
				BarcodeConfig.MWB_CODE_MASK_CODABAR|
				BarcodeConfig.MWB_CODE_MASK_11     |
				BarcodeConfig.MWB_CODE_MASK_MSI    |
				BarcodeConfig.MWB_CODE_MASK_RSS);

			// But for better performance, only activate the symbologies your application requires...
			// BarcodeConfig.MWB_setActiveCodes( BarcodeConfig.MWB_CODE_MASK_25 );
			// BarcodeConfig.MWB_setActiveCodes( BarcodeConfig.MWB_CODE_MASK_39 );
			// BarcodeConfig.MWB_setActiveCodes( BarcodeConfig.MWB_CODE_MASK_93 );
			// BarcodeConfig.MWB_setActiveCodes( BarcodeConfig.MWB_CODE_MASK_128 );
			// BarcodeConfig.MWB_setActiveCodes( BarcodeConfig.MWB_CODE_MASK_AZTEC );
			// BarcodeConfig.MWB_setActiveCodes( BarcodeConfig.MWB_CODE_MASK_DM );
			// BarcodeConfig.MWB_setActiveCodes( BarcodeConfig.MWB_CODE_MASK_EANUPC );
			// BarcodeConfig.MWB_setActiveCodes( BarcodeConfig.MWB_CODE_MASK_PDF );
			// BarcodeConfig.MWB_setActiveCodes( BarcodeConfig.MWB_CODE_MASK_QR );
			// BarcodeConfig.MWB_setActiveCodes( BarcodeConfig.MWB_CODE_MASK_RSS );
			// BarcodeConfig.MWB_setActiveCodes( BarcodeConfig.MWB_CODE_MASK_CODABAR );
			// BarcodeConfig.MWB_setActiveCodes( BarcodeConfig.MWB_CODE_MASK_DOTCODE );
			// BarcodeConfig.MWB_setActiveCodes( BarcodeConfig.MWB_CODE_MASK_11 );
			// BarcodeConfig.MWB_setActiveCodes( BarcodeConfig.MWB_CODE_MASK_MSI );


			// Our sample app is configured by default to search both directions...
			BarcodeConfig.MWB_setDirection(BarcodeConfig.MWB_SCANDIRECTION_HORIZONTAL | BarcodeConfig.MWB_SCANDIRECTION_VERTICAL);
			// set the scanning rectangle based on scan direction(format in pct: x, y, width, height)
			BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_25,     RECT_FULL_1D);
			BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_39,     RECT_FULL_1D);
			BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_93,     RECT_FULL_1D);
			BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_128,    RECT_FULL_1D);
			BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_AZTEC,  RECT_FULL_2D);
			BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_DM,     RECT_FULL_2D);
			BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_EANUPC, RECT_FULL_1D);
			BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_PDF,    RECT_FULL_1D);
			BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_QR,     RECT_FULL_2D);
			BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_RSS,    RECT_FULL_1D);
			BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_CODABAR,RECT_FULL_1D);
			BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_DOTCODE,RECT_DOTCODE);
			BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_11,     RECT_FULL_1D);
			BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_MSI,    RECT_FULL_1D);


			// But for better performance, set like this for PORTRAIT scanning...
			// BarcodeConfig.MWB_setDirection(BarcodeConfig.MWB_SCANDIRECTION_VERTICAL);
			// set the scanning rectangle based on scan direction(format in pct: x, y, width, height)
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_25,     RECT_PORTRAIT_1D);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_39,     RECT_PORTRAIT_1D);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_93,     RECT_PORTRAIT_1D);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_128,    RECT_PORTRAIT_1D);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_AZTEC,  RECT_PORTRAIT_2D);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_DM,     RECT_PORTRAIT_2D);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_EANUPC, RECT_PORTRAIT_1D);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_PDF,    RECT_PORTRAIT_1D);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_QR,     RECT_PORTRAIT_2D);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_RSS,    RECT_PORTRAIT_1D);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_CODABAR,RECT_PORTRAIT_1D);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_DOTCODE,RECT_DOTCODE);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_11,     RECT_PORTRAIT_1D);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_MSI,    RECT_PORTRAIT_1D);

			// or like this for LANDSCAPE scanning - Preferred for dense or wide codes...
			// BarcodeConfig.MWB_setDirection(BarcodeConfig.MWB_SCANDIRECTION_HORIZONTAL);
			// set the scanning rectangle based on scan direction(format in pct: x, y, width, height)
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_25,     RECT_LANDSCAPE_1D);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_39,     RECT_LANDSCAPE_1D);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_93,     RECT_LANDSCAPE_1D);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_128,    RECT_LANDSCAPE_1D);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_AZTEC,  RECT_LANDSCAPE_2D);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_DM,     RECT_LANDSCAPE_2D);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_EANUPC, RECT_LANDSCAPE_1D);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_PDF,    RECT_LANDSCAPE_1D);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_QR,     RECT_LANDSCAPE_2D);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_RSS,    RECT_LANDSCAPE_1D);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_CODABAR,RECT_LANDSCAPE_1D);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_DOTCODE,RECT_DOTCODE);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_11,     RECT_LANDSCAPE_1D);
			// BarcodeConfig.MWBsetScanningRect(BarcodeConfig.MWB_CODE_MASK_MSI,    RECT_LANDSCAPE_1D);




			// set decoder effort level (1 - 5)
			// for live scanning scenarios, a setting between 1 to 3 will suffice
			// levels 4 and 5 are typically reserved for batch scanning
			BarcodeConfig.MWB_setLevel(2);

			//get and print Library version
			int ver = BarcodeConfig.MWB_getLibVersion();
			int v1 = (ver >> 16);
			int v2 = (ver >> 8) & 0xff;
			int v3 = (ver & 0xff);
			String libVersion = v1.ToString() + "." + v2.ToString() + "." + v3.ToString();
			Console.WriteLine("Lib version: " + libVersion);
		}
	}

}


