using System.Net;
using System.Text;

using JinLei.Extensions;

namespace JinLei.Utilities;

public partial class WebClientUtility
{
    public const string FirefoxUA = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/109.0";

    public static WebClient NewWebClient => new WebClient() { Encoding = Encoding.UTF8 }.Do(t => t.Headers.Add(HttpRequestHeader.UserAgent, FirefoxUA));

    public static WebClient WebClient { get; } = NewWebClient;
}
