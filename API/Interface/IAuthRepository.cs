using API.DTO;
using API.Models;

namespace API.Interface
{
    public interface IAuthRepository
    {
        public SuccessModel GetUser(AuthDTO user);
    }
}
