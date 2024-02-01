using DirectoryWebApi.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DirectoryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        readonly DirectoryDbContext _context;

        public ReportsController(DirectoryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _context.ContactInfos.GroupBy(contactInfo => contactInfo.Location)
                .Select(group => new
                    {
                        Location = group.Key,
                        PersonCount = group.Select(contactInfo => contactInfo.PersonId).Distinct().Count(),
                        PhoneNumberCount = group.Select(contactInfo => contactInfo.PhoneNumber).Distinct().Count()
                    })
                .OrderByDescending(x => x.PersonCount)
                .ThenByDescending(x => x.PhoneNumberCount)
                .ToList();

            return Ok(result);
        }
    }
}
