using System.Windows;
using ChatEngine;

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
                Window1._ClientEngine.Send(NotifyType.Request_EnterRoom, verifyRoomInfo);
                this.Close();
            }
            else
                MessageBoxCommon.Show("암호가 틀립니다.", MessageBoxType.Ok);
        }

        private void cancelBt_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
