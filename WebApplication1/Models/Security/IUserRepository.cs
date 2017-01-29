using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebApplication1.Models.Security
{
    public interface IUserRepository<T, TKey> 
        where T: IUser<TKey>
        where TKey : IEquatable<TKey>
    {
        Task<TKey> CreateAsync(T user);

        Task DeleteAsync(T user);

        Task<T> FindByIdAsync(TKey userId);

        Task<T> FindByNameAsync(string userName);

        Task UpdateAsync(T user);

        Task AddLoginAsync(T user, UserLoginInfo login);

        Task RemoveLoginAsync(T user, UserLoginInfo login);

        Task<IList<UserLoginInfo>> GetLoginsAsync(T user);

        Task<T> FindAsync(UserLoginInfo login);

        Task SetPasswordHashAsync(T user, string passwordHash);

        Task<string> GetPasswordHashAsync(T user);

        Task<bool> HasPasswordAsync(T user);
    }
}