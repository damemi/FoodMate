using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Parse;

namespace Shared
{
	public class DatabaseOperations
	{
		public DatabaseOperations ()
		{
			Console.WriteLine ("Database Operations");
			ParseClient.Initialize("zCD97bagtQLE7wZFtACpo6XzJm8OFznvF8ynUJoA", "C5jmH2AOT0T1GqF1tZpUT9bGthdfaqWjFveJbgGY");
		}

		public async Task addNewFood(string name, int quantity, int price, int barcode = 0)
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
		public async Task<IEnumerable<ParseObject>> getFoods() {
			var query = ParseObject.GetQuery ("Food").OrderByDescending("createdAt");
			var results = await query.FindAsync();
			return results;
		}

		public async Task<ParseObject> getFood(string objectId) {
			ParseQuery<ParseObject> query = ParseObject.GetQuery("Food");
			ParseObject food = await query.GetAsync(objectId);
			return food;
		}

		public void incrementFood(string id, int num_added)
		{
			//make sure food exists in database

			//incrememt in_stock number
		}
	}
}

