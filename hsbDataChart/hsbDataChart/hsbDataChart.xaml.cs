using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace hsbDataChart
{
    public partial class hsbDataChart : UserControl
    {
        public hsbDataChart()
        {
            InitializeComponent();
        }

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

        private bool m_bShowBarsThatExceedTheMaximunRange = false;
        private Range m_range = new Range(1, 1000, 10);
        private Period m_timePeriod = new Period(new DateTime(2018, 01, 01), new DateTime(2018, 12, 31));
        private PeriodType m_periodtype;
        private BarsOrder m_BarsOrder;
        private List<Ocurrence> m_lstOcurrences = new List<Ocurrence>();

        public bool ShowBarsThatExceedTheMaximunRange
        {
            get => m_bShowBarsThatExceedTheMaximunRange;
            set => m_bShowBarsThatExceedTheMaximunRange = value;
        }

        public Range RangeAssociated
        {
            get => m_range;
            set => m_range = value;
        }

        public Period TimePeriod
        {
            get => m_timePeriod;
            set => m_timePeriod = value;
        }

        public PeriodType PeriodTypeAssociated
        {
            get { return m_periodtype; }
            set { m_periodtype = value; }
        }

        public BarsOrder BarsOrder
        {
            get => m_BarsOrder;
            set => m_BarsOrder = value;
        }

        public List<Ocurrence> Ocurrences
        {
            get => m_lstOcurrences;
            set => m_lstOcurrences = value;
        }

        public double HorizontalMarginForCanvas
        {
            get => m_dHorizontalMarginForCanvas;
            set => m_dHorizontalMarginForCanvas = value;
        }

        public double VerticalMarginForCanvas
        {
            get => m_dVerticalMarginForCanvas;
            set => m_dVerticalMarginForCanvas = value;
        }

        public int DefaultWidthForLabels
        {
            get => m_iDefaultWidthForLabels;
            set => m_iDefaultWidthForLabels = value;
        }

        public int DefaultHeightForLabels
        {
            get => m_iDefaultHeightForLabels;
            set => m_iDefaultHeightForLabels = value;
        }

        public double LineRangeLength
        {
            get => m_dLineRangeLength;
            set => m_dLineRangeLength = value;
        }

        public int DistanceBetweenNumberAndLineForRange
        {
            get => m_iDistanceBetweenNumberAndLineForRange;
            set => m_iDistanceBetweenNumberAndLineForRange = value;
        }

        public double SpaceBetweenRangeLines
        {
            get => m_dSpaceBetweenRangeLines;
            set => m_dSpaceBetweenRangeLines = value;
        }

        public double MinNumberOfBars
        {
            get => m_MinNumberOfBars;
            set => m_MinNumberOfBars = value;
        }

        public double DefaultWidthForBars
        {
            get => m_dDefaultWidthForBars;
            set => m_dDefaultWidthForBars = value;
        }

        public double DefaultWidthForTextBlock
        {
            get => m_dDefaultWidthForTextBlock;
            set => m_dDefaultWidthForTextBlock = value;
        }

        public double DefaultHeightForTextBlock
        {
            get => m_dDefaultHeightForTextBlock;
            set => m_dDefaultHeightForTextBlock = value;
        }

        private static List<int> GetRangeNumbers(Range range)
        {
            List<int> lstRangeNumbers = new List<int>();

            if (range == null) return lstRangeNumbers;

            int iValue = range.Min;

            for (int i = iValue; i <= range.Max; i += range.Inteval)
            {
                lstRangeNumbers.Add(iValue);
                iValue = iValue + range.Inteval;

                if (iValue >= range.Max)
                {
                    lstRangeNumbers.Add(range.Max);
                    break;
                }
            }

            return lstRangeNumbers;
        } 

        private static List<int> GetAllLicenseIds(Dictionary<Period, Dictionary<int, List<Ocurrence>>> dcOcurrencesByPeriods)
        {
            List<int> lstAllLincenses = new List<int>();

            if (dcOcurrencesByPeriods.Keys == null || dcOcurrencesByPeriods.Count == 0) return lstAllLincenses;

            foreach (Period period in dcOcurrencesByPeriods.Keys)
            {
                foreach (int iLicenseId in dcOcurrencesByPeriods[period].Keys)
                {
                    if (dcOcurrencesByPeriods[period].ContainsKey(iLicenseId) == false) continue;
                    lstAllLincenses.Add(iLicenseId);
                }
            }

            return lstAllLincenses;
        }

        private bool DrawRangeLines(Range range)
        {
            if (range == null) return false;

            //Propiedades del Canvas
            cvWhiteBoard.Background = new SolidColorBrush(Colors.LightGray);

            //Llamada de la lista para obtener sus elementos
            List<int> lstRangeNumbers = GetRangeNumbers(range);

            if (lstRangeNumbers == null || lstRangeNumbers.Count == 0) return false;

            double dHeight = VerticalMarginForCanvas * 2 + SpaceBetweenRangeLines * (lstRangeNumbers.Count - 1);

            cvWhiteBoard.Height = dHeight;

            double dXPosition = HorizontalMarginForCanvas, dInitialYPosition = VerticalMarginForCanvas;

            double dCurrentYPosition = dInitialYPosition;

            //Dibujar Líneas
            for (int i = 0; i < lstRangeNumbers.Count; i++)
            {
                //Horizontales
                Line lnHorizontalForRange = new Line
                {
                    X1 = -LineRangeLength,
                    Stroke = System.Windows.Media.Brushes.Navy,
                    StrokeThickness = 2
                };

                Canvas.SetBottom(lnHorizontalForRange, dCurrentYPosition);
                Canvas.SetLeft(lnHorizontalForRange, dXPosition);

                cvWhiteBoard.Children.Add(lnHorizontalForRange);

                dCurrentYPosition += SpaceBetweenRangeLines;
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
            
            dCurrentYPosition = dInitialYPosition;

            foreach (int iValuesFromList in lstRangeNumbers)
            {
                Label lblNumber = new Label
                {
                    Width = DefaultWidthForLabels,
                    Height = DefaultHeightForLabels,
                    Content = iValuesFromList,
                    FlowDirection = FlowDirection.RightToLeft
                };

                Canvas.SetBottom(lblNumber, dCurrentYPosition - (lblNumber.Height / 2));

                Canvas.SetLeft(
                    lblNumber, HorizontalMarginForCanvas - lblNumber.Width - DistanceBetweenNumberAndLineForRange);

                cvWhiteBoard.Children.Add(lblNumber);

                dCurrentYPosition += SpaceBetweenRangeLines;
            }

            return true;
        }

        private bool DrawPeriodLines(Dictionary<Period, Dictionary<int, List<Ocurrence>>> dcOcurrencesByPeriods)
        {
            if (dcOcurrencesByPeriods.Keys == null || dcOcurrencesByPeriods.Count == 0)
                return false;

            double dDefaultPeriodLineLength = DefaultWidthForBars * MinNumberOfBars;
            //Propiedades del Canvas
            double dSpaceBetweenPeriodLines = DefaultWidthForBars * MinNumberOfBars;
            double dInitialXPosition = HorizontalMarginForCanvas, dYPosition = VerticalMarginForCanvas,
                dCurrentXPosition = dInitialXPosition, dInitialPositionForTextBlock = dInitialXPosition * 2,
                dSpaceBetweenPeriods = DefaultWidthForBars, dCurrentXPositionForTextBlock = dInitialXPosition;
            double dWidth = HorizontalMarginForCanvas * 2 + dSpaceBetweenPeriodLines * (
                dcOcurrencesByPeriods.Count);

            cvWhiteBoard.Width = dWidth * 2;
            //Horizontal
            Line lnDefaultLineForPeriod = new Line
            {
                X1 = (dInitialXPosition * (dcOcurrencesByPeriods.Count - 1)) * dcOcurrencesByPeriods.Count,
                Stroke = System.Windows.Media.Brushes.Green,
                StrokeThickness = 2
            };

            Canvas.SetBottom(lnDefaultLineForPeriod, dYPosition);
            Canvas.SetLeft(lnDefaultLineForPeriod, HorizontalMarginForCanvas);

            cvWhiteBoard.Children.Add(lnDefaultLineForPeriod);
            
            foreach (Period period in dcOcurrencesByPeriods.Keys)
            {
                //Insert TextBlock Here
                Dictionary<int, List<Ocurrence>> dcInternalDictionary = dcOcurrencesByPeriods[period];

                double dSpaceBetweenPeriodTextBlock = (dcInternalDictionary.Keys.Count * DefaultWidthForBars) * 0.5;

                foreach (int iLicenseId in dcInternalDictionary.Keys)
                {
                    Line lnHorizontalForPeriod = new Line
                    {
                        X1 = DefaultWidthForBars * 2,
                        Stroke = System.Windows.Media.Brushes.Black,
                        StrokeThickness = 2
                    };

                    Canvas.SetBottom(lnHorizontalForPeriod, dYPosition);
                    Canvas.SetLeft(lnHorizontalForPeriod, dCurrentXPosition);

                    cvWhiteBoard.Children.Add(lnHorizontalForPeriod);

                    dCurrentXPosition += DefaultWidthForBars;
                }

                Line lnHorizontalForSpace = new Line
                {
                    X1 = DefaultWidthForBars,
                    Stroke = System.Windows.Media.Brushes.Black,
                    StrokeThickness = 2
                };

                TextBlock txtPeriod = new TextBlock
                {
                    Width = DefaultWidthForTextBlock,
                    Height = DefaultHeightForTextBlock,
                    TextWrapping = TextWrapping.Wrap,
                    TextAlignment = TextAlignment.Center,
                    Text = "[" + period.StartDate.ToShortDateString() + "]" + "  " + "-" +
                    "[" + period.EndDate.ToShortDateString() + "]"
                };

                    Canvas.SetBottom(txtPeriod, 0);
                    Canvas.SetLeft(txtPeriod, dCurrentXPositionForTextBlock);

                    dCurrentXPositionForTextBlock += dSpaceBetweenPeriodTextBlock;

                    cvWhiteBoard.Children.Add(txtPeriod);
                

                Canvas.SetBottom(lnHorizontalForSpace, dYPosition);
                Canvas.SetLeft(lnHorizontalForSpace, dCurrentXPosition);

                cvWhiteBoard.Children.Add(lnHorizontalForSpace);
            }

            //TextBlock
            dCurrentXPosition = dInitialPositionForTextBlock;

            //foreach (var vPeriods in dcOcurrencesByPeriods.Keys)
            //{
            //    TextBlock txtPeriod = new TextBlock
            //    {
            //        Width = DefaultWidthForTextBlock,
            //        Height = DefaultHeightForTextBlock,
            //        TextWrapping = TextWrapping.Wrap,
            //        TextAlignment = TextAlignment.Center,
            //        Text = "[" + vPeriods.StartDate.ToShortDateString() + "]" + "  " + "-" +
            //        "[" + vPeriods.EndDate.ToShortDateString() + "]"
            //    };

            //    Canvas.SetBottom(txtPeriod, 0);
            //    Canvas.SetLeft(txtPeriod, dCurrentXPosition);

            //    dCurrentXPosition += dSpaceBetweenPeriodLines + 100;

            //    cvWhiteBoard.Children.Add(txtPeriod);
            //}

            return true;
        }

        private bool DrawBars(
            Dictionary<Period, Dictionary<int, List<Ocurrence>>> dcOcurrencesByPeriods,
            Dictionary<int, LicenseDisplayer> dicLicenseDisplayers, List<int> lstRangeNumbers)
        {
            int iBarsCreated = 0;

            if (lstRangeNumbers == null || lstRangeNumbers.Count == 0 || RangeAssociated == null)
                return false;

            if (dcOcurrencesByPeriods.Keys == null || dcOcurrencesByPeriods.Count == 0)
                return false;

            double dInitialXPosition = m_dHorizontalMarginForCanvas, dInitialYPosition = m_dVerticalMarginForCanvas,
                dYPositionForLicenseLabel = dInitialYPosition, dXPositionForLicenseLabel = dInitialXPosition;
            double dCurrentXPosition = dInitialXPosition;

            foreach (Period period in dcOcurrencesByPeriods.Keys)
            {
                if (period == null) continue;

                Dictionary<int, List<Ocurrence>> dcOcurrencesByLicenseId = dcOcurrencesByPeriods[period];

                if (dcOcurrencesByLicenseId == null || dcOcurrencesByLicenseId.Count == 0)
                    continue;

                double dMaxHeightForBars = dInitialYPosition * (lstRangeNumbers.Count - 1);

                foreach (int iLicenseId in dcOcurrencesByLicenseId.Keys)
                {
                    if (!dcOcurrencesByLicenseId.ContainsKey(iLicenseId)) continue;

                    double dIntervalOfRange = ((lstRangeNumbers.Last() - lstRangeNumbers.First()) / (lstRangeNumbers.Count));

                    double dHeightForEachBar = (dcOcurrencesByLicenseId[iLicenseId].Count * SpaceBetweenRangeLines) / Math.Round(dIntervalOfRange);

                    SolidColorBrush solidColorBrush = new SolidColorBrush(Colors.Black);

                    if (dicLicenseDisplayers != null && dicLicenseDisplayers.Count > 0 &&
                        dicLicenseDisplayers.ContainsKey(iLicenseId))
                    {
                        LicenseDisplayer licenseDisplayer = dicLicenseDisplayers[iLicenseId];
                        if (licenseDisplayer.ColorAssociated != null)
                            solidColorBrush = new SolidColorBrush(licenseDisplayer.ColorAssociated);
                    }
                    //Bars
                    Rectangle rBarForEachLicense = new Rectangle
                    {
                        Width = m_dDefaultWidthForBars,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1,
                        Height = iLicenseId * dcOcurrencesByLicenseId[iLicenseId].Count, //Valores de prueba
                        //Height = dHeightForEachBar, //Valores reales
                        Fill = solidColorBrush,
                        Opacity = 0.8
                    };
                    //LabelsForLicense
                    Label lblLicenseId = new Label
                    {
                        Height = m_iDefaultHeightForLabels,
                        Width = m_iDefaultWidthForLabels,
                        Content = iLicenseId,
                        Foreground = Brushes.DarkRed
                    };
                    //LabelsForOcurrences
                    Label lblOcurrences = new Label
                    {
                        Height = m_iDefaultHeightForLabels,
                        Width = m_iDefaultWidthForLabels,
                        Content = dcOcurrencesByLicenseId[iLicenseId].Count
                    };

                    Panel.SetZIndex(lblOcurrences, 2);
                    Canvas.SetBottom(lblOcurrences, dInitialYPosition + m_dVerticalMarginForCanvas);
                    Canvas.SetLeft(lblOcurrences, dCurrentXPosition + 5);

                    Panel.SetZIndex(lblLicenseId, 2);
                    Canvas.SetBottom(lblLicenseId, dInitialYPosition);
                    Canvas.SetLeft(lblLicenseId, dCurrentXPosition + 5);

                    Panel.SetZIndex(rBarForEachLicense, 1);
                    Canvas.SetBottom(rBarForEachLicense, dInitialYPosition);
                    Canvas.SetLeft(rBarForEachLicense, dCurrentXPosition);

                    dCurrentXPosition += m_dDefaultWidthForBars;

                    iBarsCreated += 1;
                    if (iBarsCreated == dcOcurrencesByLicenseId.Keys.Count)
                    {
                        dCurrentXPosition += m_dDefaultWidthForBars;
                        break;
                    }

                    if (ShowBarsThatExceedTheMaximunRange == false && rBarForEachLicense.Height > RangeAssociated.Max)
                        continue;

                    cvWhiteBoard.Children.Add(lblLicenseId);
                    cvWhiteBoard.Children.Add(lblOcurrences);
                    cvWhiteBoard.Children.Add(rBarForEachLicense);
                }

                iBarsCreated = 0;
            }

            return true;
        }

        public bool Draw()
        {
            if (RangeAssociated == null) return false;

            List<Period> lstPeriods = ControlManager.Instance.GetPeriodsByMonths(TimePeriod);

            List<int> lstRangeNumbers = GetRangeNumbers(RangeAssociated);

            Dictionary<Period, Dictionary<int, List<Ocurrence>>> dcOcurrencesByPeriods =
                Ocurrence.GetOcurrencesByPeriod(lstPeriods, Ocurrences); //////

            List<int> lstLicenseIds = GetAllLicenseIds(dcOcurrencesByPeriods);

            Dictionary<int, LicenseDisplayer> dcLicensesDisplayer = LicensesDisplayer.Instance.SetColorsForLicenses(lstLicenseIds);

            if (!DrawRangeLines(RangeAssociated))
                return false;

            if (!DrawPeriodLines(dcOcurrencesByPeriods))
                return false;

           if (!DrawBars(dcOcurrencesByPeriods, dcLicensesDisplayer, lstRangeNumbers))
                return false;

            return true;
        }
    }
}
