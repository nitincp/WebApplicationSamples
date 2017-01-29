using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication1.App_Start;

namespace WebApplication1.Models.Security
{
    public class ApplicationUserStore<T, TKey> : IUserStore<T, TKey>, IUserLoginStore<T, TKey>, IUserPasswordStore<T, TKey> 
        where T : class, IUser<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly IUserRepository<T, TKey> _userRepository;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ApplicationDbContext() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        public static ApplicationUserStore<T, TKey> Create()
        {
            return Bootstrapper.Container.GetInstance<ApplicationUserStore<T, TKey>>();
        }

        public ApplicationUserStore(IUserRepository<T, TKey> userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task CreateAsync(T user)
        {
            await this._userRepository.CreateAsync(user);
        }

        public async Task DeleteAsync(T user)
        {
            await this._userRepository.DeleteAsync(user);
        }

        public async Task<T> FindByIdAsync(TKey userId)
        {
            return await this._userRepository.FindByIdAsync(userId);
        }

        public async Task<T> FindByNameAsync(string userName)
        {
            return await this._userRepository.FindByNameAsync(userName);
        }

        public async Task UpdateAsync(T user)
        {
            await this._userRepository.UpdateAsync(user);
        }

        public async Task AddLoginAsync(T user, UserLoginInfo login)
        {
            await this._userRepository.AddLoginAsync(user, login);
        }

        public async Task RemoveLoginAsync(T user, UserLoginInfo login)
        {
            await this._userRepository.RemoveLoginAsync(user, login);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(T user)
        {
            return await this._userRepository.GetLoginsAsync(user);
        }

        public async Task<T> FindAsync(UserLoginInfo login)
        {
            return await this._userRepository.FindAsync(login);
        }

        public async Task SetPasswordHashAsync(T user, string passwordHash)
        {
            await this._userRepository.SetPasswordHashAsync(user, passwordHash);
        }

        public async Task<string> GetPasswordHashAsync(T user)
        {
            return await this._userRepository.GetPasswordHashAsync(user);
        }

        public async Task<bool> HasPasswordAsync(T user)
        {
            return await this._userRepository.HasPasswordAsync(user);
        }
    }
}