﻿namespace DirectoryWebApi.Models.Entities
{
    public class ContactInfo
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }

        public Guid PersonId { get; set; }
    }
}
