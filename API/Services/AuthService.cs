using API.DTO;
using API.Interface;
using API.Models;
using API.Repositories;

namespace API.Services
{
    public class AuthService
    {
        private readonly IAuth _auth;
        public AuthService(IAuth auth) 
        {
            _auth = auth;
        }

        public SuccessModel GetUser(AuthDTO user) => _auth.GetUser(user);
    }
}
