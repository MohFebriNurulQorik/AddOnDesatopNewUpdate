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
    public partial class ScaleCalibrationPrint2 : Form
    {
        public string docNum { get; set; }
        public string docdate { get; set; }
        public string ClientID { get; set; }
        public string Warehouse { get; set; }
        public string DocType { get; set; }
        public string CreatorID { get; set; }
        public string Company { get; set; }
        public string WarehouseDescr { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DataSetAddon DocDetails { get; set; }

        public ScaleCalibrationPrint2()
        {
            InitializeComponent();
        }

        private void ScaleCalibrationPrint2_Load(object sender, EventArgs e)
        {
            ReportParameter parDocNbr = new ReportParameter("parDocNbr", docNum);
            ReportParameter parDocDate = new ReportParameter("parDocDate", docdate);
            ReportParameter parClientID = new ReportParameter("parClientID", ClientID);
            ReportParameter parWarehouse = new ReportParameter("parWarehouse", Warehouse);
            ReportParameter parCreatorID = new ReportParameter("parCreatorID", CreatorID);
            ReportParameter parDocType = new ReportParameter("parDocType", DocType);
            ReportParameter parCompany = new ReportParameter("parCompany", Company);
            ReportParameter parWarehouseDescr = new ReportParameter("parWarehouseDescr", WarehouseDescr);
            ReportParameter parAddress = new ReportParameter("parAddress", Address);
            ReportParameter parPhone = new ReportParameter("parPhone", Phone);
          

            this.reportViewer1.LocalReport.EnableExternalImages = true;
            this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ScaleCalibrationDetail", DocDetails.Tables["ScaleCalibrationDetail"]));
            this.reportViewer1.LocalReport.SetParameters(new ReportParameterCollection() { parClientID, parPhone, parAddress, parWarehouseDescr, parCompany, parWarehouse, parDocNbr, parDocType, parDocDate, parCreatorID });
    
            this.reportViewer1.RefreshReport();
        }


        private void reportViewer1_Load_1(object sender, EventArgs e)
        {

        }
    }
}
