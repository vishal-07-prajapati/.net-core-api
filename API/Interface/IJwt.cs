using API.DTO;
namespace API.Interface
{
    public interface IJwt
    {
        string GenerateToken(AuthDTO user);
    }
}
