Manatee Works Barcode Scanner SDK
=================================

In order to use the Manatee Works Barcode Scanner SDK component, one need to follow the next few steps:

* Downloading and installing the component
* Registering on our [Developer Network](http://manateeworks.com/developers) and generating a free evaluation license with an initial 30 days duration on demand for any of the supported barcode symbologies and platforms. 
* OPTIONAL: contacting our sales department by filling the contact form on our [website](http://manateeworks.com/home) or via `sales@manateeworks.com` in order to obtain production license keys
* integrating the Manatee Works Barcode Scanner SDK by following the integration examples described bellow

Install the Manatee Works Barcode Scanner SDK component
-------------------------------------------------------

Add the Manatee Works Barcode Scanner SDK component to your project via the Component Manager. 

OPTIONAL: Get a valid evaluation or production license 
------------------------------------------------------

This step is optional simply because the scanner will work even without inserting a valid user name and license key. If you miss valid licensing credentials, either evaluation or commercial, every fifth character of a result shown upon a successful scan will be masked by asterisks (*).

Integration
-----------

Integrating the Manatee Works SDK component into your Xamarin app can be done in a few steps:

### iOS

* Copy MWBarcodeScanner.dll to the project folder and then proceed by adding it to references
* Add **using MWBarcodeScanner; **
* Sample of calling the scanner (assumed is that you already have scanButton created):

Simple example:

```csharp  
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
```

With some customization included:

```csharp  
scanButton.TouchUpInside += async (sender, e) => {

    //Make an instance of scanner
	var scanner = new MWBarcodeScanner.Scanner();

	/*
	 * Customize the scanner by using options below
	 */

	/* Select desired scanner interface orientation
	 * Available options are: LandscapeLeft, LandscapeRight, Portrait; Default: LandscapeLeft
	 */
	//scanner.setInterfaceOrientation("LandscapeLeft");

	/* Toggle visibility of Flash button
	 * Available options are: true, false; Default: true
	 */
	//scanner.useFlash = true;

	/* Toggle high resolution scanning - 1280x720 vs normal resolution scaning - 640x480
	 * Available options are: true, false; Default: true
	 */
	//scanner.useHiRes = true;

	/* Choose between overlay types
	 * Available options are: OM_NONE, OM_MW (dynamic viewfinder), OM_IMAGE (static image overlay); Default: OM_MW
	 */
	//scanner.overlayMode = Scanner.OM_MW;

	//Perform decoder initialization
	customDecoderInit();

	//Call the scaner and wait for result
	var result =  await scanner.Scan();

	//If canceled, result is null
	if (result != null){

        new UIAlertView(result.type, result.code, null, "Close", null).Show();

	}

};


```

### Android

* Copy MWBarcodeScanner.dll to the project folder and add it to the references
* Add **using MWBarcodeScanner; **
* Add **using System.Drawing; ** 
* Select **Camera** and **Flashlight** permissions in AndroidManifest.xml
* Sample of calling the scanner (assumed is that you already have scanButton created):

Simple example:

```csharp  

scanButton.Click += async delegate {
		
	var scanner = new MWBarcodeScanner.Scanner(this);

	var result =  await scanner.Scan();

	//If canceled, result is null
	if (result != null){
		var dialog = new AlertDialog.Builder(this);
		dialog.SetTitle(result.type);
		dialog.SetMessage(result.code);
		dialog.Show();
	}
};

```

With some customization included:

```csharp  
scanButton.TouchUpInside += async (sender, e) => {

    //Make an instance of scanner
	var scanner = new MWBarcodeScanner.Scanner();

	/*
	 * Customize the scanner by using options below
	 */

	/* Select desired scanner interface orientation
	 * Available options are: LandscapeLeft, LandscapeRight, Portrait; Default: LandscapeLeft
	 */
	//scanner.setInterfaceOrientation("LandscapeLeft");

	/* Toggle visibility of Flash button
	 * Available options are: true, false; Default: true
	 */
	//scanner.useFlash = true;

	/* Toggle high resolution scanning - 1280x720 vs normal resolution scaning - 640x480
	 * Available options are: true, false; Default: true
	 */
	//scanner.useHiRes = true;

	/* Choose between overlay types
	 * Available options are: OM_NONE, OM_MW (dynamic viewfinder), OM_IMAGE (static image overlay); Default: OM_MW
	 */
	//scanner.overlayMode = Scanner.OM_MW;

	//Perform decoder initialization
	customDecoderInit();

	//Call the scaner and wait for result
	var result =  await scanner.Scan();

	//If canceled, result is null
	if (result != null){

        new UIAlertView(result.type, result.code, null, "Close", null).Show();

	}

};


```

Refer to MWBCameraDemo project for a sample for customDecoderInit function

Support
-------

If there are any questions or concerns, contact our support team via `support@manateeworks.com`

    