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

		public String getGroup()
		{
			return _currentUser.Get<String> ("Group");
		}

		public async void UpdateGroup(String groupName)
		{
			_currentUser ["Group"] = groupName;
			await SaveAsync ();
		}
	}
}

