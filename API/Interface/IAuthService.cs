using API.DTO;
using API.Models;

namespace API.Interface
{
    public interface IAuthService
    {
        public SuccessModel GetUser(AuthDTO user);
    }
}
