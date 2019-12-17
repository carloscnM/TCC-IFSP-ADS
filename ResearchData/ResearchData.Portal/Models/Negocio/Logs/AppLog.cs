using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Models.Negocio.Logs
{
    [Table("RESEARCHLOG")]
    public class AppLog
    {
        [Column("log_id"), Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid LogId { get; set; }

        [Column("log_message")]
        public string Mensagem { get; set; }

        [Column("log_captura")]
        public DateTime Captura { get; set; }
    }
}
