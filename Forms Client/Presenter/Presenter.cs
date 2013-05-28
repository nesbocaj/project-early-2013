using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.ComponentModel;
using System.Diagnostics;


namespace Forms_Client.Presenter
{
    class Presenter
    {
        private static Presenter _instance = null;
        private bool _okButtonState = true;
        private Proxy _prox = null;

        private string[] _response = null;
        private string[] _filteredArray = null;


        /// <summary>
        /// Creates a new instance of the Presenter and the Proxy
        /// Then sends a request to the Proxy
        /// </summary>
        private Presenter()
        {
            _prox = new Proxy();
        }

        /// <summary>
        /// Instance Accessor of the Presenter class
        /// </summary>
        /// <returns></returns>
        public static Presenter GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Presenter();
            }

            return _instance;
        }

        private void TransmitToServer(string command, Action<string> act ,Action<SocketException> handler)
        {
            var backgroundWorker = new BackgroundWorker();
            var resultString = "";

            Action<object, DoWorkEventArgs> doWork = (sender, e) =>
            {
                try
                {
                    e.Result = _prox.Request(command);
                }
                catch (SocketException se)
                {
                    handler(se);
                }
            };
            Action<object, RunWorkerCompletedEventArgs> runWorkerCompleted = (sender, e) =>
            {
                resultString = e.Result as string;
                act(resultString);
            };

            backgroundWorker.DoWork += new DoWorkEventHandler(doWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(runWorkerCompleted);
            backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Saves an instance of the Overview Form
        /// </summary>
        public View.Overview_Form OverviewForm { get; set; }
        /// <summary>
        /// Saves an instance of the Main Form
        /// </summary>
        public View.MainWindow MainForm { get; set; }

        /// <summary>
        /// Calls the overview if the Main Form's text comboboxes contain anything
        /// </summary>
        public void CallOverview()
        {
            var arr = MainForm.ChosenCities();
            if (arr[0] == "" || arr[1] == "")
                MessageBox.Show("Du mangler at udfylde felterne", "FEJL!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                _instance.OverviewForm = new View.Overview_Form();
                OverviewForm.ShowDialog();
            }
        }

        /// <summary>
        /// Removes the Cancel button once the OK button has been clicked
        /// </summary>
        public void ChangeOverview()
        {
            if (_okButtonState == true)
            {
                OverviewForm.SetCancelButtonVisibility(false);
                _okButtonState = false;
            }
            else
            {
                OverviewForm.Close();
                OverviewForm.SetCancelButtonVisibility(true);
                _okButtonState = true;
            }
        }


        /// <summary>
        /// Populates all the labels in the Overview Form
        /// </summary>
        public void PopulateOverview()
        {
            var request = String.Format("search flights {0} {1}", MainForm.FromBoxText, MainForm.ToBoxText);

            var message = "Kunne ikke beregne din rejse" +
                "\nprøv venligst igen senere\n" +
                "\nVis denne fejl til din nærmeste IT support: \n";

            var title = "Fejl! Kunne ikke beregne rejse!";

            TransmitToServer(request, flight =>
            {
                var response = TCP_Shared.Response<Tuple<String[], Decimal>>.FromSerialized(flight);

                var item1 = response.Value.Item1;

                var price = response.Value.Item2;
                var description = String.Format("Din rejse:\n\n{0}", String.Join("\n\n", item1));

                OverviewForm.DescriptopnLabelText(description);
                OverviewForm.PriceLabelText(price);
            }, se => ErrorMessage(se, message + se.Message, title));
        }

        /// <summary>
        /// Fills the FromBox Combobox with the list of cities from the server
        /// </summary>
        public void PopulateFromList()
        {
            var message = "Kunne ikke hente by-listen fra serveren" +
                "\nprøv venligst igen senere\n" +
                "\nVis denne fejl til din nærmeste IT support: \n";

            var title = "Fejl! Kunne ikke forbinde til serveren";

            TransmitToServer("list cities", res =>
            {
                if (!String.IsNullOrEmpty(res))
                {
                    var responseString = TCP_Shared.Response<String[]>.FromSerialized(res);
                    _response = responseString.Value;
                    MainForm.PopulateFromBox(_response);
                }
            }, se => ErrorMessage(se, message + se.Message, title));
        }

        /// <summary>
        /// Fills the ToBox Combobox with the list of cities from the server
        /// </summary>
        public void PopulateToList()
        {
            _filteredArray = _response.Where(x => x != MainForm.FromBoxText).ToArray();
            MainForm.PopulateToBox(_filteredArray);
        }

        public void ErrorMessage(SocketException se, String text, String title)
        {
            MessageBox.Show(
                text,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }
}
