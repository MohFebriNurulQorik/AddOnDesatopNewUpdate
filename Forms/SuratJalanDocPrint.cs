using Microsoft.Reporting.WinForms;
using System;
using System.Windows.Forms;

namespace ScaleAddon.Forms
{
    public partial class SuratJalanDocPrint : Form
    {
        public string Company { get; set; }
        public string Warehouse { get; set; }
        public string Warehouse2 { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string Phone { get; set; }
        public string DocNumber { get; set; }
        public string DocType { get; set; }
        public string DocDate { get; set; }
        public string Notes { get; set; }
        public string Logistic { get; set; }
        public string LisencePlate { get; set; }
        public DataSetAddon DispatchDetails { get; set; }

        public SuratJalanDocPrint()
        {
            InitializeComponent();
        }

        private void SuratJalanDocPrint_Load(object sender, EventArgs e)
        {
            ReportParameter parCompany = new ReportParameter("parCompany", Company);
            ReportParameter parWarehouse = new ReportParameter("parWarehouse", Warehouse);
            ReportParameter parWarehouse2 = new ReportParameter("parWarehouse2", Warehouse2);
            ReportParameter parAddress = new ReportParameter("parAddress", Address);
            ReportParameter parAddress2 = new ReportParameter("parAddress2", Address2);
            ReportParameter parPhone = new ReportParameter("parPhone", Phone);
            ReportParameter parDocNbr = new ReportParameter("parDocNbr", DocNumber);
            ReportParameter parDocType = new ReportParameter("parDocType", DocType);
            ReportParameter parDocDate = new ReportParameter("parDocDate", DocDate);
            ReportParameter parNotes = new ReportParameter("parNotes", Notes);
            ReportParameter parLogistic = new ReportParameter("parLogistic", Logistic);
            ReportParameter parLisencePlate = new ReportParameter("parLisencePlate", LisencePlate);

            this.reportViewer1.LocalReport.EnableExternalImages = true;
            this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", DispatchDetails.Tables["SuratJalanDetail"]));
            this.reportViewer1.LocalReport.SetParameters(new ReportParameterCollection() { parCompany, parWarehouse, parWarehouse2, parAddress, parAddress2, parPhone, parDocNbr, parDocType, parDocDate, parNotes,parLogistic,parLisencePlate });

            this.reportViewer1.RefreshReport();
        }
    }
}