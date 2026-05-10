using API.DTO;
using API.Interface;
using API.Repositories;

namespace API.Services
{
    public class JwtService
    {
        private readonly IJwt _jwt;
        public JwtService(IJwt jwt)
        {
            _jwt = jwt;
        }

        public string GenerateToken(AuthDTO user) => _jwt.GenerateToken(user);
    }
}
