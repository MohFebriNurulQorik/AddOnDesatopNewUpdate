namespace ScaleAddon
{
    public class WarehouseModel
    {
        public string WarehouseID { get; set; }
        public string Descr { get; set; }
        public string Company { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Branch { get; set; }

        public void ClearWarehouse()
        {
            WarehouseID = "";
            Descr = "";
            Company = "";
            AddressLine1 = "";
            AddressLine2 = "";
            Phone1 = "";
            Phone2 = "";
            Branch = "";
        }
    }
}