namespace JOIEnergy.Services.Interfaces
{

    public interface IAccountService
    {
        /// <summary>
        /// Returns the price plan ID associated with the provided smart meter ID.
        /// </summary>
        /// <param name="smartMeterId">Smart meter identifier.</param>
        /// <returns>Price plan identifier.</returns>
        string GetPricePlanIdForSmartMeterId(string smartMeterId);
    }


}
