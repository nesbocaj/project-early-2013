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

        public MainWindow()
        {
            InitializeComponent();
            _presenter = Presenter.Presenter.GetInstance();
            _presenter.Main = this;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            _presenter.CallOverview();
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

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
