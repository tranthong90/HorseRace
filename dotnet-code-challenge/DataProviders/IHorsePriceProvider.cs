using dotnet_code_challenge.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_code_challenge.DataProviders
{
    public interface IHorsePriceProvider
    {
        Task<IEnumerable<HorsePrice>> GetHorsePrices();
        bool CanProcess();
    }
}
