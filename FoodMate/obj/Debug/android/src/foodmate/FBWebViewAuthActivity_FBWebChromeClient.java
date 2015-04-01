package foodmate;


public class FBWebViewAuthActivity_FBWebChromeClient
	extends android.webkit.WebChromeClient
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onProgressChanged:(Landroid/webkit/WebView;I)V:GetOnProgressChanged_Landroid_webkit_WebView_IHandler\n" +
			"";
		mono.android.Runtime.register ("FoodMate.FBWebViewAuthActivity/FBWebChromeClient, FoodMate, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", FBWebViewAuthActivity_FBWebChromeClient.class, __md_methods);
	}


	public FBWebViewAuthActivity_FBWebChromeClient () throws java.lang.Throwable
	{
		super ();
		if (getClass () == FBWebViewAuthActivity_FBWebChromeClient.class)
			mono.android.TypeManager.Activate ("FoodMate.FBWebViewAuthActivity/FBWebChromeClient, FoodMate, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public FBWebViewAuthActivity_FBWebChromeClient (android.app.Activity p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == FBWebViewAuthActivity_FBWebChromeClient.class)
			mono.android.TypeManager.Activate ("FoodMate.FBWebViewAuthActivity/FBWebChromeClient, FoodMate, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.App.Activity, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public void onProgressChanged (android.webkit.WebView p0, int p1)
	{
		n_onProgressChanged (p0, p1);
	}

	private native void n_onProgressChanged (android.webkit.WebView p0, int p1);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
