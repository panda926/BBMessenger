using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameControls
{
    public class CLapseCount
    {
        //变量定义
        int m_dwQueryTickCount;					//查询时间

        //变量定义
        static int m_dwCurrentTickCount;				//当前时间

        //流逝配置
        public void Initialization()
        {
            m_dwQueryTickCount = System.Environment.TickCount;
        }

        //流逝判断
        public int GetLapseCount(int dwPulseCount)
        {
            if (System.Environment.TickCount - m_dwQueryTickCount >= dwPulseCount)
            {
                m_dwQueryTickCount = System.Environment.TickCount;
                return 1;
            }

            return 0;
        }

        //更新时间
        public static void PerformLapseCount()
        {
        }

    }
}
