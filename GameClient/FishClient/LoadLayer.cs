using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameControls.XLBE;

namespace FishClient
{
    public class CLoadLayer : Layer
    {
        private Sprite m_sprBg;
        private Font m_Font;

        private string m_strDateLoading;
        private string m_strHelp;
        private string[] m_strHelps;

        public CLoadLayer()
        {
            this.m_sprBg = null;
            this.m_Font = null;
            this.m_strHelps = new string[4];

            m_sprBg = new Sprite(Root.instance().imageset_manager().imageset("bg_load").image("bg"));
            m_sprBg.set_hot( new Point(0, 0));
            add_child(m_sprBg);

            m_Font = Root.instance().font_manager().create_font_ttf("simsun", "fish\\simsun.ttc");
            m_Font.set_size( new Size(15, 16));

            m_strDateLoading = "数据载入中.";
            m_strHelps[0] = "欢迎进入猎鱼高手。";
            m_strHelps[1] = "上下键可换炮，左右键可改变炮方向，空格可发射哦。";
            m_strHelps[2] = "脉冲光炮任务和海底炸弹需要触发引爆。";
            m_strHelps[3] = "游戏内帮助只有从大厅进入才会出现。";

            Random random = new Random();
            m_strHelp = m_strHelps[random.Next() % 4];
        }

        public override void update(double dt)
        {
            base.update(dt);

            if (update_count_ % 75 == 0)
            {
                m_strDateLoading = "数据载入中.";
            }
            else if (update_count_ % 75 == 25)
            {
                m_strDateLoading = "数据载入中..";
            }
            else if (update_count_ % 75 == 50)
            {
                m_strDateLoading = "数据载入中...";
            }
        }

        public override void draw()
        {
            base.draw();

            //m_Font.set_align(1);
            //m_Font.draw_string( new Point(640, 510), m_strDateLoading, new Color());

            //m_Font.set_align(1);
            //m_Font.draw_string( new Point(640, 710), m_strHelp, new Color(176, 222, 238));
        }

    }
}
