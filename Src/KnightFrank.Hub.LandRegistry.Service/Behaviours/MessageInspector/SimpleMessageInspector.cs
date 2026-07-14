using System;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;

namespace KnightFrank.Hub.LandRegistry.Service
{
    // Client message inspector  
    public class SimpleMessageInspector : IClientMessageInspector
    {
        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            // Implement this method to inspect/modify messages after a message  
            // is received but prior to passing it back to the client
            Console.WriteLine("AfterReceiveReply called :" + reply);
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel)
        {
            // Implement this method to inspect/modify messages before they
            // are sent to the service  
            Console.WriteLine("BeforeSendRequest called :" + request);
            return null;
        }
    }
}
