namespace ZChat.Shared
{
    public class ServiceDataResult<T> : ServiceResult
    {
        public T Data { get; set; }

        public ServiceDataResult()
        {
            
        }

        public ServiceDataResult(T data) : base()
        {
            Data = data;
        }

        public ServiceDataResult(string errorMessage) : base(errorMessage)
        {
        }
    }
}
