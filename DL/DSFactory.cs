using System;
using System.Collections.Specialized; //NameValueCollection

namespace CurrencyConversion.DL
{
    public static class DSFactory<TKey, TVal>
    {
        public static IDSUtils<TKey, TVal> Create(NameValueCollection dsConfig, DateTime? acceptableCreationDate = null)
        {
            string type = dsConfig["DsType"].Trim().ToLower();
            switch (type)
            {
                case "xml":
                    return new DSFromXmlUtils<TKey, TVal>(dsConfig, acceptableCreationDate);
                default:
                    throw new ArgumentException("Unknown data source {0}. Known data source are <xml>" +
                        "Please change data source in App.config-currenciesConfig-DsType", type);
            }
        }
    }
}
