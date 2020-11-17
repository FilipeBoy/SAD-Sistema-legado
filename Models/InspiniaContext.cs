using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5.Models
{
    public class InspiniaContext : DbContext
    {
        public InspiniaContext() : base("name=InspiniaContext")
        {
        }

        
        public System.Data.Entity.DbSet<Inspinia_MVC5.Models.Ambiente> Ambientes { get; set; }

        public System.Data.Entity.DbSet<Inspinia_MVC5.Models.Avaliacao> Avaliacaos { get; set; }

        public System.Data.Entity.DbSet<Inspinia_MVC5.Models.Feedback> Feedbacks { get; set; }

        public System.Data.Entity.DbSet<Inspinia_MVC5.Models.Negocio> Negocios { get; set; }

        public System.Data.Entity.DbSet<Inspinia_MVC5.Models.Tecnico> Tecnicoes { get; set; }

        public System.Data.Entity.DbSet<Inspinia_MVC5.Models.Usuario> Usuarios { get; set; }

        


    }
}