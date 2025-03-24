using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JOIEnergy.Repository.Interfaces
{
    public interface ISmartMeterPricePlanRepository
    {
        Dictionary<string, string> GetSmartMeterToPricePlanMappings();
    }
}
