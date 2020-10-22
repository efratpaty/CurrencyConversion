using Microsoft.VisualStudio.TestTools.UnitTesting;
using System; //Exception
using System.Collections.Generic;
using System.IO; //Path

namespace CurrencyConversion.BL.Tests
{
    [TestClass()]
    public class CurrencyConversionFileParserTests
    {
        string _testFile;
        public CurrencyConversionFileParserTests() //Create temp file for test purposes
        {
            try
            {
                _testFile = Path.GetTempFileName();
                StreamWriter streamWriter = File.AppendText(_testFile);
                streamWriter.WriteLine("UsD");
                streamWriter.WriteLine("iLs");
                streamWriter.WriteLine("1");
                streamWriter.WriteLine("10");
                streamWriter.WriteLine("19.5");
                streamWriter.WriteLine("17.4");
                streamWriter.Flush();
                streamWriter.Close();
            }
            catch (Exception e)
            {
                Assert.Fail("Failed to create tmp file for testing file parsing, exception message: {0}", e.Message);
            }
        }

        [TestMethod()]
        public void ParseFileTest()
        {
            IFileParser fileParser = new CurrencyConversionFileParser();
            List<RequestedCurrencies> requestedCurrencies = fileParser.ParseFile(_testFile);
            Assert.IsTrue(requestedCurrencies[0]._givenCurrency == "USD" &&
                requestedCurrencies[0]._expectedCurrency == "ILS" &&
                requestedCurrencies[0]._sums[0] == 1 &&
                requestedCurrencies[0]._sums[1] == 10 &&
                requestedCurrencies[0]._sums[2] == 19.5 &&
                requestedCurrencies[0]._sums[3] == 17.4);
        }
    }
}