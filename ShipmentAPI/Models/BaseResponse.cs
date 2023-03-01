namespace ShipmentAPI.Models
{
    public class BaseResponse
    {
        public ResultMessage ResultMessage { get; set; }

        public BaseResponse()
        {
            this.ResultMessage = new ResultMessage();
        }
    }
}
