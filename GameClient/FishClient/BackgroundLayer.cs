using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameControls.XLBE;
using ChatEngine;

namespace FishClient
{
    public class CBackgroundLayer : Layer
    {
		public int m_IsSceneing;
		public int m_cbSceneSound;
		private int m_cbSceneSoundOld;

		private FishDefine.enSceneType m_SceneType;

		private Sprite m_sprTideWater;

		private Image m_imgBackground0;
		private Image m_imgBackground1;

		private Sound_Instance[] m_BackSound = Arrays.InitializeWithDefaultInstances<Sound_Instance>(10);

	    public CBackgroundLayer()
	    {
		    this.m_SceneType = FishDefine.enSceneType.SceneTypeCount;
		    this.m_imgBackground0 = null;
		    this.m_imgBackground1 = null;
		    m_sprTideWater = new Sprite();
	      //    m_sprTideWater = new Sprite(Root::instance()->imageset_manager()->imageset("ui_game")->image("tide_water"));
    
		    m_sprTideWater.set_hot( new Point(0,0));
		    m_sprTideWater.set_position( new Point(1272 - 20, 0));
		    m_sprTideWater.set_visible(false);
		    add_child(m_sprTideWater);
    
    
		    for (int i = 80; i < 90; i++)
		    {
			    try
			    {
				    m_BackSound[i - 80] = Root.instance().music_manager().sound_instance(i);
			    }
			    catch
			    {
				    m_BackSound[i - 80] = null;
			    }
    
		    }
    
		    m_IsSceneing = 0;
		    m_cbSceneSound = 0;
		    m_cbSceneSoundOld = 0;
	    }

        public FishDefine.enSceneType GetSceneType() { return m_SceneType; }

	    public void SetSceneType(FishDefine.enSceneType SceneType)
	    {
		    if (SceneType == FishDefine.enSceneType.SceneTypeCount)
			    return;
    
		    //if (SceneType == m_SceneType)
		    //    return;
		    //MessageBox(0,"","",0);
    
		    m_SceneType = SceneType;
    
		    ostringstream ostr = new ostringstream();
		    ostr = ostr + "bg_game_" + (int)SceneType;
    
		    m_imgBackground0 = Root.instance().imageset_manager().imageset(ostr.str()).image("bg");
		    m_imgBackground1 = null;
    
		    m_sprTideWater.set_position( new Point(1272 - 20, 0));
		    m_sprTideWater.set_visible(false);
    
		    if (m_BackSound[m_cbSceneSound] != null)
		    {
			    m_BackSound[m_cbSceneSound].play(true, false);
		    }

		    m_cbSceneSoundOld = m_cbSceneSound;
    
	    }

	    public void ChangeSceneType(FishDefine.enSceneType SceneType)
	    {
		    if (SceneType == m_SceneType)
			    return;
    
		    if (m_BackSound[m_cbSceneSoundOld] != null)
		    {
			    m_BackSound[m_cbSceneSoundOld].stop();
		    }
    
		    m_cbSceneSoundOld = m_cbSceneSound;
    
		    try
		    {
			    Sound_Instance pSound = Root.instance().sound_manager().sound_instance(4);
			    pSound.play(false, true);
		    }
		    catch
		    {
		    }
    
		    m_SceneType = SceneType;
		    m_IsSceneing = 1;
    
		    ostringstream ostr = new ostringstream();
    
		    int nSide = 1;
    
		    Point ptBegin = (nSide == 0) ? new Point(-263, 0) : new Point(1280 + 263, 0);
		    Point ptEnd = (nSide == 0) ? new Point(1272 - 20, 0) : new Point(-283, 0);
    
		    ostr.str("");
		    ostr = ostr +  "tide_water_" + nSide;
    
		    m_sprTideWater.set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
		    m_sprTideWater.set_position(ptBegin);
		    m_sprTideWater.set_visible(true);
    
		    ostr.str("");
		    ostr = ostr + "bg_game_" + (int)SceneType;
		    m_imgBackground1 = Root.instance().imageset_manager().imageset(ostr.str()).image("bg");
    
		    GameControls.XLBE.Action actWater = new Action_Sequence(new Action_Delay(4), new Action_Move_To(4.4135, ptEnd), new Action_Func(ChangeSceneEnd), null);
		    m_sprTideWater.run_action(actWater);
	    }


	    public bool ChangeSceneEnd(Node node, int tag)
	    {
		    SetSceneType(m_SceneType);
		    m_IsSceneing = 0;
    
		    return true;
	    }
	    public override void update(double dt)
	    {
		    base.update(dt);
	    }
	    public override void draw()
	    {
		    Point pt = m_sprTideWater.position();
    
		    if (pt.x_ < -20)
		    {
			    pt.x_ = -20;
		    }
		    //Point  pt = (((m_SceneType)%2)==1) ? m_sprTideWater->position() : m_sprTideWater->position() + Point(243,0);
		    //if(((m_SceneType)%2)==1)
		    //{
		    //	if (pt.x_ < -20)
		    //	{
		    //		pt.x_ = -20;
		    //	}
		    //}
		    //else
		    //{
		    //	if (pt.x_ > 1280)
		    //	{
		    //		pt.x_ = 1280;
		    //	}
		    //}
    
		    if (m_imgBackground0 != null)
		    {
			    m_imgBackground0.draw(position());
		    }
    
		    if (m_imgBackground1 != null )
		    {
			    m_imgBackground1.draw(pt + position() + new Point(20,0), new Size(1272 - pt.x_ + 20, m_imgBackground1.height()), pt + new Point(20,0), new Size(1272 - pt.x_ + 20, m_imgBackground1.height()));
			    //if(((m_SceneType)%2)==1)	m_imgBackground1->draw(pt+position()+Point(20,0), Size(1272-pt.x_+20, m_imgBackground1->height()), pt+Point(20,0), Size(1272-pt.x_+20, m_imgBackground1->height()));
			    //else						m_imgBackground1->draw(position(), Size(pt.x_, m_imgBackground1->height()), Point(1280,0)-pt, Size(pt.x_, m_imgBackground1->height()));
		    }
    
		    base.draw();
	    }

    }
}
