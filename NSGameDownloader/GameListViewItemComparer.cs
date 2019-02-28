using System;
using System.Collections;
using System.Windows.Forms;

namespace NSGameDownloader
{
    public class GameListViewItemComparer : IComparer
    {
        public int sortedColumn;

        public SortOrder sortOrder = SortOrder.None;


        public int Compare(object x, object y)
        {
            int returnVal = -1;
            returnVal = String.Compare(((ListViewItem)x).SubItems[sortedColumn].Text,
            ((ListViewItem)y).SubItems[sortedColumn].Text);
            return (sortOrder == SortOrder.Ascending || sortOrder == SortOrder.None ? 1 : -1) * returnVal;
        }
    }
}