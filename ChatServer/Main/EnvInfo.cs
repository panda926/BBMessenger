using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ChatServer
{
    public class EnvInfo : BaseInfo
    {
        public int NewCount = 0;
        public int LoginBonusPoint = 0;
        public int ChargeBonusPoint = 0;

        public string ImageUploadPath = string.Empty;
        public string ChargeSiteUrl = string.Empty;

        public int ChargeGivePercent = 0;
        public int EveryDayPoint = 0;
        public int CashRate = 1;
    }
}
