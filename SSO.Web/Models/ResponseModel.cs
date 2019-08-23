using MongoDB.Bson;
using MongoDB.Bson.IO;
using System.Text;
using System.Web.Mvc;

namespace SSO.Web.Models
{
    public class ResponseModel<T> : ContentResult
    {
        public ResponseModel(ErrorCode code, T t, long count = 0)
        {
            if (t is string)
            {
                Content = "{\"code\":" + (int)code + ",\"message\":\"" + code.ToString() + "\",\"result\":\"" + t.ToString() + "\",\"count\":" + count + "}";
            }
            else
            {
                Content = "{\"code\":" + (int)code + ",\"message\":\"" + code.ToString() + "\",\"result\":" + t.ToJson(new JsonWriterSettings() { OutputMode = JsonOutputMode.Strict }) + ",\"count\":" + count + "}";
            }
            ContentEncoding = Encoding.UTF8;
            ContentType = "application/json";
        }

    }
}