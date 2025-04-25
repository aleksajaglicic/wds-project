namespace Trainer.Server.DTOs
{
    public class UserDto
    {
        public string? Id { get; private set; }
        public string Name { get; }
        public string? LastName { get; }
        public string? Email { get; }
        public string? Password { get; }
    }
}
