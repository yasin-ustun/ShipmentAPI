namespace ShipmentAPI.Models
{
    public class ShipmentDetailInfo
    {
        public List<PackageInfo> Packages { get; set; }
        public decimal TotalWeight { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime ShipmentDate { get; set; }

        public ShipmentDetailInfo()
        {
            this.Packages = new List<PackageInfo>();
        }
    }
}
