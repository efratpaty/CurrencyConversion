using Microsoft.VisualStudio.TestTools.UnitTesting;
using System; //Exception
using System.Collections.Generic; //List
using System.Collections.Specialized; //NameValueCollection

namespace CurrencyConversion.BL.Tests
{
    [TestClass()]
    public class CurrencyConverterTests
    {
        NameValueCollection _dsConfig = new NameValueCollection {
            { "DsUrl", "https://treasury.un.org/operationalrates/xsql2XML.php" },
            { "LocalDsPath",  @".\currencies.xml" },
            { "DsType", "xml" },
            { "IterElemName", "UN_OPERATIONAL_RATES" },
            { "KeyElemName", "f_curr_code" },
            { "ValElemName", "rate" } };

        RequestedCurrencies _requestedCurrencies = new RequestedCurrencies()
        {
            _givenCurrency = "ILS",
            _expectedCurrency = "EUR",
            _sums = { 1, 10.5, 19.4 }
        };

        [TestMethod()]
        public void CurrencyConverterTest()
        {
            try
            {
                CurrencyConverter currencyConverter = new CurrencyConverter(_dsConfig);
            }
            catch (Exception e)
            {
                Assert.Fail("Failed to create currencyConverter, exception message: {0}", e.Message);
            }
        }

        [TestMethod()]
        public void ConvertCurrenciesTest()
        {
            try
            {
                CurrencyConverter currencyConverter = new CurrencyConverter(_dsConfig);
                List<double> convertedCurrencies = currencyConverter.ConvertCurrencies(_requestedCurrencies);
                double rate = convertedCurrencies[0];
                bool res = true;
                for (int i = 0; res && i < convertedCurrencies.Count; ++i)
                {
                    res &= ((convertedCurrencies[i] < _requestedCurrencies._sums[i]) &&
                         (Math.Round(convertedCurrencies[i], 2) == Math.Round(_requestedCurrencies._sums[i] * rate, 2)));
                }
                Assert.IsTrue(res);
            }
            catch (Exception e)
            {
                Assert.Fail("Failed to convert currencies, exception message: {0}", e.Message);
            }
        }

        [TestMethod()]
        public void ConvertUnknownCurrencyTest()
        {
            bool exceptionCaught = false;
            try
            {
                CurrencyConverter currencyConverter = new CurrencyConverter(_dsConfig);
                RequestedCurrencies requested = _requestedCurrencies;
                requested._expectedCurrency = "TXT";
                List<double> convertedCurrencies = currencyConverter.ConvertCurrencies(requested);
            }
            catch (KeyNotFoundException)
            {
                exceptionCaught = true;
            }
            Assert.IsTrue(exceptionCaught);
        }
        [TestMethod()]
        public void ConvertZeroCurrencyTest()
        {
            try
            {
                CurrencyConverter currencyConverter = new CurrencyConverter(_dsConfig);
                RequestedCurrencies requested = _requestedCurrencies;
                requested._sums[0] = 0;
                List<double> convertedCurrencies = currencyConverter.ConvertCurrencies(requested);
                Assert.IsTrue(convertedCurrencies[0] == 0);
            }
            catch (Exception e)
            {
                Assert.Fail("Failed to convert currencies, exception message: {0}", e.Message);
            }
        }

    }
}