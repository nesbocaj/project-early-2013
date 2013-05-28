using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forms_Client.View
{
    public partial class MainWindow : Form
    {
        private readonly Presenter.Presenter _presenter = null;

        /// <summary>
        /// Initializes form, adds the instance of the form to the Presenter, 
        /// and populates the Comboboxes
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            _presenter = Presenter.Presenter.GetInstance();
            _presenter.MainForm = this;
            //_presenter.PopulateFromList();
        }

        /// <summary>
        /// Calls the View.Overview_Form form from the Presenter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, EventArgs e)
        {
            _presenter.CallOverview();
        }

        /// <summary>
        /// Closes the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Takes the content of the comboboxes and returns it in a string array
        /// if the comboboxes contain a null reference, they're treated as empty
        /// </summary>
        /// <returns>An array of strings with the text from the comboboxes</returns>
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
        /// Populates the Combobox that shows the departure cities
        /// </summary>
        /// <param name="items">An array of city names</param>
        public void PopulateFromBox(string[] items)
        {
            ToBox.Items.Clear();
            FromBox.Items.AddRange(items);
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
        /// Gets the text in the FromBox Combobox and makes it publicly visible
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
        /// Updates the ToBox Combobox whenever the FromBox Combobox's selection changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FromBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _presenter.PopulateToList();
        }

        private void FromBox_Click(object sender, EventArgs e)
        {
            FromBox.Items.Clear();
            _presenter.OnFromBoxClicked();
        }
    }
}
