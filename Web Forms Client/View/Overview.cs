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
    public partial class Overview : Form
    {
        private Presenter.Presenter _presenter = null;

        public Overview()
        {
            InitializeComponent();
            _presenter = Presenter.Presenter.Getinstance();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void OkButton_Click(object sender, EventArgs e)
        {
            _presenter.ChangeOverview();
        }
        
        public void SetCancelButtonVisibility(bool visibility)
        {
            CancelButton.Visible = visibility;
        }

        /// <summary>
        /// Sets the text that will be displayed in the DescriptionLabel
        /// </summary>
        /// <param name="text"></param>
        public void DescriptopnLabelText(string text)
        {
            DescriptionLabel.Text = text;
        }

        /// <summary>
        /// Sets the text that will be displayed in the PriceLabel
        /// </summary>
        /// <param name="price"></param>
        public void PriceLabelText(decimal price)
        {
            PriceLabel.Text = "Pris:\n\n" + price.ToString() + " kr.";
        }

        private void Overview_Shown(object sender, EventArgs e)
        {
            _presenter.GetSearchList();
        }
    }
}
