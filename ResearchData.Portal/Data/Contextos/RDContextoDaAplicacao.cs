using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using ResearchData.Portal.GerenciamentoUsuario;
using ResearchData.Portal.Models.Negocio.Analises;
using ResearchData.Portal.Models.Negocio.Experimentos;
using ResearchData.Portal.Models.Negocio.Sujeitos;
using ResearchData.Portal.Models.Projetos.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Data.Contextos
{
    public class RDContextoDaAplicacao : IdentityDbContext<UsuarioAplicacao>
    {

        public RDContextoDaAplicacao(DbContextOptions<RDContextoDaAplicacao> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            
            

            #region MapProjeto
            //Mapeamento Projeto
            builder.Entity<Projeto>().HasKey(p => p.Id);
            #endregion


            #region MapAnalise
            //Mapeamento Analise
            builder.Entity<Analise>().HasKey(a => a.Id);
            #endregion


            #region MapColaborador
            
            builder.Entity<ColaboradorAnalise>().HasKey(ca => new { ca.UsuarioAplicacaoId, ca.AnaliseId });

            builder.Entity<ColaboradorAnalise>().HasOne(ca => ca.UsuarioAplicacao)
                .WithMany(usu => usu.ColaboradorAnalise)
                .HasForeignKey(ca => ca.UsuarioAplicacaoId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ColaboradorAnalise>().HasOne(ca => ca.Analise)
                .WithMany(ana => ana.ColaboradorAnalise)
                .HasForeignKey(ca => ca.AnaliseId);

            #endregion

        
            #region MapComentario
            //Mapeamento de Comentário
            builder.Entity<Comentario>().HasKey(com => com.Id);

            #endregion


            #region MapSujeito

            builder.Entity<Grupo>().HasKey(gru => gru.Id);

            builder.Entity<SujeitoExperimental>().HasKey(cob => cob.Id);
            

            builder.Entity<Caracteristica>().HasKey(car => car.Id);

            builder.Entity<Caracteristica>()
               .Property(car => car.TipoDoDado)
               .HasConversion<string>();

            builder.Entity<Medicao>().HasKey(carCob => carCob.Id);

            builder.Entity<Medicao>().HasOne(carCob => carCob.SujeitoExperimental)
                .WithMany(cob => cob.Medicao)
                .HasForeignKey(carCob => carCob.SujeitoExperimentalId);


            #endregion

            #region Experimento

            builder.Entity<Experimento>().HasKey(exp => exp.Id);

            builder.Entity<TemplateExperimento>().HasKey(temple => temple.Id);


            #endregion



        }
    }
}
