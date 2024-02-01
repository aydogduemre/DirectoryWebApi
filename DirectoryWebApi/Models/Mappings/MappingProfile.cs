using AutoMapper;
using DirectoryWebApi.Models.Dtos;
using DirectoryWebApi.Models.Entities;

namespace DirectoryWebApi.Models.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PersonAddDto, Person>();
            CreateMap<ContactInfoAddDto, ContactInfo>();
        }
    }
}
