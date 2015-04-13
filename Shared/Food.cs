using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Parse;

namespace Shared
{
	public class Food
	{
		private string name { get; set; }
		private string ID { get; set; }
		private int in_stock { get; set; }
		private int price { get; set; }
		private int barcode { get; set; }
		private DateTime expiration { get; set; }
		private ParseObject _parseObj;

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

		public Food(ParseObject food) {
			name = food.Get<String>("name");
			_parseObj = food;
		}

		public String getName() {
			return name;
		}

		public String getObjectId() {
			return _parseObj.Get<String> ("objectId");
		}
	}
}

