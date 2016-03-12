using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for ProgressBarWindow.xaml
    /// </summary>
    public partial class ProgressBarWindow : Window
    {
        string bitUnit1 = null;
        string bitUnit2 = null;
        string timeUnit = null;
        string remainTime = null;

        public ProgressBarWindow()
        {
            InitializeComponent();

        }

        public void ProgressValue(int _value)
        {
            progressBar1.Value = _value;
        }

        public void ProgrssStateDisplay(string _name, double _perBit, double _capacity, double _currentcap)
        {
            label1.Content = "";
            proName.Content = "";
            label1.Content = _name;
            if ((_capacity % 1000) > 0)
            {
                _capacity = _capacity / 1000;
                bitUnit1 = "MB";
            }
            else
                bitUnit1 = "KB";

            if ((_currentcap % 1000) > 0)
            {
                _currentcap = _currentcap / 1000;
                bitUnit2 = "MB";
            }
            else
                bitUnit2 = "KB";

            double d_seconds = (_capacity - _currentcap) * 1000 / _perBit / 60;
            if (d_seconds > 1)
            {
                timeUnit = "분";

                remainTime = ((int)d_seconds).ToString();
            }
            else
            {
                timeUnit = "초";
                int s_seconds = (int)((_capacity - _currentcap) * 1000 / _perBit) % 60;
                if (s_seconds <= 0)
                    remainTime = "0";
                else
                    remainTime = s_seconds.ToString();
            }

            proName.Content = Math.Round(_perBit, 1) + "KB/초" + " - " +
                Math.Round(_capacity, 1) + bitUnit1 + "중 " + Math.Round(_currentcap, 1) + bitUnit2 +  
                "  " + remainTime + timeUnit + " 남음";
        }
    }
}
