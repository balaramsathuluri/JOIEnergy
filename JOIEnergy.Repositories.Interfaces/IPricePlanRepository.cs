using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOIEnergy.Domain.Models;

namespace JOIEnergy.Repository.Interfaces
{
    public interface IPricePlanRepository
    {
        List<PricePlan> GetPricePlans();
    }
}
