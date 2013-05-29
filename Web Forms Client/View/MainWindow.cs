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
    public partial class MainWindow : Form
    {
        private readonly Presenter.Presenter _presenter = null;

        public MainWindow()
        {
            InitializeComponent();
            FromBox.Enabled = ToBox.Enabled = OKButton.Enabled = false;
            _presenter = Presenter.Presenter.Getinstance();
            _presenter.Main = this;
            _presenter.GetCityList();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            _presenter.CallOverview();
        }

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

        private void ChancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }



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

        private void FromBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _presenter.PopulateToList();
            ToBox.Enabled = true;
        }

        private void ToBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            OKButton.Enabled = true;
        }
    }
}
