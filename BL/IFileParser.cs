using System;
using System.Collections.Generic;//List
using System.Text;

namespace CurrencyConversion.BL
{
    public interface IFileParser
    {
        //Parse given file to list of requested currencies 
        List<RequestedCurrencies> ParseFile(string fileName);
    }
}
