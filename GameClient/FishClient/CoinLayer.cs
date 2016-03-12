using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameControls.XLBE;

namespace FishClient
{
    public class CCoinLayer : Layer
    {
        public bool CoinSilverEnd(Node node, int tag)
        {
            remove_child(node);
            node = null;

            return true;
        }

        public bool CoinGoldEnd(Node node, int tag)
        {
            remove_child(node);
            node = null;

            return true;
        }

        public bool CoinValueEnd(Node node, int tag)
        {
            remove_child(node);
            node = null;

            return true;
        }
    }
}
