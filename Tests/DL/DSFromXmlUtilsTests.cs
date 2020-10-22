using Microsoft.VisualStudio.TestTools.UnitTesting;
using System; //Exception
using System.Collections.Specialized; // NameValueCollection
using System.IO; //File

namespace CurrencyConversion.Tests
{
    [TestClass()]
    public class DSFromXmlUtilsTests
    {

        NameValueCollection _dsConfig = new NameValueCollection {
            { "DsUrl", "https://treasury.un.org/operationalrates/xsql2XML.php" },
            { "LocalDsPath",  @".\currencies.xml" },
            { "DsType", "xml" },
            { "IterElemName", "UN_OPERATIONAL_RATES" },
            { "KeyElemName", "f_curr_code" },
            { "ValElemName", "rate" } };


        [TestMethod()]
        public void DSFromXmlUtilsTest()
        {
            try
            {
                IDSUtils<string, float> db = new DSFromXmlUtils<string, float>(_dsConfig);
                Assert.IsTrue(File.Exists(_dsConfig["LocalDsPath"]));
            }
            catch (Exception e)
            {
                Assert.Fail("Failed to create XmlUtils: " + e.Message);
            }
        }

        [TestMethod()]
        public void DSFromXmlUtilsBadUrlTest()
        {
            bool exceptionCaught = false;
            NameValueCollection _dsConfigBadUrl = _dsConfig;
            _dsConfigBadUrl["DsUrl"] = "https://treasury.un.org/operationalrates";
            try
            {
                IDSUtils<string, float> db = new DSFromXmlUtils<string, float>(_dsConfigBadUrl);
            }
            catch
            {
                exceptionCaught = true;
            }
            Assert.IsTrue(exceptionCaught);
        }

        [TestMethod()]
        public void UpdateDSTest()
        {
            try
            {
                IDSUtils<string, float> db = new DSFromXmlUtils<string, float>(_dsConfig);
                bool res = db.UpdateDS(_dsConfig);
                Assert.IsTrue(res = true && db.GetVal("USD") == 1);
            }
            catch (Exception e)
            {
                Assert.Fail("Failed to update XmlUtils: " + e.Message);
            }
        }

        [TestMethod()]
        public void GetValTest()
        {
            try
            {
                IDSUtils<string, float> db = new DSFromXmlUtils<string, float>(_dsConfig);
                //Given currency rates db is according to 1 USD, so USD rate has to be 1
                Assert.IsTrue(db.GetVal("USD") == 1);
            }
            catch (Exception e)
            {
                Assert.Fail("Failed to create XmlUtils: " + e.Message);
            }
        }

        public void GetBadValTest()
        {
            bool exceptionCaught = false;
            IDSUtils<string, float> db = new DSFromXmlUtils<string, float>(_dsConfig);
            try
            {
                //Given currency rates db is according to 1 USD, so USD rate has to be 1
                Assert.IsTrue(db.GetVal("TXT") == 1);
            }
            catch
            {
                exceptionCaught = true;
            }
            Assert.IsTrue(exceptionCaught);
        }


    }
}