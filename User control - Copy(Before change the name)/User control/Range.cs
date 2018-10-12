using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User_control
{
    public class Range
    {
        private int m_iMin = 0;
        private int m_iMax = 0;
        private int m_iInterval = 0;

        public int Min
        {
            get { return m_iMin; }
            set
            {
                if ((m_iMin > m_iMax))
                {
                    m_iMax = m_iMin;
                    m_iInterval = 0;
                }
            }
        }
        public int Max
        {
            get { return m_iMax; }
            set { m_iMax = value; }
        }
        public int Inteval
        {
            get { return m_iInterval; }
            set { m_iInterval = value; }
        }

        public Range(int iMin, int iMax, int iInterval)
        {
            m_iMin = iMin;
            m_iMax = iMax;
            m_iInterval = iInterval;
        }

        public Range()
        {

        }

        public int NumberOfInternations()
        {
            int iNumberOfInternations = (m_iMax - m_iMin) / m_iInterval;
            return iNumberOfInternations;
        }

        public int ShowNumberOfRegistrations()
        {
            int Count = 0;
            for (int i = m_iMin; i <= m_iMax; i = i + m_iInterval)
            {
                Count = Count + m_iInterval;
            }

            return Count;
        }
    }
}
