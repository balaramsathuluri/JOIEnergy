using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOIEnergy.Domain.Models;

namespace JOIEnergy.Services.Interfaces
{
    public interface IMeterReadingService
    {
        /// <summary>
        /// Retrieves the list of electricity readings for the specified smart meter.
        /// </summary>
        /// <param name="smartMeterId">Smart meter identifier.</param>
        /// <returns>List of electricity readings.</returns>
        List<ElectricityReading> GetReadings(string smartMeterId);

        /// <summary>
        /// Stores the provided electricity readings for the specified smart meter.
        /// </summary>
        /// <param name="smartMeterId">Smart meter identifier.</param>
        /// <param name="electricityReadings">List of electricity readings to store.</param>
        void StoreReadings(string smartMeterId, List<ElectricityReading> electricityReadings);
    }
}
