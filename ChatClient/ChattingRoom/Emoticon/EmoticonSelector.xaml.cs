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
using System.Windows.Media.Animation;

namespace ChatClient.Emoticon
{
    public partial class EmoticonSelector : Window
    {
        public string ID { get; set; }
        public string UUri { get; set; }
        public List<IconInfo> listEmoticonInfo { get; set; }

        public event EventHandler EmoticonSelected = delegate { };
        public event EventHandler EmoticonClosing = delegate { };
        //GifImage gifImage = null;

        bool closing = false;

        public EmoticonSelector()
        {
            InitializeComponent();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if (!closing)
            {
                EmoticonClosing(this, new EventArgs());
                Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            closing = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
                EmoticonClosing(this, new EventArgs());
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (listEmoticonInfo != null || listEmoticonInfo.Count > 0)
            {
                foreach (IconInfo emoticonInfo in listEmoticonInfo)
                {
                    var emoticon = new Emoticon();
                    emoticon.MouseEnter += new MouseEventHandler(emoticon_MouseEnter);

                    //string uri = "\\\\" + Login._ServerUri + "\\" + emoticonInfo.Icon;
                    string uri = emoticonInfo.Icon;

                    emoticon.DataContext = new Emoticon(new Uri("/Resources;component/image/emoticon/" + emoticonInfo.Icon, UriKind.RelativeOrAbsolute), emoticonInfo.Name, emoticonInfo.Id);
                    
                    string id = emoticonInfo.Id;

                    emoticon.MouseDown += (send, ex) =>
                    {
                        ID = id;
                        UUri = uri;
                        EmoticonSelected(this, EventArgs.Empty);
                        Close();
                    };

                    root.Children.Add(emoticon);
                }
            }
        }

        void emoticon_MouseEnter(object sender, MouseEventArgs e)
        {
            var emoIcon = sender as Emoticon;

            //gifImage = new GifImage();
            //gifImage.GifSource = emoIcon.DataContext.ToString();
            //gifImage.StartAnimation();
        }
    }

    //class GifImage : Image
    //{
    //    private bool _isInitialized;
    //    private GifBitmapDecoder _gifDecoder;
    //    private Int32Animation _animation;
        

    //    public int FrameIndex
    //    {
    //        get { return (int)GetValue(FrameIndexProperty); }
    //        set { SetValue(FrameIndexProperty, value); }
    //    }

    //    private void Initialize()
    //    {
    //        _gifDecoder = new GifBitmapDecoder(new Uri("E:\\Messenger_Work\\2013-06-02\\ChatClient\\bin\\Debug\\image\\0.gif"), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
    //        _animation = new Int32Animation(0, _gifDecoder.Frames.Count - 1, new Duration(new TimeSpan(0, 0, 0, _gifDecoder.Frames.Count / 10, (int)((_gifDecoder.Frames.Count / 10.0 - _gifDecoder.Frames.Count / 10) * 1000))));
    //        _animation.RepeatBehavior = RepeatBehavior.Forever;
    //        this.Source = _gifDecoder.Frames[0];

    //        _isInitialized = true;
    //    }

    //    static GifImage()
    //    {
    //        VisibilityProperty.OverrideMetadata(typeof(GifImage),
    //            new FrameworkPropertyMetadata(VisibilityPropertyChanged));
    //    }

    //    private static void VisibilityPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    //    {
    //        if ((Visibility)e.NewValue == Visibility.Visible)
    //        {
    //            ((GifImage)sender).StartAnimation();
    //        }
    //        else
    //        {
    //            ((GifImage)sender).StopAnimation();
    //        }
    //    }

    //    public static readonly DependencyProperty FrameIndexProperty =
    //        DependencyProperty.Register("FrameIndex", typeof(int), typeof(GifImage), new UIPropertyMetadata(0, new PropertyChangedCallback(ChangingFrameIndex)));

    //    static void ChangingFrameIndex(DependencyObject obj, DependencyPropertyChangedEventArgs ev)
    //    {
    //        var gifImage = obj as GifImage;
    //        gifImage.Source = gifImage._gifDecoder.Frames[(int)ev.NewValue];
    //    }

    //    /// <summary>
    //    /// Defines whether the animation starts on it's own
    //    /// </summary>
    //    public bool AutoStart
    //    {
    //        get { return (bool)GetValue(AutoStartProperty); }
    //        set { SetValue(AutoStartProperty, value); }
    //    }

    //    public static readonly DependencyProperty AutoStartProperty =
    //        DependencyProperty.Register("AutoStart", typeof(bool), typeof(GifImage), new UIPropertyMetadata(false, AutoStartPropertyChanged));

    //    private static void AutoStartPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    //    {
    //        if ((bool)e.NewValue)
    //            (sender as GifImage).StartAnimation();
    //    }

    //    public string GifSource
    //    {
    //        get { return (string)GetValue(GifSourceProperty); }
    //        set { SetValue(GifSourceProperty, value); }
    //    }

    //    public static readonly DependencyProperty GifSourceProperty =
    //        DependencyProperty.Register("GifSource", typeof(string), typeof(GifImage), new UIPropertyMetadata(string.Empty, GifSourcePropertyChanged));

    //    private static void GifSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    //    {
    //        (sender as GifImage).Initialize();
    //    }

    //    /// <summary>
    //    /// Starts the animation
    //    /// </summary>
    //    public void StartAnimation()
    //    {

    //        if (!_isInitialized)
    //            this.Initialize();

    //        BeginAnimation(FrameIndexProperty, _animation);
    //    }

    //    /// <summary>
    //    /// Stops the animation
    //    /// </summary>
    //    public void StopAnimation()
    //    {
    //        BeginAnimation(FrameIndexProperty, null);
    //    }
    //}
}
