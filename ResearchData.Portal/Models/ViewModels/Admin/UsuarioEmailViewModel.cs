using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.ViewModels.Admin
{
    public class UsuarioEmailViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
