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

        public List<float> ConvertCurrencies(RequestedCurrencies requestedCurrencies)
        {   //Get conversion rate by dividing expected currency value with  given currency value 
            float rate =  (float)_ds.GetVal(requestedCurrencies._expectedCurrency)/ (float)_ds.GetVal(requestedCurrencies._givenCurrency);
            List<float> convertedSums = new List<float>();
            foreach (float sum in requestedCurrencies._sums)
            {
                convertedSums.Add(rate * sum); //The actual conversion
            }
            return convertedSums;
        }
        public List<List<float>> ConvertCurrencies(List<RequestedCurrencies> requestedCurrencies)
        {
            List<List<float>> sumsLists = new List<List<float>>();
            // convert currencies for each requested currency conversion
            foreach (RequestedCurrencies requested in requestedCurrencies)
            {
                sumsLists.Add(ConvertCurrencies(requested));
            }
            return sumsLists;
        }
    }
}
