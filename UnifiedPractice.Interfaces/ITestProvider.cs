using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnifiedPractice.Models;

namespace UnifiedPractice.Interfaces
{
    public interface ITestProvider
    {
        Task<IEnumerable<AvailabilityTime>> GetAvailabilityTimes(int limit = 10);
        Task<AvailabilityTime> GetAvailabilityTime(int id);
        Task<List<AvailabilityTime>> GetAvailabilityTime(DateTime date);
    }
}