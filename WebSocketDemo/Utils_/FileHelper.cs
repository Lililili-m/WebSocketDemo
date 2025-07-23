using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketDemo
{
    public static class FileHelper
    {
        /// <summary>
        /// 根据Base64数据自动确定文件类型并保存
        /// </summary>
        public static bool SaveBase64WithAutoExtension(string base64String, string outputPathWithoutExtension)
        {
            try
            {
                string base64Data = base64String.Contains(",")
                    ? base64String.Split(',')[1]
                    : base64String;

                byte[] imageBytes = Convert.FromBase64String(base64Data);

                // 获取图片格式
                string extension = GetImageExtension(imageBytes);
                if (string.IsNullOrEmpty(extension))
                {
                    extension = ".png"; // 默认使用png格式
                }

                string finalPath = outputPathWithoutExtension + extension;

                File.WriteAllBytes(finalPath, imageBytes);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存图片失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 根据图片字节数组确定图片类型
        /// </summary>
        private static string GetImageExtension(byte[] imageBytes)
        {
            if (imageBytes.Length < 4) return null;

            // PNG: 89 50 4E 47
            if (imageBytes[0] == 0x89 && imageBytes[1] == 0x50 && imageBytes[2] == 0x4E && imageBytes[3] == 0x47)
                return ".png";

            // JPEG: FF D8 FF
            if (imageBytes[0] == 0xFF && imageBytes[1] == 0xD8 && imageBytes[2] == 0xFF)
                return ".jpg";

            // GIF: 47 49 46 38
            if (imageBytes[0] == 0x47 && imageBytes[1] == 0x49 && imageBytes[2] == 0x46 && imageBytes[3] == 0x38)
                return ".gif";

            // BMP: 42 4D
            if (imageBytes[0] == 0x42 && imageBytes[1] == 0x4D)
                return ".bmp";

            return null;
        }
    }
}
