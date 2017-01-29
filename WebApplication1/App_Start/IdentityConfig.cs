using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using WebApplication1.Models;
using WebApplication1.App_Start;
using System;

namespace WebApplication1
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager<T, TKey> : UserManager<T, TKey> 
        where T : class, IUser<TKey>
        where TKey : IEquatable<TKey>
    {
        public ApplicationUserManager(IUserStore<T, TKey> store)
            : base(store)
        {
        }

        public static ApplicationUserManager<T, TKey> Create(IdentityFactoryOptions<ApplicationUserManager<T, TKey>> options, IOwinContext context)
        {
            var manager = Bootstrapper.Container.GetInstance<ApplicationUserManager<T, TKey>>();
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<T, TKey>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<T, TKey>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}
