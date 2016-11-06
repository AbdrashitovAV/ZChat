namespace ZChat.Shared
{
    public class ServiceResult
    {
        public bool IsSucessful { get; set; }

        public string ErrorMessage { get; set; }


        public ServiceResult()
        {
            IsSucessful = true;
        }

        public ServiceResult(string errorMessage)
        {
            IsSucessful = false;
            ErrorMessage = errorMessage;
        }
    }
}
