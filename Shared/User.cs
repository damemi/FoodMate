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

namespace Shared
{
	public class User
	{
		private ParseUser _currentUser;
		private String _userName;
		public User (ParseUser currentUser, string id = "0")
		{
			if (id != "0") {
				currentUser["fb_id"] = id;
				var task = Task.Run (async() => {
					await currentUser.SaveAsync ();
				});
				task.Wait ();
			}
			_currentUser = currentUser;
			_userName = null;
		}

		public async Task SaveAsync()
		{
			await _currentUser.SaveAsync ();
		}

		public async Task updateUser()
		{
		}
	
		public async Task getName() {
			ParseUser puser = await ParseUser.Query.GetAsync(_currentUser.ObjectId);
			string userId = puser.Get<string> ("fb_id");
			Console.WriteLine (userId);
			var fb = new FacebookClient ();
			fb.AccessToken = ParseFacebookUtils.AccessToken;
			fb.GetCompleted +=
				(o, e) => {
				var result = (IDictionary<string, object>) e.GetResultData();
				var name = (string) result["name"];
				_userName = name;
			};
			await fb.GetTaskAsync(userId);
		}

		public String getUserName() {
			return _userName;
		}


		public String objId() {
			return _currentUser.ObjectId;
		}

		public String getGroup()
		{
			var user = _currentUser;
			String groupName = _currentUser.Get<String> ("Group");
			if (!(groupName == null)) {
				return groupName;
			} else {
				return "";
			}
		}

		public async void UpdateGroup(String groupName)
		{
			_currentUser ["Group"] = groupName;
			await SaveAsync ();
		}
	}
}

