using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameControls.XLBE;
using ChatEngine;

namespace FishClient
{
    public class CMessageLayer : Layer, Button_Listener
    {
        private Font m_Font;

        private Button_Widget m_btnMessageHide;
        private Button_Widget m_btnMessageSend;

        private Sprite m_sprBackground;

        public CMessageLayer()
        {
            m_Font = Root.instance().font_manager().create_font_ttf("simsun", "fish\\simsun.ttc");
            m_Font.set_size(new Size(14, 15));

            m_btnMessageHide = new Button_Widget(Root.instance().imageset_manager().imageset("ui_load").image("btn_message_hide_image"), Root.instance().imageset_manager().imageset("ui_load").image("btn_message_hide_over"), Root.instance().imageset_manager().imageset("ui_load").image("btn_message_hide_down"), Root.instance().imageset_manager().imageset("ui_load").image("btn_message_hide_disable"));
            m_btnMessageHide.set_tag(10200);
            m_btnMessageHide.set_position(new Point(0, 0));
            m_btnMessageHide.set_content_size(new Size(29, 30));
            m_btnMessageHide.add_listener(this);

            m_btnMessageSend = new Button_Widget(Root.instance().imageset_manager().imageset("ui_load").image("btn_message_send_image"), Root.instance().imageset_manager().imageset("ui_load").image("btn_message_send_over"), Root.instance().imageset_manager().imageset("ui_load").image("btn_message_send_down"), Root.instance().imageset_manager().imageset("ui_load").image("btn_message_send_disable"));
            m_btnMessageSend.set_tag(10201);
            m_btnMessageSend.set_position(new Point(0, 0));
            m_btnMessageSend.set_content_size(new Size(29, 30));
            m_btnMessageSend.add_listener(this);

            m_sprBackground = new Sprite(Root.instance().imageset_manager().imageset("ui_load").image("message_back_image"));
            m_sprBackground.set_hot(new Point(0, 0));
            add_child(m_sprBackground);

        }
        public void Dispose()
        {
        }
        public override void draw()
        {
            base.draw();

            CGameScene pGameScene = (CGameScene)parent();
            CClientKernel pClientKernel = pGameScene.GetClientKernel();
            if (pClientKernel == null)
                return;

        }
        public void add_widget(Node node)
        {
            node.add_child(this);
            node.add_child(m_btnMessageHide);
            node.add_child(m_btnMessageSend);
        }
        public void button_press(int tag)
	{
		if (tag == 10200)
		{
			CGameScene pGameScene = (CGameScene)parent();
			//pakcj global::ShowWindow(pGameScene.GetClientKernel().m_hWndEdit,SW_HIDE);
			//pakcj global::SetWindowText(pGameScene.GetClientKernel().m_hWndEdit,"");
			Show(false);
		}
		else if (tag == 10201)
		{
			CGameScene pGameScene = (CGameScene)parent();
    
			CMD_C_Send_Message SendMessage = new CMD_C_Send_Message();
	//C++ TO C# CONVERTER TODO TASK: The memory management function 'memset' has no equivalent in C#:
			SendMessage.cbData = string.Empty;
    
			SendMessage.wChair = pGameScene.GetMeChairID();
			//pakcj SendMessage.nLen = global::GetWindowText(pGameScene.GetClientKernel().m_hWndEdit,(string)SendMessage.cbData,32);
    
			pGameScene.GetClientKernel().SendSocketData(FishDefine.SUB_C_SEND_MESSAGE, SendMessage);
		}
		else if (tag == 10202)
		{
			CGameScene pGameScene = (CGameScene)parent();
			Point pt = (pGameScene.GetMeChairID() == 1) ? pGameScene.m_layRoles[pGameScene.GetMeChairID()].position() - new Point(60,0) : pGameScene.m_layRoles[pGameScene.GetMeChairID()].position();
    
			pt += new Point(10,-60);
			resize(pt, new Size(396,286));
    
			//pakcj global::ShowWindow(pGameScene.GetClientKernel().m_hWndEdit,SW_SHOW);
			pt = position();
			//pakcj global::MoveWindow(pGameScene.GetClientKernel().m_hWndEdit,pt.x_ + 32,pt.y_ + 8,222,18,1);
			//pakcj global::SetFocus(pGameScene.GetClientKernel().m_hWndEdit);
			Show(true);
		}
	}
        public void resize(Point pt, Size size)
	{
		base.resize(pt, size);
    
		set_position(pt);
		m_btnMessageHide.set_position(new Point(pt.x_, pt.y_));
		m_btnMessageSend.set_position(new Point(pt.x_ + 257, pt.y_));
		CGameScene pGameScene = (CGameScene)parent();
		//pakcj global::MoveWindow(pGameScene.GetClientKernel().m_hWndEdit,pt.x_ + 32,pt.y_ + 8,222,18,1);
	}
        public void Show(bool bShow)
	{
		bool bVisible = visible();
		if (bVisible == bShow)
			return;
    
		set_visible(bShow);
		m_btnMessageHide.set_visible(bShow);
		m_btnMessageSend.set_visible(bShow);
		m_sprBackground.set_visible(bShow);
    
		CGameScene pGameScene = (CGameScene)parent();
		pGameScene.m_bIsShowMessageBox = bShow;
		if (bShow)
		{
			//pakcj global::ShowWindow(pGameScene.GetClientKernel().m_hWndEdit,SW_SHOW);
		}
		else
		{
			//pakcj global::ShowWindow(pGameScene.GetClientKernel().m_hWndEdit,SW_HIDE);
		}
	}

    }
}
