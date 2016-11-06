using System.ServiceModel;
using System.Threading.Tasks;

namespace ZChat.Shared.Contracts
{
//    [ServiceContract(CallbackContract = typeof(IChatServiceCallback))]
    [ServiceContract()]
    public interface IChatService
    {
        [OperationContract]
        Task<bool> LogUser(string userName);

        /// <summary>
        /// Keeps the connection between the client and server.
        /// Connection between a client and server has a time-out,
        /// so the client needs to call this before that happens
        /// to remain connected to the server.
        /// </summary>

        [OperationContract(IsOneWay = true)]
        Task KeepConnection();

        Task SendMessage(Message message);
    }
}
