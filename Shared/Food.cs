using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Parse;

namespace Shared
{
	public class Food
	{
		public string name { get; private set; }
		private string ID { get; set; }
		public int in_stock { get; private set; }
		public double price { get; private set; }
		private int barcode { get; set; }
		private DateTime expiration { get; set; }
		private ParseObject _parseObj;
		public List<object> wanted_by { get; set; }

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
			in_stock = food.Get<int> ("in_stock");
			price = food.Get<Double> ("price");
			wanted_by = food.Get<List<object>> ("wanted_by");
			_parseObj = food;
		}

		public ParseObject getParseObject() {
			return _parseObj;
		}

		public String getName() {
			return name;
		}

		public int getStock() {
			return in_stock;
		}

		public String objId() {
			return _parseObj.ObjectId;
		}

		public double getPrice()	{
			return price;
		}

		public String getObjectId() {
			return _parseObj.Get<String> ("objectId");
		}
	}
}