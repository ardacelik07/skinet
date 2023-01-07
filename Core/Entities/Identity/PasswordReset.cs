using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#nullable disable
namespace Core.Entities.Identity
{
    public class PasswordReset 
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }

        public AppUser appUser {get; set;}

        public string ClientURL { get; set; }

        public bool Status { get; set; }

        public int Code { get; set; }


    }
}