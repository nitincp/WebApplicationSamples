using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Security
{
    public class UserDb<T, TKey> : Dictionary<TKey, T>
        where T : class, IUser<TKey>
        where TKey : IEquatable<TKey>
    {
        public readonly Func<UserDb<T, TKey>, TKey> GetNextKey;

        public UserDb(Func<UserDb<T, TKey>, TKey> getNextKeyFunc)
        {
            this.GetNextKey = getNextKeyFunc;
        }
    }
}