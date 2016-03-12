using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameControls;
using System.Drawing;

namespace FishClient
{
    public abstract class CAction
    {
        bool _IsDone;
        
        CLapseCount _LapseCount = new CLapseCount();
        int _LapseTime = 0;

        public void Start( int lapseTime )
        {
            _LapseCount.Initialization();
            _LapseTime = lapseTime;
        }

        public void Stop()
        {
            _IsDone = true;
        }

        public bool IsDone()
        {
            return _IsDone;
        }

        public bool Run(GameObject gameObject)
        {
            if( IsDone())
                return false;

            if( _LapseCount.GetLapseCount( _LapseTime ) <= 0 )
                return false;

            return Update(gameObject);
        }

        protected abstract bool Update( GameObject gameObject);

    }

    public class CAnimation : CAction
    {
        int _FrameIndex;
        List<int> _Frames = new List<int>();

        public CAnimation( int frameCount )
        {
            for( int i = 0; i < frameCount; i++ )
                _Frames.Add(i);

            for( int i = frameCount; i > 0; i-- )
                _Frames.Add(i-1);

            Start( 60 );
        }

        protected override bool Update(GameObject gameObject)
        {
            gameObject._ImagePos.X = _Frames[_FrameIndex] * gameObject._ImageSize.Width;

            _FrameIndex = (_FrameIndex + 1) % _Frames.Count;

            return true;
        }
    }

    public class CMove : CAction
    {
        Point _Start;
        Point _End;
        
        int _StepIndex;
        int _StepCount;

        public CMove(int delay, Point start, Point end)
        {
            _Start = start;
            _End = end;
            _StepCount = delay / 60;

            Start(60);
        }

        protected override bool Update(GameObject gameObject)
        {
            gameObject._Position.X = _Start.X + (_End.X - _Start.X) * _StepIndex / _StepCount;
            gameObject._Position.Y = _Start.Y + (_End.Y - _Start.Y) * _StepIndex / _StepCount;

            if (gameObject._Position.X == _End.X && gameObject._Position.Y == _End.Y)
                Stop();
            else
                _StepIndex++;

            return true;
        }
    }
}
