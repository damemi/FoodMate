using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.App;

using Facebook;
using Android.Graphics;

namespace FoodMate
{
	[Activity (Label = "InventoryActivity")]			
	public class InventoryActivity : FragmentActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Home);
			ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
			var pager = FindViewById<ViewPager> (Resource.Id.pager);
			var adaptor = new GenericFragmentPagerAdaptor (SupportFragmentManager);

			/*
				ActionBar.Tab tab = ActionBar.NewTab ();
				tab.SetText ("Home");
				tab.TabSelected += (sender, args) => {
				};
				ActionBar.AddTab (tab);

				tab = ActionBar.NewTab ();
				tab.SetText ("Shopping List");
				tab.TabSelected += (sender, args) => {
					// Do something when tab is selected
				};
				ActionBar.AddTab (tab);

				tab = ActionBar.NewTab ();
				tab.SetText ("My List");
				tab.TabSelected += (sender, args) => {
					// Do something when tab is selected
				};
				ActionBar.AddTab (tab);

				tab = ActionBar.NewTab ();
				tab.SetText ("Settings");
				tab.TabSelected += (sender, args) => {
					// Do something when tab is selected
				};
				ActionBar.AddTab (tab);
				*/

			adaptor.AddFragmentView((i, v, b) =>
				{
					var view = i.Inflate(Resource.Layout.tab, v, false);
					var sampleTextView = view.FindViewById<TextView>(Resource.Id.textView1);
					sampleTextView.Text = "Home";
					return view;
				}
			);

			adaptor.AddFragmentView((i, v, b) =>
				{
					var view = i.Inflate(Resource.Layout.tab, v, false);
					var sampleTextView = view.FindViewById<TextView>(Resource.Id.textView1);
					sampleTextView.Text = "Shopping List";
					return view;
				}
			);
			adaptor.AddFragmentView((i, v, b) =>
				{
					var view = i.Inflate(Resource.Layout.tab, v, false);
					var sampleTextView = view.FindViewById<TextView>(Resource.Id.textView1);
					sampleTextView.Text = "My List";
					return view;
				}
			);

			adaptor.AddFragmentView((i, v, b) =>
				{
					var view = i.Inflate(Resource.Layout.tab, v, false);
					var sampleTextView = view.FindViewById<TextView>(Resource.Id.textView1);
					sampleTextView.Text = "Settings";
					return view;
				}
			);

			pager.Adapter = adaptor;
			pager.SetOnPageChangeListener(new ViewPageListenerForActionBar(ActionBar));

			ActionBar.Tab tab = ActionBar.NewTab();
			ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "Home"));
			ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "Shopping List"));
			ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "My List"));
			ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "Settings"));
		}
	}
}

