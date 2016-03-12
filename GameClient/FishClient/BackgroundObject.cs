using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameControls;
using System.Drawing;
using Microsoft.DirectX.Direct3D;

namespace FishClient
{
    public class BackgroundObject : GameObject
    {
        enSceneType _SceneType;

        bool m_IsSceneing;

        public BackgroundObject(FishView parent) 
            : base(parent)
        {
        }

        public void SetSceneType( enSceneType SceneType)
        {
            if (SceneType == enSceneType.SceneTypeCount)
                return ;

            _SceneType = SceneType;

		    string szResource = string.Format( "bg_game_{0}",(int)_SceneType);
            LoadImage(szResource);
        }
    }
}
