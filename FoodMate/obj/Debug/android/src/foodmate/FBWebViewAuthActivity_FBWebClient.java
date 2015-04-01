package foodmate;


public class FBWebViewAuthActivity_FBWebClient
	extends android.webkit.WebViewClient
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onPageFinished:(Landroid/webkit/WebView;Ljava/lang/String;)V:GetOnPageFinished_Landroid_webkit_WebView_Ljava_lang_String_Handler\n" +
			"";
		mono.android.Runtime.register ("FoodMate.FBWebViewAuthActivity/FBWebClient, FoodMate, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", FBWebViewAuthActivity_FBWebClient.class, __md_methods);
	}


	public FBWebViewAuthActivity_FBWebClient () throws java.lang.Throwable
	{
		super ();
		if (getClass () == FBWebViewAuthActivity_FBWebClient.class)
			mono.android.TypeManager.Activate ("FoodMate.FBWebViewAuthActivity/FBWebClient, FoodMate, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public FBWebViewAuthActivity_FBWebClient (foodmate.FBWebViewAuthActivity p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == FBWebViewAuthActivity_FBWebClient.class)
			mono.android.TypeManager.Activate ("FoodMate.FBWebViewAuthActivity/FBWebClient, FoodMate, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "FoodMate.FBWebViewAuthActivity, FoodMate, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public void onPageFinished (android.webkit.WebView p0, java.lang.String p1)
	{
		n_onPageFinished (p0, p1);
	}

	private native void n_onPageFinished (android.webkit.WebView p0, java.lang.String p1);

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
