using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

#nullable disable
namespace Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        // id string olarak baba classdan geliyor

        public string DisplayName { get; set; }

        public Address Address {get; set;}
    }

   
}