namespace FlightsMinimalWebApi.Auth;

public class UserRepository : IUserRepository
{
    private List<UserDto> _users = new()
    {
        new UserDto("Bob", "000"),
        new UserDto("Tom", "000"),
        new UserDto("Sem", "000")
    };


    public UserDto? GetUser(UserModel userModel) =>
        _users.FirstOrDefault(u =>
            u.Login == userModel.Login && u.Password == userModel.Password);
}