using System;
using System.Collections.Generic;
using System.Linq;

namespace hsbDataChart
{
    public class Ocurrence
    {
        private int m_iId = 0;
        private DateTime m_dtDate = Convert.ToDateTime(null);

        public int Id
        {
            get { return m_iId; }
            set { m_iId = value; }
        }

        public DateTime Date
        {
            get { return m_dtDate; }
            set { m_dtDate = value; }
        }

        public Ocurrence(int iId, DateTime dtDate)
        {
            m_iId = iId;
            m_dtDate = dtDate;
        }

        static public Period GetPeriodByOcurrence(List<Period> lstPeriods, Ocurrence ocurrence)
        {
            if (lstPeriods == null || lstPeriods.Count() == 0 || ocurrence == null)
                return null;

            IEnumerable<Period> colPeriods = from period in lstPeriods
                                             where period.StartDate < ocurrence.Date && period.EndDate > ocurrence.Date
                                             select period;

            if (colPeriods == null || colPeriods.Count() == 0)
                return null;

            return colPeriods.First();
        }

        static public Dictionary<Period, Dictionary<int, List<Ocurrence>>> GetOcurrencesByPeriod(
            List<Period> lstPeriods, List<Ocurrence> lstOcurrences)
        {
            Dictionary<Period, Dictionary<int, List<Ocurrence>>> dcOcurrencesByPeriod =
                new Dictionary<Period, Dictionary<int, List<Ocurrence>>>();

            if (lstPeriods.Count == 0 || lstPeriods == null)
                return dcOcurrencesByPeriod;

            foreach (Period period in lstPeriods)
            {
                if (period == null) continue;

                dcOcurrencesByPeriod.Add(period, new Dictionary<int, List<Ocurrence>>());
            }

            foreach (Ocurrence ocurrence in lstOcurrences)
            {
                if (ocurrence == null) continue;

                Period period = GetPeriodByOcurrence(lstPeriods, ocurrence);

                if (period == null) continue;

                if (!dcOcurrencesByPeriod[period].ContainsKey(ocurrence.Id))
                    dcOcurrencesByPeriod[period].Add(ocurrence.Id, new List<Ocurrence>());

                dcOcurrencesByPeriod[period][ocurrence.Id].Add(ocurrence);
            }

            return dcOcurrencesByPeriod;
        }
    }
}
