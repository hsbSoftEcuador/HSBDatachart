using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace User_control
{
    /// <summary>
    /// Interaction logic for hsbDataChart.xaml
    /// </summary>
    public partial class hsbDataChart : UserControl
    {

        private double m_dHorizontalMarginForCanvas = 50;
        private double m_dVerticalMarginForCanvas = 50;

        private int m_iDefaultWidthForLabels = 60;
        private int m_iDefaultHeightForLabels = 30;
        private double m_dLineRangeLength = 10;
        private int m_iDistanceBetweenNumberAndLineForRange = 5;
        private double m_dSpaceBetweenRangeLines = 50;

        private double m_MinNumberOfBars = 5;
        private double m_dDefaultWidthForBars = 40;
        private double m_dDefaultWidthForTextBlock = 65;
        private double m_dDefaultHeightForTextBlock = 45;

        private Range m_range;
        private Period m_period;
        private PeriodType periodtime;
        private Dictionary<int, List<Ocurrence>> m_dcOcurrencesByLicenseId = new Dictionary<int, List<Ocurrence>>();

        public Range Range
        {
            get => m_range;
            set => m_range = value;
        }

        public Period PeriodTime
        {
            get => m_period;
            set => m_period = value;
        }

        public PeriodType PeriodTimeAssociated
        {
            get { return periodtime; }
            set { periodtime = value; }
        }

        public Dictionary<int, List<Ocurrence>> DcOcurrencesByLicenseId
        {
            get => m_dcOcurrencesByLicenseId;
            set => m_dcOcurrencesByLicenseId = value;
        }

        public double DHorizontalMarginForCanvas
        {
            get => m_dHorizontalMarginForCanvas;
            set => m_dHorizontalMarginForCanvas = value;
        }

        public double DVerticalMarginForCanvas
        {
            get => m_dVerticalMarginForCanvas;
            set => m_dVerticalMarginForCanvas = value;
        }
        public int IDefaultWidthForLabels
        {
            get => m_iDefaultWidthForLabels;
            set => m_iDefaultWidthForLabels = value;
        }
        public int IDefaultHeightForLabels
        {
            get => m_iDefaultHeightForLabels;
            set => m_iDefaultHeightForLabels = value;
        }
        public double DLineRangeLength
        {
            get => m_dLineRangeLength;
            set => m_dLineRangeLength = value;
        }
        public int IDistanceBetweenNumberAndLineForRange
        {
            get => m_iDistanceBetweenNumberAndLineForRange;
            set => m_iDistanceBetweenNumberAndLineForRange = value;
        }
        public double DSpaceBetweenRangeLines
        {
            get => m_dSpaceBetweenRangeLines;
            set => m_dSpaceBetweenRangeLines = value;
        }
        public double MinNumberOfBars
        {
            get => m_MinNumberOfBars;
            set => m_MinNumberOfBars = value;
        }
        public double DDefaultWidthForBars
        {
            get => m_dDefaultWidthForBars;
            set => m_dDefaultWidthForBars = value;
        }
        public double DDefaultWidthForTextBlock
        {
            get => m_dDefaultWidthForTextBlock;
            set => m_dDefaultWidthForTextBlock = value;
        }
        public double DDefaultHeightForTextBlock
        {
            get => m_dDefaultHeightForTextBlock;
            set => m_dDefaultHeightForTextBlock = value;
        }

        public hsbDataChart()
        {
            InitializeComponent();
            cbPeriod.ItemsSource = Enum.GetValues(typeof(PeriodType));
        }

        public List<int> GetRangeNumbers(int iMin, int iMax, int iInterval)
        {
            List<int> lstRangeNumbers = new List<int>();

            for (int i = iMin; i <= iMax; i += iInterval)
            {
                lstRangeNumbers.Add(iMin);
                iMin = iMin + iInterval;

                if (iMin >= iMax)
                {
                    lstRangeNumbers.Add(iMax);
                    break;
                }
            }

            return lstRangeNumbers;
        }

        private List<int> GetAllLicenseIds(Dictionary<Period, Dictionary<int, List<Ocurrence>>> dcPeriodsAndOcurrences)
        {
            List<int> lstAllLincenses = new List<int>();

            if (dcPeriodsAndOcurrences.Keys == null || dcPeriodsAndOcurrences.Count == 0) return lstAllLincenses;

            foreach (Period period in dcPeriodsAndOcurrences.Keys)
            {
                foreach (int iLicenseId in dcPeriodsAndOcurrences[period].Keys)
                {
                    if (dcPeriodsAndOcurrences[period].ContainsKey(iLicenseId) == false) continue;
                    lstAllLincenses.Add(iLicenseId);
                }
            }

            return lstAllLincenses;
        }

        private void DrawRangeLines(int iMax, int iMin, int iInterval)
        {
            //Propiedades del Canvas
            cvWhiteBoard.Background = new SolidColorBrush(Colors.LightGray);

            //Llamada de la lista para obtener sus elementos
            List<int> lstRangeNumbers = GetRangeNumbers(iMin, iMax, iInterval);

            if (lstRangeNumbers == null || lstRangeNumbers.Count == 0) return;

            double dHeight = DVerticalMarginForCanvas * 2 + DSpaceBetweenRangeLines * (lstRangeNumbers.Count - 1);

            cvWhiteBoard.Height = dHeight;

            double dXPosition = DHorizontalMarginForCanvas, dInitialYPosition = DVerticalMarginForCanvas;

            double dCurrentYPosition = dInitialYPosition;

            //Dibujar Líneas
            for (int i = 0; i < lstRangeNumbers.Count; i++)
            {
                //Horizontales
                Line lnHorizontalForRange = new Line
                {
                    X1 = -DLineRangeLength,
                    Stroke = System.Windows.Media.Brushes.Navy,
                    StrokeThickness = 2
                };

                Canvas.SetBottom(lnHorizontalForRange, dCurrentYPosition);
                Canvas.SetLeft(lnHorizontalForRange, dXPosition);

                cvWhiteBoard.Children.Add(lnHorizontalForRange);

                dCurrentYPosition += DSpaceBetweenRangeLines;
            }
            //Vertical
            Line lnVerticalForRange = new Line
            {
                Y1 = dInitialYPosition * (lstRangeNumbers.Count - 1),
                Stroke = System.Windows.Media.Brushes.Navy,
                StrokeThickness = 2
            };

            Canvas.SetBottom(lnVerticalForRange, dInitialYPosition);
            Canvas.SetLeft(lnVerticalForRange, dXPosition);

            cvWhiteBoard.Children.Add(lnVerticalForRange);

            /////////////////////////////////////////////////////////////
            dCurrentYPosition = dInitialYPosition;

            foreach (int iValuesFromList in lstRangeNumbers)
            {
                Label lblNumber = new Label
                {
                    Width = IDefaultWidthForLabels,
                    Height = IDefaultHeightForLabels,
                    Content = iValuesFromList,
                    FlowDirection = FlowDirection.RightToLeft
                };

                Canvas.SetBottom(lblNumber, dCurrentYPosition - (lblNumber.Height / 2));
                Canvas.SetLeft(
                    lblNumber, DHorizontalMarginForCanvas - lblNumber.Width - IDistanceBetweenNumberAndLineForRange);

                cvWhiteBoard.Children.Add(lblNumber);

                dCurrentYPosition += DSpaceBetweenRangeLines;
            }
        }

        private void DrawPeriodLines(Dictionary<Period, Dictionary<int, List<Ocurrence>>> dcPeriodsAndOcurrences)
        {
            if (dcPeriodsAndOcurrences.Keys == null || dcPeriodsAndOcurrences.Count == 0) return;
            double dDefaultPeriodLineLength = DDefaultWidthForBars * MinNumberOfBars;
            //Propiedades del Canvas
            double dSpaceBetweenPeriodLines = DDefaultWidthForBars * MinNumberOfBars;
            double dInitialXPosition = DHorizontalMarginForCanvas, dYPosition = DVerticalMarginForCanvas,
                dCurrentXPosition = dInitialXPosition, dInitialPositionForTextBlock = dInitialXPosition * 2,
                dSpaceBetweenPeriods = DDefaultWidthForBars;
            double dWidth = DHorizontalMarginForCanvas * 2 + dSpaceBetweenPeriodLines * (
                dcPeriodsAndOcurrences.Count);

            cvWhiteBoard.Width = dWidth * 2;
            //Horizontal
            Line lnDefaultLineForPeriod = new Line
            {
                X1 = (dInitialXPosition * (dcPeriodsAndOcurrences.Count - 1)) * dcPeriodsAndOcurrences.Count,
                Stroke = System.Windows.Media.Brushes.Green,
                StrokeThickness = 2
            };

            Canvas.SetBottom(lnDefaultLineForPeriod, dYPosition);
            Canvas.SetLeft(lnDefaultLineForPeriod, DHorizontalMarginForCanvas);

            cvWhiteBoard.Children.Add(lnDefaultLineForPeriod);

            foreach (Period period in dcPeriodsAndOcurrences.Keys)
            {
                Dictionary<int, List<Ocurrence>> dcInternalDictionary = dcPeriodsAndOcurrences[period];

                foreach (int iLicenseId in dcInternalDictionary.Keys)
                {
                    Line lnHorizontalForPeriod = new Line
                    {
                        X1 = DDefaultWidthForBars * 2,
                        Stroke = System.Windows.Media.Brushes.Black,
                        StrokeThickness = 2
                    };

                    Canvas.SetBottom(lnHorizontalForPeriod, dYPosition);
                    Canvas.SetLeft(lnHorizontalForPeriod, dCurrentXPosition);

                    cvWhiteBoard.Children.Add(lnHorizontalForPeriod);

                    dCurrentXPosition += DDefaultWidthForBars;
                }

                Line lnHorizontalForSpace = new Line
                {
                    X1 = DDefaultWidthForBars,
                    Stroke = System.Windows.Media.Brushes.Black,
                    StrokeThickness = 2
                };

                Canvas.SetBottom(lnHorizontalForSpace, dYPosition);
                Canvas.SetLeft(lnHorizontalForSpace, dCurrentXPosition);

                cvWhiteBoard.Children.Add(lnHorizontalForSpace);
            }

            //TextBlock
            dCurrentXPosition = dInitialPositionForTextBlock;

            foreach (var vPeriods in dcPeriodsAndOcurrences.Keys)
            {
                TextBlock txtPeriod = new TextBlock
                {
                    Width = DDefaultWidthForTextBlock,
                    Height = DDefaultHeightForTextBlock,
                    TextWrapping = TextWrapping.Wrap,
                    TextAlignment = TextAlignment.Center,
                    Text = "[" + vPeriods.StartDate.ToShortDateString() + "]" + "  " + "-" +
                    "[" + vPeriods.EndDate.ToShortDateString() + "]"
                };

                Canvas.SetBottom(txtPeriod, 0);
                Canvas.SetLeft(txtPeriod, dCurrentXPosition);

                dCurrentXPosition += dSpaceBetweenPeriodLines + 100;

                cvWhiteBoard.Children.Add(txtPeriod);
            }
        }

        private void DrawBars(
            Dictionary<Period, Dictionary<int, List<Ocurrence>>> dcPeriodsAndOcurrences,
            Dictionary<int, LicenseDisplayer> dicLicenseDisplayers)
        {
            int num = 0;

            if (dcPeriodsAndOcurrences.Keys == null || dcPeriodsAndOcurrences.Count == 0) return;

            double dInitialXPosition = DHorizontalMarginForCanvas, dInitialYPosition = DVerticalMarginForCanvas,
                dYPositionForLicenseLabel = dInitialYPosition, dXPositionForLicenseLabel = dInitialXPosition;
            double dCurrentXPosition = dInitialXPosition;

            //if (lstLicenses.Count == 0) return;

            foreach (Period period in dcPeriodsAndOcurrences.Keys)
            {
                if (period == null) continue;

                Dictionary<int, List<Ocurrence>> dcOcurrencesByLicenseId = dcPeriodsAndOcurrences[period];

                if (dcOcurrencesByLicenseId == null || dcOcurrencesByLicenseId.Count == 0)
                    continue;

                foreach (int iLicenseId in dcOcurrencesByLicenseId.Keys)
                {
                    if (!dcOcurrencesByLicenseId.ContainsKey(iLicenseId)) continue;

                    SolidColorBrush solidColorBrush = new SolidColorBrush(Colors.Black);

                    if (dicLicenseDisplayers != null && dicLicenseDisplayers.Count > 0 &&
                        dicLicenseDisplayers.ContainsKey(iLicenseId))
                    {
                        LicenseDisplayer licenseDisplayer = dicLicenseDisplayers[iLicenseId];
                        if (licenseDisplayer.ColorAssociated != null)
                            solidColorBrush = new SolidColorBrush(licenseDisplayer.ColorAssociated);
                    }

                    Rectangle rBarForEachLicense = new Rectangle
                    {
                        Width = DDefaultWidthForBars,
                        Stroke = System.Windows.Media.Brushes.Black,
                        StrokeThickness = 1,
                        Height = iLicenseId * dcOcurrencesByLicenseId[iLicenseId].Count,
                        Fill = solidColorBrush
                    };

                    Label lblLicenseId = new Label
                    {
                        Height = IDefaultHeightForLabels,
                        Width = IDefaultWidthForLabels,
                        Content = iLicenseId,
                        Foreground = System.Windows.Media.Brushes.DarkRed
                    };

                    Canvas.SetZIndex(lblLicenseId, 2);
                    Canvas.SetBottom(lblLicenseId, dInitialYPosition);
                    Canvas.SetLeft(lblLicenseId, dCurrentXPosition + 5);
                    cvWhiteBoard.Children.Add(lblLicenseId);

                    Canvas.SetZIndex(rBarForEachLicense, 1);
                    Canvas.SetBottom(rBarForEachLicense, dInitialYPosition);
                    Canvas.SetLeft(rBarForEachLicense, dCurrentXPosition);
                    cvWhiteBoard.Children.Add(rBarForEachLicense);

                    dCurrentXPosition += DDefaultWidthForBars;

                    num += 1;
                    if (num == dcOcurrencesByLicenseId.Keys.Count)
                    {
                        dCurrentXPosition += DDefaultWidthForBars;
                        break;
                    }
                }
                num = 0;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int iSelectedPeriod = (int)(cbPeriod.SelectedItem);

            Ocurrence ocurrence = new Ocurrence();

            //List<Period> lstPeriods = ControlManager.Instance.GetPeriodsByWeeks(
            //    dpDateStart.SelectedDate.Value, dpDateEnd.SelectedDate.Value);

            List<Period> lstPeriods = ControlManager.Instance.GetPeriodsByMonth(
                dpDateStart.SelectedDate.Value, dpDateEnd.SelectedDate.Value);

            List<Ocurrence> lstOcurrences = ocurrence.GetListOfOcurrences(Convert.ToInt32(txtMin.Text),
                Convert.ToInt32(txtMax.Text), Convert.ToInt32(txtInterval.Text), dpDateStart.SelectedDate.Value);

            Dictionary<Period, Dictionary<int, List<Ocurrence>>> dcPeriodsAndOcurrences =
                ocurrence.GetOcurrencesByPeriod(lstPeriods, lstOcurrences);

            List<int> lstLicenses = GetAllLicenseIds(dcPeriodsAndOcurrences);

            if (lstLicenses != null && lstLicenses.Count > 0)
                LicensesDisplayer.Instance.SetColorsForLicenses(lstLicenses);

            DrawRangeLines(
                Convert.ToInt32(txtMax.Text), Convert.ToInt32(txtMin.Text), Convert.ToInt32(txtInterval.Text));

            DrawPeriodLines(dcPeriodsAndOcurrences);

            DrawBars(dcPeriodsAndOcurrences, LicensesDisplayer.Instance.LicensesDisplayers);

            btnStart.IsEnabled = false;
        }
    }
}
