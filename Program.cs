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

        static private void PrintListsVals(List<List<float>> lists)
        {
            //Print converted currencies
            foreach (List<float> sums in lists)
            {
                foreach (float val in sums)
                {
                    Console.WriteLine("{0}", val);
                }
            }
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
            List<RequestedCurrencies> requestedCurrencies = fileParser.ParseFile(args[0]);
            List<List<float>> sumsInRequestedCurrency = currencyConverter.ConvertCurrencies(requestedCurrencies);
            PrintListsVals(sumsInRequestedCurrency);
            Console.ReadLine();
        }

    }
}
