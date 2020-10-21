using System;
using System.IO; // File
using System.Xml.Linq; // Xelement
using System.Collections.Concurrent; //ConcurrentDictionary
using System.ComponentModel; //TypeDescriptor
using System.Collections.Specialized; //NameValueCollection
using System.Collections.Generic; //KeyNotFoundException

namespace CurrencyConversion
{
    public class DSFromXmlUtils<TKey, TVal> : IDSUtils<TKey, TVal>
    {
        private string _xmlPath;
        private string _dsUrl;
        private ConcurrentDictionary<TKey, TVal> _db;

        private void DownloadXml(string url)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                client.DownloadFile(url, _xmlPath);
            }
        }

        public DSFromXmlUtils(NameValueCollection dsConfig, DateTime? acceptableCreationDate = null)
        {
            _xmlPath = dsConfig["LocalDsPath"];
            _dsUrl = dsConfig["DsUrl"];
            acceptableCreationDate ??= DateTime.MinValue;
            //if file doesn't exist, or it was created before acceptableCreationDate, download the file from the configured url
            if (!File.Exists(_xmlPath) || (File.GetCreationTime(_xmlPath).Date < DateTime.Today))
            {
                DownloadXml(_dsUrl);
            }
            _db = DeserializeDSToDict(dsConfig);
        }


        /// <summary>
        /// Deserialize xml in format of:
        /// <root>
        ///     <iterElement>
        ///         <keyElement> key <keyElement/>
        ///         <valElement> val <valElement/>
        ///         ......
        ///     <iterElement/>
        ///     <iterElement> .... <iterElement/>
        /// </root>
        /// to dictionary of {"key": val ....} pairs
        /// </summary>

        private ConcurrentDictionary<TKey, TVal> DeserializeXmlToDict(string iterElemName, string keyElemName, string valElemName)
        {
            XElement xml = XElement.Load(_xmlPath);
            var tKeyConverter = TypeDescriptor.GetConverter(typeof(TKey));
            var tValConverter = TypeDescriptor.GetConverter(typeof(TVal));

            ConcurrentDictionary<TKey, TVal> data = xml.Elements(iterElemName).ToConcurrentDictionaryIgnoreDups(
                el => (TKey)(tKeyConverter.ConvertFromInvariantString(el.Element(keyElemName).Value.TrimEnd())),
                el => (TVal)(tValConverter.ConvertFromInvariantString(el.Element(valElemName).Value.TrimEnd()))
            );
            return data;
        }

        private ConcurrentDictionary<TKey, TVal> DeserializeDSToDict(NameValueCollection dsConfig)
        {
            return DeserializeXmlToDict(dsConfig["IterElemName"], dsConfig["KeyElemName"], dsConfig["ValElemName"]);
        }

        public TVal GetVal(TKey key)
        {
            if (!_db.ContainsKey(key))
            {
                throw new KeyNotFoundException(key.ToString());
            }
            return _db[key];
        }

        //Update data set from ds url 
        public bool UpdateDS(NameValueCollection dsConfig)
        {
            DateTime preUpDate = File.GetCreationTime(_xmlPath);
            DownloadXml(_dsUrl);
            if (preUpDate == File.GetCreationTime(_xmlPath))//if file wasn't recreated
            {
                return false;
            }
            _db = DeserializeDSToDict(dsConfig);
            return true;
        }
    }
}
   
