﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ServiceModel.Web;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;

namespace Web_Forms_Client.Presenter
{
    class Presenter
    {
        private static Presenter _instance = null;
        private WebClient _webClient;
        private string _baseUrl;
        private string[] _cityList;
        private bool _okButtonState = true; 

        private Presenter()
        {
            _webClient = new WebClient();
            _baseUrl = "http://localhost:64922/ForwardingService.svc/";
        }

        public static Presenter Getinstance()
        {
            if (_instance == null)
            {
                _instance = new Presenter();
                _instance.Overview = new View.Overview();
            }
            return _instance;
        }

        public View.Overview Overview {get; set;}
        public Web_Forms_Client.MainWindow Main{ get; set; }

        public void CallOverview()
        {
            var arr = Main.ChosenCities();
            if (arr[0] == "" || arr[1] == "")
                MessageBox.Show("Begge felter skal udfyldes.", "ERROR 43021", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                _instance.Overview = new View.Overview();
                Overview.ShowDialog();
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

        //Jeg arbejder her. -Dennis-
        public void GetCityList()
        {
            _webClient.DownloadStringCompleted += client_GetCityListCompleted;

            _webClient.DownloadStringAsync(
                new Uri(string.Format("{0}/list/cities",
                _baseUrl)));
        }

        private void client_GetCityListCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                var json = new DataContractJsonSerializer(typeof(string[]));
                _cityList = json.ReadObject(new MemoryStream(Encoding.Default.GetBytes(e.Result))) as string[];

                Main.PopulateFromBox(_cityList);
            }

            _webClient.DownloadStringCompleted -= client_GetCityListCompleted;
        }

        public void GetDestinationList(string from)
        {
            _webClient.DownloadStringCompleted += client_GetDestinationListCompleted;

            _webClient.DownloadStringAsync(
                new Uri(string.Format("{0}/list/destinations/{1}",
                _baseUrl, from)));
        }

        private void client_GetDestinationListCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                var json = new DataContractJsonSerializer(typeof(string[]));
                var dList = json.ReadObject(new MemoryStream(Encoding.Default.GetBytes(e.Result))) as string[];

                //Main.ShowCities(dList);
            }

            _webClient.DownloadStringCompleted -= client_GetDestinationListCompleted;
        }

        public void GetSearchList()
        {
            _webClient.DownloadStringCompleted += client_GetSearchListCompleted;

            _webClient.DownloadStringAsync(
                new Uri(string.Format("{0}/search?From={1}&To={2}",
                _baseUrl, Main.FromBoxText, Main.ToBoxText)));
        }

        private void client_GetSearchListCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                var json = new DataContractJsonSerializer(typeof(Tuple<string[], decimal>));
                var sTuple = json.ReadObject(new MemoryStream(Encoding.Default.GetBytes(e.Result))) as Tuple<string[], decimal>;

                var sList = sTuple.Item1;
                var sPrice = sTuple.Item2;

                var description = String.Format("Din rejse:\n\n{0}", String.Join("\n\n", sList));

                Overview.DescriptopnLabelText(description);
                Overview.PriceLabelText(sPrice);
            }

            _webClient.DownloadStringCompleted -= client_GetSearchListCompleted;
        }

        public void GetWatchList(string from, string to)
        {
            _webClient.DownloadStringCompleted += client_GetWatchListCompleted;

            _webClient.DownloadStringAsync(
                new Uri(string.Format("{0}/watch?From={1}&To={2}",
                _baseUrl, from, to)));
        }

        private void client_GetWatchListCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                var json = new DataContractJsonSerializer(typeof(string[]));
                var dList = json.ReadObject(new MemoryStream(Encoding.Default.GetBytes(e.Result))) as string[];

                //Main.ShowCities(wList);
            }

            _webClient.DownloadStringCompleted -= client_GetWatchListCompleted;
        }

        public void PopulateToList()
        {
            string[] _filteredArray = _cityList.Where(x => x != Main.FromBoxText).ToArray();
            Main.PopulateToBox(_filteredArray);
        }
    }
}
