namespace DirectoryWebApi.Models.Entities
{
    public class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; } 
        public string Surname { get; set; } 
        public string Company { get; set; }

        public ICollection<ContactInfo> ContactInfos { get; set; }
    }
}
