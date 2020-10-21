using System.Collections.Generic;

namespace CurrencyConversion.BL
{
    public class RequestedCurrencies
    {
        public string _givenCurrency;
        public string _expectedCurrency;
        public List<float> _sums;
        public RequestedCurrencies()
        {
            _sums = new List<float>();
        }
    }
}
