using System;

namespace UnifiedPractice.Models
{
    public partial class AvailabilityTime
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int Precedence => (int)(EndDate - StartDate).TotalDays;
        public string Rrule { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
    }
}