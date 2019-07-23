using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using UnifiedPractice.Utility;

namespace UnifiedPractice.Providers
{
    public class Caching
    {
        private static readonly MemoryCache Cache = new MemoryCache(new MemoryCacheOptions());
        private static readonly TimeSpan QuarterHour = new TimeSpan(0, 15, 0);
        private static readonly TimeSpan OneHour = new TimeSpan(1, 0, 0);
        private static readonly TimeSpan OneDay = new TimeSpan(1, 0, 0, 0);

        /// <summary>
        /// Cache item list
        /// </summary>
        public static readonly String[] Keys =
        {
            "Avatars", "Products", "ProductYarns", "ProductYarnVns", "ReferenceMap", "GreigeStock", "GreigeVnStock", "YarnStock", "YarnVnStock",
            "RolesList", "BuyMonthList","CustomerList","CustomerVnList","OrderCount","OrderAmount","OrderValue","UserMapping","OrderPlan"
        };

        public static Dictionary<string, Tuple<DateTime, int>> CacheList = new Dictionary<string, Tuple<DateTime, int>>();

        #region clear function
        /// <summary>
        /// Clear one cache
        /// </summary>
        /// <param name="name"></param>
        public static void ClearCache(string name)
        {
            Cache.Remove(name);
            CacheList.Remove(name);
        }

        /// <summary>
        /// Clear all cache
        /// </summary>
        public static void ClearAllCache()
        {
            foreach (var k in Keys)
            {
                Cache.Remove(k);
            }
            CacheList.Clear();
        }
        #endregion

        #region AvailabilityTime

        public static bool SetAvailabilityTime(string date, List<string> at)
        {
            try
            {
                const string key = "AvailabilityTime";
                var ht = GetAvailabilityTime();
                ht.Add(date, at);
                Cache.Set(key, ht, OneDay);
                CacheList[key] = Tuple.Create(DateTime.Now, ht.Count);
                return true;
            }
            catch (Exception ex)
            {
                LogUtility.LogError(MethodBase.GetCurrentMethod().ReflectedType, MethodBase.GetCurrentMethod().Name, ex);
                return false;
            }

        }

        public static List<string> GetAvailabilityTime(DateTime date)
        {
            var ht = GetAvailabilityTime();
            if (ht.ContainsKey(date))
            {
                return (List<string>)ht[date];
            }
            return null;
        }

        public static Hashtable GetAvailabilityTime()
        {
            return Cache.TryGetValue("AvailabilityTime", out Hashtable ht) ? ht : new Hashtable();
        }
        #endregion

        #region Auth Token
        /// <summary>
        /// Set authToken by company
        /// </summary>
        /// <param name="name"></param>
        /// <param name="token"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public static bool SetCompanyAuthToken(string name, string token, int expire)
        {
            try
            {
                Cache.Set(name, token, new TimeSpan(0, 0, expire));
                return true;
            }
            catch (Exception ex)
            {
                LogUtility.LogError(MethodBase.GetCurrentMethod().ReflectedType, MethodBase.GetCurrentMethod().Name, ex);
                return false;
            }
        }

        /// <summary>
        /// Get authToken by company
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetCompanyAuthToken(string name)
        {
            try
            {
                return Cache.TryGetValue(name, out string value) ? value : string.Empty;
            }
            catch (Exception ex)
            {
                LogUtility.LogError(MethodBase.GetCurrentMethod().ReflectedType, MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }
        #endregion

    }

    public class DateValueDecimal
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Value { get; set; }
    }

    public class DateValueInt
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Value { get; set; }
    }
}
