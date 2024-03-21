using System.IO;
using System.Net;

using JinLei.Extensions;

using Newtonsoft.Json.Linq;

namespace JinLei.Utilities;
public static partial class TranslateUtility
{
    /// <summary>
    /// 获取百度翻译译文
    /// </summary>
    public static string BaiduFanyi(string appId, string secretKey, string queryString, string from = "auto", string to = "en")
    {
        var url = "http://api.fanyi.baidu.com/api/trans/vip/translate";
        var salt = new Random().Next(0, 1000).ToString();
        var sign = (appId + queryString + salt + secretKey).GetMD5();

        url = $"{url}?appid={appId}&q={WebUtility.UrlEncode(queryString)}&from={from}&to={to}&salt={salt}&sign={sign}";

        var result = WebClientUtility.WebClient.DownloadString(url);

        return JToken.Parse(result)?["trans_result"]?[0]?["dst"]?.Value<string>();
    }

    /// <summary>
    /// 翻译语言资源文件(.resx)
    /// </summary>
    /// <returns>已翻译资源</returns>
    public static Dictionary<string, string> TranslateFromResx(FileInfo sourceFile, string appId, string secretKey, string from = "auto", string to = "en", string newFileName = "已翻译.resx")
    {
        sourceFile ??= ConsoleUtility.ReadFilePath();

        var translated = ResxUtility.ReadToDictionary<string>(new(sourceFile.FullName)).ToDictionary(t => t.Key, t => BaiduFanyi(appId, secretKey, t.Value, from, to));

        if(string.IsNullOrWhiteSpace(newFileName) == false)
        {
            var targetFile = new FileInfo(Path.Combine(sourceFile.DirectoryName, newFileName)).GetPathNonAlreadyExists();
            ResxUtility.WriteFromItems(new(targetFile.FullName), translated);
        }

        return translated;
    }
}
