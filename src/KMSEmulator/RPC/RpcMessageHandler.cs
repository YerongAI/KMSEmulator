using System;
using KMSEmulator.KMS;
using KMSEmulator.RPC.Bind;
using KMSEmulator.RPC.Request;

namespace KMSEmulator.RPC
{
    public class RpcMessageHandler : IMessageHandler
    {
        private IMessageHandler RequestMessageHandler { get; set; }
        private IKMSServerSettings Settings { get; set; }

        public RpcMessageHandler(IKMSServerSettings settings, IMessageHandler requestMessageHandler)
        {
            RequestMessageHandler = requestMessageHandler;
            Settings = settings;
        }

        public byte[] HandleRequest(byte[] request)
        {
            byte messageType = request[2];

            IMessageHandler requestHandler = GetMessageHandler(messageType);

            byte[] response = requestHandler.HandleRequest(request);
            return response;
        }

        private IMessageHandler GetMessageHandler(byte messageType)
        {
            IMessageHandler requestHandler = messageType switch
            {
                0x0b => new RpcBindMessageHandler(Settings),
                0x00 => new RpcRequestMessageHandler(RequestMessageHandler),
                _ => throw new ApplicationException("Unhandled message type: " + messageType),
            };
            return requestHandler;
        }
    }
}
