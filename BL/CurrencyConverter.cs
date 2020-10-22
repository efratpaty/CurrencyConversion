using CurrencyConversion.DL; //DataSourceFactory
using System;
using System.Collections.Generic; // List
using System.Collections.Specialized; // NameValueCollection


namespace CurrencyConversion.BL
{
    public class CurrencyConverter
    {
        private IDSUtils<string, float> _ds;
        public CurrencyConverter(NameValueCollection dsConfig)
        {
            DateTime acceptableCreationTime = DateTime.Today;
            _ds = DSFactory<string, float>.Create(dsConfig, acceptableCreationTime); // create data set
            MidnightNotifier.DayChanged += (s, e) => { _ds.UpdateDS(dsConfig); Console.WriteLine("updatad ds"); }; // Update ds at midnight every day 
        }

        public List<double> ConvertCurrencies(RequestedCurrencies requestedCurrencies)
        {   //Get conversion rate by dividing expected currency value with  given currency value 
            float rate = _ds.GetVal(requestedCurrencies._expectedCurrency) / _ds.GetVal(requestedCurrencies._givenCurrency);
            List<double> convertedSums = new List<double>();
            foreach (float sum in requestedCurrencies._sums)
            {
                convertedSums.Add(rate * sum); //The actual conversion
            }
            return convertedSums;
        }
        public List<List<double>> ConvertCurrencies(List<RequestedCurrencies> requestedCurrencies)
        {
            List<List<double>> sumsLists = new List<List<Double>>();
            // convert currencies for each requested currency conversion
            foreach (RequestedCurrencies requested in requestedCurrencies)
            {
                sumsLists.Add(ConvertCurrencies(requested));
            }
            return sumsLists;
        }
    }
}
