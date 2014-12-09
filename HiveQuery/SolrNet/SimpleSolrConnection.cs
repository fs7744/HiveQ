using HttpWebAdapters;
using HttpWebAdapters.Adapters;
using SolrNet;
using SolrNet.Exceptions;
using SolrNet.Impl;
using SolrNet.Impl.ResponseParsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace HiveQuery.SolrNet
{
    public class SimpleSolrConnection : SolrConnection
    {
        private string m_ServerURL;
        private readonly ISolrAbstractResponseParser<Dictionary<string, object>> resultParser;

        public SimpleSolrConnection(string serverURL)
            : base(serverURL)
        {
            m_ServerURL = serverURL;
            resultParser = new DefaultResponseParser<Dictionary<string, object>>(new SolrDictionaryDocumentResponseParser(Startup.Container.GetInstance<ISolrFieldParser>()));
        }

        private struct SolrResponse
        {
            public string ETag { get; private set; }

            public string Data { get; private set; }

            public SolrResponse(string eTag, string data)
                : this()
            {
                ETag = eTag;
                Data = data;
            }
        }

        private string ReadResponseToString(IHttpWebResponse response)
        {
            using (var responseStream = response.GetResponseStream())
            using (var reader = new StreamReader(responseStream, TryGetEncoding(response)))
            {
                return reader.ReadToEnd();
            }
        }

        private Encoding TryGetEncoding(IHttpWebResponse response)
        {
            try
            {
                return Encoding.GetEncoding(response.CharacterSet);
            }
            catch
            {
                return Encoding.UTF8;
            }
        }

        private SolrResponse GetResponse(IHttpWebRequest request)
        {
            using (var response = request.GetResponse())
            {
                var etag = response.Headers[HttpResponseHeader.ETag];
                var cacheControl = response.Headers[HttpResponseHeader.CacheControl];
                if (cacheControl != null && cacheControl.Contains("no-cache"))
                    etag = null; // avoid caching things marked as no-cache

                return new SolrResponse(etag, ReadResponseToString(response));
            }
        }

        public string Get(string relativeUrl)
        {
            var u = new UriBuilder(m_ServerURL + relativeUrl);

            var request = HttpWebRequestFactory.Create(u.Uri);
            request.Method = HttpWebRequestMethod.GET;
            request.KeepAlive = true;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            var cached = Cache[u.Uri.ToString()];
            if (cached != null)
            {
                request.Headers.Add(HttpRequestHeader.IfNoneMatch, cached.ETag);
            }
            if (Timeout > 0)
            {
                request.ReadWriteTimeout = Timeout;
                request.Timeout = Timeout;
            }
            try
            {
                var response = GetResponse(request);
                if (response.ETag != null)
                    Cache.Add(new SolrCacheEntity(u.Uri.ToString(), response.ETag, response.Data));
                return response.Data;
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    using (e.Response)
                    {
                        var r = new HttpWebResponseAdapter(e.Response);
                        if (r.StatusCode == HttpStatusCode.NotModified)
                        {
                            return cached.Data;
                        }
                        using (var s = e.Response.GetResponseStream())
                        using (var sr = new StreamReader(s))
                        {
                            throw new SolrConnectionException(sr.ReadToEnd(), e, u.Uri.ToString());
                        }
                    }
                }
                throw new SolrConnectionException(e, u.Uri.ToString());
            }
        }

        public SolrQueryResults<Dictionary<string, object>> Execute(string query)
        {
            var results = new SolrQueryResults<Dictionary<string, object>>();
            var r = Get(query);
            var xml = XDocument.Parse(r);
            resultParser.Parse(xml, results);
            return results;
        }
    }
}