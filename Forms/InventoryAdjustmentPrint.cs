using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace ScaleAddon.Forms
{
    public partial class InventoryAdjustmentPrint : Form
    {
        public string Company { get; set; }
        public string Warehouse { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string DocNumber { get; set; }
        public string DocType { get; set; }
        public string DocDate { get; set; }
        public string DocStatus { get; set; }

        public string Remark { get; set; }
        public DataSetAddon DocDetails { get; set; }

        public InventoryAdjustmentPrint()
        {
            InitializeComponent();
        }

        private void InventoryAdjustmentPrint_Load(object sender, EventArgs e)
        {
            ReportParameter parCompany = new ReportParameter("parCompany", Company);
            ReportParameter parWarehouse = new ReportParameter("parWarehouse", Warehouse);
            ReportParameter parAddress = new ReportParameter("parAddress", Address);
            ReportParameter parPhone = new ReportParameter("parPhone", Phone);
            ReportParameter parDocNbr = new ReportParameter("parDocNbr", DocNumber);
            ReportParameter parDocType = new ReportParameter("parDocType", DocType);
            ReportParameter parDocDate = new ReportParameter("parDocDate", DocDate);
            ReportParameter parDocStatus = new ReportParameter("parDocStatus", DocStatus);
            ReportParameter parRemark = new ReportParameter("parRemark", Remark);

            this.reportViewer1.LocalReport.EnableExternalImages = true;
            this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", DocDetails.Tables["InventoryAdjustmentDetail"]));
            this.reportViewer1.LocalReport.SetParameters(new ReportParameterCollection() { parCompany, parWarehouse, parAddress, parPhone, parDocNbr, parDocType, parDocDate, parDocStatus, parRemark });

            this.reportViewer1.RefreshReport();

            this.reportViewer1.RefreshReport();
        }
    }
}
