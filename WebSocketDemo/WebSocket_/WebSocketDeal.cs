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
        public static event Action<ScrollData> OnScrollDataReceived;
        public static event Action<string> OnMessageReceived;

        protected override void OnMessage(MessageEventArgs e)
        {
            //Console.WriteLine("[OnMessage]" + e.Data);
            
            try
            {
                // 尝试直接解析为WebSocketMessage格式
                var webSocketMessage = JsonConvert.DeserializeObject<WebSocketMessage>(e.Data);
                if (webSocketMessage != null && !string.IsNullOrEmpty(webSocketMessage.Action))
                {
                    HandleMessage(webSocketMessage.Action, webSocketMessage.Data);
                    return;
                }

                // 如果不是，尝试解析为包装的SocketData格式
                SocketData data = JsonConvert.DeserializeObject<SocketData>(e.Data);
                HandleMessage(data.Message.Action, data.Message.Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OnMessage] 解析错误: {ex.Message}");
            }
        }

        private void HandleMessage(string action, object data)
        {
            switch (action)
            {
                case "scroll":
                    // 根据data的实际类型进行转换
                    if (data is JObject jObject)
                    {
                        ScrollData scrollData = jObject.ToObject<ScrollData>();
                        OnScrollDataReceived?.Invoke(scrollData);
                        //Console.WriteLine($"[OnMessage] scroll {scrollData.ScrollX},{scrollData.ScrollY}");
                    }
                    break;
                // 可以继续添加其他Action类型
            }
        }



        protected override void OnOpen()
        {
            Console.WriteLine("[OnOpen] WebSocket连接已建立");
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Console.WriteLine($"[OnClose] WebSocket连接已关闭: {e.Reason}");
        }

        protected override void OnError(ErrorEventArgs e)
        {
            Console.WriteLine($"[OnError] WebSocket错误: {e.Message}");
        }


    }
}
