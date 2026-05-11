using API.DTO;
namespace API.Interface
{
    public interface IJwtRepository
    {
        string GenerateToken(AuthDTO user);
    }
}
