using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User_control
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

        public Ocurrence()
        {

        }

        public List<Ocurrence> GetListOfOcurrences(
            int iMinNumberOfLicence, int iMaxNumberOfLicences, int iNumOfOcurrences, DateTime Date)
        {
            List<Ocurrence> lstOcurrences = new List<Ocurrence>();

            if (iMinNumberOfLicence == 0 || iMaxNumberOfLicences == 0 || iNumOfOcurrences == 0)
                return lstOcurrences;

            Random rRandomNumber = new Random();

            for (int i = 0; i < iNumOfOcurrences; i++)
            {
                int iLincense = rRandomNumber.Next(iMinNumberOfLicence, iMaxNumberOfLicences);
                int iDate = rRandomNumber.Next(1, 5);
                int iOperation = rRandomNumber.Next(1, 2);
                lstOcurrences.Add(new Ocurrence(iLincense, Date));

                if (iOperation == 1)
                    Date = Date.AddDays(iDate);

                else
                {
                    iDate = iDate * -1;
                    Date = Date.AddDays(iDate);
                }
            }

            return lstOcurrences;
        }

        public Period GetPeriodByOcurrence(List<Period> lstPeriods, Ocurrence ocurrence)
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

        public Dictionary<Period, Dictionary<int, List<Ocurrence>>> GetOcurrencesByPeriod(
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
