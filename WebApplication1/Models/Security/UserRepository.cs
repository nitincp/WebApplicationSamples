using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebApplication1.Models.Security
{
    public class UserRepository<T, TKey> : IUserRepository<T, TKey> 
        where T : class, IUser<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly UserDb<T, TKey> _userDb;
        private readonly UserLoginInfoDb<TKey> _userLoginDb;
        private readonly UserPasswordHashDb<TKey> _userPasswordHashDb;

        public UserRepository(UserDb<T, TKey> userDb, UserLoginInfoDb<TKey> userLoginDb, UserPasswordHashDb<TKey> userPasswordHashDb)
        {
            this._userDb = userDb;
            this._userLoginDb = userLoginDb;
            this._userPasswordHashDb = userPasswordHashDb;
        }

        public async Task AddLoginAsync(T user, UserLoginInfo login)
        {
            if (this._userLoginDb.ContainsKey(user.Id) == false)
            {
                this._userLoginDb[user.Id] = new List<UserLoginInfo>();
            }

            this._userLoginDb[user.Id].Add(login);

            await Task.FromResult(0);
        }

        public async Task<TKey> CreateAsync(T user)
        {
            var existingUser = await this.FindByNameAsync(user.UserName);
            if (existingUser == default(T))
            {
                var id = this._userDb.GetNextKey(this._userDb);
                this._userDb.Add(id, user);
                return await Task.FromResult(id);
            }
            else
            {
                throw new Exception("User name already exists.");
            }
        }

        public async Task DeleteAsync(T user)
        {
            this._userDb.Remove(user.Id);
            await Task.FromResult(0);
        }

        public async Task<T> FindAsync(UserLoginInfo login)
        {
            var userLogins = this._userLoginDb.Where(item => item.Value.Contains(login)).ToList();
            if (userLogins.Count == 0)
            {
                return await Task.FromResult(default(T));
            }
            else
            {
                return await Task.FromResult(await this.FindByIdAsync(userLogins.First().Key));
            }
        }

        public async Task<T> FindByIdAsync(TKey userId)
        {
            if (this._userDb.ContainsKey(userId))
            {
                return await Task.FromResult(this._userDb[userId]);

            }
            else
            {
                return await Task.FromResult(default(T));
            }
        }

        public async Task<T> FindByNameAsync(string userName)
        {
            var users = this._userDb.Where(item => item.Value.UserName == userName).ToList();

            if (users.Count > 0)
            {
                return await Task.FromResult(users.First().Value);
            }
            else
            {
                return await Task.FromResult(default(T));
            }
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(T user)
        {
            if (this._userLoginDb.ContainsKey(user.Id) == false)
            {
                return await Task.FromResult(default(IList<UserLoginInfo>));
            }
            else
            {
                return await Task.FromResult(this._userLoginDb[user.Id]);
            }
        }

        public async Task<string> GetPasswordHashAsync(T user)
        {
            if (this._userPasswordHashDb.ContainsKey(user.Id) == false)
            {
                throw new Exception("User not found");
            }
            else
            {
                return await Task.FromResult(this._userPasswordHashDb[user.Id]);
            }
        }

        public async Task<bool> HasPasswordAsync(T user)
        {
            var hasPassword = this._userPasswordHashDb.ContainsKey(user.Id);
            return await Task.FromResult(hasPassword);
        }

        public async Task RemoveLoginAsync(T user, UserLoginInfo login)
        {
            if (this._userLoginDb.ContainsKey(user.Id) == false)
            {
                this._userLoginDb.Remove(user.Id);
                await Task.FromResult(0);
            }
            else
            {
                throw new Exception("User not found");
            }
        }

        public async Task SetPasswordHashAsync(T user, string passwordHash)
        {
            TKey id;

            if (Object.Equals(user.Id, default(TKey)))
            {
                id = await this.CreateAsync(user);
            }
            else
            {
                id = user.Id;
            }

            if (this._userPasswordHashDb.ContainsKey(id) == false)
            {
                this._userPasswordHashDb[id] = string.Empty;
            }

            this._userPasswordHashDb[id] = passwordHash;
            await Task.FromResult(0);
        }

        public async Task UpdateAsync(T user)
        {
            if (this._userDb.ContainsKey(user.Id) == false)
            {
                throw new Exception("User not found");
            }
            else
            {
                var currentUser = this._userDb.Single(item => item.Key.Equals(user.Id));
                currentUser.Value.UserName = user.UserName;
                await Task.FromResult(0);
            }
        }
    }
}