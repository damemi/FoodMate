using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Parse;

namespace Shared
{
	//will restructure this later to use food object, currently addNewFood should work
	public class User
	{
		private ParseUser _currentUser;
		public User (ParseUser currentUser)
		{
			_currentUser = currentUser;
		}

		public async Task SaveAsync()
		{
			await this._currentUser.SaveAsync ();
		}

		public async void UpdateGroup(String groupName)
		{
			this._currentUser ["Group"] = groupName;
			await this.SaveAsync ();
		}
	}
}

