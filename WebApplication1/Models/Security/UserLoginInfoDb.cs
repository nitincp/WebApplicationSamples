using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;

namespace WebApplication1.Models.Security
{
    public class UserLoginInfoDb<TKey> : Dictionary<TKey, List<UserLoginInfo>>
        where TKey : IEquatable<TKey>
    {
    }
}