using Microsoft.Reporting.WinForms;
using System;
using System.Windows.Forms;

namespace ScaleAddon.Forms
{
    public partial class BuyingProcessDocPrint : Form
    {
        public string Company { get; set; }
        public string Warehouse { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string DocNumber { get; set; }
        public string DocType { get; set; }
        public string DocDate { get; set; }
        public string NoReg { get; set; }
        public string VendorID { get; set; }
        public string VendorDetails { get; set; }
        public string OrderNbr { get; set; }
        public string DocStatus { get; set; }
        public DataSetAddon DocDetails { get; set; }

        public BuyingProcessDocPrint()
        {
            InitializeComponent();
        }

        private void BuyingProcessDocPrint_Load(object sender, EventArgs e)
        {
            ReportParameter parCompany = new ReportParameter("parCompany", Company);
            ReportParameter parWarehouse = new ReportParameter("parWarehouse", Warehouse);
            ReportParameter parAddress = new ReportParameter("parAddress", Address);
            ReportParameter parPhone = new ReportParameter("parPhone", Phone);
            ReportParameter parDocNbr = new ReportParameter("parDocNbr", DocNumber);
            ReportParameter parDocType = new ReportParameter("parDocType", DocType);
            ReportParameter parDocDate = new ReportParameter("parDocDate", DocDate);
            ReportParameter parNoReg = new ReportParameter("parNoReg", NoReg);
            ReportParameter parVendorID = new ReportParameter("parVendorID", VendorID);
            ReportParameter parVendorDetails = new ReportParameter("parVendorDetails", VendorDetails);
            ReportParameter parOrderNbr = new ReportParameter("parOrderNbr", OrderNbr);
            ReportParameter parDocStatus = new ReportParameter("parDocStatus", DocStatus);

            this.reportViewer1.LocalReport.EnableExternalImages = true;
            this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", DocDetails.Tables["BuyingLineDetail"]));
            this.reportViewer1.LocalReport.SetParameters(new ReportParameterCollection() { parCompany, parWarehouse, parAddress, parPhone, parDocNbr, parDocType, parDocDate, parNoReg, parVendorID, parVendorDetails, parOrderNbr, parDocStatus });

            this.reportViewer1.RefreshReport();
        }
    }
}