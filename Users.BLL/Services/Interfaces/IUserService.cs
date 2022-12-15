using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.BLL.Common;
using Users.BLL.Models;
using Users.DAL.Entities;

namespace Users.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> RegisterAsync(RegisterModel model);
        Task<Output<bool>> EditAsync(RegisterModel model);
        Task<bool> DeleteAsync(string id);
        Task<UserDataModel> GetByIdAsync(string id);
        Task<UserModel> GetTokenAsync(TokenRequestModel model);

    }
}
