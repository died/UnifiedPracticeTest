using Microsoft.AspNetCore.Mvc;
using UnifiedPractice.Handlers;

namespace UnifiedPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentHandler _ah;
        public AppointmentController(AppointmentHandler ah)
        {
            _ah = ah;
        }

        // GET: api/Appointment
        [HttpGet("{date}")]
        public dynamic Get(string date)
        {
            return _ah.GetAvailabilityTime(date);
        }


    }
}