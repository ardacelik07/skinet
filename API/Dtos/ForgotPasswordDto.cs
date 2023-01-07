using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
#nullable disable


namespace API.Dtos
{
    public class ForgotPasswordDto
    {
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string ClientURI { get; set; }

    public int Code { get; set; }
    }
}