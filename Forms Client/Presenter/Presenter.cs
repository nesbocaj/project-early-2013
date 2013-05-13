using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Forms_Client.Presenter
{
    class Presenter
    {
        private View.Overview_Form _overview = null;
        private View.MainWindow _main = null;
        public Presenter(View.MainWindow main)
        {
            _main = main;
            _overview = new View.Overview_Form();
        }

        public void CallOverview()
        {
            var arr = _main.ChosenCities();
            if (arr[0] == "" || arr[1] == "")
                MessageBox.Show("Du mangler at udfylde felterne", "FEJL!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //else
                _overview.Show();
        }

        public void ChangeOverview()
        {

        }
    }
}
