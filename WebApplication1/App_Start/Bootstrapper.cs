using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.DataHandler.Serializer;
using Microsoft.Owin.Security.DataProtection;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using WebApplication1.Models.Security;

namespace WebApplication1.App_Start
{
    public class Bootstrapper
    {
        private static Container _container;

        public static Container Container
        {
            get
            {
                if (Bootstrapper._container == null)
                {
                    var container = new Container();
                    container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();

                    Bootstrapper._container = container;
                }

                return Bootstrapper._container;
            }
        }

        public static void StartUp()
        {
            SimpleInjectorInitializer.Initialize(Bootstrapper.Container);
            SimpleInjectorWebApiInitializer.Initialize(Bootstrapper.Container);

            Bootstrapper.Container.Register(typeof(UserManager<,>), typeof(ApplicationUserManager<,>), Lifestyle.Scoped);
            Bootstrapper.Container.Register(typeof(IUserStore<,>), typeof(ApplicationUserStore<,>), Lifestyle.Scoped);
            Bootstrapper.Container.Register(typeof(IUserRepository<,>), typeof(UserRepository<,>), Lifestyle.Scoped);

            Bootstrapper.Container.Register(typeof(ISecureDataFormat<>), typeof(SecureDataFormat<>), Lifestyle.Scoped);
            Bootstrapper.Container.Register<IDataSerializer<AuthenticationTicket>, TicketSerializer>(Lifestyle.Scoped);
            Bootstrapper.Container.Register<ITextEncoder, Base64TextEncoder>(Lifestyle.Scoped);
            Bootstrapper.Container.Register<IDataProtector>(() => new DpapiDataProtectionProvider().Create("ASP.NET Identity"), Lifestyle.Scoped);
            Bootstrapper.Container.Register<IAuthenticationManager>(() => HttpContext.Current.GetOwinContext().Authentication);

            Bootstrapper.Container.RegisterSingleton<UserDb<ApplicationUser, string>>(() => new UserDb<ApplicationUser, string>((db) => db.Count.ToString()));
            Bootstrapper.Container.RegisterSingleton<UserPasswordHashDb<string>>();
            Bootstrapper.Container.RegisterSingleton<UserLoginInfoDb<string>>();
        }
    }
}