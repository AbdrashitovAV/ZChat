using ZChat.Shared;

namespace ZChat.Server.Logic
{
    public interface ISignalRServerConnectionManager
    {
        ServiceDataResult<ConnectionData> Join(string username, string connectionId);
        ServiceResult MessageRecieved(Message message, string connectionId);
        ServiceResult Left(string connectionId, bool b);
    }
}
