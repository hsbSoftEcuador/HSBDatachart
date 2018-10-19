using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using hsbDataChart;

namespace hsbDataChartTesting
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<Ocurrence> lstOcurrences = GetListOfOcurrences(10, 1000, 80, new DateTime(2018, 06, 22));

            hsbDataChartCtrl.Ocurrences = lstOcurrences;
            hsbDataChartCtrl.PeriodTypeAssociated = PeriodType.Week;
            hsbDataChartCtrl.RangeAssociated = new Range(100, 3500, 123);

            hsbDataChartCtrl.Draw();
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
    }
}
