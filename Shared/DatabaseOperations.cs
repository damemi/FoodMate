﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
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

		//Currently returns a list of all food objects
		//Should be easy to filter by the user's group
		public async Task<IEnumerable<ParseObject>> getFoods() {
			var query = ParseObject.GetQuery ("Food");
			var results = await query.FindAsync();
			return results;
		}

		public void incrementFood(string id, int num_added)
		{
			//make sure food exists in database

			//incrememt in_stock number
		}
	}
}

