using API.DTO;

namespace API.Interface
{
    public interface IJwtService
    {
        string GenerateToken(AuthDTO user);
    }
}
