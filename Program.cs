using CurrencyConversion.BL; // IFileParser, CurrencyConversionFileParser
using System;
using System.Collections.Generic; //List
using System.Collections.Specialized; //NameValueCollection
using System.Configuration; //ConfigurationManager
using System.Linq;

namespace CurrencyConversion
{
    class Program
    {

        static private void PrintListsVals<T>(List<List<T>> lists)
        {
            //Print converted currencies
            foreach (var sums in lists)
            {
                foreach (var val in sums)
                {
                    Console.WriteLine("{0}", val);
                }
            }
        }

        public static void ConvertCurrenciesFromGivenFile(IFileParser fileParser, CurrencyConverter currencyConverter, string fileName)
        {
            List<RequestedCurrencies> requestedCurrencies = fileParser.ParseFile(fileName);
            List<List<double>> sumsInRequestedCurrency = currencyConverter.ConvertCurrencies(requestedCurrencies);
            PrintListsVals<double>(sumsInRequestedCurrency);
        }

        static void Main(string[] args)
        {
            if (args.Count() == 0)
            {
                throw new ArgumentNullException("fileName (args[0])", "A currency conversion file must be provided");
            }
            //Load config
            NameValueCollection dsConfig = ConfigurationManager.GetSection("currenciesConfig") as NameValueCollection;

            CurrencyConverter currencyConverter = new CurrencyConverter(dsConfig);
            IFileParser fileParser = new CurrencyConversionFileParser();
            ConvertCurrenciesFromGivenFile(fileParser, currencyConverter, args[0]);
        }

    }
}
