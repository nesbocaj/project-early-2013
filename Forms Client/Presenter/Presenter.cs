using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace Forms_Client.Presenter
{
    class Presenter
    {
        private static Presenter _instance = null;
        private bool _okButtonState = true;
        private Proxy _prox = null;
        private string _resultString = null;
        private string _flight = null;


        private string[] _response = null;
        private string[] _filteredArray = null;



        private Presenter()
        {
            _prox = new Proxy();
            _resultString = _prox.Request("list cities");
        }

        public static Presenter GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Presenter();
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
            else
            {
                var request = String.Format("search flights {0} {1}", Main.FromBoxText, Main.ToBoxText);
                _flight = _prox.Request(request);
                _instance.Overview = new View.Overview_Form();
                Overview.Show();
            }
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

        public void PopulateOverview()
        {
            var response = TCP_Shared.Response<Tuple<String[], Decimal>>.FromSerialized(_flight);
            
            var item1 = response.Value.Item1;

            var price = response.Value.Item2;
            var description = String.Format("Din rejse:\n\n{0}", String.Join("\n\n", item1));

            Overview.DescriptopnLabelText(description);
            Overview.PriceLabelText(price);
        }

        public void PopulateLists()
        {
            if (_resultString != null)
            {
                var responseString = TCP_Shared.Response<String[]>.FromSerialized(_resultString);
                _response = responseString.Value;
                Main.PopulateFromBox(_response);
            }
        }

        public void ToBoxText()
        {
            _filteredArray = _response.Where(x => x != Main.FromBoxText).ToArray();
            Main.PopulateToBox(_filteredArray);
        }
    }
}
