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
        #region private variables
        private static Presenter _instance = null;
        private bool _okButtonState = true;
        private Proxy _prox = null;
        private BackgroundWorker _observer = null;
        private bool _obsAllowed = true;

        private string[] _response = null;
        private string[] _filteredArray = null;
        #endregion

        #region Constructor and instance accessor
        /// <summary>
        /// Creates a new instance of the Presenter and the Proxy
        /// Then sends a request to the Proxy
        /// </summary>
        private Presenter()
        {
            _prox = new Proxy();
            _observer = new BackgroundWorker();
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
        #endregion

        #region public properties
        /// <summary>
        /// Saves an instance of the Overview Form
        /// </summary>
        public View.IOverviewWindow OverviewWindow { get; set; }


        /// <summary>
        /// Saves an instance of the Main Form
        /// </summary>
        public View.IMainWindow MainForm { get; set; }
        #endregion

        #region private methods
        /// <summary>
        /// Creates a BackgroundWorker that accesses the server in a different thread
        /// </summary>
        /// <param name="command">The command to be sent to the server</param>
        /// <param name="act">The code to be exectuted on completion</param>
        /// <param name="handler">The built-in ExceptionHandler</param>
        private void TransmitToServer(string command, Action<string> act, Action<SocketException> handler)
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
        /// Shows a messagebox with the exception
        /// </summary>
        /// <typeparam name="T">Any given Exception</typeparam>
        /// <param name="ex">An Exception of type T</param>
        /// <param name="text">The Error Description</param>
        /// <param name="title">The MessageBox Title</param>
        private void ErrorMessage<T>(T ex, String text, String title) where T : Exception
        {
            MessageBox.Show(
                text + ex.Message,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        #endregion

        #region public methods

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
                _instance.OverviewWindow = new View.OverviewWindow();
                OverviewWindow.ShowDialog();
            }
        }

        /// <summary>
        /// Removes the Cancel button once the OK button has been clicked
        /// </summary>
        public void ChangeOverview()
        {
            if (_okButtonState == true)
            {
                OverviewWindow.SetCancelButtonVisibility(false);
                _okButtonState = false;
            }
            else
            {
                OverviewWindow.Close();
                OverviewWindow.SetCancelButtonVisibility(true);
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

                // adding some additional info to the description... messy solution at best...
                item1[0] = "Udrejse: " + item1[0];
                item1[item1.Length - 1] = "Destination: " + item1[item1.Length - 1];

                for (int i = 1; i <= item1.Length - 2; i++)
                {
                    if (item1.Length - 2 <= 0)// if the array's size doesn't work in its favour
                    {
                        // do nothing
                    }
                    else // otherwise if it does
                    {
                        item1[i] = "Viapunkt: " + item1[i];
                    }
                }

                var description = String.Format("Din rejse:\n\n{0}", String.Join("\n\n", item1));

                OverviewWindow.DescriptopnLabelText(description);
                OverviewWindow.PriceLabelText(price);
            }, se => ErrorMessage(se, message, title));
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

            TransmitToServer("list cities", result =>
            {
                if (!String.IsNullOrEmpty(result))
                {
                    var responseString = TCP_Shared.Response<String[]>.FromSerialized(result);
                    _response = responseString.Value;
                    MainForm.PopulateFromBox(_response);
                }
            }, se => ErrorMessage(se, message, title));
        }

        /// <summary>
        /// Fills the ToBox Combobox with the list of cities from the server
        /// </summary>
        public void PopulateToList()
        {
            _filteredArray = _response.Where(x => x != MainForm.FromBoxText).ToArray();
            MainForm.PopulateToBox(_filteredArray);
        }

        public void PopulateSalesListView()
        {
            Action<object, DoWorkEventArgs> doWork = (sender, e) =>
            {
                try
                {
                    _prox.Post("watch");
                }
                catch (SocketException se)
                {
                }

                for (; ; )
                {
                    if (!_obsAllowed) 
                        _prox.ObserverState = false;

                    else if (!_prox.ObserverState && _obsAllowed) 
                        _prox.ObserverState = true;

                    if (!String.IsNullOrEmpty(_prox.ObserverResult))
                    {
                        e.Result = _prox.ObserverResult;
                        _prox.ObserverResult = null;
                        break;
                    }
                }

            };

            Action<object, RunWorkerCompletedEventArgs> runWorkerCompleted = (sender, e) =>
            {
                var response = TCP_Shared.Response<Tuple<string, string, decimal>>
                    .FromSerialized(e.Result as string);

                MainForm.UpdateListView(
                    response.Value.Item1, response.Value.Item2, response.Value.Item3);
            };

            _observer.DoWork += new DoWorkEventHandler(doWork);
            _observer.RunWorkerCompleted += new RunWorkerCompletedEventHandler(runWorkerCompleted);
            _observer.RunWorkerAsync();
        }
        #endregion
    }
}
