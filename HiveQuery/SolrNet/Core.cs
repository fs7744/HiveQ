using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SolrNet.Impl
{
    public class CoreIndexResult
    {
        /// <summary>
        /// Gets or sets the number of searchable documents in the index.
        /// </summary>
        /// <remarks>Represents the numDocs value in a Solr status result.</remarks>
        public long SearchableDocumentCount { get; set; }

        /// <summary>
        /// Gets or sets the total number of documents including logically deleted documents that have not been removed from the index yet.
        /// </summary>
        /// <remarks>Represents the maxDoc value in a Solr status result.</remarks>
        public long TotalDocumentCount { get; set; }

        /// <summary>
        /// Gets or sets the Index version.
        /// </summary>
        public long Version { get; set; }

        /// <summary>
        /// The number of Segments that exist for the index.
        /// </summary>
        public int SegmentCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the index is current.
        /// </summary>
        public bool IsCurrent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the index is optimized.
        /// </summary>
        public bool IsOptimized { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the instance has deletions.
        /// </summary>
        /// <remarks>
        /// If an index has deletions, it may need to undergo a Optimization in order to fully remove any deleted documents.
        /// </remarks>
        public bool HasDeletions { get; set; }

        /// <summary>
        /// Gets or sets the directory implementation being used by Lucene.
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// Gets or sets the date the index was last modified.
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// The physical Size of the Index.
        /// </summary>
        /// <remarks>
        /// Appears in Solr R4.0 and above.
        /// </remarks>
        public string Size { get; set; }

        /// <summary>
        /// the physical Size of the Index in bytes.
        /// </summary>
        /// <remarks>
        /// Appears in Solr R4.0 and above.
        /// </remarks>
        public long SizeInBytes { get; set; }
    }

    public class CoreResult
    {
        /// <summary>
        /// The name of the Core.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True if this is the default core. Otherwise false.
        /// Note that this is only available on Solr 4+.
        /// For older versions this will always be false.
        /// </summary>
        public bool IsDefaultCore { get; set; }

        /// <summary>
        /// The directory the Solr instance is located at.
        /// </summary>
        public string InstanceDir { get; set; }

        /// <summary>
        /// The directory all data for this Core is located at.
        /// </summary>
        public string DataDir { get; set; }

        /// <summary>
        /// The time when this Core was last started.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// The number of milliseconds this Core has been live.
        /// </summary>
        public long Uptime { get; set; }

        /// <summary>
        /// Gets or sets the Core's Index Result.
        /// </summary>
        public CoreIndexResult Index { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreResult"/> class.
        /// </summary>
        public CoreResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreResult"/> class.
        /// </summary>
        /// <param name="coreName">Name of the core.</param>
        public CoreResult(string coreName)
        {
            Name = coreName;
        }
    }

    /// <summary>
    /// Parses a Solr Core Result from a Core Status command.
    /// </summary>
    public interface ISolrStatusResponseParser
    {
        /// <summary>
        /// Parses the list of returned <see cref="CoreResult"/> instances from the response returned.
        /// </summary>
        /// <param name="xml">The XML Document to parse.</param>
        /// <returns>
        /// The list of results.
        /// </returns>
        List<CoreResult> Parse(XDocument xml);
    }

    public class SolrStatusResponseParser : ISolrStatusResponseParser
    {
        /// <summary>
        /// Parses the results of a Core Status command.
        /// </summary>
        /// <param name="xml">The XML Document to parse.</param>
        /// <returns></returns>
        public List<CoreResult> Parse(XDocument xml)
        {
            var statusNode = xml.XPathSelectElement("response/lst[@name='status']");
            if (statusNode == null || !statusNode.HasElements)
                return new List<CoreResult>();

            var results = statusNode.Elements().Select(ParseCore).ToList();
            return results;
        }

        /// <summary>
        /// Parses the details of a <see cref="CoreResult"/> that could be parsed.
        /// </summary>
        /// <param name="node">The node to inspect.</param>
        /// <returns>The parsed <see cref="CoreResult"/>.</returns>
        private static CoreResult ParseCore(XElement node)
        {
            var core = new CoreResult();

            foreach (var propNode in node.Elements())
            {
                var nodeValue = propNode.Value;

                switch (propNode.Attribute("name").Value.ToLower())
                {
                    case "name":
                        if (!string.IsNullOrEmpty(nodeValue))
                            core.Name = nodeValue;
                        break;
                    case "isdefaultcore":
                        core.IsDefaultCore = bool.Parse(nodeValue);
                        break;
                    case "instancedir":
                        if (!string.IsNullOrEmpty(nodeValue))
                            core.InstanceDir = nodeValue;
                        break;
                    case "datadir":
                        if (!string.IsNullOrEmpty(nodeValue))
                            core.DataDir = nodeValue;
                        break;
                    case "starttime":
                        if (!string.IsNullOrEmpty(nodeValue))
                            core.StartTime = DateTime.Parse(nodeValue);
                        break;
                    case "uptime":
                        if (!string.IsNullOrEmpty(nodeValue))
                            core.Uptime = long.Parse(nodeValue);
                        break;
                    case "index":
                        // Parse all Index responses.
                        core.Index = ParseCoreIndex(propNode);
                        break;
                }
            }

            return core;
        }

        /// <summary>
        /// Parses the details of the index for a Core.
        /// </summary>
        /// <param name="node">The node to parse.</param>
        /// <returns>The <see cref="CoreIndexResult"/> that was parsed.</returns>
        private static CoreIndexResult ParseCoreIndex(XElement node)
        {
            var coreIndex = new CoreIndexResult();

            foreach (var indexNode in node.Elements())
            {
                var nodeValue = indexNode.Value;

                switch (indexNode.Attribute("name").Value.ToLower())
                {
                    case "numdocs":
                        if (!string.IsNullOrEmpty(nodeValue))
                            coreIndex.SearchableDocumentCount = long.Parse(nodeValue);
                        break;
                    case "maxdoc":
                        if (!string.IsNullOrEmpty(nodeValue))
                            coreIndex.TotalDocumentCount = long.Parse(nodeValue);
                        break;
                    case "version":
                        if (!string.IsNullOrEmpty(nodeValue))
                            coreIndex.Version = long.Parse(nodeValue);
                        break;
                    case "segmentcount":
                        if (!string.IsNullOrEmpty(nodeValue))
                            coreIndex.SegmentCount = int.Parse(nodeValue);
                        break;
                    case "current":
                        if (!string.IsNullOrEmpty(nodeValue))
                            coreIndex.IsCurrent = bool.Parse(nodeValue);
                        break;
                    case "hasdeletions":
                        if (!string.IsNullOrEmpty(nodeValue))
                            coreIndex.HasDeletions = bool.Parse(nodeValue);
                        break;
                    case "optimized":
                        if (!string.IsNullOrEmpty(nodeValue))
                            coreIndex.IsOptimized = bool.Parse(nodeValue);
                        break;
                    case "directory":
                        if (!string.IsNullOrEmpty(nodeValue))
                            coreIndex.Directory = nodeValue;
                        break;
                    case "lastmodified":
                        if (!string.IsNullOrEmpty(nodeValue))
                            coreIndex.LastModified = DateTime.Parse(nodeValue);
                        break;
                    case "sizeinbytes":
                        if (!string.IsNullOrEmpty(nodeValue))
                            coreIndex.SizeInBytes = long.Parse(nodeValue);
                        break;
                    case "size":
                        if (!string.IsNullOrEmpty(nodeValue))
                            coreIndex.Size = nodeValue;
                        break;
                }
            }

            return coreIndex;
        }
    }

    public class CoreCommand : ISolrCommand
    {
        /// <summary>
        /// List of Parameters that will be sent to the /admin/cores handler.
        /// </summary>
        protected readonly List<KeyValuePair<string, string>> Parameters = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// Executes a Core command
        /// </summary>
        /// <param name="connection">The SolrConnection to use.</param>
        /// <returns>The results of the Command.</returns>
        public string Execute(ISolrConnection connection)
        {
            return connection.Get("/admin/cores", Parameters.ToArray());
        }

        /// <summary>
        /// Adds the specified parameter to the current command.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected void AddParameter(string key, string value)
        {
            Parameters.Add(new KeyValuePair<string, string>(key, value));
        }

        public IEnumerable<KeyValuePair<string, string>> GetParameters()
        {
            return Parameters;
        }
    }

    public class StatusCommand : CoreCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusCommand"/> class.
        /// </summary>
        public StatusCommand()
        {
            AddParameter("action", "STATUS");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusCommand"/> class.
        /// </summary>
        /// <param name="coreName">Name of the core to get status for.</param>
        public StatusCommand(string coreName)
            : this()
        {
            if (!string.IsNullOrEmpty(coreName))
                AddParameter("core", coreName);
        }
    }

    public class SolrCoreAdmin 
    {
        private readonly ISolrConnection connection;
        private readonly ISolrHeaderResponseParser headerParser;
        private readonly ISolrStatusResponseParser resultParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="SolrCoreAdmin"/> class.
        /// </summary>
        public SolrCoreAdmin(ISolrConnection connection, ISolrHeaderResponseParser headerParser, ISolrStatusResponseParser resultParser)
        {
            this.connection = connection;
            this.headerParser = headerParser;
            this.resultParser = resultParser;
        }

        /// <summary>
        /// The STATUS action returns the status of all running cores.
        /// </summary>
        public List<CoreResult> Status()
        {
            return ParseStatusResponse(Send(new StatusCommand()));
        }


        /// <summary>
        /// Sends a command and parses the ResponseHeader.
        /// </summary>
        /// <param name="cmd">The CMD.</param>
        /// <returns></returns>
        public ResponseHeader SendAndParseHeader(ISolrCommand cmd)
        {
            var r = Send(cmd);
            var xml = XDocument.Parse(r);
            return headerParser.Parse(xml);
        }

        /// <summary>
        /// Sends the specified Command to Solr.
        /// </summary>
        /// <param name="command">The Command to send.</param>
        /// <returns></returns>
        public string Send(ISolrCommand command)
        {
            return command.Execute(connection);
        }

        /// <summary>
        /// Parses the status response.
        /// </summary>
        /// <param name="responseXml">The response XML.</param>
        /// <returns></returns>
        protected List<CoreResult> ParseStatusResponse(string responseXml)
        {
            var xml = XDocument.Parse(responseXml);
            return resultParser.Parse(xml);
        }
    }
}