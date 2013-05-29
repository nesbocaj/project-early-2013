using System;
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


        public View.Overview Overview { get; set; }
        public Web_Forms_Client.MainWindow Main { get; set; }

        /// <summary>
        /// //Creates instances of Presenter and WebClient
        /// //And defining the base Url to REST service
        /// </summary>
        private Presenter()
        {
            _webClient = new WebClient();
            _baseUrl = "http://localhost:64922/ForwardingService.svc/";
        }

        /// <summary>
        /// //Instance Accessor of the Presenter class
        /// </summary>
        public static Presenter Getinstance()
        {
            if (_instance == null)
            {
                _instance = new Presenter();
                _instance.Overview = new View.Overview();
            }
            return _instance;
        }

        /// <summary>
        /// //Create and calls the instance of the Overview Form,
        /// //if the MainWindow Form contains any information.
        /// </summary>
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

        /// <summary>
        /// //Changes Overview's button's visibility or closes the Overview Form.
        /// </summary>
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

        /// <summary>
        /// //Requests the Web Server's REST service to get Citylist,
        /// //calls the method client_GetCityListCompleted
        /// </summary>
        public void GetCityList()
        {
            _webClient.DownloadStringCompleted += client_GetCityListCompleted;

            _webClient.DownloadStringAsync(
                new Uri(string.Format("{0}/list/cities",
                _baseUrl)));
        }

        /// <summary>
        /// //Requests the Web Server's REST service to get Destinationlist,
        /// //calls the method client_GetDestinationListCompleted
        /// </summary>
        /// <param name="from">the name of the initial city</param>
        public void GetDestinationList(string from)
        {
            _webClient.DownloadStringCompleted += client_GetDestinationListCompleted;

            _webClient.DownloadStringAsync(
                new Uri(string.Format("{0}/list/destinations/{1}",
                _baseUrl, from)));
        }

        /// <summary>
        /// //Requests the Web Server's REST service to get flight information,
        /// //calls the method client_GetSearchListCompleted
        /// </summary>
        public void GetSearchList()
        {
            _webClient.DownloadStringCompleted += client_GetSearchListCompleted;

            _webClient.DownloadStringAsync(
                new Uri(string.Format("{0}/search?From={1}&To={2}",
                _baseUrl, Main.FromBoxText, Main.ToBoxText)));
        }

        
        /// <summary>
        ///  //Requests the Web Server's REST service to get Watchlist,
        ///  //calls the method client_GetWatchListCompleted
        /// </summary>
        /// <param name="from">Initial city name</param>
        /// <param name="to">Destination City name</param>
        public void GetWatchList(string from, string to)
        {
            _webClient.DownloadStringCompleted += client_GetWatchListCompleted;

            _webClient.DownloadStringAsync(
                new Uri(string.Format("{0}/watch?From={1}&To={2}",
                _baseUrl, from, to)));
        }

        /// <summary>
        /// //Filters initial city from cityList to
        /// //create a destination list, and calls a method in Main to fill ToBox combobox.
        /// </summary>
        public void PopulateToList()
        {
            string[] _filteredArray = _cityList.Where(x => x != Main.FromBoxText).ToArray();
            Main.PopulateToBox(_filteredArray);
        }
        
        /// <summary>
        /// //Deserialize the requested Array from Web Server's REST Service
        /// //Calls method in Main to fill FromBox combobox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Contains Json encoded citylist Array</param>
        private void client_GetCityListCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null || e.Cancelled == null)
            {
                var json = new DataContractJsonSerializer(typeof(string[]));
                _cityList = json.ReadObject(new MemoryStream(Encoding.Default.GetBytes(e.Result))) as string[];

                Main.PopulateFromBox(_cityList);
            }

            _webClient.DownloadStringCompleted -= client_GetCityListCompleted;
        }

        /// <summary>
        /// //Deserialize the requested Array from Web Server's REST Service
        /// //Calls method in Main?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Contains Json encoded destinationlist Array</param>
        private void client_GetDestinationListCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null || e.Cancelled == null)
            {
                var json = new DataContractJsonSerializer(typeof(string[]));
                var dList = json.ReadObject(new MemoryStream(Encoding.Default.GetBytes(e.Result))) as string[];

                //Main.ShowCities(dList);
            }

            _webClient.DownloadStringCompleted -= client_GetDestinationListCompleted;
        }
        
        /// <summary>
        /// //Deserialize the requested flight information from Web Server's REST Service
        /// //Separates description and price from Tuple.
        /// //Calls methods in Overview to fill Labels with flight information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Contains Json encoded Tuple, which within contains a list of cities and price</param>
        private void client_GetSearchListCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null || e.Cancelled == null)
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

        /// <summary>
        /// //Deserialize the requested Watchlist from Web Server's REST Service
        /// //Calls method in Main?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Contains a Json encoded citylist</param>
        private void client_GetWatchListCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null || e.Cancelled == null)
            {
                var json = new DataContractJsonSerializer(typeof(string[]));
                var dList = json.ReadObject(new MemoryStream(Encoding.Default.GetBytes(e.Result))) as string[];

                //Main.ShowCities(wList);
            }

            _webClient.DownloadStringCompleted -= client_GetWatchListCompleted;
        }
    }
}
