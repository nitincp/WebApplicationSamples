using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;

namespace WebApplication1.Models.Security
{
    public class UserPasswordHashDb<TKey> : Dictionary<TKey, string>
        where TKey : IEquatable<TKey>
    {
    }
}