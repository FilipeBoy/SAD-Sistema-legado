using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5.Models
{
    public class Feedback
    {
        [Key]
        public int ID_FEEDBACK { get; set; }

        public int ID_AVALIACAO { get; set; }

        [Display(Name = "Estratégia sugerida")]
        public string RESULTADO { get; set; }

        [Display(Name = "Estratégia adotada")]
        public string ESTRATEGIA_ADOTADA { get; set; }

        [Display(Name = "Comentário")]
        [DataType(DataType.MultilineText)]
        public string COMENTARIO { get; set; }

        public int RESPOSTA1 { get; set; }

        public string RESPOSTA2 { get; set; }

        public int RESPOSTA3 { get; set; }

        [DataType(DataType.MultilineText)]
        public string RESPOSTA4 { get; set; }

        public int RESPOSTA5 { get; set; }

        [DataType(DataType.MultilineText)]
        public string RESPOSTA6 { get; set; }

        public int RESPOSTA7 { get; set; }

        public virtual Avaliacao Avaliacao { get; set; }
    }
}