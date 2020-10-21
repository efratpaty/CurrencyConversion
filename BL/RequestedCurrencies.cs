using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
