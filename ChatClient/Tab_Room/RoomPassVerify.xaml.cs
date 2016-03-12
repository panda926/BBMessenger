using System.Windows;
using ChatEngine;

using ControlExs;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for RoomPassVerify.xaml
    /// </summary>
    public partial class RoomPassVerify : Window
    {
        private string _veriPass = null;
        private RoomInfo verifyRoomInfo = null;

        public RoomPassVerify()
        {
            InitializeComponent();
        }

        public void VerifyRoomInfo(RoomInfo verfyRoom)
        {
            verifyRoomInfo = verfyRoom;
            _veriPass = verfyRoom.RoomPass;
        }

        private void okBt_Click(object sender, RoutedEventArgs e)
        {
            if (_veriPass == passwordRoom.Password)
            {
                Login._ClientEngine.Send(NotifyType.Request_EnterRoom, verifyRoomInfo);
                this.Close();
            }
            else
            {                
                TempWindowForm tempWindowForm = new TempWindowForm();
                QQMessageBox.Show(tempWindowForm, "암호가 틀립니다.", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
            }
        }

        private void cancelBt_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
