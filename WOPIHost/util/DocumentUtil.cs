using Newtonsoft.Json;
using WOPIHost.model;

namespace WOPIHost.util
{
    public class DocumentUtil
    {
        private HttpUtil httpUtil = new HttpUtil();

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="uuid">文件Id</param>
        /// <returns></returns>
        public documentInfo GetInfo(string uuid)
        {
            var url = System.Configuration.ConfigurationManager.AppSettings["InfoURL"];
            var result = httpUtil.DoGet(url + $"?uuid={uuid}");
            var info = JsonConvert.DeserializeObject<documentInfo>(result);
            info.uuid = uuid;
            return info;
        }
    }
}