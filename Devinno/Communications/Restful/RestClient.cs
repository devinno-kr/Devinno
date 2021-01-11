using Devinno.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Communications.Restful
{
    #region class : MediaTypeConst
    public class MediaTypeConst
    {
        public const string APPLICATION_ATOM_XML = "application/atom+xml";
        public const string APPLICATION_ATOM_XML_TYPE = "application/atom+xml";
        public const string APPLICATION_FORM_URLENCODED = "application/x-www-form-urlencoded";
        public const string APPLICATION_FORM_URLENCODED_TYPE = "application/x-www-form-urlencoded";
        public const string APPLICATION_JSON = "application/json";
        public const string APPLICATION_JSON_TYPE = "application/json";
        public const string APPLICATION_OCTET_STREAM = "application/octet-stream";
        public const string APPLICATION_OCTET_STREAM_TYPE = "application/octet-stream";
        public const string APPLICATION_SVG_XML = "application/svg+xml";
        public const string APPLICATION_SVG_XML_TYPE = "application/svg+xml";
        public const string APPLICATION_XHTML_XML = "application/xhtml+xml";
        public const string APPLICATION_XHTML_XML_TYPE = "application/xhtml+xml";
        public const string APPLICATION_XML = "application/xml";
        public const string APPLICATION_XML_TYPE = "application/xml";
        public const string MEDIA_TYPE_WILDCARD = "*";
        public const string MULTIPART_FORM_DATA = "multipart/form-data";
        public const string MULTIPART_FORM_DATA_TYPE = "multipart/form-data";
        public const string TEXT_HTML = "text/html";
        public const string TEXT_HTML_TYPE = "text/html";
        public const string TEXT_PLAIN = "text/plain";
        public const string TEXT_PLAIN_TYPE = "text/plain";
        public const string TEXT_XML = "text/xml";
        public const string TEXT_XML_TYPE = "text/xml";
        public const string WILDCARD = "*/*";
        public const string WILDCARD_TYPE = "*/*";
    }
    #endregion
    #region class : JsonContent
    public class JsonContent : StringContent
    {
        public JsonContent(object obj) : base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
        {
        }
    }
    #endregion

    public class RestClient
    {
        #region Properties
        public string BaseAddress { get; set; } = "http://127.0.0.1";
        public int Timeout { get; set; } = 3000;
        public string MediaType { get; set; } = MediaTypeConst.APPLICATION_JSON;
        public List<JsonConverter> JsonConverter { get; private set; } = new List<JsonConverter>();
        #endregion

        #region Constructor
        public RestClient()
        {
        }
        #endregion

        #region Method
        #region Get
        #region STRING  Get(Url)
        public async Task<string> Get(string url)
        {
            string ret = null;
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(Timeout);
                client.BaseAddress = new Uri(BaseAddress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));

                var result = await client.GetAsync(url);
                using (HttpResponseMessage response = result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var v = response.Content.ReadAsStringAsync().Result;
                        ret = v;
                    }
                }
            }
            return ret;
        }
        #endregion
        #region RT      Get(Url)
        public async Task<RT> Get<RT>(string url)
        {
            RT ret = default(RT);
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(Timeout);
                client.BaseAddress = new Uri(BaseAddress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));

                var result = await client.GetAsync(url);
                using (HttpResponseMessage response = result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var v = response.Content.ReadAsStringAsync().Result;
                        ret = Serialize.JsonDeserialize<RT>(v);
                    }
                }
            }
            return ret;
        }
        #endregion
        #region RT      Get(Url, Id)
        public async Task<RT> Get<RT>(string url, string id)
        {
            RT ret = default(RT);
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(Timeout);
                client.BaseAddress = new Uri(BaseAddress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));

                var result = await client.GetAsync(url + "/" + id);
                using (HttpResponseMessage response = result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var v = response.Content.ReadAsStringAsync().Result;
                        ret = Serialize.JsonDeserialize<RT>(v);
                    }
                }
            }
            return ret;
        }
        #endregion
        #endregion

        #region Post
        #region STRING  Post(url, data)
        public async Task<string> Post(string url, string data)
        {
            string ret = null;
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(Timeout);
                client.BaseAddress = new Uri(BaseAddress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));

                var c = new System.Net.Http.StringContent(data, Encoding.UTF8, MediaType);

                var result = await client.PostAsync(url, c);
                using (HttpResponseMessage response = result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var v = response.Content.ReadAsStringAsync().Result;
                        ret = v;
                    }
                }
            }
            return ret;
        }
        #endregion
        #region BOOL    Post(url, data)
        public async Task<bool> Post<T>(string url, T data)
        {
            bool ret = false;
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(Timeout);
                client.BaseAddress = new Uri(BaseAddress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));

                var result = await client.PostAsync(url, new JsonContent(data));
                using (HttpResponseMessage response = result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ret = true;
                    }
                }
            }
            return ret;
        }
        #endregion
        #region RT      Post(url, data)
        public async Task<RT> Post<RT, T>(string url, T data)
        {
            RT ret = default(RT);
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(Timeout);
                client.BaseAddress = new Uri(BaseAddress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));

                var result = await client.PostAsync(url, new JsonContent(data));
                using (HttpResponseMessage response = result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var v = response.Content.ReadAsStringAsync().Result;
                        ret = Serialize.JsonDeserialize<RT>(v);
                    }
                }
            }
            return ret;
        }
        #endregion
        #endregion

        #region Put
        #region STRING  Put(url, data)
        public async Task<string> Put(string url, string data)
        {
            string ret = null;
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(Timeout);
                client.BaseAddress = new Uri(BaseAddress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));

                var c = new StringContent(data, Encoding.UTF8, MediaType);

                var result = await client.PutAsync(url, c);
                using (HttpResponseMessage response = result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var v = response.Content.ReadAsStringAsync().Result;
                        ret = v;
                    }
                }
            }
            return ret;
        }
        #endregion
        #region BOOL    Put(url, id, data)
        public async Task<bool> Put<T>(string url, string id, T data)
        {
            bool ret = false;
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(Timeout);
                client.BaseAddress = new Uri(BaseAddress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));

                var result = await client.PutAsync(url + "/" + id, new JsonContent(data));
                using (HttpResponseMessage response = result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ret = true;
                    }
                }
            }
            return ret;
        }
        #endregion
        #region RT      Put(url, id, data)
        public async Task<RT> Put<RT, T>(string url, string id, T data)
        {
            RT ret = default(RT);
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(Timeout);
                client.BaseAddress = new Uri(BaseAddress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));

                var result = await client.PutAsync(url + "/" + id, new JsonContent(data));
                using (HttpResponseMessage response = result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var v = response.Content.ReadAsStringAsync().Result;
                        ret = Serialize.JsonDeserialize<RT>(v);
                    }
                }
            }
            return ret;
        }
        #endregion
        #endregion

        #region Delete
        #region STRING  Delete(url)
        public async Task<string> Delete<RT>(string url)
        {
            string ret = null;
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(Timeout);
                client.BaseAddress = new Uri(BaseAddress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));

                var result = await client.DeleteAsync(url);
                using (HttpResponseMessage response = result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var v = response.Content.ReadAsStringAsync().Result;
                        ret = v;
                    }
                }
            }
            return ret;
        }
        #endregion
        #region BOOL    Delete(url, id)
        public async Task<bool> Delete(string url, string id)
        {
            bool ret = false;
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(Timeout);
                client.BaseAddress = new Uri(BaseAddress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));

                var result = await client.DeleteAsync(url + "/" + id);
                using (HttpResponseMessage response = result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ret = true;
                    }
                }
            }
            return ret;
        }
        #endregion
        #region RT      Delete(url, id)
        public async Task<RT> Delete<RT>(string url, string id)
        {
            RT ret = default(RT);
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(Timeout);
                client.BaseAddress = new Uri(BaseAddress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));

                var result = await client.DeleteAsync(url + "/" + id);
                using (HttpResponseMessage response = result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var v = response.Content.ReadAsStringAsync().Result;
                        ret = Serialize.JsonDeserialize<RT>(v);
                    }
                }
            }
            return ret;
        }
        #endregion
        #endregion
        #endregion
    }

    
}
