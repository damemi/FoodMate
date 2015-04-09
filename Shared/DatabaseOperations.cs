using System;
using System.Threading;
using System.Threading.Tasks;
using Parse;

namespace Shared
{
	//will restructure this later to use food object, currently addNewFood should work
	public class DatabaseOperations
	{
		public DatabaseOperations ()
		{
			Console.WriteLine ("Database Operations");
			ParseClient.Initialize("zCD97bagtQLE7wZFtACpo6XzJm8OFznvF8ynUJoA", "C5jmH2AOT0T1GqF1tZpUT9bGthdfaqWjFveJbgGY");
		}

		public async Task addNewFood(string name, int price, int barcode = 0)
		{
			//debugging
			Console.WriteLine ("Attempting to add new food...");
			Console.WriteLine ("Food name: " + name);
			Console.WriteLine ("Food price: " + price);
			Console.WriteLine ("Food barcode: " + barcode);

			//check if food already exists in database
				//(not implemented yet)

			//add food to database
			try
			{
				ParseObject food = new ParseObject ("Food");
				food ["name"] = name;
				food ["price"] = price;
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

		public void incrementFood(string id, int num_added)
		{
			//make sure food exists in database

			//incrememt in_stock number
		}

		public void addUser()
		{
		}
	}


	public class Food
	{
		private string name { get; set; }
		private string ID { get; set; }
		private int in_stock { get; set; }
		private int price { get; set; }
		private int barcode { get; set; }
		private DateTime expiration { get; set; }

		//food constructor. in_stock, barcode and expiration date are optional parameters.
		// name, id and price are required
		public Food(string _name, string _id, int _price, int _in_stock = 0, int _barcode = 0)
		{
			name = _name;
			ID = _id;
			price = _price;
			barcode = _barcode;
			in_stock = _in_stock;
		}
	}
}

