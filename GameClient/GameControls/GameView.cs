using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatEngine;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace GameControls
{
    public enum FrameWindowMessage
    {
        Frame_Close = 0
    }

    public delegate bool FrameWindowProc(FrameWindowMessage msgType, object data);


    public class TimerInfo
    {
        public Timer _Timer;
        public int _Id;
        public int Elapse;
        public int chairID = GameDefine.INVALID_CHAIR;
    }

    public class GameView : UserControl
    {
        protected ChatEngine.Client m_ClientSocket;
        public UserInfo m_UserInfo;
        public GameInfo m_GameInfo;
        public FrameWindowProc m_FrameWindowProc;
        public bool m_bLoaded = false;

        //位置变量
        protected int m_nAnimeStep;						//移动间距
        protected Point[] m_ptName = new Point[GameDefine.MAX_CHAIR];				//名字位置
        protected Point[] m_ptFace = new Point[GameDefine.MAX_CHAIR];				//头像位置
        protected Point[] m_ptTimer = new Point[GameDefine.MAX_CHAIR];				//时间位置
        protected Point[] m_ptReady = new Point[GameDefine.MAX_CHAIR];				//准备位置

        //用户变量
        protected const int m_nXBorder = 8;						//定时器高

        protected List<TimerInfo> _TimerList = new List<TimerInfo>();
        //int[]						m_wUserClock = new int[GameDefine.MAX_CHAIR];			//6603用户时钟
        protected TimerInfo _UserTimer;

        protected bool m_bAllowSound = true;
        protected bool m_bMuteStatuts = false;

        // added by usc at 2014/03/19
        public static bool _bGameSound = true;

        public bool m_bGameSound
        {
            get
            {
                return _bGameSound;
            }
            set
            {
                _bGameSound = value;

                if (_bGameSound)
                    PlayAllSound();
                else
                    StopAllSound();
            }
        }

        public GameView()
        {
            for (int i = 0; i < m_ptName.Length; i++)
            {
                m_ptName[i] = new Point();
                m_ptFace[i] = new Point();
                m_ptTimer[i] = new Point();
                m_ptReady[i] = new Point();
            }

            InitGameView();
        }

        public virtual void CloseView()
        {
            foreach (TimerInfo timerInfo in _TimerList)
            {
                timerInfo._Timer.Enabled = false;
            }

            _TimerList.Clear();

            foreach (GameControls.XLBE.Sound_Instance playingSound in _AudioPlayList)
            {
                playingSound.release();
            }

            _AudioPlayList.Clear();

            int k = 0;

            Type type = GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo fieldInfo = fields[i];

                switch (fieldInfo.FieldType.Name)
                {
                    case "Image":
                        {
                            Bitmap bitmap = (Bitmap)fieldInfo.GetValue(this);
                            
                            if( bitmap != null )
                                bitmap.Dispose();
                        }
                        break;

                    case "CPngImage":
                        {
                            CPngImage pngImage = (CPngImage)fieldInfo.GetValue(this);
                            pngImage.Dispose();
                        }
                        break;

                    case "CPngImage[]":
                        {
                            CPngImage[] pngImageList = (CPngImage[])fieldInfo.GetValue(this);

                            foreach (CPngImage pngImage in pngImageList)
                                pngImage.Dispose();
                        }
                        break;

                    case "PictureButton":
                        {
                            PictureButton pictureButton = (PictureButton)fieldInfo.GetValue(this);

                            if( pictureButton != null )
                                pictureButton.Dispose();
                        }
                        break;

                    case "CSkinButton":
                        {
                            CSkinButton skinButton = (CSkinButton)fieldInfo.GetValue(this);

                            if( skinButton != null )
                                skinButton.Dispose();
                        }
                        break;

                    case "CMyD3DTexture":
                        {
                            CMyD3DTexture d3dTexture = (CMyD3DTexture)fieldInfo.GetValue(this);
                            d3dTexture.Dispose();
                        }
                        break;

                    case "CMyD3DTexture[]":
                        {
                            CMyD3DTexture[] d3dTextureList = (CMyD3DTexture[])fieldInfo.GetValue(this);

                            foreach (CMyD3DTexture d3dTexture in d3dTextureList)
                                d3dTexture.Dispose();
                        }
                        break;


                }
            }

        }

        protected virtual void InitGameView()
        {
        }

        public void SetClientSocket(ChatEngine.Client clientSocket)
        {
            m_ClientSocket = clientSocket;
            m_ClientSocket.AttachHandler(NotifyOccured);
        }

        public void SetUserInfo(UserInfo userInfo)
        {
            m_UserInfo = userInfo;
        }

        public void SetGameInfo(GameInfo gameInfo)
        {
            m_GameInfo = gameInfo;
        }

        public void SetFrameWindowProc(FrameWindowProc frameWindowProc)
        {
            m_FrameWindowProc = frameWindowProc;
        }

        public UserControl GetHandle()
        {
            return this;
        }

        public void NotifyReceived(NotifyType notifyType, Socket socket, BaseInfo baseInfo)
        {
            NotifyOccured(notifyType, socket, baseInfo);
        }

        public virtual void NotifyOccured(NotifyType notifyType, Socket socket, BaseInfo baseInfo)
        {
        }

        List<GameControls.XLBE.Sound_Instance> _AudioPlayList = new List<GameControls.XLBE.Sound_Instance>();

        protected void PlayDirectSound(string fileName, bool bLooping)
        {
            if (!_bGameSound) return;

            try
            {
                int index = 0;

                while (index < _AudioPlayList.Count)
                {
                    GameControls.XLBE.Sound_Instance playingSound = _AudioPlayList[index];

                    playingSound.check_state();

                    if (playingSound.audio_.Playing == false)
                    {
                        playingSound.release();
                        _AudioPlayList.Remove(playingSound);
                        continue;
                    }

                    index++;
                }

                GameControls.XLBE.Sound_Instance soundInstance = XLBE.Sound_Instance.FromFile(fileName, false);

                soundInstance.play(bLooping, true);

                _AudioPlayList.Add(soundInstance);
            }
            catch { }
        }

        protected void StopDirectSound(string fileName)
        {
            int index = 0;

            while (index < _AudioPlayList.Count)
            {
                GameControls.XLBE.Sound_Instance playingSound = _AudioPlayList[index];

                if (playingSound.audio_.Playing == true && playingSound.isLooping_ == false)
                {
                    playingSound.stop();
                    playingSound.release();
                    _AudioPlayList.Remove(playingSound);
                    continue;
                }

                index++;
            }
        }

        // added by usc at 2014/03/19
        protected void PlayAllSound()
        {
            int index = 0;

            while (index < _AudioPlayList.Count)
            {
                GameControls.XLBE.Sound_Instance playingSound = _AudioPlayList[index];

                if (playingSound.audio_.Paused)
                {
                    playingSound.play(playingSound.isLooping_, true);
                    continue;
                }

                index++;
            }
        }

        // added by usc at 2014/03/19
        protected void StopAllSound()
        {
            int index = 0;

            while (index < _AudioPlayList.Count)
            {
                GameControls.XLBE.Sound_Instance playingSound = _AudioPlayList[index];

                //if (playingSound.audio_.Playing == true)
                //{
                if (playingSound.isLooping_)
                {
                    playingSound.stop();
                }
                else
                {
                    playingSound.stop();
                    playingSound.release();
                    _AudioPlayList.Remove(playingSound);
                    continue;
                }
                //}

                index++;
            }
        }

        protected void PlayGameSound(UnmanagedMemoryStream soundStream)
        {

            System.Threading.Thread thread = new System.Threading.Thread(PlayAudioFile);

            thread.Start((object)soundStream);
        }

        public void PlayAudioFile(object objStream)
        {
            UnmanagedMemoryStream soundStream = (UnmanagedMemoryStream)objStream;

            soundStream.Seek(0, SeekOrigin.Begin);
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(soundStream);
            player.Play();
        }

        protected void PlayingTimer_Tick(object sender, EventArgs e)
        {
            Timer timer = (Timer)sender;

            TimerInfo timerInfo = null;

            foreach (TimerInfo elementInfo in _TimerList)
            {
                if (elementInfo._Timer == timer)
                {
                    timerInfo = elementInfo;
                    break;
                }
            }

            if (timerInfo == null)
                return;

            OnTimer(timerInfo);
        }

        protected virtual void OnTimer(TimerInfo timerInfo)
        {
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            Timer timer = (Timer)sender;

            TimerInfo timerInfo = null;

            foreach (TimerInfo elementInfo in _TimerList)
            {
                if (elementInfo._Timer == timer)
                {
                    timerInfo = elementInfo;
                    break;
                }
            }

            if (timerInfo == null)
                return;

            timerInfo.Elapse--;

            OnGameTimer(timerInfo);

            if (timerInfo.Elapse == 0)
            {
                if (timerInfo == _UserTimer)
                    _UserTimer = null;


                KillTimer(timerInfo._Id);
            }
        }

        protected virtual void OnGameTimer(TimerInfo timerInfo)
        {
        }


        protected TimerInfo SetTimer(int timerId, int delay)
        {
            Timer timer = new System.Windows.Forms.Timer();

            timer.Interval = delay;
            timer.Tick += new System.EventHandler(this.PlayingTimer_Tick);
            timer.Enabled = true;

            TimerInfo timerInfo = new TimerInfo();
            timerInfo._Id = timerId;
            timerInfo._Timer = timer;

            _TimerList.Add(timerInfo);

            return timerInfo;
        }

        public TimerInfo SetGameTimer(int timerId, int elapse)
        {
            foreach (TimerInfo oldInfo in _TimerList)
            {
                if (oldInfo._Id == timerId)
                {
                    // added by usc at 2014/01/19
                    _TimerList.Remove(oldInfo);
                    break;
                    // removed by usc
                    //return oldInfo;
                }
            }

            Timer timer = new System.Windows.Forms.Timer();

            timer.Interval = 1000;
            timer.Tick += new System.EventHandler(this.GameTimer_Tick);
            timer.Enabled = true;

            TimerInfo timerInfo = new TimerInfo();
            timerInfo._Id = timerId;
            timerInfo._Timer = timer;
            timerInfo.Elapse = elapse;

            _TimerList.Add(timerInfo);

            return timerInfo;
        }

        protected void KillTimer(int timerId)
        {
            foreach (TimerInfo timerInfo in _TimerList)
            {
                if (timerInfo._Id == timerId)
                {
                    timerInfo._Timer.Enabled = false;
                    _TimerList.Remove(timerInfo);
                    break;
                }
            }

        }

        //绘画准备
        protected void DrawUserReady(Graphics g, int nXPos, int nYPos)
        {
            //加载资源
            Bitmap ImageUserReady = Properties.Resources.USER_READY;
            //ImageUserReady.LoadImage(GetModuleHandle(GAME_FRAME_DLL_NAME),TEXT("USER_READY"));

            //绘画准备
            Size SizeImage = new Size(ImageUserReady.Width, ImageUserReady.Height);
            g.DrawImage(ImageUserReady, nXPos - SizeImage.Width / 2, nYPos - SizeImage.Height / 2);

            return;
        }

        //绘画时间
        protected void DrawUserTimer(Graphics g, int nXPos, int nYPos, int wTime, int wTimerArea)
        {
            //加载资源
            Bitmap ImageTimeBack = Properties.Resources.TIME_BACK;
            Bitmap ImageTimeNumber = Properties.Resources.TIME_NUMBER;
            //ImageTimeBack.LoadImage(GetModuleHandle(GAME_FRAME_DLL_NAME),TEXT("TIME_BACK"));
            //ImageTimeNumber.LoadImage(GetModuleHandle(GAME_FRAME_DLL_NAME),TEXT("TIME_NUMBER"));

            //获取属性
            int nNumberHeight = ImageTimeNumber.Height;
            int nNumberWidth = ImageTimeNumber.Width / 10;

            //计算数目
            int lNumberCount = 0;
            int wNumberTemp = wTime;
            do
            {
                lNumberCount++;
                wNumberTemp /= 10;
            } while (wNumberTemp > 0L);

            //位置定义
            int nYDrawPos = nYPos - nNumberHeight / 2 + 1;
            int nXDrawPos = nXPos + (lNumberCount * nNumberWidth) / 2 - nNumberWidth;

            //绘画背景
            Size SizeTimeBack = new Size(ImageTimeBack.Width, ImageTimeBack.Height);
            //g.DrawImage( ImageTimeBack, nXPos-SizeTimeBack.Width/2, nYPos-SizeTimeBack.Height/2);
            DrawImage(g, ImageTimeBack, nXPos - SizeTimeBack.Width / 2, nYPos - SizeTimeBack.Height / 2, ImageTimeBack.Width, ImageTimeBack.Height, 0, 0);

            //绘画号码
            for (int i = 0; i < lNumberCount; i++)
            {
                //绘画号码
                int wCellNumber = wTime % 10;
                DrawImage(g, ImageTimeNumber, nXDrawPos, nYDrawPos, nNumberWidth, nNumberHeight, wCellNumber * nNumberWidth, 0);

                //设置变量
                wTime /= 10;
                nXDrawPos -= nNumberWidth;
            };

            return;
        }

        protected void DrawImage(Graphics g, Image image, int dstX, int dstY, int width, int height, int srcX, int srcY)
        {
            GameGraphics.DrawImage(g, image, dstX, dstY, width, height, srcX, srcY);
        }


        protected void DrawAlphaImage(Graphics g, Image image, int dstX, int dstY, int width, int height, int srcX, int srcY, Color tranColor)
        {
            GameGraphics.DrawAlphaImage(g, image, dstX, dstY, width, height, srcX, srcY, tranColor);
        }

        public virtual void NotifyMessage(int message, object wParam, object lParam)
        {

        }

        //6603获取时间
        public void SetUserClock(int wChairID, int timerId, int delay)
        {
            KillUserClock(wChairID);

            _UserTimer = SetGameTimer(timerId, delay);

        }

        public void KillUserClock(int wChairID)
        {
            if (_UserTimer == null)
                return;

            KillTimer(_UserTimer._Id);

            _UserTimer = null;
        }

        public int GetUserClock(int wChairID)
        {
            //6603效验参数
            //ASSERT(wChairID<MAX_CHAIR);

            if (_UserTimer == null)
                return 0;

            return _UserTimer.Elapse;
            //6603获取时间
        }

        public bool IsGameCheatUser()
        {
            return false;
        }

        public void ZeroMemory(Array array)
        {
            Array.Clear(array, 0, array.Length);
        }

        public void memcpy(int[] dest, int[] src)
        {
            int length = src.Length;

            if (length > dest.Length)
                length = dest.Length;

            Array.Copy(src, dest, length);
        }

        public int CountArray(Array array)
        {
            return array.Length;
        }

        public int _tcscmp(string src, string desc)
        {
            if (src == desc)
                return 0;

            return 1;
        }

        protected virtual void CartoonMovie()
        {
        }

        public bool CallFrameWindowProc(FrameWindowMessage msgType, object data)
        {
            if (m_FrameWindowProc == null)
                return false;

            return m_FrameWindowProc(msgType, data);
        }

        // added by usc at 2014/03/11
        public Bitmap GetFaceImage(string fileName)
        {
            Bitmap resultBmp = null;

            if (fileName == string.Empty)
                return null;

            try
            {
                resultBmp = new Bitmap(fileName);
            }
            catch { }

            return resultBmp;
        }

    }


}
