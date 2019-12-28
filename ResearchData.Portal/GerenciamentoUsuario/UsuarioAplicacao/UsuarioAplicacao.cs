using Microsoft.AspNetCore.Identity;
using ResearchData.Portal.Models.Negocio.Analises;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.GerenciamentoUsuario
{
    public class UsuarioAplicacao : IdentityUser
    {
        public string Nome { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataUltimoAcesso { get; set; }

        public List<ColaboradorAnalise> ColaboradorAnalise { get; set; }
    }
}
