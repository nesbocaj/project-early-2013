﻿using System;
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
        private string _resultString = null;
        private string _flight = null;
        private BackgroundWorker _backgroundWorker = null;

        private string[] _response = null;
        private string[] _filteredArray = null;


        /// <summary>
        /// Creates a new instance of the Presenter and the Proxy
        /// Then sends a request to the Proxy
        /// </summary>
        private Presenter()
        {
            _prox = new Proxy();

            //_resultString = _prox.Request("list cities");

            ///*
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += new DoWorkEventHandler(DoWork);
            _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkCompleted);
            _backgroundWorker.RunWorkerAsync();
            Debug.WriteLine("++ OUR RESULT STRING = "+ _resultString); // check to see if it contains anything after exiting the bg worker
            //*/
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

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                e.Result = _prox.Request("list cities");
            }
            catch (SocketException se)
            {
                ErrorMessage(se);
            }
        }

        private void WorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _resultString = e.Result as string;
            Debug.WriteLine("-- OUR RESULT STRING = " + _resultString);
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
                var request = String.Format("search flights {0} {1}", MainForm.FromBoxText, MainForm.ToBoxText);
                _flight = _prox.Request(request);
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
            var response = TCP_Shared.Response<Tuple<String[], Decimal>>.FromSerialized(_flight);
            
            var item1 = response.Value.Item1;

            var price = response.Value.Item2;
            var description = String.Format("Din rejse:\n\n{0}", String.Join("\n\n", item1));

            OverviewForm.DescriptopnLabelText(description);
            OverviewForm.PriceLabelText(price);
        }

        /// <summary>
        /// Fills the FromBox Combobox with the list of cities from the server
        /// </summary>
        public void PopulateFromList()
        {
            if (_resultString != null)
            {
                var responseString = TCP_Shared.Response<String[]>.FromSerialized(_resultString);
                _response = responseString.Value;
                MainForm.PopulateFromBox(_response);
            }
        }

        /// <summary>
        /// Fills the ToBox Combobox with the list of cities from the server
        /// </summary>
        public void PopulateToList()
        {
            _filteredArray = _response.Where(x => x != MainForm.FromBoxText).ToArray();
            MainForm.PopulateToBox(_filteredArray);
        }

        public void ErrorMessage(SocketException se)
        {
            MessageBox.Show(
                "Kunne ikke hente by-listen fra serveren" +
                "\nprøv venligst igen senere\n" +
                "\nVis denne fejl til din nærmeste IT support: \n" +
                se.Message,
                "Fejl! Kunne ikke forbinde til serveren",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        public void OnFromBoxClicked()
        {
            PopulateFromList();
        }
    }
}
