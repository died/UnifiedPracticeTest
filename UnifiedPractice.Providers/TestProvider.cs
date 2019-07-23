using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnifiedPractice.Interfaces;
using UnifiedPractice.Models;
using UnifiedPractice.Utility;

namespace UnifiedPractice.Providers
{

    public class TestProvider : ITestProvider
    {
        private readonly TestContext _context;

        public TestProvider(TestContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get last N=10 record as list
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AvailabilityTime>> GetAvailabilityTimes(int limit = 10)
        {
            //TODO add debug
            return await _context.AvailabilityTime.OrderByDescending(x => x.Id).Take(limit).ToListAsync();
        }

        /// <summary>
        /// Get by id, null = not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AvailabilityTime> GetAvailabilityTime(int id)
        {
            return await _context.AvailabilityTime.FindAsync(id);
        }

        public async Task<List<AvailabilityTime>> GetAvailabilityTime(DateTime date)
        {
            try
            {
                return await _context.AvailabilityTime.Where(x => x.StartDate <= date && x.EndDate >= date).ToListAsync();
            }
            catch (DbException dbEx)
            {
                LogUtility.LogError(MethodBase.GetCurrentMethod().ReflectedType, MethodBase.GetCurrentMethod().Name, dbEx);
                return null;
            }
            catch (Exception ex)
            {
                LogUtility.LogError(MethodBase.GetCurrentMethod().ReflectedType, MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }
    }
}
