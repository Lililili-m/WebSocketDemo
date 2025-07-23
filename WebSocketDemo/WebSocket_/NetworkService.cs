using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace NativeMessageTest
{
    public class NetworkService
    {
        private WebSocketServer _server;
        private int _port = 59773;

        public void Start()
        {
            _server = new WebSocketServer(_port);
            _server.AddWebSocketService<WebSocketDeal>("/WebSocketDeal");
            _server.Start();
        }
    }
}
