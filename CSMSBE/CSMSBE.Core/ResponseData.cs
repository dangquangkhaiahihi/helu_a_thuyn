namespace CSMSBE.Core
{
    public class ResponseData
    {
        public ResponseData()
        {
        }
        public object Content { get; set; }
        public ResponseErrorData Err { get; set; }
    }
    public class ResponseErrorData{
        public ResponseErrorData()
        {
        }
        public ResponseErrorData(string ErrorType, string ErrorMessage)
        {
            this.ErrorType = ErrorType;
            this.ErrorMessage = ErrorMessage;
        }
        public string ErrorType { get; set; }
        public string ErrorMessage { get; set; }
    }
}
