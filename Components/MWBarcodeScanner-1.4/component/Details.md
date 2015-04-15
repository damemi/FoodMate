Manatee Works Barcode Scanner
=============================

MWBarcodeScanner is a C#/.NET library based on the Manatee Works Barcode Scanner SDK.

Pricing
-------

You can obtain a free evaluation license with an initial 30 days duration simply by registering on our [Developer Network](http://manateeworks.com/developers) and generating it for free on demand for any of the supported barcode symbologies and platforms. For more information on how to obtain a valid commercial license for using our SDK in your Xamarin application, contact us by filling the contact form on our [website](http://manateeworks.com/home) with accurate info about your project or by contacting our sales department directly via `sales@manateeworks.com`.

Features & Benefits
-------------------

### High Accuracy

While most decoders only work with high-quality barcode images, Manatee Works decoders are tuned to read the densest barcode images even in low light, extreme angles, varying brightness/contrast levels, and inverted surfaces.

### Supports Every Major Barcode Symbology

Our barcode module supports EAN, UPC, Code 39, Code 93, Code 128, Interleaved and Industrial 2of5, Codabar, Data Matrix, QR, Aztec, DotCode and PDF417 (including compact/truncated format) codes.

### Fast

Along with our proprietary algorithms, we use native machine language rather than "interpretive" languages like Java, giving Manatee Works decoders the best performance by a large margin.

### Simple

With SLOC at a fraction of the size of open source platforms, our Barcode Scanner SDK proves great things do come in small packages. Forget the learning curve... keep developers focused on application development, instead of the complexities of an unsupported, bloated, lame solution.

### Multiple Platform Support

Apart from our Xamarin iOS and Android support, Manatee Works has expanded across every major OS platform, including Windows Phone 8, Windows 8, BlackBerry 10, Linux, Phonegap, Titanium, Worklight, Tomcat and OSX.

### Free EAN & UPC decoder 

Our EAN & UPC decoders are free for inclusion simply by downloading our component and integrating it into your Xamarin app. In return, we only ask that you abide by our [License Agreement](http://manateeworks.com/files/get_file/Manatee-License-Agreement.pdf) and to provide a valid copyright notice  within the user documentation, start-up screen or in the “help-about” section that our Software is embedded and constites a part of your Xamarin application.

Integration
-----------

Integrating the Manatee Works SDK into your Xamarin app can be done in a few steps:

### iOS

* Copy MWBarcodeScanner.dll to the project folder and the proceed by adding it to references
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

    