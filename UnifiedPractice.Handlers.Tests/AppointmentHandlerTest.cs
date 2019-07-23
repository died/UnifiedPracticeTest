using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnifiedPractice.Interfaces;

namespace UnifiedPractice.Handlers.Tests
{
    [TestClass]
    public class AppointmentHandlerTest
    {
        private AppointmentHandler _ah;
        private ITestProvider _tp;

        [TestInitialize]
        public void Initialize()
        {
            _ah = new AppointmentHandler(_tp);
        }

        private readonly List<TimeZone> _tzList = new List<TimeZone>
        {
            new TimeZone {Start = new TimeSpan(0, 9, 0), End = new TimeSpan(0, 15, 0)},
            new TimeZone {Start = new TimeSpan(0, 12, 0), End = new TimeSpan(0, 17, 0)}
        };

        [TestMethod]
        public void OverlapTestCount()
        {
            var tz = _ah.MergeOverlapping(_tzList);
            Assert.AreEqual(tz.Count(), 1,"Should merged to one");
        }

        [TestMethod]
        public void OverlapTestObjFail()
        {
            var expected = new TimeZone {Start = new TimeSpan(0, 10, 0), End = new TimeSpan(0, 17, 0)};
            
            var tz = _ah.MergeOverlapping(_tzList).ToList().First();
            Assert.AreNotEqual(tz.Start, expected.Start,"Should not equal");   //9!=10
            Assert.AreEqual(tz.End, expected.End,"Should be 17");  
        }

        [TestMethod]
        public void OverlapTestObjSuccess()
        {
            var expected = new TimeZone { Start = new TimeSpan(0, 9, 0), End = new TimeSpan(0, 17, 0) };

            var tz = _ah.MergeOverlapping(_tzList).ToList().First();
            Assert.AreEqual(tz.Start, expected.Start, "Should be 9");
            Assert.AreEqual(tz.End, expected.End, "Should be 17");
        }
    }


}
