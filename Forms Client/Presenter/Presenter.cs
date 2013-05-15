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
        private static Presenter _instance = null;

        private bool _okButtonState = true;

        private Presenter() 
        {
            var prox = new Proxy();
            prox.Request("list destinations Ljubljana");
        }

        public static Presenter GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Presenter();
                _instance.Overview = new View.Overview_Form();
            }

            return _instance;
        }

        public View.Overview_Form Overview { get; set; }
        public View.MainWindow Main { get; set; }

        public void CallOverview()
        {
            var arr = Main.ChosenCities();
            if (arr[0] == "" || arr[1] == "")
                MessageBox.Show("Du mangler at udfylde felterne", "FEJL!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //else
                Overview.Show();
        }

        public void ChangeOverview()
        {
            if (_okButtonState == true)
            {
                Overview.SetCancelButtonVisibility(false);
                _okButtonState = false;
            }
            else
            {
                Overview.Close();
                Overview.SetCancelButtonVisibility(true);
                _okButtonState = true;
            }
        }
    }
}
