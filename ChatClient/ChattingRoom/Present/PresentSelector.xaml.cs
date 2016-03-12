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

using ChatEngine;

namespace ChatClient.Present
{
    public partial class PresentSelector : Window
    {
        public string ID { get; set; }
        public string UUri { get; set; }
        
        public List<IconInfo> listPresentInfo { get; set; }
       

        public event EventHandler PresentSelected = delegate { };

        bool closing = false;


        public PresentSelector()
        {
            InitializeComponent();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if (!closing)
                Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            closing = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (IconInfo presentInfo in listPresentInfo)
            {
                var present = new Present();

                

                string uri = "\\\\" + Login._ServerUri + "\\" + presentInfo.Icon;
                //present.DataContext = new Present(new Uri(uri, UriKind.RelativeOrAbsolute), presentInfo.Name, presentInfo.Id);

                present.DataContext = new Present(presentInfo.Name, presentInfo.Name, presentInfo.Id);
                string id = presentInfo.Id;
                int nPoint = presentInfo.Point;
                
                present.MouseDown += (send, ex) =>
                {
                    ID = id;
                    UUri = uri;
                    
                    
                    PresentSelected(this, EventArgs.Empty);
                    Close();
                };

                root.Children.Add(present);
            }
        }
    }
}
