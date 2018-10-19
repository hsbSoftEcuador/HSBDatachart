using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace hsbDataChart
{
    public class LicensesDisplayer
    {
        private Dictionary<int, LicenseDisplayer> m_dicLicenseDisplayersByLicenseId = new Dictionary<int, LicenseDisplayer>();
        private static LicensesDisplayer m_Instance = null;

        public static LicensesDisplayer Instance
        {
            get { return m_Instance; }
        }

        public Dictionary<int, LicenseDisplayer> LicensesDisplayers
        {
            get => m_dicLicenseDisplayersByLicenseId;
            set => m_dicLicenseDisplayersByLicenseId = value;
        }

        private LicensesDisplayer() { }

        static LicensesDisplayer()
        {
            m_Instance = new LicensesDisplayer();
        }

        public bool IsColorUsed(Dictionary<int, LicenseDisplayer> dcLicenseDisplayersByLicense, Color color)
        {
            if (dcLicenseDisplayersByLicense == null || dcLicenseDisplayersByLicense.Count == 0)
                return false;

            var arLicenseDisplayers = from licenseDisplayer in dcLicenseDisplayersByLicense.Values
                                      where licenseDisplayer != null && licenseDisplayer.ColorAssociated == color
                                      select licenseDisplayer;

            if (arLicenseDisplayers != null && arLicenseDisplayers.Count() > 0)
                return true;

            return false;
        }

        public Dictionary<int, LicenseDisplayer> SetColorsForLicenses(List<int> lstLicensesIds)
        {
            if (m_dicLicenseDisplayersByLicenseId == null)
                m_dicLicenseDisplayersByLicenseId = new Dictionary<int, LicenseDisplayer>();
            else
                m_dicLicenseDisplayersByLicenseId.Clear();

            if (lstLicensesIds == null || lstLicensesIds.Count == 0)
                return m_dicLicenseDisplayersByLicenseId;

            foreach (int iLicense in lstLicensesIds)
            {
                Color color = Colors.Black;

                do
                {
                    Random rColor = new Random();

                    Byte[] arByteNumbers = new Byte[3];
                    rColor.NextBytes(arByteNumbers);
                    color = Color.FromRgb(arByteNumbers[0], arByteNumbers[1], arByteNumbers[2]);

                } while (IsColorUsed(m_dicLicenseDisplayersByLicenseId, color) == true);

                if (m_dicLicenseDisplayersByLicenseId.ContainsKey(iLicense)) continue;

                LicenseDisplayer licenseDisplayer = new LicenseDisplayer(iLicense, color);

                m_dicLicenseDisplayersByLicenseId.Add(iLicense, licenseDisplayer);
            }

            return m_dicLicenseDisplayersByLicenseId;
        }
    }
}
