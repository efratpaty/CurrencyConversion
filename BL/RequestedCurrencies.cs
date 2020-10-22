using System.Collections.Generic;

namespace CurrencyConversion.BL
{
    public class RequestedCurrencies
    {
        public string _givenCurrency;
        public string _expectedCurrency;
        public List<double> _sums;
        public RequestedCurrencies()
        {
            _sums = new List<double>();
        }
    }
}
