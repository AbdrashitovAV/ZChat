using System.ServiceModel;

namespace ZChat.Shared.Contracts
{
    public interface IChatServiceCallback
    {

        [OperationContract(IsOneWay = true)]
        void HandleMessage(string message);

    }
}
