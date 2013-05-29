using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Forms_Client.View
{
    public interface IOverwiewWindow
    {
        void SetCancelButtonVisibility(bool visibility);
        void DescriptopnLabelText(string text);
        void PriceLabelText(decimal price);

        void Close();
        DialogResult ShowDialog();
    }
}
