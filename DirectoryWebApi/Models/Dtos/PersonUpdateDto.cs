namespace DirectoryWebApi.Models.Dtos
{
    public record PersonUpdateDto(Guid Id,string Name, string Surname, string Company);
}
