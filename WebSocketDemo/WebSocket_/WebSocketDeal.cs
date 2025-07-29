using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace NativeMessageTest
{
    class WebSocketDeal : WebSocketBehavior
    {
        private static readonly List<WebSocketDeal> _clients = new List<WebSocketDeal>();
        
        public static event Action<long, ScrollData> OnScrollDataReceived;
        public static event Action<long, CanvasData> OnCanvasDataReceived;
        public static event Action<string> OnMessageReceived;

        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine("[OnMessage]" + e.Data);
            
            try
            {
                SocketData data = JsonConvert.DeserializeObject<SocketData>(e.Data);
                HandleMessage(data.Message.Action, data.Message.ClientId, data.Message.Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OnMessage] 解析错误: {ex.Message}");
            }
        }

        private void HandleMessage(string action, long clientId, object data)
        {
            if (!(data is JObject jObject)) return;
            switch (action)
            {
                case "scroll":
                    ScrollData scrollData = jObject.ToObject<ScrollData>();
                    OnScrollDataReceived?.Invoke(clientId, scrollData);
                    break;
                case "sendCanvasData":
                    CanvasData canvasData = jObject.ToObject<CanvasData>();
                    OnCanvasDataReceived?.Invoke(clientId, canvasData);
                    break;
                case "ping":
                    SendToAll($"{{\"action\":\"pong\"}}");
                    break;
            }
        }


        // 发送消息到所有客户端
        public static void SendToAll(string message)
        {
            foreach (var client in _clients.ToList())
            {
                try
                {
                    client.Send(message);
                }
                catch
                {
                    _clients.Remove(client);
                }
            }
        }

        protected override void OnOpen()
        {
            Console.WriteLine("[OnOpen] WebSocket连接已建立");
            _clients.Add(this);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Console.WriteLine($"[OnClose] WebSocket连接已关闭: {e.Reason}");
            _clients.Remove(this);
        }

        protected override void OnError(ErrorEventArgs e)
        {
            Console.WriteLine($"[OnError] WebSocket错误: {e.Message}");
        }


    }
}
