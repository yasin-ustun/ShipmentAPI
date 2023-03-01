namespace ShipmentAPI.Models
{
    public class ResultMessage
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }

        public ResultMessage()
        {
            this.Message = "OK";
            this.IsSuccess = true;
        }

        public void SetExceptionMessage(int code, string message)
        {
            this.Code = code;
            this.Message = message;
            this.IsSuccess = false;
        }
    }
}
