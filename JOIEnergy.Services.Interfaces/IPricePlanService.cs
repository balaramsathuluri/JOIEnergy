using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JOIEnergy.Services.Interfaces
{
    public interface IPricePlanService
    {
        /// <summary>
        /// Calculates and returns the consumption cost for each price plan for the given smart meter.
        /// </summary>
        /// <param name="smartMeterId">Smart meter identifier.</param>
        /// <returns>A dictionary where keys are price plan names and values are the corresponding consumption costs.</returns>
        Dictionary<string, decimal> GetConsumptionCostOfElectricityReadingsForEachPricePlan(string smartMeterId);
    }
}
