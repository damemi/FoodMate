using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace FoodMate_iOS
{
	partial class HomeTableView : UITableView
	{
		public HomeTableView (IntPtr handle) : base (handle)
		{
		}
	}
	public class HomeTableSource : UITableViewSource {
		string[] tableItems;
		string cellIdentifier = "TableCell";
		public int currentIndex;
		public event EventHandler RowTouched;
		public HomeTableSource (string[] items)
		{
			tableItems = items;
			currentIndex = -1;
		}
		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return tableItems.Length;
		}
		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
			// if there are no cells to reuse, create a new one
			if (cell == null)
				cell = new UITableViewCell (UITableViewCellStyle.Default, cellIdentifier);
			cell.TextLabel.Text = tableItems[indexPath.Row];
			return cell;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			currentIndex = indexPath.Row;
			RowTouched(this, EventArgs.Empty);
			tableView.DeselectRow (indexPath, true); // normal iOS behaviour is to remove the blue highlight
		}

	}

}
