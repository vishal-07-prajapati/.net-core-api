using API.DTO;
using API.Interface;
using API.Models;
using API.Repositories;

namespace API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _auth;
        public AuthService(IAuthRepository auth) 
        {
            _auth = auth;
        }

        public SuccessModel GetUser(AuthDTO user) => _auth.GetUser(user);
    }
}
