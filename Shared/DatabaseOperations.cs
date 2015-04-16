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

		public DatabaseOperations ()
		{
			Console.WriteLine ("Database Operations");
			ParseClient.Initialize("zCD97bagtQLE7wZFtACpo6XzJm8OFznvF8ynUJoA", "C5jmH2AOT0T1GqF1tZpUT9bGthdfaqWjFveJbgGY");
			AllFoods = new List<Food> ();
		}

		public async Task addNewFood(string name, double price, int quantity=0, int barcode = 0)
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
				if (barcode != 0)
					food ["barcode"] = barcode;
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
			Console.WriteLine ("Async task");
			var query = ParseObject.GetQuery ("Food").OrderByDescending("createdAt");
			var results = await query.FindAsync();
			Console.WriteLine ("Async task returning");
			foreach(var result in results)
			{
				Food food = new Food (result);
				AllFoods.Add(food);
				Console.WriteLine (food.getName ());
			}
		}

		public void incrementFood(string id, int num_added)
		{
			//make sure food exists in database

			//incrememt in_stock number
		}

		public List<Food> getUserFoods(ParseObject user)
		{
			List<Food> userFoods = new List<Food> ();
			return userFoods;
		}

		public List<Food> getShoppingListFoods()
		{
			List<Food> shoppingListFoods = new List<Food> ();
			return shoppingListFoods;

		}
	}
}

