using NativeMessageTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WebSocketDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private NetworkService _ns = new NetworkService();
        private TransformGroup _transformGroup;
        private ScaleTransform _scaleTransform = new ScaleTransform();
        private TranslateTransform _translateTransform = new TranslateTransform();
        private long _lastScrollTime = 0;
        private long _clientId = 0;

        public MainWindow()
        {
            InitializeComponent();
            _transformGroup = new TransformGroup();
            _transformGroup.Children.Add(_scaleTransform);
            _transformGroup.Children.Add(_translateTransform);
            SyncRect.RenderTransform = _transformGroup;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _ns.Start();
            WebSocketDeal.OnScrollDataReceived += WebSocketDeal_OnScrollDataReceived;
            WebSocketDeal.OnCanvasDataReceived += WebSocketDeal_OnCanvasDataReceived;
        }

        private void WebSocketDeal_OnCanvasDataReceived(long arg1, CanvasData arg2)
        {
            var base64String = arg2.Base64;
            var result = FileHelper.SaveBase64WithAutoExtension(base64String, @"D:\l78z\Test\111\savePic");
            Console.WriteLine($"======保存图片 {result}");
        }

        private void WebSocketDeal_OnScrollDataReceived(long clientId, ScrollData obj)
        {
            if (_lastScrollTime >= obj.Timestamp) return;
            _lastScrollTime = obj.Timestamp;
            long unixTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            //Console.WriteLine($"======={_lastScrollTime}    {unixTimestamp}    {unixTimestamp - _lastScrollTime}");
            _clientId = clientId; //先在这里记录 clientId 验证可行性
            HandleScrollMessage(obj.ScrollX, obj.ScrollY);
        }

        private void HandleScrollMessage(double scrollX, double scrollY)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    _translateTransform.X = -scrollX;
                    _translateTransform.Y = -scrollY;
                });
                //LogMessage($"滚动同步: X={scrollX:F1}, Y={scrollY:F1}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"滚动处理错误: {ex.Message}");
            }
        }

        private void MessageButton_Click(object sender, RoutedEventArgs e)
        {
            // 发送简单消息到所有WebSocket客户端
            var message = new
            {
                clientId = _clientId,
                type = "test_message",
                data = "Hello from C# Server!",
                timestamp = DateTime.Now.ToString()
            };
            
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(message);
            WebSocketDeal.SendToAll(json);
            
            Console.WriteLine("消息已发送: " + json);
        }
    }
}
