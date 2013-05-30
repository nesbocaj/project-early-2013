using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Forms_Client.View
{
    public interface IMainWindow
    {
        #region methods
        string[] ChosenCities();
        void PopulateFromBox(string[] items);
        void PopulateToBox(string[] items);
        void UpdateListView(string to, string from, decimal price);
        #endregion

        #region properties
        String FromBoxText { get; }
        String ToBoxText { get; }
        #endregion
    }
}
