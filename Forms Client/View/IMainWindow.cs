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
        string[] ChosenCities();
        void PopulateFromBox(string[] items);
        void PopulateToBox(string[] items);
        String FromBoxText { get; }
        String ToBoxText { get; }
    }
}
