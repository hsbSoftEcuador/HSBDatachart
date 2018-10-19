using System;
using System.Collections.Generic;

namespace hsbDataChart
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

        public List<Period> GetPeriodsByMonths(Period timePeriod)
        {
            List<Period> lstPeriods = new List<Period>();

            if (timePeriod == null || timePeriod.StartDate == null || timePeriod.EndDate == null)
                return lstPeriods;

            if (timePeriod.StartDate.Year == 0 || timePeriod.EndDate.Year == 0) return lstPeriods;

            if (timePeriod.EndDate <= timePeriod.StartDate) return lstPeriods;

            int iDaynum = new DateTime(timePeriod.StartDate.Year, timePeriod.StartDate.AddMonths(1).Month, 1).AddDays(-1).Day;

            iDaynum = iDaynum == 0 ?
                new DateTime(timePeriod.StartDate.Year, timePeriod.StartDate.AddMonths(1).Month, 1).AddDays(-2).Day :
                iDaynum;

            DateTime dtPeriodStart = new DateTime(timePeriod.StartDate.Year, timePeriod.StartDate.Month, timePeriod.StartDate.Day);
            DateTime dtPeriodEnd = new DateTime(timePeriod.StartDate.Year, timePeriod.StartDate.Month, iDaynum);

            do
            {
                if (timePeriod.EndDate < dtPeriodEnd)
                {
                    if (timePeriod.EndDate.Hour < dtPeriodEnd.Hour || timePeriod.EndDate.Minute < dtPeriodEnd.Minute ||
                        timePeriod.EndDate.Second < dtPeriodEnd.Second)
                    {
                        dtPeriodEnd = timePeriod.EndDate;
                    }
                    dtPeriodEnd = timePeriod.EndDate;
                }

                lstPeriods.Add(new Period(dtPeriodStart, dtPeriodEnd));
                dtPeriodStart = dtPeriodEnd.AddDays(1);

                if (dtPeriodEnd == timePeriod.EndDate)
                {
                    break;
                }

                dtPeriodEnd = dtPeriodStart.AddMonths(1).AddDays(-1);
            } while (dtPeriodEnd <= timePeriod.EndDate);

            if (dtPeriodEnd > timePeriod.EndDate)
            {
                dtPeriodEnd = new DateTime(timePeriod.EndDate.Year, timePeriod.EndDate.Month, timePeriod.EndDate.Day);
                lstPeriods.Add(new Period(dtPeriodStart, dtPeriodEnd));
            }

            return lstPeriods;
        }

        public List<Period> GetPeriodsByWeeks(Period timePeriod)
        {
            List<Period> lstPeriods = new List<Period>();

            if (timePeriod == null || timePeriod.StartDate == null || timePeriod.EndDate == null)
                return lstPeriods;

            int iDayOfWeek = (int)timePeriod.StartDate.DayOfWeek;

            if (timePeriod.EndDate < timePeriod.StartDate) return lstPeriods;

            DateTime dtPeriodStart = new DateTime();
            DateTime dtPeriodEnd = new DateTime();

            if (iDayOfWeek == 0)
            {
                dtPeriodStart = timePeriod.StartDate;
                dtPeriodEnd = timePeriod.StartDate;
            }
            else if (iDayOfWeek > 0)
            {
                dtPeriodStart = timePeriod.StartDate;
                dtPeriodEnd = timePeriod.StartDate.AddDays(7 - iDayOfWeek);
            }

            do
            {
                if (dtPeriodStart.Year == timePeriod.EndDate.Year && dtPeriodStart.Month == timePeriod.EndDate.Month &&
                    dtPeriodStart.Day == timePeriod.EndDate.Day)
                {
                    dtPeriodStart = timePeriod.EndDate;
                    dtPeriodEnd = timePeriod.EndDate;
                    lstPeriods.Add(new Period(dtPeriodStart, dtPeriodEnd));
                    break;
                }

                if (timePeriod.EndDate < dtPeriodEnd)
                {
                    if (timePeriod.EndDate.Hour < dtPeriodEnd.Hour || timePeriod.EndDate.Minute < dtPeriodEnd.Minute ||
                        timePeriod.EndDate.Second < dtPeriodEnd.Second)
                    {
                        dtPeriodEnd = timePeriod.EndDate;
                    }
                    dtPeriodEnd = timePeriod.EndDate;
                }

                lstPeriods.Add(new Period(dtPeriodStart, dtPeriodEnd));

                dtPeriodStart = dtPeriodEnd.AddDays(1);
                if (dtPeriodEnd == timePeriod.EndDate) break;

                dtPeriodEnd = dtPeriodStart.AddDays(6);
            } while (dtPeriodEnd <= timePeriod.EndDate);

            if (dtPeriodEnd > timePeriod.EndDate)
            {
                dtPeriodEnd = new DateTime(timePeriod.EndDate.Year, timePeriod.EndDate.Month, timePeriod.EndDate.Day);
                lstPeriods.Add(new Period(dtPeriodStart, dtPeriodEnd));
            }

            return lstPeriods;
        }
    }
}
