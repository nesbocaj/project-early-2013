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

        public Overview_Form()
        {
            InitializeComponent();
            _presenter = Presenter.Presenter.GetInstance();
        }

        private void Overview_Form_Shown(object sender, EventArgs e)
        {
            _presenter.PopulateOverview();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            _presenter.ChangeOverview();
        }

        public void SetCancelButtonVisibility(bool visibility)
        {
            CancelButton.Visible = visibility;
        }

        private void Overview_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            _presenter.Overview = new Overview_Form();
        }

        public void DescriptopnLabelText(string text)
        {
            DescriptionLabel.Text =  text;
        }
        public void PriceLabelText(decimal price)
        {
            PriceLabel.Text = "Pris:\n\n" + price.ToString() +" kr.";
        }

    }
}
