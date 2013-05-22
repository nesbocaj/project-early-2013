﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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

        public void GetCityList()
        {
            _webClient.DownloadStringCompleted += client_GetCityListCompleted;

            _webClient.DownloadStringAsync(
                new Uri(string.Format("{0}/list/Test",
                _baseUrl)));
        }

        private void client_GetCityListCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                DataContractJsonSerializer Serializer = new DataContractJsonSerializer(typeof(JsonMessage));
                JsonMessage message = (JsonMessage)Serializer.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(e.Result)));
                JavaScriptSerializer js = new JavaScriptSerializer();
                string[] Cities = js.Deserialize<string[]>(message.Message);

                //string test = xmlResponse.Root.Value;

            }

            _webClient.DownloadStringCompleted -= client_GetCityListCompleted;
        }

        public class JsonMessage
        {
            public string Message;
        }
    }
}
