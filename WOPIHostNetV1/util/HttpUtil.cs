using System.Net.Http;
using System.Text;
using WOPIHostNetV1.model;

namespace WOPIHostNetV1.util
{
    public class HttpUtil
    {
        /// <summary>
        /// 获取请求返回值
        /// </summary>
        /// <param name="url">url地址</param>
        /// <returns></returns>
        public string DoGet(string url)
        {
            using (var client = new HttpClient())
            {
                var resultBytes = client.GetByteArrayAsync(url).Result;
                return Encoding.UTF8.GetString(resultBytes);
            }
        }
    }
}