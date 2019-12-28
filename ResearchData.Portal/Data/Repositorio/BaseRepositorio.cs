using Microsoft.EntityFrameworkCore;
using ResearchData.Portal.Data.Contextos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Data.Repositorio
{
    public abstract class BaseRepositorio<T> where T : class
    {
        protected readonly RDContextoDaAplicacao contexto;
        protected readonly DbSet<T> dbSet;


        public BaseRepositorio(RDContextoDaAplicacao contexto)
        {
            this.contexto = contexto;
            dbSet = contexto.Set<T>();
        }
    }
}
