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

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for PNFWindowsView.xaml
    /// </summary>
    public partial class PNFWindowsView : Window
    {
        int m_count = 0;
        int m_class = 0;
        int m_callCount = 0;
        int selNumber = 0;
        int m_classCount = 0;
        int firstIndex = 1;
        int endIndex = 0;

        System.Windows.Threading.DispatcherTimer disapthcerTimer = null;
        int _nStep = 0;
        ClassTypeListInfo m_classTypeListInfo = null;
        bool _bSuccess = false;

        public PNFWindowsView()
        {
            InitializeComponent();
        }

        public void InitPNFView(ClassTypeListInfo classTypeListInfo)
        {
            m_classTypeListInfo = classTypeListInfo;

            firstIndex = 1;
            wrapPanel1.Children.Clear();
            m_class = classTypeListInfo.ClassType[0].Class_File_Type;
            m_callCount = classTypeListInfo.ClassType[0].Class_Row_Number;
            m_count = classTypeListInfo.ClassType[0].Class_File_Count;

            _nStep = 0;

            for (int j = 0; j < Window1._ClassListInfo.Classes.Count; j++)
            {
                if (Window1._ClassListInfo.Classes[j].ClassInfo_Id == classTypeListInfo.ClassType[0].Class_File_Type)
                {
                    titleLabel.Content = Window1._ClassListInfo.Classes[j].Class_Name;
                    break;
                }
            }

            m_classCount = 12;

            if (m_classTypeListInfo.ClassType.Count > 0)
            {
                disapthcerTimer = new System.Windows.Threading.DispatcherTimer();
                disapthcerTimer.Interval = new TimeSpan(0, 0, 0, 0, 5);
                disapthcerTimer.Tick += new EventHandler(Each_Tick);
                disapthcerTimer.Start();
            }
            
            for (int i = 0; i < classTypeListInfo.ClassType.Count; i++)
            {
                if (classTypeListInfo.ClassType[i].Class_File_Type == 2 || classTypeListInfo.ClassType[i].Class_File_Type == 0)
                {
                    
                }
                else
                {
                    m_classCount = 18;
                    Window1._ClassTypeInfo = classTypeListInfo.ClassType[i];
                    NovelUserControl novelUserControl = new NovelUserControl();
                    novelUserControl.InitNovelView(Window1._ClassTypeInfo);
                    wrapPanel1.Children.Add(novelUserControl);
                }

            }
            if ((m_callCount / m_classCount) / 10 > 0)
            {
                int addIndex = ((m_callCount / m_classCount) / 10) * 10;
                firstIndex = firstIndex + addIndex;
            }

            if (m_count / m_classCount > 10)
            {
                if ((firstIndex + 9) * m_classCount < m_count)
                    endIndex = firstIndex + 9;
                else
                {
                    int m_Page = m_count - (firstIndex * m_classCount);
                    if (m_Page % m_classCount > 0)
                        endIndex = firstIndex + m_Page / m_classCount + 1;
                    else
                        endIndex = firstIndex + m_Page / m_classCount - 1;
                }
            }
            else
            {
                if (m_count % m_classCount > 0)
                    endIndex = firstIndex + m_count / m_classCount;
                else
                    endIndex = firstIndex + m_count / m_classCount - 1;
            }


            PageDisplay(m_class, firstIndex, endIndex);
        }

        private void Each_Tick(Object obj, EventArgs ev)
        {
            try
            {
                if (_nStep + 1 == m_classTypeListInfo.ClassType.Count)
                    disapthcerTimer.Stop();

                if ((m_classTypeListInfo.ClassType[_nStep].Class_File_Type == 2 || m_classTypeListInfo.ClassType[_nStep].Class_File_Type == 0) && !_bSuccess)
                {
                    _bSuccess = true;

                    Window1._ClassTypeInfo = m_classTypeListInfo.ClassType[_nStep];
                    FilmUserControl filmUserControl = new FilmUserControl();

                    wrapPanel1.Children.Add(filmUserControl);
                    filmUserControl.InitFilmView(Window1._ClassTypeInfo);

                    _nStep++;
                    _bSuccess = false;
                }
            }
            catch (Exception e)
            {
                string s = e.ToString();
            }
        }

        private void PageDisplay(int classes, int toInt, int fromInt)
        {
            Pagegrid.Children.Clear();
            resultGrid.Children.Clear();
            Label lg = new Label();
            lg.Foreground = new SolidColorBrush(Colors.Black);
            lg.Content = "총 조회건수: " + m_count;
            resultGrid.Children.Add(lg);
            if (fromInt > 2)
            {

                selNumber = m_callCount / m_classCount + 1;

                System.Windows.Controls.Primitives.UniformGrid uniformGrid = new System.Windows.Controls.Primitives.UniformGrid();
                uniformGrid.Rows = 1;
                uniformGrid.Columns = fromInt + 2;
                Button preBt = new Button();
                preBt.Content = "Pre";
                if (selNumber == 1)
                    preBt.IsEnabled = false;
                preBt.Foreground = new SolidColorBrush(Colors.Black);
                preBt.Click += new RoutedEventHandler(preBt_Click);
                uniformGrid.Children.Add(preBt);


                for (int i = toInt; i <= fromInt; i++)
                {
                    Button pgBt = new Button();
                    pgBt.Content = i;
                    pgBt.DataContext = i;

                    if (selNumber == i)
                        pgBt.Foreground = new SolidColorBrush(Colors.Red);
                    else
                        pgBt.Foreground = new SolidColorBrush(Colors.Black);

                    pgBt.Click += new RoutedEventHandler(pgBt_Click);
                    uniformGrid.Children.Add(pgBt);
                }

                Button nextBt = new Button();
                nextBt.Content = "Next";

                if (selNumber == m_count / m_classCount && m_count % m_classCount == 0)
                    nextBt.IsEnabled = false;
                else if (m_count % m_classCount != 0 && selNumber == (m_count / m_classCount) + 1)
                    nextBt.IsEnabled = false;
                nextBt.Foreground = new SolidColorBrush(Colors.Black);
                nextBt.Click += new RoutedEventHandler(nextBt_Click);
                uniformGrid.Children.Add(nextBt);

                Pagegrid.Children.Add(uniformGrid);
            }

        }

        void nextBt_Click(object sender, RoutedEventArgs e)
        {
            if (selNumber <= (m_count / m_classCount))
            {

                Window1._ClassInfo.ToIndex = 1 + (m_classCount * selNumber);
                Window1._ClassInfo.FromIndex = m_classCount * (selNumber + 1);
                Window1._ClientEngine.Send(NotifyType.Request_ClassTypeInfo, Window1._ClassInfo);
            }
        }

        void preBt_Click(object sender, RoutedEventArgs e)
        {
            if (selNumber != 1)
            {
                Window1._ClassInfo.ToIndex = 1 + (m_classCount * (selNumber - 2));
                Window1._ClassInfo.FromIndex = m_classCount * (selNumber - 1);
                Window1._ClientEngine.Send(NotifyType.Request_ClassTypeInfo, Window1._ClassInfo);
            }
        }

        void pgBt_Click(object sender, RoutedEventArgs e)
        {
            Button btClick = sender as Button;
            int currentPg = Convert.ToInt32(btClick.DataContext);

            Window1._ClassInfo.ToIndex = 1 + (m_classCount * (currentPg - 1));
            Window1._ClassInfo.FromIndex = m_classCount * currentPg;
            Window1._ClientEngine.Send(NotifyType.Request_ClassTypeInfo, Window1._ClassInfo);

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Main.pnfWindowsView = null;
            Main.pictureName = "";
            Main.novelPlayStateFlag = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
