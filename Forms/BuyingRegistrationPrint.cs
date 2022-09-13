using Microsoft.Reporting.WinForms;
using System;
using System.Windows.Forms;

namespace ScaleAddon.Forms
{
    public partial class BuyingRegistrationPrint : Form
    {
        public string Company { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Warehouse { get; set; }
        public string RegDate { get; set; }
        public string RegNumber { get; set; }
        public string VendorID { get; set; }
        public string VendorName { get; set; }
        public string Contract { get; set; }
        public string QRImage { get; set; }

        public BuyingRegistrationPrint()
        {
            InitializeComponent();
        }

        private void BuyingRegistrationPrint_Load(object sender, EventArgs e)
        {
            ReportParameter parCompany = new ReportParameter("parCompany", Company);
            ReportParameter parAddress = new ReportParameter("parAddress", Address);
            ReportParameter parPhone = new ReportParameter("parPhone", Phone);
            ReportParameter parWarehouse = new ReportParameter("parWarehouse", Warehouse);
            ReportParameter parRegDate = new ReportParameter("parRegDate", RegDate);
            ReportParameter parRegNumber = new ReportParameter("parRegNumber", RegNumber);
            ReportParameter parVendorID = new ReportParameter("parVendorID", VendorID);
            ReportParameter parVendorName = new ReportParameter("parVendorName", VendorName);
            ReportParameter parContract = new ReportParameter("parContract", Contract);
            ReportParameter parQRImage = new ReportParameter("parQRImage", QRImage);

            this.reportViewer1.LocalReport.EnableExternalImages = true;
            this.reportViewer1.LocalReport.SetParameters(new ReportParameterCollection() { parCompany, parAddress, parPhone, parWarehouse, parRegDate, parRegNumber, parVendorID, parVendorName, parContract,  parQRImage });

            this.reportViewer1.RefreshReport();
        }
    }
}