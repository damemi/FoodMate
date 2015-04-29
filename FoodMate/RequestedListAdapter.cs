﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Shared;

namespace FoodMate
{
	public class RequestedListAdapter : BaseAdapter<User>
	{
		Activity context;
		List<User> list;

		public RequestedListAdapter (Activity _context, List<User> _list)
			:base()
		{
			this.context = _context;
			this.list = _list;
		}

		public override int Count {
			get { return list.Count; }
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override User this[int index] {
			get { return list [index]; }
		}

		public override Java.Lang.Object GetItem (int position) {
			// could wrap a Contact in a Java.Lang.Object
			// to return it here if needed
			return null;
		}

		public User getFood(int index) {
			return list[index];
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			View view = convertView;
			if (view == null)
				view = context.LayoutInflater.Inflate (Resource.Layout.ListItemRow, parent, false);

			User item = this [position];
			view.FindViewById<TextView> (Resource.Id.Name).Text = item.getName();
			return view;
		}
	}
}


