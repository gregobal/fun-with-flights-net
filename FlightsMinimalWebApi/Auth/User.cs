namespace FlightsMinimalWebApi.Auth;

public record UserDto(string Login, string Password);

public record UserModel
{
    [Required]
    public string Login { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}