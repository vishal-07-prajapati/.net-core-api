using API.DTO;
using API.Interface;
using API.Repositories;

namespace API.Services
{
    public class JwtService : IJwtService
    {
        private readonly IJwtRepository _jwt;
        public JwtService(IJwtRepository jwt)
        {
            _jwt = jwt;
        }

        public string GenerateToken(AuthDTO user) => _jwt.GenerateToken(user);
    }
}
