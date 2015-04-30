using System;
using System.Json;
using System.Threading.Tasks;
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

using Parse;
using Xamarin.Auth;
using System.Linq;
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

			var task = Task.Run (async() => {
				await item.getName();
			});
			task.Wait ();


			view.FindViewById<TextView> (Resource.Id.Name).Text = item.getUserName();
			return view;
		}
	}
}


