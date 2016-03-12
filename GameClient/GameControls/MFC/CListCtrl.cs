using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace GameControls
{
    public class CListCtrl : CControl
    {
        public void SetTextBkColor(Color color)
        {
        }

        public const int LVCFMT_LEFT            =     0x0000; // Same as HDF_LEFT
        public const int LVCFMT_RIGHT           =     0x0001; // Same as HDF_RIGHT
        public const int LVCFMT_CENTER          =     0x0002; // Same as HDF_CENTER
        public const int LVCFMT_JUSTIFYMASK = 0x0003; // Same as HDF_JUSTIFYMASK

       	public void InsertColumn( int nCol, string lpszColumnHeading, int nFormat, int nWidth )
        {
            if (_control == null)
                return;

            ListView listView = (ListView)_control;

            listView.BeginUpdate();

            listView.View = View.Details;

            // modified by usc at 2013/07/21
            listView.Scrollable = false;

            listView.Columns.Add( lpszColumnHeading, nWidth, HorizontalAlignment.Center );

            listView.EndUpdate();
        }

        public int GetItemCount()
        {
            if (_control == null)
                return 0;

            ListView listView = (ListView)_control;

            return listView.Items.Count;
        }

        public void SetItemText(int nItem, int nSubItem, string lpszText)
        {
            if (_control == null)
                return;

            ListView listView = (ListView)_control;

            listView.BeginUpdate();

            ListViewItem listItem = listView.Items[nItem];

            listItem.SubItems.Add(lpszText);

            listView.EndUpdate();
        }

        public void InsertItem(int nItem, string lpszItem)
        {
            if (_control == null)
                return;

            ListView listView = (ListView)_control;

            listView.BeginUpdate();

            listView.Items.Add(lpszItem);

            listView.EndUpdate();
        }
    }
}
