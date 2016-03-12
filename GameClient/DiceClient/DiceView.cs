using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GameControls;
using ChatEngine;
using System.Net.Sockets;

namespace DiceClient
{
    public enum DiceGameStatus
    {
        GS_FREE = 0,
        GS_BET,
        GS_BET_END,
        GS_DICING,
        GS_END,

        GS_EMPTY = 100
    }

    public partial class DiceView : GameView
    {
        public DiceView()
        {
            InitializeComponent();
        }
    }
}
