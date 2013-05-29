using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Web_Forms_Client.View
{
    public interface IOverwiew
    {
        void SetCancelButtonVisibility(bool visibility);
        void DescriptopnLabelText(string text);
        void PriceLabelText(decimal price);

        void Close();
        DialogResult ShowDialog();
    }
}
