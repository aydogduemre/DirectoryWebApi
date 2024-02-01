using AutoMapper;
using DirectoryWebApi.Data.Context;
using DirectoryWebApi.Models.Dtos;
using DirectoryWebApi.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DirectoryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactInfosController : ControllerBase
    {
        readonly DirectoryDbContext _context;
        readonly IMapper _mapper;

        public ContactInfosController(DirectoryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<ContactInfo> contactInfos = await _context.ContactInfos.ToListAsync();
            return Ok(contactInfos);
        }

        [HttpGet("{personId}")]
        public async Task<IActionResult> Get(Guid personId)
        {
            ContactInfo? contactInfo = await _context.ContactInfos.Where(contactInfo => contactInfo.PersonId == personId).FirstOrDefaultAsync();
            return Ok(contactInfo);
        }

        [HttpPost("{personId}")]
        public async Task<IActionResult> Post(ContactInfoAddDto contactInfoAddDto, Guid personId)
        {
            ContactInfo contactInfo = _mapper.Map<ContactInfo>(contactInfoAddDto);
            contactInfo.PersonId = personId;

            await _context.ContactInfos.AddAsync(contactInfo);
            await _context.SaveChangesAsync();

            Person? person = await _context.People.Include(person => person.ContactInfos).FirstOrDefaultAsync(person => person.Id == personId);
            return Ok(person);
        }

        [HttpDelete("{contactInfoId}")]
        public async Task<IActionResult> Delete(Guid contactInfoId)
        {
            ContactInfo? contactInfo = await _context.ContactInfos.FindAsync(contactInfoId);
            _context.ContactInfos.Remove(contactInfo);
            await _context.SaveChangesAsync();
            return Ok(contactInfo);
        }
    }
}
