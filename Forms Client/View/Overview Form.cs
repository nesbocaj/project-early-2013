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
    public partial class Overview_Form : Form
    {
        private Presenter.Presenter _presenter = null;

        /// <summary>
        /// Initializes a new instance of the Overview Form
        /// and gets the instance of the Presenter.Presenter class
        /// </summary>
        public Overview_Form()
        {
            InitializeComponent();
            _presenter = Presenter.Presenter.GetInstance();
        }

        /// <summary>
        /// Populates the form with all the data pulled from the Presenter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Overview_Form_Shown(object sender, EventArgs e)
        {
            _presenter.PopulateOverview();
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
        /// Triggers the ChangeOverview method in the Presenter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, EventArgs e)
        {
            _presenter.ChangeOverview();
        }

        /// <summary>
        /// Changes the visibility of the Cancel button
        /// </summary>
        /// <param name="visibility"></param>
        public void SetCancelButtonVisibility(bool visibility)
        {
            CancelButton.Visible = visibility;
        }

        /// <summary>
        /// Saves a new instance of the form when the form is closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Overview_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            _presenter.OverviewForm = new Overview_Form();
        }

        /// <summary>
        /// Sets the text that will be displayed in the DescriptionLabel
        /// </summary>
        /// <param name="text"></param>
        public void DescriptopnLabelText(string text)
        {
            DescriptionLabel.Text =  text;
        }

        /// <summary>
        /// Sets the text that will be displayed in the PriceLabel
        /// </summary>
        /// <param name="price"></param>
        public void PriceLabelText(decimal price)
        {
            PriceLabel.Text = "Pris:\n\n" + price.ToString() +" kr.";
        }

    }
}
