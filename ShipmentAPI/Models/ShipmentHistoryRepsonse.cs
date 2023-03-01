namespace ShipmentAPI.Models
{
    public class ShipmentHistoryRepsonse : BaseResponse
    {
        public List<ShipmentDetailInfo> Shipments { get; set; }

        public ShipmentHistoryRepsonse() : base()
        {
            this.Shipments = new List<ShipmentDetailInfo>();
        }
    }
}
