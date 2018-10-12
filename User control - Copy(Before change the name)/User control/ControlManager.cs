using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User_control
{
    public class ControlManager
    {
        static private ControlManager m_Instance = null;

        static public ControlManager Instance
        {
            get { return m_Instance; }
        }

        private ControlManager() { }

        static ControlManager()
        {
            m_Instance = new ControlManager();
        }

        public List<Period> GetPeriodsByMonth(DateTime dtDateStart, DateTime dtEndDate)
        {
            List<Period> lstPeriods = new List<Period>();

            if (dtDateStart.Year == 0 || dtEndDate.Year == 0) return lstPeriods;

            if (dtEndDate <= dtDateStart) return lstPeriods;

            int iDaynum = new DateTime(dtDateStart.Year, dtDateStart.AddMonths(1).Month, 1).AddDays(-1).Day;
            iDaynum = iDaynum == 0 ?
                new DateTime(dtDateStart.Year, dtDateStart.AddMonths(1).Month, 1).AddDays(-2).Day :
                iDaynum;

            DateTime dtPeriodStart = new DateTime(dtDateStart.Year, dtDateStart.Month, dtDateStart.Day);
            DateTime dtPeriodEnd = new DateTime(dtDateStart.Year, dtDateStart.Month, iDaynum);

            do
            {
                if (dtEndDate < dtPeriodEnd)
                {
                    if (dtEndDate.Hour < dtPeriodEnd.Hour || dtEndDate.Minute < dtPeriodEnd.Minute ||
                        dtEndDate.Second < dtPeriodEnd.Second)
                    {
                        dtPeriodEnd = dtEndDate;
                    }
                    dtPeriodEnd = dtEndDate;
                }

                lstPeriods.Add(new Period(dtPeriodStart, dtPeriodEnd));
                dtPeriodStart = dtPeriodEnd.AddDays(1);

                if (dtPeriodEnd == dtEndDate)
                {
                    break;
                }

                dtPeriodEnd = dtPeriodStart.AddMonths(1).AddDays(-1);
            } while (dtPeriodEnd <= dtEndDate);

            if (dtPeriodEnd > dtEndDate)
            {
                dtPeriodEnd = new DateTime(dtEndDate.Year, dtEndDate.Month, dtEndDate.Day);
                lstPeriods.Add(new Period(dtPeriodStart, dtPeriodEnd));
            }

            return lstPeriods;
        }

        public List<Period> GetPeriodsByWeeks(DateTime dtStartDate, DateTime dtEndDate)
        {
            int iDayOfWeek = (int)dtStartDate.DayOfWeek;

            List<Period> lstPeriods = new List<Period>();

            if (dtEndDate < dtStartDate) return lstPeriods;

            DateTime dtPeriodStart = new DateTime();
            DateTime dtPeriodEnd = new DateTime();

            if (iDayOfWeek == 0)
            {
                dtPeriodStart = dtStartDate;
                dtPeriodEnd = dtStartDate;
            }
            else if (iDayOfWeek > 0)
            {
                dtPeriodStart = dtStartDate;
                dtPeriodEnd = dtStartDate.AddDays(7 - iDayOfWeek);
            }

            do
            {
                if (dtPeriodStart.Year == dtEndDate.Year && dtPeriodStart.Month == dtEndDate.Month &&
                    dtPeriodStart.Day == dtEndDate.Day)
                {
                    dtPeriodStart = dtEndDate;
                    dtPeriodEnd = dtEndDate;
                    lstPeriods.Add(new Period(dtPeriodStart, dtPeriodEnd));
                    break;
                }

                if (dtEndDate < dtPeriodEnd)
                {
                    if (dtEndDate.Hour < dtPeriodEnd.Hour || dtEndDate.Minute < dtPeriodEnd.Minute ||
                        dtEndDate.Second < dtPeriodEnd.Second)
                    {
                        dtPeriodEnd = dtEndDate;
                    }
                    dtPeriodEnd = dtEndDate;
                }

                lstPeriods.Add(new Period(dtPeriodStart, dtPeriodEnd));

                dtPeriodStart = dtPeriodEnd.AddDays(1);
                if (dtPeriodEnd == dtEndDate) break;

                dtPeriodEnd = dtPeriodStart.AddDays(6);
            } while (dtPeriodEnd <= dtEndDate);

            if (dtPeriodEnd > dtEndDate)
            {
                dtPeriodEnd = new DateTime(dtEndDate.Year, dtEndDate.Month, dtEndDate.Day);
                lstPeriods.Add(new Period(dtPeriodStart, dtPeriodEnd));
            }

            return lstPeriods;
        }
    }
}
