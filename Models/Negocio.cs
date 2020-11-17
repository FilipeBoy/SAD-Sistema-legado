using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5.Models
{
    public class Negocio
    {
        [Key]
        public int ID_AVNEGOCIO { get; set; }

        public int ID_AVALIACAO { get; set; }

        public int RESPOSTA1 { get; set; }

        public int RESPOSTA2 { get; set; }

        public float RESPOSTA3 { get; set; }

        public int RESPOSTA4 { get; set; }

        public int RESPOSTA5 { get; set; }

        public int RESPOSTA6 { get; set; }

        public int RESPOSTA7 { get; set; }

        public int RESPOSTA8 { get; set; }

        public int RESPOSTA9 { get; set; }

        public int RESPOSTA10 { get; set; }

        public int RESPOSTA11 { get; set; }

        public int RESPOSTA12 { get; set; }

        public float NEG_NOTA_FINAL { get; set; }

        public virtual Avaliacao Avaliacao { get; set; }

    }
}