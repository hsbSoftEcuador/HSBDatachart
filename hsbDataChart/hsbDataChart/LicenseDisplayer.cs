using System.Windows.Media;

namespace hsbDataChart
{
    public class LicenseDisplayer
    {
        private int m_iLicenseId = 0;
        private SolidColorBrush m_bColors = new SolidColorBrush(Colors.Black);
        private Color m_Color = Colors.Black;

        public SolidColorBrush ColorBrush
        {
            get => m_bColors;
            set => m_bColors = value;
        }

        public Color ColorAssociated
        {
            get => m_Color;
            set => m_Color = value;
        }

        public int LicenseId
        {
            get => m_iLicenseId;
            set => m_iLicenseId = value;
        }

        public LicenseDisplayer(int iLicense, Color color)
        {
            m_iLicenseId = iLicense;
            m_Color = color;
        }
    }
}
