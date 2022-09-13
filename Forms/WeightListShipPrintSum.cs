using Microsoft.Reporting.WinForms;
using System;
using System.Windows.Forms;

namespace ScaleAddon.Forms
{
    public partial class WeightListShipPrintSum : Form
    {
        public string Company { get; set; }
        public string Warehouse { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string DocNumber { get; set; }
        public string DocType { get; set; }
        public string DocStatus { get; set; }
        public string DispatchDate { get; set; }
        public DataSetAddon DispatchDetails { get; set; }
        public string FromTo { get; set; }
        public string LogisticService { get; set; }
        public string LisencePlate { get; set; }
        public WeightListShipPrintSum()
        {
            InitializeComponent();
        }

        private void WeightListPrintSum_Load(object sender, EventArgs e)
        {
            ReportParameter parCompany = new ReportParameter("parCompany", Company);
            ReportParameter parWarehouse = new ReportParameter("parWarehouse", Warehouse);
            ReportParameter parAddress = new ReportParameter("parAddress", Address);
            ReportParameter parPhone = new ReportParameter("parPhone", Phone);
            ReportParameter parDocNbr = new ReportParameter("parDocNbr", DocNumber);
            ReportParameter parDocType = new ReportParameter("parDocType", DocType);
            ReportParameter parDocStatus = new ReportParameter("parDocStatus", DocStatus);
            ReportParameter parDispatchDate = new ReportParameter("parDispatchDate", DispatchDate);
            ReportParameter parFromTo = new ReportParameter("parFromTo", FromTo);
            ReportParameter parLogisticService = new ReportParameter("parLogisticService", LogisticService);
            ReportParameter parLisencePlate = new ReportParameter("parLisencePlate", LisencePlate);

            this.reportViewer1.LocalReport.EnableExternalImages = true;
            this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", DispatchDetails.Tables["WeightListLineDetail"]));
            this.reportViewer1.LocalReport.SetParameters(new ReportParameterCollection() { parCompany, parWarehouse, parAddress, parPhone, parDocNbr, parDocType, parDocStatus, parDispatchDate, parFromTo, parLogisticService, parLisencePlate });

            this.reportViewer1.RefreshReport();
        }
    }
}