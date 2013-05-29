using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Web_Forms_Client.View
{
    public partial class Overview : Form, IOverwiew
    {
        private Presenter.Presenter _presenter = null;

        /// <summary>
        /// //Gets presenter instance
        /// </summary>
        public Overview()
        {
            InitializeComponent();
            _presenter = Presenter.Presenter.Getinstance();
        }

        /// <summary>
        /// //Closes this Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        /// <summary>
        /// //Calls method in Presenter to either close or removes Cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButton_Click(object sender, EventArgs e)
        {
            _presenter.ChangeOverview();
        }
        
        /// <summary>
        /// //Removes Cancel button from GUI
        /// </summary>
        /// <param name="visibility">boolean of if the button is visible or not</param>
        public void SetCancelButtonVisibility(bool visibility)
        {
            CancelButton.Visible = visibility;
        }

        /// <summary>
        /// Sets the text that will be displayed in the DescriptionLabel
        /// </summary>
        /// <param name="text">Description of flight course to fill in the label</param>
        public void DescriptopnLabelText(string text)
        {
            DescriptionLabel.Text = text;
        }

        /// <summary>
        /// Sets the text that will be displayed in the PriceLabel
        /// </summary>
        /// <param name="price">The total price between initial to destination city to fill in the label</param>
        public void PriceLabelText(decimal price)
        {
            PriceLabel.Text = "Pris:\n\n" + price.ToString() + " kr.";
        }

        /// <summary>
        /// //When GUI Overview i shown, get flight information from presenter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Overview_Shown(object sender, EventArgs e)
        {
            _presenter.GetSearchList();
        }
    }
}
