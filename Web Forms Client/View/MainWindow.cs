using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Web_Forms_Client
{
    public partial class MainWindow : Form, Web_Forms_Client.View.IMainWindow
    {
        private readonly Presenter.Presenter _presenter = null;

        /// <summary>
        /// //Disables FromBox, ToBox and OK button till requirements are fullfilled.
        /// //Creates MainWindow instance, gets Presenter instance and CityList
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            FromBox.Enabled = ToBox.Enabled = OKButton.Enabled = false;
            _presenter = Presenter.Presenter.Getinstance();
            _presenter.Main = this;
            _presenter.GetCityList();
        }

        /// <summary>
        /// //Calls Presenter to Open Overview Form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, EventArgs e)
        {
            _presenter.CallOverview();
        }

        /// <summary>
        /// //Clears outdated info in ToBox combobox
        /// //Enables and fills in FromBox combobox
        /// </summary>
        /// <param name="items">Contains an array of citynames to fill FromBox</param>
        public void PopulateFromBox(string[] items)
        {
            ToBox.Items.Clear();
            FromBox.Items.AddRange(items);
            FromBox.Enabled = true;
        }

        /// <summary>
        /// populates the Combobox that shows the Arrival cities
        /// </summary>
        /// <param name="items">An array of city names</param>
        public void PopulateToBox(string[] items)
        {
            ToBox.Items.Clear(); // makes sure the list is cleared before adding items, this is to avoid duplicates
            ToBox.Items.AddRange(items);
        }

        /// <summary>
        /// //Gets text from FromBox & ToBox comboboxes.
        /// </summary>
        /// <returns>Returns the chosen citynames from the comboboxes</returns>
        public string[] ChosenCities()
        {
            string from, to;
            if (FromBox.Text == null)
                from = "";
            else
                from = FromBox.Text;

            if (ToBox.Text == null)
                to = "";
            else
                to = ToBox.Text;
            var arr = new String[] { from, to };
            return arr;
        }

        /// <summary>
        /// //Closes this Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// //Gets text from FromBox combobox
        /// </summary>
        public String FromBoxText
        {
            get { return FromBox.Text; }
        }

        /// <summary>
        /// Gets the text in the ToBox Combobox and makes it publicly visible
        /// </summary>
        public String ToBoxText
        {
            get { return ToBox.Text; }
        }

        /// <summary>
        /// //Enables and calls method to fill in ToBox combobox, when selects an item in FromBox combobox 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FromBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _presenter.PopulateToList();
            ToBox.Enabled = true;
        }

        /// <summary>
        /// //Enables OK button when selects an item in ToBox combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            OKButton.Enabled = true;
        }
    }
}
