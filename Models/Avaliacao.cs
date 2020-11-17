using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5.Models
{
    public class Avaliacao
    {
        [Key]
        public int ID_AVALIACAO { get; set; }

        [Display(Name = "Nome do sistema")]
        [Required(ErrorMessage = "Você precisa entrar com o {0}")]
        public string NOME_SISTEMA { get; set; }

        [Display(Name = "Data")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DATA { get; set; }

        [Display(Name = "Status")]
        public string STATUS { get; set; }

        [Display(Name = "Resultado")]
        public string RESULTADO { get; set; }

        [Display(Name = "Nota Final")]
        public float AV_NOTA_FINAL { get; set; }

        public int ID_USUARIO { get; set; }

        

        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<Negocio> Negocio { get; set; }
        public virtual ICollection<Tecnico> Tecnico { get; set; }
        public virtual ICollection<Ambiente> Ambiente { get; set; }
        public virtual ICollection<Feedback> Feedback { get; set; }
    }
}