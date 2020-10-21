using System.Collections.Generic; //List
using System.IO; //File

namespace CurrencyConversion.BL
{
    public class CurrencyConversionFileParser : IFileParser
    {
        public List<RequestedCurrencies> ParseFile(string fileName)
        {
            List<RequestedCurrencies> lrc = new List<RequestedCurrencies>();
            RequestedCurrencies rc = new RequestedCurrencies();

            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(fileName);
            }

            using (StreamReader sr = File.OpenText(fileName))
            {
                rc._givenCurrency = sr.ReadLine().Trim().ToUpper(); //First line is given currency
                rc._expectedCurrency = sr.ReadLine().Trim().ToUpper(); //Second line is expected currency
                while (!sr.EndOfStream) // Remaining lines are sums in given currency that needs to be converted to expected currencies
                {
                    rc._sums.Add(float.Parse(sr.ReadLine().Trim()));
                }
            }
            lrc.Add(rc);
            return lrc;
        }
    }
}
