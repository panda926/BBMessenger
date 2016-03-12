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
    public partial class AddUser : Form
    {
        public bool _IsEditMode = false;
        public bool _IsAuto = false;
        
        public string _UserId = "";
        public int _Kind = (int)UserKind.Buyer;

        public AddUser()
        {
            InitializeComponent();
        }

        // 2013-12-30: GreenRose
        // recommender controls Init
        public void InitRecommenderControl()
        {
            string configPath = System.IO.Path.GetFullPath("Config.ini");
            IniFileEdit configInfo = new IniFileEdit(configPath);
            this.lblDefaultUrl.Text = configInfo.GetIniValue("DefaultURL", "DEFURL");

            if (_Kind == (int)UserKind.Recommender)
            {
                this.txtRecommenderUrl.Enabled = true;
            }
            else
                this.txtRecommenderUrl.Enabled = false;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            string Password = textPassword.Text.Trim();

            if (_Kind != (int)UserKind.Buyer)
            {
                if (_IsEditMode == false)
                {
                    string recommenderId = textRecommender.Text.Trim();

                    if (recommenderId.Length == 0)
                    {
                        MessageBox.Show("请输入帐号.");
                        return;
                    }

                    UserInfo userInfo = Database.GetInstance().FindUser(recommenderId);

                    if (userInfo != null)
                    {
                        MessageBox.Show("已存在的帐号.");
                        return;
                    }
                }
            }

            if (_IsAuto == false)
            {
                if (Password.Length == 0)
                {
                    MessageBox.Show("请输入密码.");
                    return;
                }

                if (Password != textAgain.Text.Trim())
                {
                    MessageBox.Show("密码不一致.");
                    return;
                }

                //if (textYear.Text.Trim().Length == 0 || textMonth.Text.Trim().Length == 0 || textDay.Text.Trim().Length == 0)
                //{
                //    MessageBox.Show("생년월일을 입력하십시오.");
                //    return;
                //}
            }

            string nameStr = textName.Text.Trim();

            if (nameStr.Length == 0)
            {
                MessageBox.Show("请输入名字.");
                return;
            }


            UserInfo newInfo = new UserInfo();

            if (_IsEditMode == false)
            {
                newInfo.Recommender = comboRecommender.Text;
                newInfo.Kind = _Kind;

                if (_Kind == (int)UserKind.Buyer)
                {
                    newInfo.Id = comboBuyers.Text;

                    if (_IsAuto == true)
                        newInfo.Auto = comboAutoLevel.SelectedIndex + 1;
                }
                else
                {
                    newInfo.Id = textRecommender.Text.Trim();

                    if (_Kind == (int)UserKind.RecommendOfficer || _Kind == (int)UserKind.ServiceOfficer)
                    {
                        newInfo.Recommender = "";
                    }
                    else if (_Kind == (int)UserKind.ServiceWoman)
                    {
                        newInfo.ChatPercent = 10;
                    }
                    else if (_Kind == (int)UserKind.Recommender)
                    {
                        newInfo.MaxBuyers = 100;
                    }
                }
            }
            else
            {
                newInfo = Database.GetInstance().FindUser(_UserId);

                if (_Kind == (int)UserKind.Recommender || _Kind == (int)UserKind.ServiceWoman)
                {
                    newInfo.Recommender = comboRecommender.Text;
                }

                if (_IsAuto == true)
                    newInfo.Auto = comboAutoLevel.SelectedIndex + 1;
            }

            newInfo.Password = Password;
            newInfo.Nickname = nameStr;

            try
            {
                newInfo.Year = Convert.ToInt32(textYear.Text.Trim());
                newInfo.Month = Convert.ToInt32(textMonth.Text.Trim());
                newInfo.Day = Convert.ToInt32(textDay.Text.Trim());
            }
            catch { }

            newInfo.Address = textAddress.Text;
            newInfo.Icon = comboIcon.Text;
            newInfo.Sign = textSign.Text;
            newInfo.RegistTime = DateTime.Now;

            bool ret = true;

            // 2013-12-30
            // GreenRose Modify begin
            if (_Kind == (int)UserKind.Recommender)
            {
                if (string.IsNullOrEmpty(this.txtRecommenderUrl.Text))
                {
                    MessageBox.Show("Please enter your URL");
                    return;
                }

                UserInfo userInfo = Database.GetInstance().FindUserByUrl(lblDefaultUrl.Text + this.txtRecommenderUrl.Text);
                if (_IsEditMode == false)
                {
                    if (userInfo != null)
                    {
                        MessageBox.Show("Your entered URL is existing now. Please enter another URL.");
                        return;
                    }
                }
                else
                {
                    if (userInfo != null && userInfo.Id != newInfo.Id)
                    {
                        MessageBox.Show("Your entered URL is existing now. Please enter another URL.");
                        return;
                    }
                }

                newInfo.strUrl = lblDefaultUrl.Text + this.txtRecommenderUrl.Text;
            }            
            // GreenRose Modify end

            if( _IsEditMode == false )
            {
                ret = Database.GetInstance().AddUser(newInfo);
            }
            else
            {
                ret = Database.GetInstance().UpdateUser(newInfo);
            }

            if( ret == false )
            {
                ErrorInfo errorInfo = BaseInfo.GetError();
                MessageBox.Show(errorInfo.ErrorString);
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            // 2013-12-30: GreenRose
            InitRecommenderControl();

            if (_Kind != (int)UserKind.Buyer)
            {
                labelBuyerId.Visible = false;
                comboBuyers.Visible = false;

                labelRecommenderId.Left = labelBuyerId.Left;
                labelRecommenderId.Top = labelBuyerId.Top;
                labelRecommenderId.Visible = true;

                textRecommender.Left = comboBuyers.Left;
                textRecommender.Top = comboBuyers.Top;
                textRecommender.Visible = true;

                if (_Kind == (int)UserKind.RecommendOfficer || _Kind == (int)UserKind.ServiceOfficer)
                {
                    labelRecommender.Text = "种类";
                }
                else
                {
                    labelRecommender.Text = "管理人";
                }
            }

            if (_IsAuto == true)
            {
                labelPassword.Visible = false;
                textPassword.Visible = false;

                labelAgain.Visible = false;
                textAgain.Visible = false;

                labelAuto.Left = labelPassword.Left;
                labelAuto.Top = labelPassword.Top;
                labelAuto.Visible = true;

                comboAutoLevel.Left = textPassword.Left;
                comboAutoLevel.Top = textPassword.Top;
                comboAutoLevel.Visible = true;

                for (int i = 1; i <= 3; i++)
                    comboAutoLevel.Items.Add(i);

                comboAutoLevel.SelectedIndex = 0;
            }

            if (_IsEditMode == true)
            {
                this.Text = "会员修正";
                buttonOk.Text = "修正";

                UserInfo userInfo = Database.GetInstance().FindUser(_UserId);

                if (userInfo != null)
                {
                    if (_Kind == (int)UserKind.RecommendOfficer || _Kind == (int)UserKind.ServiceOfficer)
                    {
                        comboRecommender.Items.Add(userInfo.KindString);
                        comboRecommender.SelectedIndex = 0;

                        textRecommender.Text = userInfo.Id;
                        textRecommender.Enabled = false;
                    }
                    else if (_Kind == (int)UserKind.ServiceWoman)
                    {
                        List<UserInfo> userList = Database.GetInstance().GetManager(UserKind.ServiceOfficer);

                        if (userList != null)
                        {
                            foreach (UserInfo officerInfo in userList)
                            {
                                comboRecommender.Items.Add(officerInfo.Id);

                                if (officerInfo.Id == userInfo.Recommender)
                                    comboRecommender.SelectedIndex = comboRecommender.Items.Count - 1;
                            }
                        }

                        textRecommender.Text = userInfo.Id;
                        textRecommender.Enabled = false;
                    }
                    else if (_Kind == (int)UserKind.Recommender )
                    {
                        List<UserInfo> userList = Database.GetInstance().GetManager(UserKind.RecommendOfficer);

                        if (userList != null)
                        {
                            foreach (UserInfo officerInfo in userList)
                            {
                                comboRecommender.Items.Add(officerInfo.Id);

                                if (officerInfo.Id == userInfo.Recommender)
                                    comboRecommender.SelectedIndex = comboRecommender.Items.Count - 1;
                            }
                        }

                        textRecommender.Text = userInfo.Id;
                        textRecommender.Enabled = false;
                    }
                    else if (_Kind == (int)UserKind.Buyer)
                    {
                        comboRecommender.Items.Add(userInfo.Recommender);
                        comboRecommender.SelectedIndex = 0;

                        comboBuyers.Items.Add(userInfo.Id);
                        comboBuyers.SelectedIndex = 0;

                        if (_IsAuto == true)
                            comboAutoLevel.Text = userInfo.Auto.ToString();
                    }
                    else
                    {
                        textRecommender.Text = userInfo.Id;
                        textRecommender.Enabled = false;
                    }

                    textPassword.Text = userInfo.Password;
                    textName.Text = userInfo.Nickname;

                    textYear.Text = userInfo.Year.ToString();
                    textMonth.Text = userInfo.Month.ToString();
                    textDay.Text = userInfo.Day.ToString();
                    textAddress.Text = userInfo.Address;

                    textSign.Text = userInfo.Sign;

                    comboIcon.Items.Add(userInfo.Icon);
                    comboIcon.SelectedIndex = 0;
                }
            }
            else
            {
                List<IconInfo> iconList = Database.GetInstance().GetAllFaces("default");

                for (int i = 0; i < iconList.Count; i++)
                {
                    comboIcon.Items.Add(iconList[i].Icon);
                }
                comboIcon.SelectedIndex = 0;

                if (_Kind == (int)UserKind.RecommendOfficer)
                {
                    comboRecommender.Items.Add("广告总代理");
                    comboRecommender.SelectedIndex = 0;
                }
                else if (_Kind == (int)UserKind.ServiceOfficer)
                {
                    comboRecommender.Items.Add("宝贝总代理");
                    comboRecommender.SelectedIndex = 0;
                }
                else if (_Kind == (int)UserKind.Recommender)
                {
                    List<UserInfo> userList = Database.GetInstance().GetManager(UserKind.RecommendOfficer);

                    if (userList != null)
                    {
                        foreach (UserInfo userInfo in userList)
                            comboRecommender.Items.Add(userInfo.Id);

                        if (comboRecommender.Items.Count > 0)
                            comboRecommender.SelectedIndex = 0;
                    }
                }
                else if (_Kind == (int)UserKind.ServiceWoman)
                {
                    List<UserInfo> userList = Database.GetInstance().GetManager(UserKind.ServiceOfficer);

                    if (userList != null)
                    {
                        foreach (UserInfo userInfo in userList)
                            comboRecommender.Items.Add(userInfo.Id);

                        if (comboRecommender.Items.Count > 0)
                            comboRecommender.SelectedIndex = 0;
                    }
                }
                else if (_Kind == (int)UserKind.Buyer)
                {
                    List<UserInfo> recommenders = Database.GetInstance().GetRecommenders();

                    for (int i = 0; i < recommenders.Count; i++)
                        comboRecommender.Items.Add(recommenders[i].Id);

                    comboRecommender.SelectedIndex = 0;
                    comboRecommender_SelectedIndexChanged(null, null);
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void comboRecommender_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_IsEditMode == true)
                return;

            if (_Kind != (int)UserKind.Buyer)
                return;

            UserInfo userInfo = Database.GetInstance().FindUser(comboRecommender.Text);
            
            if (userInfo == null)
                return;

            string newId = Database.GetInstance().GetNewIDList( userInfo );

            if (newId == null)
                return;

            comboBuyers.Items.Clear();
            comboBuyers.Items.Add(newId);

            if (comboBuyers.Items.Count > 0)
                comboBuyers.SelectedIndex = 0;
        }

    }
}
