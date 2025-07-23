using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeMessageTest
{
    public class SocketData
    {
        [JsonProperty("message")]
        public SocketMessage Message { get; set; }
    }

    public class SocketMessage
    {
        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; } // 改为object类型，支持复杂对象
    }

    /// <summary>
    /// 滚动数据完整定义
    /// </summary>
    public class ScrollData
    {
        [JsonProperty("scrollX")]
        public double ScrollX { get; set; }

        [JsonProperty("scrollY")]
        public double ScrollY { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }

    /// <summary>
    /// 基础消息数据（用于简单字符串消息）
    /// </summary>
    public class SimpleMessageData
    {
        public string Content { get; set; }
    }

    /// <summary>
    /// 标签页信息
    /// </summary>
    public class TabInfo
    {
        [JsonProperty("tabId")]
        public int? TabId { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    /// <summary>
    /// 完整的WebSocket消息格式
    /// </summary>
    public class WebSocketMessage
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }

        [JsonProperty("tabId")]
        public int? TabId { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }
}
