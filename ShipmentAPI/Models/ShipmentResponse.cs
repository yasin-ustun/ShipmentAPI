namespace ShipmentAPI.Models
{
    public class ShipmentResponse : BaseResponse
    {
        public ShipmentDetailInfo ShipmentDetail { get; set; }

        public ShipmentResponse() : base()
        {
            this.ShipmentDetail = new ShipmentDetailInfo();
        }
    }
}
