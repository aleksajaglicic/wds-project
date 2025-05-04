using System.ComponentModel.DataAnnotations;

namespace Trainer.Server.DTOs
{
    public class UserDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
