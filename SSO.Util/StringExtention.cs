using MongoDB.Bson;
using SSO.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SSO.Util
{
    public static class StringExtention
    {
        public static string ToMD5(this string str)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] md5bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in md5bytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
        public static string GetSha256(this string str)
        {
            byte[] SHA256Data = Encoding.UTF8.GetBytes(str);
            SHA256Managed Sha256 = new SHA256Managed();
            byte[] by = Sha256.ComputeHash(SHA256Data);
            return BitConverter.ToString(by).Replace("-", "").ToLower();
        }
        public static string ToStr(this Stream stream)
        {
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            return reader.ReadToEnd();
        }
        public static Stream ToStream(this string str)
        {
            byte[] array = Encoding.UTF8.GetBytes(str);
            MemoryStream stream = new MemoryStream(array);
            stream.Position = 0;
            return stream;
        }
        public static byte[] ToBytes(this Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }
        public static string GetMachineName(this string str)
        {
            Match match = Regex.Match(str, @"\\\\(.+?)\\");
            return match.Groups[1].Value;
        }
        public static List<ObjectId> GetTsIds(this string str)
        {
            List<ObjectId> tsIds = new List<ObjectId>();
            var matchs = Regex.Matches(str, "(\\w+).ts");
            for (var i = 0; i < matchs.Count; i++)
            {
                Match match = matchs[i];
                if (match.Success) tsIds.Add(ObjectId.Parse(match.Groups[1].Value));
            }
            return tsIds;
        }
        public static string RemoveHtml(this string str)
        {
            return Regex.Replace(str, "<[^>]+>", "").Replace("&[^;]+;", "");
        }
        /// <summary>
        /// 字符串转UTF8字节数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] StrToBuffer(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }
        public static string BufferToStr(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }
        /// <summary>
        /// string 转成 url 安全的base64 编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StrToBase64(this string str)
        {
            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
            return Base64SecureURL.Encode(base64);
        }
        /// <summary>
        /// url 安全的base64 编码 转 string
        /// </summary>
        /// <returns></returns>
        public static string Base64ToStr(this string base64)
        {
            base64 = Base64SecureURL.Decode(base64);
            return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        }
        /// <summary>
        /// base64字符串转UTF8字节数组
        /// </summary>
        /// <param name="base64Str"></param>
        /// <returns></returns>
        public static byte[] Base64StrToBuffer(this string base64Str)
        {
            return Convert.FromBase64String(base64Str);
        }
        public static string GetFileName(this string str)
        {
            var index = str.LastIndexOf("\\");
            return str.Substring(index + 1);
        }

        /// <summary>
        /// 获取网站的title和icon
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static WebsiteMeta GetWebSiteMeta(this string url)
        {
            Regex linkLine = new Regex("<link[^>]+?>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex iconTagExists = new Regex("rel=\"?.*icon\"?", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex iconHref = new Regex("\\shref=\"(.*?)\"", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex titleLine = new Regex("<title>(.+)</title>", RegexOptions.IgnoreCase);
            WebsiteMeta result = new WebsiteMeta();
            try
            {
                string responseText = new HttpRequestHelper().Get(url, null).Result;
                MatchCollection matchs = linkLine.Matches(responseText);
                string icon = "";
                for (var i = 0; i < matchs.Count; i++)
                {
                    var match = matchs[i];
                    if (match.Success)
                    {
                        string line = match.Value;
                        if (iconTagExists.IsMatch(line))
                        {
                            Match href = iconHref.Match(line);
                            if (href.Success)
                            {
                                string iconUrl = href.Groups[1].Value;
                                icon = iconUrl.Contains(@"//") ? iconUrl : url + "/" + iconUrl;
                            }
                        }
                    }
                }
                result.IconUrl = icon;
                Match matchTitle = titleLine.Match(responseText);
                string title = "";
                if (matchTitle.Success)
                {
                    title = matchTitle.Groups[1].Value;
                }
                result.Title = title;
                result.Url = url;
            }
            catch (Exception ex)
            {
                result.IconUrl = "";
                result.Title = new Uri(url).Host;
                result.Url = url;
            }
            return result;
        }
    }
}
