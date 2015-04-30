using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Parse;

namespace Shared
{
	public class DatabaseOperations
	{
		public List<Food> AllFoods { get; private set; }
		public List<Food> OutOfStockFoods { get; private set; }
		public List<Food> RequestedFoods { get; private set; }

		public DatabaseOperations ()
		{
			Console.WriteLine ("Database Operations");
			ParseClient.Initialize("zCD97bagtQLE7wZFtACpo6XzJm8OFznvF8ynUJoA", "C5jmH2AOT0T1GqF1tZpUT9bGthdfaqWjFveJbgGY");
			AllFoods = new List<Food> ();
			OutOfStockFoods = new List<Food> ();
			RequestedFoods = new List<Food> ();
		}

		public async Task addNewFood(string name, double price, int quantity, string barcode = "0")
		{
			//check if food already exists in database
				//(not implemented yet)

			//add food to database
			try
			{
				ParseObject food = new ParseObject ("Food");
				food ["name"] = name;
				food ["price"] = price;
				food ["in_stock"] = quantity;
				food ["wanted_by"] = new List<String> {"0"};
				if (barcode != "0")
					food ["barcode"] = barcode;
				Console.WriteLine ("Adding new food to parse database");
				await food.SaveAsync();
			}
			catch(Exception e) 
			{
				Console.WriteLine (e.Message);
			}
		}

		public async Task requestNewFood(string name, double price, int quantity, string userId)
		{
			//check if food already exists in database
			//(not implemented yet)

			//add food to database
			try
			{
				ParseObject food = new ParseObject ("Food");
				food ["name"] = name;
				food ["price"] = price;
				food ["in_stock"] = quantity;
				food ["wanted_by"] = new List<String> {"0", userId};
				Console.WriteLine ("Adding new food to parse database");
				await food.SaveAsync();
			}
			catch(Exception e) 
			{
				Console.WriteLine (e.Message);
			}
		}

		//Currently returns a list of all food objects
		//Should be easy to filter by the user's group
		public async Task getFoods() {
		/*	var query = from food in ParseObject.GetQuery ("Food")
			            where food.ContainsKey ("name")
			            select food; */
			var query = ParseObject.GetQuery ("Food").OrderByDescending("createdAt");
			var results = await query.FindAsync();
			AllFoods.Clear ();
			foreach(var result in results) {
				Food food = new Food (result);
				AllFoods.Add(food);
			}
		}

		public async Task getOutOfStockFoods() {
			var query = ParseObject.GetQuery ("Food").WhereEqualTo ("in_stock", 0).OrderByDescending("createdAt");
			var results = await query.FindAsync ();
			OutOfStockFoods.Clear ();
			foreach (var result in results) {
				Food food = new Food (result);
				OutOfStockFoods.Add (food);
			}
		}

		public async Task getRequestedFoods(string userId) {
			var query = ParseObject.GetQuery ("Food").OrderByDescending("createdAt");
			var results = await query.FindAsync ();
			foreach (var f in results) {
				List<object> wanted_by = f.Get<List<object>> ("wanted_by");
				if (wanted_by.Contains (userId)) {
					Food food = new Food (f);
					RequestedFoods.Add (food);
				}
			}

		}

		public async Task<ParseObject> getFood(string objectId) {
			ParseQuery<ParseObject> query = ParseObject.GetQuery("Food");
			ParseObject food = await query.GetAsync(objectId);
			return food;
		}

		public async Task<ParseUser> getUser(string objectId) {
			//ParseQuery<ParseUser> query = ParseUser.Query.GetAsync(objectId);
			ParseUser user = await ParseUser.Query.GetAsync(objectId);
			return user;
		}

		public void incrementFood(string id, int num_added)
		{
			//make sure food exists in database

			//incrememt in_stock number
		}

		public List<Food> getUserFoods(ParseUser user)
		{
			List<Food> userFoods = new List<Food> ();
			var query = ParseObject.GetQuery ("User");
			return userFoods;
		}

		public List<Food> getShoppingListFoods()
		{
			List<Food> shoppingListFoods = new List<Food> ();
			//var query = ParseObject.GetQuery ("User").OrderByDescending("createdAt");

			return shoppingListFoods;

		}
	}
}

