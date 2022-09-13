using Microsoft.Reporting.WinForms;
using System;
using System.Windows.Forms;

namespace ScaleAddon.Forms
{
    public partial class BuyingProcessLotPrintLbl : Form
    {
        public string Company { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string DocNumber { get; set; }
        public string BuyDate { get; set; }
        public string LotNumber { get; set; }
        public string VendorID { get; set; }
        public string VendorName { get; set; }
        public string VendorClass { get; set; }
        public string Grade { get; set; }
        public string Mutu { get; set; }
        public string Netto { get; set; }
        public string NTRM { get; set; }
        public string MC { get; set; }
        public string Reject { get; set; }
        public string Remark { get; set; }
        public string QRImage { get; set; }
        public string Warehouse { get; set; }
        public string Source { get; set; }
        public string Length { get; set; }
        public string Process { get; set; }
        public string StalkPos { get; set; }
        public string Color { get; set; }
        public string InventoryID { get; set; }
        public string Buyer { get; set; }
        public string Ferment { get; set; }
        public string Area { get; set; }
        public string Residue { get; set; }
        public string Rope { get; set; }
        public BuyingProcessLotPrintLbl()
        {
            InitializeComponent();
        }

        private void BuyingProcessLotPrintLbl_Load(object sender, EventArgs e)
        {
            this.Text = $"Label Print [{LotNumber}]";

            ReportParameter parCompany = new ReportParameter("parCompany", Company);
            ReportParameter parAddress = new ReportParameter("parAddress", Address);
            ReportParameter parPhone = new ReportParameter("parPhone", Phone);
            ReportParameter parDocNbr = new ReportParameter("parDocNbr", DocNumber);
            ReportParameter parBuyDate = new ReportParameter("parBuyDate", BuyDate);
            ReportParameter parLotNbr = new ReportParameter("parLotNbr", LotNumber);
            ReportParameter parGrade = new ReportParameter("parGrade", Grade);
            ReportParameter parMutu = new ReportParameter("parMutu", Mutu);
            ReportParameter parNetto = new ReportParameter("parNetto", Netto);
            ReportParameter parVendor = new ReportParameter("parVendor", $"{VendorID} / {VendorName}");
            ReportParameter parVendorClass = new ReportParameter("parVendorClass", VendorClass);
            ReportParameter parNTRM = new ReportParameter("parNTRM", NTRM);
            ReportParameter parMC = new ReportParameter("parMC", MC);
            ReportParameter parRejected = new ReportParameter("parRejected", Reject);
            ReportParameter parRemark = new ReportParameter("parRemark", Remark);
            ReportParameter parQRImage = new ReportParameter("parQRImage", QRImage);
            ReportParameter parWarehouse = new ReportParameter("parWarehouse", Warehouse);
            ReportParameter parSource = new ReportParameter("parSource", Source);
            ReportParameter parLength = new ReportParameter("parLength", Length);
            ReportParameter parProcess = new ReportParameter("parProcess", Process);
            ReportParameter parStalkPos = new ReportParameter("parStalkPos", StalkPos);
            ReportParameter parColor = new ReportParameter("parColor", Color);
            ReportParameter parInventoryID = new ReportParameter("parInventoryID", InventoryID);
            ReportParameter parBuyer = new ReportParameter("parBuyer", Buyer);
            ReportParameter parFerment = new ReportParameter("parFerment", Ferment);
            ReportParameter parArea = new ReportParameter("parArea", Area);
            ReportParameter parResidue = new ReportParameter("parResidue", Residue);
            ReportParameter parRope = new ReportParameter("parRope", Rope);

            this.reportViewer1.LocalReport.EnableExternalImages = true;
            this.reportViewer1.LocalReport.SetParameters(new ReportParameterCollection() { parCompany, parAddress, parPhone, parDocNbr, parBuyDate, parLotNbr, parGrade, parMutu, parNetto, parVendor, parVendorClass, parNTRM, parMC, parRejected, parRemark, parQRImage, parWarehouse, parSource, parLength, parProcess, parStalkPos, parColor, parInventoryID, parBuyer, parFerment, parArea, parResidue, parRope });

            this.reportViewer1.RefreshReport();

            //direct print and disable preview
            this.reportViewer1.LocalReport.PrintToPrinter();
            this.Close();
        }
    }
}
