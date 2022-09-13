using Microsoft.Reporting.WinForms;
using System;
using System.Windows.Forms;

namespace ScaleAddon.Forms
{
    public partial class PurchaseInvoicePrint : Form
    {
        public string Company { get; set; }
        public string Warehouse { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string DocNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string VendorID { get; set; }
        public string VendorDetails { get; set; }
        public DataSetAddon invDetails { get; set; }
        public string CashAdvance { get; set; }
        public string TaxDeduct { get; set; }
        public string LoanDeduct { get; set; }
        public string TotalPayment { get; set; }
        public string BuyerName { get; set; }
        public string AdminName { get; set; }
        public string VendorClass  { get; set; }

        public PurchaseInvoicePrint()
        {
            InitializeComponent();
        }

        private void PurchaseInvoicePrint_Load(object sender, EventArgs e)
        {
            ReportParameter parCompany = new ReportParameter("parCompany", Company);
            ReportParameter parWarehouse = new ReportParameter("parWarehouse", Warehouse);
            ReportParameter parAddress = new ReportParameter("parAddress", Address);
            ReportParameter parPhone = new ReportParameter("parPhone", Phone);
            ReportParameter parDocNbr = new ReportParameter("parDocNbr", DocNumber);
            ReportParameter parInvoiceDate = new ReportParameter("parInvoiceDate", InvoiceDate);
            ReportParameter parVendor = new ReportParameter("parVendor", VendorID);
            ReportParameter parVendorDetails = new ReportParameter("parVendorDetails", VendorDetails);
            ReportParameter parCashAdvance = new ReportParameter("parCashAdvance", CashAdvance);
            ReportParameter parTaxDeduct = new ReportParameter("parTaxDeduct", TaxDeduct);
            ReportParameter parLoanDeduct = new ReportParameter("parLoanDeduct", LoanDeduct);
            ReportParameter parTotalPayment = new ReportParameter("parTotalPayment", TotalPayment);

          

            string[] lines = VendorDetails.Split(
                                new[] { Environment.NewLine },
                                StringSplitOptions.None
                            );
            string VendorName = lines[0];

            ReportParameter parVendorName = new ReportParameter("parVendorName", VendorName);
            ReportParameter parBuyerName = new ReportParameter("parBuyerName", BuyerName);
            ReportParameter parAdminName = new ReportParameter("parAdminName", AdminName);
            ReportParameter parVendorClass = new ReportParameter("parVendorClass", VendorClass);

            this.reportViewer1.LocalReport.EnableExternalImages = true;
            this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", invDetails.Tables["PurchaseInvoiceDetailGrade"]));
            this.reportViewer1.LocalReport.SetParameters(new ReportParameterCollection() { parCompany, parWarehouse, parAddress, parPhone, parDocNbr, parInvoiceDate, parVendor, parVendorDetails, parCashAdvance, parTaxDeduct, parLoanDeduct, parTotalPayment, parVendorName, parBuyerName, parAdminName, parVendorClass });

            this.reportViewer1.RefreshReport();
        }
    }
}