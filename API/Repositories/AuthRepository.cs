using API.DTO;
using API.Interface;
using API.Models;
using API.Utilities;

namespace API.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private static List<AuthDTO> listUsers = new List<AuthDTO>()
        {
            new AuthDTO() { Email = "admin@gmail.com", Password = "admin@123", Role = "Admin" },
            new AuthDTO() { Email = "staff@gmail.com", Password = "admin@123", Role = "Staff" },
            new AuthDTO() { Email = "customer@gmail.com", Password = "customer@123", Role = "Customer" },
        };
        private readonly SuccessModel SuccessModel = new SuccessModel();
        public SuccessModel GetUser(AuthDTO user)
        {
            if (listUsers.AnyItems(x => x.Email == user.Email && x.Password == user.Password))
            {
                SuccessModel.Success = true;
                SuccessModel.Message = "Login Successfull";
                SuccessModel.Data = listUsers.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);
            }
            else
            {
                SuccessModel.Success = false;
                SuccessModel.Message = "User Not Found!";
            }
            return SuccessModel;
        }
    }
}
