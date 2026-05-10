using API.DTO;
using API.Models;

namespace API.Interface
{
    public interface IAuth
    {
        public SuccessModel GetUser(AuthDTO user);
    }
}
