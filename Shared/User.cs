using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Parse;

namespace Shared
{
	public class User
	{
		private ParseUser _currentUser;
		public User (ParseUser currentUser)
		{
			_currentUser = currentUser;
		}

		public async Task SaveAsync()
		{
			await _currentUser.SaveAsync ();
		}

		public async Task updateUser()
		{
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

