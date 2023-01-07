using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#nullable disable

namespace EmailService
{
    public class EmailConfig
    {
        public string From { get; set; }

        public string SmtServer { get; set; }
        public int port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        
    }
}