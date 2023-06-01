namespace FlightsMinimalWebApi.Auth;

public interface IUserRepository
{
    UserDto? GetUser(UserModel userModel);
}
