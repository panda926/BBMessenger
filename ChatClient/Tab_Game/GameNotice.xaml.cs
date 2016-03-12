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
    /// Interaction logic for GameNotice.xaml
    /// </summary>
    public partial class GameNotice : Window
    {
        public GameNotice()
        {
            InitializeComponent();
        }

        public void SetGameNotice(GameInfo gameInfo)
        {
            string strMsg = string.Empty;

            if (gameInfo.Source == "Dice")
            {
                strMsg =  "1.骰宝游戏为玩家对赌游戏，系统每盘只扣押注金额的3%手续费\r\n";
                strMsg += "2.玩家每盘押注金额最高封顶为2万金币r\n";
                strMsg += "3.公平起见，大小 ，单双 每盘押注金额相差不得高于5万金币，高出后系统将停止继续加注";
            }
            else if (gameInfo.Source == "Horse")
            {
                strMsg =  "1.赛马游戏为玩家对赌游戏，系统每盘只扣押注金额的3%手续费\r\n";
                strMsg += "2.玩家每盘押注金额封顶为2万金币\r\n";
                strMsg += "3.公平起见，系统最高下注金额与最低下注金额不得超过5万金币  超出系统将自动停止下注\r\n";
                strMsg += "4.第一名番4倍，第二名番2倍";
            }

            FlowDocument flowDoc = new FlowDocument();
            Paragraph para = new Paragraph();
            Run runMsg = new Run(strMsg);
            runMsg.FontSize = 12;
            para.LineHeight = 5;
            para.Inlines.Add(new Bold(runMsg));
            flowDoc.Blocks.Add(para);

            rtxtNotice.Document = flowDoc;
        }
    }
}
