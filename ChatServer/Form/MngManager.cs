using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChatEngine;

namespace ChatServer
{
    public partial class MngManager : Form
    {
        public static bool _showManager = true;
        public static bool _showRecommendOfficer = true;
        public static bool _showRecommender = true;
        public static bool _showServiceOfficer = true;
        public static bool _showServiceman = true;
        public static bool _showBanker = true;

        public bool _isUpdateUserList = true;
        public bool _isDisconnectWeb = false;

        public List<UserInfo> _AllUserList = null;

        public bool _isSaveUserList = false;
        public List<UserInfo> _saveList = new List<UserInfo>();

        public MngManager()
        {
            InitializeComponent();
        }

        private void MngUser_Load(object sender, EventArgs e)
        {
            if (Users.ManagerInfo.Kind != (int)UserKind.Manager)
            {
                if (Users.ManagerInfo.Kind != (int)UserKind.ServiceOfficer)
                {
                    checkServiceOfficer.Enabled = false;
                    _showServiceOfficer = false;

                    checkServiceman.Enabled = false;
                    _showServiceman = false;

                    buttonAddServiceman.Enabled = false;
                }

                if (Users.ManagerInfo.Kind != (int)UserKind.RecommendOfficer)
                {
                    checkRecommendOfficer.Enabled = false;
                    _showRecommendOfficer = false;

                    checkRecommender.Enabled = false;
                    _showRecommender = false;

                    buttonAddRecommender.Enabled = false;
                }

                checkManager.Enabled = false;
                _showManager = false;

                checkBanker.Enabled = false;
                _showBanker = false;

                comboRecommender.Enabled = false;
                buttonAddRecommendOfficer.Enabled = false;
                buttonAddServiceOfficer.Enabled = false;
                buttonAddBanker.Enabled = false;
            }

            checkManager.Checked = _showManager;
            checkServiceOfficer.Checked = _showServiceOfficer;
            checkServiceman.Checked = _showServiceman;
            checkRecommendOfficer.Checked = _showRecommendOfficer;
            checkRecommender.Checked = _showRecommender;
            checkBanker.Checked = _showBanker;

            _AllUserList = Database.GetInstance().GetAllUsers();

            RefreshRecommenderList();

            RefreshUserList();
        }


        public void RefreshRecommenderList()
        {
            comboRecommender.Items.Clear();
            comboID.Items.Clear();

            string queryStr = string.Format("select * from tblUser where Kind != {0} ", (int)UserKind.Buyer);

            if (Users.ManagerInfo.Kind == (int)UserKind.RecommendOfficer)
            {

                queryStr += string.Format(" and Kind != {0}", (int)UserKind.Manager);
                queryStr += string.Format(" and Kind != {0}", (int)UserKind.ServiceOfficer);
                queryStr += string.Format(" and Kind != {0}", (int)UserKind.ServiceWoman);
                queryStr += string.Format(" and Recommender = '{0}'", Users.ManagerInfo.Id);
                queryStr += string.Format(" or Id = '{0}'", Users.ManagerInfo.Id);
            }
            else if (Users.ManagerInfo.Kind == (int)UserKind.ServiceOfficer)
            {
                queryStr += string.Format(" and Kind != {0}", (int)UserKind.Manager);
                queryStr += string.Format(" and Kind != {0}", (int)UserKind.RecommendOfficer);
                queryStr += string.Format(" and Kind != {0}", (int)UserKind.Recommender);
                queryStr += string.Format(" and Recommender = '{0}'", Users.ManagerInfo.Id);
                queryStr += string.Format(" or Id = '{0}'", Users.ManagerInfo.Id);
            }

            queryStr += " order by Id";


            List<UserInfo> recommenderList = Database.GetInstance().GetUserList(queryStr);

            if (recommenderList != null)
            {
                foreach (UserInfo userInfo in recommenderList)
                {
                    if( userInfo.Kind != (int)UserKind.Recommender && userInfo.Kind != (int)UserKind.ServiceWoman )
                        comboRecommender.Items.Add(userInfo.Id);

                    comboID.Items.Add(userInfo.Id);
                }
            }
        }

        public int GetChildCount( string userId )
        {
            int childCount = 0;

            foreach (UserInfo userInfo in _AllUserList)
            {
                if (userInfo.Recommender == userId)
                    childCount++;
            }

            return childCount;
        }

        public int GetOfficerChatPercent(UserInfo userInfo)
        {
            if (userInfo.Kind == (int)UserKind.Manager)
                return 0;

            if (userInfo.Kind == (int)UserKind.Recommender || userInfo.Kind == (int)UserKind.RecommendOfficer)
                return 0;

            if (userInfo.Kind == (int)UserKind.ServiceOfficer )
                return 100;

            int chatPercent = 0;

            foreach (UserInfo officerInfo in _AllUserList)
            {
                if (officerInfo.Id == userInfo.Recommender)
                {
                    chatPercent = officerInfo.ChatPercent;
                    break;
                }
            }

            return chatPercent;
        }

        public int GetOfficerGamePercent(UserInfo userInfo)
        {
            if (userInfo.Kind == (int)UserKind.Manager)
                return 0;

            if (userInfo.Kind == (int)UserKind.ServiceWoman || userInfo.Kind == (int)UserKind.ServiceOfficer)
                return 100;

            if (userInfo.Kind == (int)UserKind.RecommendOfficer)
                return 100;

            int gamePercent = 0;

            foreach (UserInfo officerInfo in _AllUserList)
            {
                if (officerInfo.Id == userInfo.Recommender)
                {
                    gamePercent = officerInfo.GamePercent;
                    break;
                }
            }

            return gamePercent;
        }

        public void RefreshUserList()
        {
            if (_isUpdateUserList == true)
            {
                _AllUserList = Database.GetInstance().GetAllUsers();

                string queryStr = string.Format("select * from tblUser where 0 = 1 ");

                if (checkManager.Checked == true)
                    queryStr += string.Format(" or Kind = {0}", (int)UserKind.Manager);

                if (checkServiceOfficer.Checked == true)
                {
                    if (Users.ManagerInfo.Kind == (int)UserKind.ServiceOfficer)
                        queryStr += string.Format(" or Id = '{0}'", Users.ManagerInfo.Id);
                    else
                        queryStr += string.Format(" or Kind = {0}", (int)UserKind.ServiceOfficer);
                }

                if (checkServiceman.Checked == true)
                {
                    if (Users.ManagerInfo.Kind == (int)UserKind.ServiceOfficer)
                        queryStr += string.Format(" or Kind = {0} and Recommender = '{1}'", (int)UserKind.ServiceWoman, Users.ManagerInfo.Id);
                    else
                    {
                        if (string.IsNullOrEmpty(comboRecommender.Text.Trim()) == false)
                            queryStr += string.Format(" or Kind = {0} and Recommender = '{1}'", (int)UserKind.ServiceWoman, comboRecommender.Text.Trim());
                        else
                            queryStr += string.Format(" or Kind = {0}", (int)UserKind.ServiceWoman);
                    }
                }

                if (checkRecommendOfficer.Checked == true)
                {
                    if (Users.ManagerInfo.Kind == (int)UserKind.RecommendOfficer)
                        queryStr += string.Format(" or Id = '{0}'", Users.ManagerInfo.Id);
                    else
                        queryStr += string.Format(" or Kind = {0}", (int)UserKind.RecommendOfficer);
                }

                if (checkRecommender.Checked == true)
                {
                    if (Users.ManagerInfo.Kind == (int)UserKind.RecommendOfficer)
                        queryStr += string.Format(" or Kind = {0} and Recommender = '{1}'", (int)UserKind.Recommender, Users.ManagerInfo.Id);
                    else
                    {
                        if (string.IsNullOrEmpty(comboRecommender.Text.Trim()) == false)
                            queryStr += string.Format(" or Kind = {0} and Recommender = '{1}'", (int)UserKind.Recommender, comboRecommender.Text.Trim());
                        else
                            queryStr += string.Format(" or Kind = {0}", (int)UserKind.Recommender);
                    }
                }

                if (checkBanker.Checked == true)
                    queryStr += string.Format(" or Kind = {0}", (int)UserKind.Banker);

                queryStr += " order by Kind desc";

                List<UserInfo> userList = Database.GetInstance().GetUserList(queryStr);

                if (userList == null)
                {
                    MessageBox.Show("不能在资料库获取管理信息.");
                    return;
                }

                _showManager = checkManager.Checked;
                _showServiceOfficer = checkServiceOfficer.Checked;
                _showServiceman = checkServiceman.Checked;
                _showRecommendOfficer = checkRecommendOfficer.Checked;
                _showRecommender = checkRecommender.Checked;

                UserView.Rows.Clear();

                for (int i = 0; i < userList.Count; i++)
                {
                    UserInfo userInfo = userList[i];

                    string checkSum = string.Empty;

                    if (userInfo.Cash != userInfo.ChargeSum + userInfo.DischargeSum + userInfo.ChatSum + userInfo.GameSum + userInfo.SendSum + userInfo.ReceiveSum)
                        checkSum = "总额错误";

                    int officerChatPercent = GetOfficerChatPercent(userInfo);
                    int officerGamePercent = GetOfficerGamePercent(userInfo);

                    if (userInfo.ChatPercent > officerChatPercent || userInfo.GamePercent > officerGamePercent)
                        checkSum = "益率错误";

                    UserView.Rows.Add(
                        string.Format("{0} ( {1} ) ", userInfo.Kind, userInfo.KindString),
                        userInfo.Id,
                        userInfo.Nickname,
                        userInfo.Recommender,
                        GetChildCount(userInfo.Id),
                        userInfo.ChatPercent,
                        officerChatPercent,
                        userInfo.GamePercent,
                        officerGamePercent,
                        string.Format("{0}", userInfo.Cash),
                        userInfo.ChargeSum + userInfo.DischargeSum,
                        userInfo.ChatSum,
                        userInfo.GameSum,
                        userInfo.SendSum + userInfo.ReceiveSum,
                        checkSum,
                        userInfo.LoginTime,
                        userInfo.strUrl
                        );

                    DataGridViewRow curRow = UserView.Rows[UserView.Rows.Count - 1];

                    curRow.Tag = userInfo;

                    if (string.IsNullOrEmpty(checkSum) == false)
                    {
                        for( int k = 9; k <= 14; k++ )
                            curRow.Cells[k].Style.ForeColor = Color.Red;
                    }

                    if (userInfo.ChatPercent > officerChatPercent)
                    {
                        curRow.Cells[5].Style.ForeColor = Color.Red;
                        curRow.Cells[14].Style.ForeColor = Color.Red;
                    }

                    if (userInfo.GamePercent > officerGamePercent)
                    {
                        curRow.Cells[7].Style.ForeColor = Color.Red;
                        curRow.Cells[14].Style.ForeColor = Color.Red;
                    }

                    if (userInfo.Kind == (int)UserKind.ServiceWoman)
                    {
                        if (Users.ManagerInfo.Kind == (int)UserKind.ServiceOfficer)
                        {
                            curRow.Cells[7].ReadOnly = true;
                            curRow.Cells[7].Style.BackColor = Color.White;
                        }
                    }
                    else if (userInfo.Kind == (int)UserKind.Recommender )
                    {
                        curRow.Cells[5].ReadOnly = true;
                        curRow.Cells[5].Style.BackColor = Color.White;
                    }
                    else if (userInfo.Kind == (int)UserKind.ServiceOfficer)
                    {
                        if (Users.ManagerInfo.Kind == (int)UserKind.ServiceOfficer)
                        {
                            curRow.Cells[5].ReadOnly = true;
                            curRow.Cells[5].Style.BackColor = Color.White;

                            curRow.Cells[7].ReadOnly = true;
                            curRow.Cells[7].Style.BackColor = Color.White;
                        }
                    }
                    else if (userInfo.Kind == (int)UserKind.RecommendOfficer)
                    {
                        curRow.Cells[5].ReadOnly = true;
                        curRow.Cells[5].Style.BackColor = Color.White;

                        if (Users.ManagerInfo.Kind == (int)UserKind.RecommendOfficer)
                        {
                            curRow.Cells[7].ReadOnly = true;
                            curRow.Cells[7].Style.BackColor = Color.White;
                        }
                    }
                    else if (userInfo.Kind == (int)UserKind.Manager)
                    {
                        curRow.Cells[5].ReadOnly = true;
                        curRow.Cells[5].Style.BackColor = Color.White;

                        curRow.Cells[7].ReadOnly = true;
                        curRow.Cells[7].Style.BackColor = Color.White;
                    }
                }

                labelSummary.Text = string.Format("总会员数: {0}", userList.Count);
            }

            if (string.IsNullOrEmpty(comboID.Text.Trim()) == false)
            {
                foreach (DataGridViewRow viewRow in UserView.Rows)
                {
                    UserInfo userInfo = (UserInfo)viewRow.Tag;

                    if (userInfo.Id.Contains(comboID.Text.Trim()) == true)
                    {
                        UserView.CurrentCell = viewRow.Cells[0];
                        break;
                    }
                }
            }

            _isUpdateUserList = false;

        }

        private void buttonEditUser_Click(object sender, EventArgs e)
        {
            if (UserView.CurrentCell == null)
            {
                MessageBox.Show("请选择会员帐号.");
                return;
            }

            UserInfo userInfo = (UserInfo)UserView.Rows[UserView.CurrentCell.RowIndex].Tag;

            if (userInfo == null)
                return;

            AddUser editUser = new AddUser();

            editUser._IsEditMode = true;
            editUser._UserId = userInfo.Id;
            editUser._Kind = (int)userInfo.Kind;

            if (editUser.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("设置成功.");

                _isUpdateUserList = true;
                RefreshUserList();
            }
        }

        private void buttonHistory_Click(object sender, EventArgs e)
        {
            if (UserView.CurrentCell == null)
            {
                MessageBox.Show("请选择会员帐号.");
                return;
            }

            UserInfo userInfo = (UserInfo)UserView.Rows[UserView.CurrentCell.RowIndex].Tag;

            if (userInfo == null)
                return;

            MngCash mngCash = new MngCash();
            mngCash._UserId = userInfo.Id;

            mngCash.ShowDialog();

        }

        private void buttonPrevilege_Click(object sender, EventArgs e)
        {
            if (UserView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择会员帐号");
                return;
            }

            string userId = UserView.SelectedRows[0].Cells["Id"].Value.ToString();

            MngPercent mngPercent = new MngPercent();
            mngPercent._UserId = userId;

            if (mngPercent.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("设置成功.");
                RefreshUserList();
            }
        }

        private void buttonDelUser_Click(object sender, EventArgs e)
        {
            if (UserView.CurrentCell == null)
            {
                MessageBox.Show("请选择会员帐号.");
                return;
            }

            UserInfo userInfo = (UserInfo)UserView.Rows[UserView.CurrentCell.RowIndex].Tag;

            if (userInfo == null)
                return;

            if (userInfo.Id == Users.ManagerInfo.Id)
            {
                MessageBox.Show("本人不能删除.");
                return;
            }

            string question = string.Format("真要删除 {0} 吗?", userInfo.Id);

            if (MessageBox.Show(question, "删除提问", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            if (Database.GetInstance().DelUser(userInfo.Id) == false)
            {
                ErrorInfo errorInfo = BaseInfo.GetError();
                MessageBox.Show(errorInfo.ErrorString);
                return;
            }

            string resultStr = string.Format("{0} 已被删除.", userInfo.Id);
            MessageBox.Show(resultStr);

            _isUpdateUserList = true;
            RefreshUserList();
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            if (_isUpdateUserList == false)
                return;

            if (buttonSave.Enabled == true)
            {
                DialogResult result = MessageBox.Show("你要不断变化？", "确认", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Cancel)
                    return;

                if (result == DialogResult.Yes)
                    SavePercent();

                _isSaveUserList = false;
                buttonSave.Enabled = false;
            }

            RefreshUserList();
        }

        private void checkManager_CheckedChanged(object sender, EventArgs e)
        {
            _isUpdateUserList = true;
        }

        private void checkRecommendOfficer_CheckedChanged(object sender, EventArgs e)
        {
            _isUpdateUserList = true;
        }

        private void checkRecommender_CheckedChanged(object sender, EventArgs e)
        {
            _isUpdateUserList = true;
        }

        private void checkServiceOfficer_CheckedChanged(object sender, EventArgs e)
        {
            _isUpdateUserList = true;
        }

        private void checkServiceman_CheckedChanged(object sender, EventArgs e)
        {
            _isUpdateUserList = true;
        }

        private void comboRecommender_TextChanged(object sender, EventArgs e)
        {
            _isUpdateUserList = true;
        }

        private void UserView_SelectionChanged(object sender, EventArgs e)
        {
            ShowCurUserInfo();
        }

        private void ShowCurUserInfo()
        {
            if (UserView.CurrentCell == null)
                return;

            UserInfo userInfo = (UserInfo)UserView.Rows[UserView.CurrentCell.RowIndex].Tag;

            if (userInfo == null)
                return;

            if( _isDisconnectWeb == false )
            {
                if (string.IsNullOrEmpty(userInfo.Icon) == false)
                {
                    try
                    {
                        Uri uri = new Uri("http://" + Users.WebResourceUrl + "/" + userInfo.Icon);

                        System.Net.WebClient webClient = new System.Net.WebClient();
                        webClient.Proxy = null;

                        pictureFace.Image = new System.Drawing.Bitmap(new System.IO.MemoryStream(webClient.DownloadData(uri.AbsoluteUri)));
                    }
                    catch (System.Net.WebException)
                    {
                        _isDisconnectWeb = true;
                        pictureFace.Image = null;
                    }
                    catch { }
                }
            }
        }

        private void buttonAddRecommendOfficer_Click(object sender, EventArgs e)
        {
            AddUser(UserKind.RecommendOfficer);
        }

        private void buttonAddServiceOfficer_Click(object sender, EventArgs e)
        {
            AddUser(UserKind.ServiceOfficer);
        }

        private void buttonAddRecommender_Click(object sender, EventArgs e)
        {
            AddUser(UserKind.Recommender);
        }

        private void buttonAddServiceman_Click(object sender, EventArgs e)
        {
            AddUser(UserKind.ServiceWoman);
        }

        private void AddUser( UserKind userKind )
        {
            AddUser addUser = new AddUser();
            addUser._Kind = (int)userKind;

            if (addUser.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("设置成功.");

                _isUpdateUserList = true;
                RefreshUserList();
            }
        }

        private void buttonEval_Click(object sender, EventArgs e)
        {

        }

        private void buttonChatPercent_Click(object sender, EventArgs e)
        {
            if (UserView.CurrentCell == null)
                return;

            UserInfo userInfo = (UserInfo)UserView.Rows[UserView.CurrentCell.RowIndex].Tag;

            if (userInfo == null)
                return;

            MngChatPercent chatPercent = MngChatPercent.GetInstance();
            chatPercent._searchId = userInfo.Id;
            
            Main._instance.ShowForm(chatPercent);

            _isUpdateUserList = true;
            RefreshUserList();
        }

        private void UserView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            UserInfo userInfo = (UserInfo)UserView.Rows[UserView.CurrentCell.RowIndex].Tag;

            try
            {
                if (UserView.CurrentCell.ColumnIndex == 5)
                {
                    int chatPercent = Convert.ToInt32(UserView.CurrentCell.Value);
                    int officePercent = Convert.ToInt32(UserView[6, UserView.CurrentCell.RowIndex].Value);

                    if (chatPercent > officePercent)
                        throw new Exception();

                    userInfo.ChatPercent = chatPercent;
                }
                else if (UserView.CurrentCell.ColumnIndex == 7)
                {
                    int gamePercent = Convert.ToInt32(UserView.CurrentCell.Value);
                    int officePercent = Convert.ToInt32(UserView[8, UserView.CurrentCell.RowIndex].Value);

                    if (gamePercent > officePercent)
                        throw new Exception();

                    userInfo.GamePercent = gamePercent;
                }

                UserView.CurrentCell.Style.ForeColor = Color.Blue;
            }
            catch
            {
                MessageBox.Show("please input percent correctly");
                UserView.CurrentCell.Value = userInfo.ChatPercent.ToString();
                return;
            }

            bool isAlready = false;

            foreach (UserInfo saveInfo in _saveList)
            {
                if (saveInfo == userInfo)
                {
                    isAlready = true;
                    break;
                }
            }

            if (isAlready == false)
                _saveList.Add(userInfo);


            _isSaveUserList = true;
            buttonSave.Enabled = true;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SavePercent();
        }

        private void SavePercent()
        {
            foreach (UserInfo saveInfo in _saveList)
            {
                if (Database.GetInstance().UpdateUser(saveInfo) == false)
                {
                    MessageBox.Show("Fail to save chat percent");
                    return;
                }
            }

            MessageBox.Show("saved successfully");

            _saveList.Clear();
            _isSaveUserList = false;
            buttonSave.Enabled = false;

            _isUpdateUserList = true;
            RefreshUserList();
        }

        private void MngManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (buttonSave.Enabled == true)
            {
                DialogResult result = MessageBox.Show("Do you save any changed?", "Confirm", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }

                if (result == DialogResult.Yes)
                    SavePercent();
            }
        }

        private void buttonAddBanker_Click(object sender, EventArgs e)
        {
            AddUser(UserKind.Banker);
        }

        private void checkBanker_CheckedChanged(object sender, EventArgs e)
        {
            _isUpdateUserList = true;
        }
    }
}
