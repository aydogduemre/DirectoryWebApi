using AutoMapper;
using DirectoryWebApi.Data.Context;
using DirectoryWebApi.Models.Dtos;
using DirectoryWebApi.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace DirectoryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        readonly DirectoryDbContext _context;
        readonly IMapper _mapper;
        readonly IDistributedCache _distributedCache;

        public PeopleController(DirectoryDbContext context, IMapper mapper, IDistributedCache distributedCache)
        {
            _context = context;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        [HttpPost]
        public async Task<IActionResult> Post(PersonAddDto personAddDto)
        {
            Person person = _mapper.Map<Person>(personAddDto);
            await _context.People.AddAsync(person);
            await _context.SaveChangesAsync();

            await RemoveCache();
            return Ok(personAddDto);
        }

        [HttpPut]
        public async Task<IActionResult> Put(PersonUpdateDto personUpdateDto)
        {
            Person? person = await _context.People.FindAsync(personUpdateDto.Id);
            person.Name = personUpdateDto.Name;
            person.Surname = personUpdateDto.Surname;
            person.Company = personUpdateDto.Company;
            await _context.SaveChangesAsync();

            await RemoveCache();
            return Ok(person);
        }

        [HttpDelete("{personId}")]
        public async Task<IActionResult> Delete(Guid personId)
        {
            Person? person = await _context.People.FindAsync(personId);
            _context.People.Remove(person);
            await _context.SaveChangesAsync();

            await RemoveCache();
            return Ok(person);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cachingPeople = await GetCache();
            if (cachingPeople is not null)
                return Ok(JsonSerializer.Deserialize<List<Person>>(cachingPeople));

            List<Person> people = await _context.People.Include(person => person.ContactInfos).ToListAsync();
            var jsonData = JsonSerializer.Serialize(people);
            await _distributedCache.SetStringAsync("people", jsonData);
            return Ok(people);
        }

        [HttpGet("{personId}")]
        public async Task<IActionResult> Get(Guid personId)
        {
            Person? person = await _context.People.FindAsync(personId);
            return Ok(person);
        }

        async Task<string?> GetCache()
        {
            var cachingPeople = await _distributedCache.GetStringAsync("people");
            return cachingPeople is not null ? cachingPeople : null;
        }

        async Task RemoveCache()
        {
            var cachingPeople = await GetCache();
            if (cachingPeople is not null)
                await _distributedCache.RemoveAsync("people");
        }
    } 
}
