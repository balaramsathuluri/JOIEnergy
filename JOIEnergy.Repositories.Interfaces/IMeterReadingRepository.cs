using JOIEnergy.Domain.Models;

namespace JOIEnergy.Repository.Interfaces
{
    public interface IMeterReadingRepository
    {
        /// <summary>
        /// Retrieves all electricity readings for a given smart meter.
        /// </summary>
        /// <param name="smartMeterId">The ID of the smart meter.</param>
        /// <returns>List of electricity readings.</returns>
        List<ElectricityReading> GetReadings(string smartMeterId);

        /// <summary>
        /// Stores electricity readings for a given smart meter.
        /// </summary>
        /// <param name="smartMeterId">The ID of the smart meter.</param>
        /// <param name="electricityReadings">The list of electricity readings to store.</param>
        void StoreReadings(string smartMeterId, List<ElectricityReading> electricityReadings);
    }
}
