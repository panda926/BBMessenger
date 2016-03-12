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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatClient.Emoticon
{
    public partial class Emoticon : UserControl
    {
        public Uri ImageUri { get; private set; }
        public string Title { get; private set; }
        public string ID { get; private set; }

        public Emoticon()
        {
            InitializeComponent();
        }

        public Emoticon(Uri imageUri, string title, string strID)
        {
            this.ImageUri = imageUri;
            this.Title = title;
            this.ID = strID;
        }
    }
}
