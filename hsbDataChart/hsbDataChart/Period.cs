using System;

namespace hsbDataChart
{
    public class Period
    {
        private DateTime m_dtStartDate = Convert.ToDateTime(null);
        private DateTime m_dtEndDate = Convert.ToDateTime(null);

        public DateTime StartDate
        {
            get { return m_dtStartDate; }
            set { m_dtStartDate = value; }
        }

        public DateTime EndDate
        {
            get { return m_dtEndDate; }
            set
            {
                if (m_dtEndDate < m_dtStartDate)
                {
                    m_dtEndDate = m_dtStartDate;
                }
            }
        }

        public override string ToString()
        {
            return StartDate + ", " + EndDate;
        }

        public Period(DateTime dtStartDate, DateTime dtEndDate)
        {
            m_dtStartDate = dtStartDate;
            m_dtEndDate = dtEndDate;
        }
    }
}
