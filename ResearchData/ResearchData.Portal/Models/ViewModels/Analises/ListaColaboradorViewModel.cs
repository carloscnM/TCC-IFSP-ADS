using System;

namespace ResearchData.Portal.Models.ViewModels.Analises
{
    public class ListaColaboradorViewModel
    {
        public string UsuarioId { get; set; }
        public string Email { get; set; }
        public DateTime DataCadastro { get; set; }

        public int Acesso { get; set; }
    }
}