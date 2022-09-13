using Microsoft.Reporting.WinForms;
using System;
using System.Windows.Forms;

namespace ScaleAddon.Forms
{
    public partial class GenericLotPrintThermal : Form
    {
        public string LotNumber { get; set; }
        public string Source { get; set; }
        public string StalkPos { get; set; }
        public string Ferment { get; set; }
        public string Buyer { get; set; }
        public string InventoryID { get; set; }
        public string Process { get; set; }
        public string Stage { get; set; }
        public string Grade { get; set; }
        public string Color { get; set; }
        public string Weight { get; set; }
        public string Length { get; set; }
        public string Warehouse { get; set; }
        public string Date { get; set; }
        public string Remark { get; set; }
        public string QRImage { get; set; }
        public string Area { get; set; }
        public string StrCrop { get; set; }
        public string Forms { get; set; }
        public GenericLotPrintThermal()
        {
            InitializeComponent();
        }

        private void GenericOUTProcessLotPrintThermal_Load(object sender, EventArgs e)
        {
            this.Text = $"Label Print [{LotNumber}]";

            ReportParameter parLotNbr = new ReportParameter("parLotNbr", LotNumber);
            ReportParameter parSource = new ReportParameter("parSource", Source);
            ReportParameter parStalkPos = new ReportParameter("parStalkPos", StalkPos);
            ReportParameter parFerment = new ReportParameter("parFerment", Ferment);
            ReportParameter parBuyer = new ReportParameter("parBuyer", Buyer);
            ReportParameter parInventoryID = new ReportParameter("parInventoryID", InventoryID);
            ReportParameter parProcess = new ReportParameter("parProcess", Process);
            ReportParameter parStage = new ReportParameter("parStage", Stage);
            ReportParameter parGrade = new ReportParameter("parGrade", Grade);
            ReportParameter parColor = new ReportParameter("parColor", Color);
            ReportParameter parWeight = new ReportParameter("parWeight", Weight);
            ReportParameter parLength = new ReportParameter("parLength", Length);
            ReportParameter parWarehouse = new ReportParameter("parWarehouse", Warehouse);
            ReportParameter parDate = new ReportParameter("parDate", Date);
            ReportParameter parRemark = new ReportParameter("parRemark", Remark);

            ReportParameter parQRImage = new ReportParameter("parQRImage", QRImage);
            ReportParameter parArea = new ReportParameter("parArea", Area);
            ReportParameter parStrCrop = new ReportParameter("parStrCrop", StrCrop);
            ReportParameter parForms = new ReportParameter("parForms", Forms);
            this.reportViewer1.LocalReport.EnableExternalImages = true;
            this.reportViewer1.LocalReport.SetParameters(new ReportParameterCollection() { parLotNbr, parSource, parStalkPos, parFerment, parBuyer, parInventoryID, parProcess, parStage, parGrade, parColor, parWeight, parLength, parWarehouse, parDate, parQRImage, parRemark, parArea, parStrCrop, parForms });

            this.reportViewer1.RefreshReport();

            //direct print and disable preview
            this.reportViewer1.LocalReport.PrintToPrinter();
            this.Close();
        }
    }
}