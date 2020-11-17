using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5.Models
{
    public class Tecnico
    {
        [Key]
        public int ID_AVTECNICO { get; set; }

        public int ID_AVALIACAO { get; set; }
                
        public int RESPOSTA1 { get; set; }
                
        public int RESPOSTA2 { get; set; }
                
        public int RESPOSTA3 { get; set; }
                
        public int RESPOSTA4 { get; set; }
                
        public int RESPOSTA5 { get; set; }
                
        public int RESPOSTA6 { get; set; }
                
        public int RESPOSTA7 { get; set; }
                
        public string RESPOSTA8 { get; set; }
                
        public int RESPOSTA9 { get; set; }
                
        public int RESPOSTA10 { get; set; }
                
        public int RESPOSTA11 { get; set; }
                
        public int RESPOSTA12 { get; set; }
                
        public int RESPOSTA13 { get; set; }
                
        public int RESPOSTA14 { get; set; }
                
        public int RESPOSTA15 { get; set; }
                
        public int RESPOSTA16 { get; set; }
                
        public int RESPOSTA17 { get; set; }
                
        public int RESPOSTA18 { get; set; }

        public int RESPOSTA19 { get; set; }

        public int RESPOSTA20 { get; set; }

        public float TEC_NOTA_FINAL { get; set; }

        public virtual Avaliacao Avaliacao { get; set; }
    }
}